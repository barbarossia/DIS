﻿//*********************************************************
//
// Copyright (c) Microsoft 2011. All rights reserved.
// This code is licensed under your Microsoft OEM Services support
//    services description or work order.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using DIS.Data.ServiceContract;
using DIS.Business.Library;
using DIS.Common.Utility;
using DIS.Data.DataContract;
using Microsoft.Http;
using Microsoft.ServiceModel.Web;
using DC = DIS.Data.DataContract;

namespace DIS.Business.Client {
    /// <summary>
    /// Provides logics to connect with REST web services
    /// </summary>
    public class ServiceClient : IServiceClient {
        /// <summary>
        /// HTTP header name of system ID
        /// </summary>
        public const string SystemIdHeaderName = "DIS-SystemId";
        /// <summary>
        /// HTTP header name of customer number
        /// </summary>
        public const string DirectionHeaderName = "DIS-Direction";
        /// <summary>
        /// HTTP header name of BasicAuthentication
        /// </summary>
        public const string AuthorizationHeaderName = "Authorization";

        #region priviate & protected member variables

        private const int timeoutMinutes = 30;
        private const string contentType = "application/xml";

        private int? systemId;
        private CallDirection callDirection;
        private ProxySetting proxySetting;
        private ServiceLocationHelper helper;
        private string userName;
        private string userKey;
        private DisCert microsoftCertificate;

        #endregion

        #region Constructors & Dispose

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="callDirection"></param>
        /// <param name="user"></param>
        public ServiceClient(int? systemId, CallDirection callDirection, User user)
            : this(systemId, callDirection, user, null, null, null) {
        }

        /// <summary>
        /// Constructor for unit test
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="callDirection"></param>
        /// <param name="user"></param>
        /// <param name="configManager"></param>
        /// <param name="hqManager"></param>
        /// <param name="ssManager"></param>
        public ServiceClient(int? systemId, CallDirection callDirection, User user,
            IConfigManager configManager, IHeadQuarterManager hqManager, ISubsidiaryManager ssManager) {
            this.callDirection = callDirection;
            this.systemId = systemId;
            IConfigManager cfgMgr = configManager ?? new ConfigManager();

            ServiceConfig serviceConfig;
            if ((callDirection & CallDirection.Internal) != 0) {
                if (user == null)
                    throw new ApplicationException("Internal web service needs authentication.");
                serviceConfig = cfgMgr.GetInternalServiceConfig();
                serviceConfig.UserName = user.LoginId;
                serviceConfig.UserKey = user.Password;
            }
            else {
                proxySetting = cfgMgr.GetProxySetting();
                if (callDirection == CallDirection.UpLevelSystem) {
                    IHeadQuarterManager hqMgr = hqManager ?? new HeadQuarterManager();
                    HeadQuarter hq = hqMgr.GetHeadQuarter(systemId.Value);
                    serviceConfig = new ServiceConfig() {
                        ServiceHostUrl = hq.ServiceHostUrl,
                        UserName = hq.UserName,
                        UserKey = hq.AccessKey
                    };
                }
                else if (callDirection == CallDirection.DownLevelSystem) {
                    ISubsidiaryManager ssMgr = ssManager ?? new SubsidiaryManager();
                    Subsidiary ss = ssMgr.GetSubsidiary(systemId.Value);
                    serviceConfig = new ServiceConfig() {
                        ServiceHostUrl = ss.ServiceHostUrl,
                        UserName = ss.UserName,
                        UserKey = ss.AccessKey
                    };
                }
                else {
                    serviceConfig = cfgMgr.GetMsServiceConfig();
                    if (systemId == null)
                        microsoftCertificate = cfgMgr.GetCertificateSubject();
                    else {
                        IHeadQuarterManager hqMgr = hqManager ?? new HeadQuarterManager();
                        HeadQuarter hq = hqMgr.GetHeadQuarter(systemId.Value);
                        microsoftCertificate = new DisCert {
                            Subject = hq.CertSubject,
                            ThumbPrint = hq.CertThumbPrint
                        };
                    }
                }
            }
            Initialize(serviceConfig);
        }

        private void Initialize(ServiceConfig serviceConfig) {
            this.helper = new ServiceLocationHelper(serviceConfig.ServiceHostUrl);
            this.userName = serviceConfig.UserName;
            this.userKey = serviceConfig.UserKey;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(CustomCertificateValidation);
        }

        private bool CustomCertificateValidation(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error) {
            if ((error & ~SslPolicyErrors.RemoteCertificateNameMismatch) == SslPolicyErrors.None)
                return true;
            else
                return false;
        }

        #endregion

        /// <summary>
        /// Indicates whether response data should be logged
        /// </summary>
        public bool ShouldLogResponseData { get; set; }

        /// <summary>
        /// Test if internal web service can be connected
        /// </summary>
        public void TestInternal() {
            Request(null, helper.TestInternal, HttpMethod.GET);
        }

        /// <summary>
        /// Test if external web service can be connected
        /// </summary>
        public void TestExternal() {
            Request(null, helper.TestExternal, HttpMethod.GET);
        }

        /// <summary>
        /// Test if Data Polling Service can be connected
        /// </summary>
        public void TestDataPollingService() {
            Request(null, helper.TestDPS, HttpMethod.GET);
        }

        /// <summary>
        /// Test if Key Provider Service can be connected
        /// </summary>
        public void TestKeyProviderService() {
            Request(null, helper.TestKPS, HttpMethod.GET);
        }

        /// <summary>
        /// Data Polling Service report self state
        /// </summary>
        public void DataPollingServiceReport() {
            Request(null, helper.DPSReport, HttpMethod.GET);
        }

        /// <summary>
        /// Key Provider Service report self state
        /// </summary>
        public void KeyProviderServiceReport() {
            Request(null, helper.KPSReport, HttpMethod.GET);
        }

        #region Key Transfer

        /// <summary>
        /// Invoke fulfillments API of Microsoft
        /// </summary>
        /// <returns></returns>
        public List<DC.FulfillmentInfo> GetFulfilments() {
            if ((callDirection & CallDirection.Internal) != 0)
                return Request<List<DC.FulfillmentInfo>>(null, helper.RetrieveFulfilmentUrl, HttpMethod.GET);
            else
                return Request<FulfillmentInfoResponse>(null, helper.RetrieveFulfilmentUrl, HttpMethod.GET).FromServiceContract();
        }

        /// <summary>
        /// Invoke fulfillments API of Microsoft with specified fulfillment number to get keys
        /// </summary>
        /// <param name="fulfillmentId"></param>
        /// <returns></returns>
        public List<KeyInfo> FulfillKeys(string fulfillmentId) {
            if ((callDirection & CallDirection.Internal) != 0)
                return Request<List<KeyInfo>>(fulfillmentId, helper.GetFulfilledKeysUrl, HttpMethod.GET);
            else
                return Request<FulfillmentResponse>(fulfillmentId, helper.GetFulfilledKeysUrl, HttpMethod.GET).FromServiceContract();
        }

        /// <summary>
        /// Invoke computerbuildreport API of Microsoft with CBR data to report
        /// </summary>
        /// <param name="cbr"></param>
        /// <returns></returns>
        public Guid ReportCbr(Cbr cbr) {
            if ((callDirection & CallDirection.Internal) != 0)
                return Request<Guid>(cbr, helper.ReportBindingUrl, HttpMethod.POST);
            else
                return Request<ComputerBuildReportResponse>(cbr.ToBindingReport(), helper.ReportBindingUrl, HttpMethod.POST).ReportUniqueID;
        }

        /// <summary>
        /// Invoke computerbuildreport/acknowledgements API of Microsoft to get a list of available CBR ACKs
        /// </summary>
        /// <returns></returns>
        public Guid[] RetrieveCbrAcks() {
            if ((callDirection & CallDirection.Internal) != 0)
                return Request<Guid[]>(null, helper.CBRAckUrl, HttpMethod.GET);
            else
                return Request<string[]>(null, helper.CBRAckUrl, HttpMethod.GET).Select(i => Guid.Parse(i)).ToArray();
        }

        /// <summary>
        /// Invoke computerbuildreport/acknowledgements API of Microsoft with specified CBR to retrieve its ACK
        /// </summary>
        /// <returns></returns>
        public Cbr RetrieveCbrAck(Cbr cbr) {
            if ((callDirection & CallDirection.Internal) != 0) {
                return Request<Cbr>(cbr, helper.CBRAckUrl, HttpMethod.POST);
            }
            else {
                var response = Request<ComputerBuildReportAckResponse>(cbr.MsReportUniqueId.Value, helper.CBRAckUrl, HttpMethod.GET);
                return response.FromServiceContract();
            }
        }

        /// <summary>
        /// Invoke ReturnReport API of Microsoft with CBR data to report
        /// </summary>
        /// <param name="returnReport"></param>
        /// <returns></returns>
        public Guid ReportReturn(ReturnReport returnReport) {
            if ((callDirection & CallDirection.Internal) != 0)
                return Request<Guid>(returnReport, helper.ReportReturnUrl, HttpMethod.POST);
            else
                return Request<ReturnResponse>(returnReport.ToReturnReport(), helper.ReportReturnUrl, HttpMethod.POST).ReturnUniqueID;
        }

        /// <summary>
        /// Invoke ReturnReport/acknowledgements API of Microsoft to get a list of available ReturnReport ACKs
        /// </summary>
        /// <returns></returns>
        public Guid[] RetrieveReturnReportAcks() {
            if ((callDirection & CallDirection.Internal) != 0)
                return Request<Guid[]>(null, helper.ReturnAckUrl, HttpMethod.GET);
            else
                return Request<string[]>(null, helper.ReturnAckUrl, HttpMethod.GET).Select(i => Guid.Parse(i)).ToArray();
        }

        /// <summary>
        /// Invoke ReturnReport/acknowledgements API of Microsoft with specified CBR to retrieve its ACK
        /// </summary>
        /// <returns></returns>
        public ReturnReport RetrievReturnReportAck(ReturnReport returnReport) {
            if ((callDirection & CallDirection.Internal) != 0) {
                return Request<ReturnReport>(returnReport, helper.ReturnAckUrl, HttpMethod.POST);
            }
            else {
                var response = Request<ReturnAck>(returnReport.ReturnUniqueId, helper.ReturnAckUrl, HttpMethod.GET);
                return response.FromServiceContract();
            }
        }

        /// <summary>
        /// Get keys from up level system
        /// </summary>
        /// <returns></returns>
        public List<KeyInfo> GetKeys() {
            return Request<KeyInfo[]>(null, helper.GetKeysUrl, HttpMethod.GET).ToList();
        }

        /// <summary>
        /// Send sync notification after keys gotten
        /// </summary>
        /// <param name="keys"></param>
        public void SyncKeys(List<KeyInfo> keys) {
            Request(keys.ToArray(), helper.SyncKeysUrl, HttpMethod.POST);
        }

        /// <summary>
        /// Report keys to up level system
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public List<KeyInfo> ReportKeys(List<KeyInfo> keys) {
            return Request<KeyInfo[]>(keys.ToArray(), helper.ReportKeysUrl, HttpMethod.POST).ToList();
        }

        /// <summary>
        /// Recall keys to up level system
        /// </summary>
        /// <param name="keys"></param>
        public void RecallKeys(List<KeyInfo> keys) {
            Request(keys.ToArray(), helper.RecallKeysUrl, HttpMethod.POST);
        }

        /// <summary>
        /// Copy fulfilled keys to up level system which got from Microsoft
        /// </summary>
        /// <param name="keys"></param>
        public void CarbonCopyFulfilledKeys(List<KeyInfo> keys) {
            Request(keys.ToArray(), helper.CarbonCopyFulfilledKeysUrl, HttpMethod.POST);
        }

        /// <summary>
        /// Copy reported keys to up level system which sent to Microsoft
        /// </summary>
        /// <param name="keys"></param>
        public void CarbonCopyReportedKeys(List<KeyInfo> keys) {
            Request(keys.ToArray(), helper.CarbonCopyReportedKeysUrl, HttpMethod.POST);
        }

        /// <summary>
        /// Copy return reported keys to up level system which sent to Microsoft
        /// </summary>
        /// <param name="keys"></param>
        public void CarbonCopyReturnReportedKeys(List<KeyInfo> keys) {
            Request(keys.ToArray(), helper.CarbonCopyReturnReportedKeysUrl, HttpMethod.POST);
        }

        /// <summary>
        /// Copy return report to up level system which sent to Microsoft
        /// </summary>
        /// <param name="keys"></param>
        public void CarbonCopyReturnReport(ReturnReport request)
        {
            Request(request, helper.CarbonCopyReturnReportUrl, HttpMethod.POST);
        }

        /// <summary>
        /// Sync keys to down level system
        /// </summary>
        /// <param name="syncs"></param>
        public long[] SendKeySyncNotifications(List<KeySyncNotification> syncs) {
            return Request<long[]>(syncs.ToArray(), helper.SyncUrl, HttpMethod.POST);
        }

        #endregion

        #region Private & protected methods

        private string Request(object request, string url, HttpMethod method) {
            HttpResponseMessage response = null;
            string postData = null;
            using (var webClient = new HttpClient()) {
                webClient.TransportSettings.ConnectionTimeout = new TimeSpan(0, timeoutMinutes, 0);

                // set credentials
                if (callDirection == CallDirection.Microsoft) {
                    if (microsoftCertificate == null)
                        throw new ApplicationException("MicrosoftCertificate is null.");
                    webClient.TransportSettings.ClientCertificates.Add(EncryptionHelper.GetCertificate(
                        StoreName.My, StoreLocation.CurrentUser, X509FindType.FindByThumbprint, microsoftCertificate.ThumbPrint));
                }
                else {
                    webClient.DefaultHeaders.Add(AuthorizationHeaderName, GetAuthHeader(userName, userKey));
                    webClient.DefaultHeaders.Add(DirectionHeaderName, ((callDirection & ~CallDirection.Internal)).ToString());
                    if ((callDirection & CallDirection.Internal) != 0)
                        webClient.DefaultHeaders.Add(SystemIdHeaderName, systemId == null ? string.Empty : systemId.ToString());
                }

                if (proxySetting != null) {
                    IWebProxy proxy = null;
                    if (proxySetting.ProxyType == ProxyType.Custom) {
                        proxy = new WebProxy(proxySetting.ServiceConfig.ServiceHostUrl,
                            proxySetting.BypassProxyOnLocal);
                        proxy.Credentials = new NetworkCredential(proxySetting.ServiceConfig.UserName,
                            proxySetting.ServiceConfig.UserKey);
                    }
                    else if (proxySetting.ProxyType == ProxyType.Default)
                        proxy = WebRequest.GetSystemWebProxy();
                    webClient.TransportSettings.Proxy = proxy;
                }

                // send request
                if (method == HttpMethod.GET) {
                    if (request != null && request.ToString().Length > 0)
                        url += string.Format("/{0}", request.ToString());
                    response = webClient.Get(url);
                }
                else {
                    postData = request.ToDataContract();
                    using (HttpContent content = HttpContent.Create(postData, Encoding.UTF8, contentType)) {
                        response = webClient.Send(method, url, content);
                    }
                }
            }
            CheckStatusCode(response);
            string responseData = response.Content.ReadAsString();
            if (ShouldLogResponseData)
                Log(url, method.ToString(), contentType, postData,
                    response.StatusCode.ToString(), responseData, response.Content.ContentType);
            return responseData;
        }

        private T Request<T>(object request, string url, HttpMethod method) {
            return Request(request, url, method).FromDataContract<T>();
        }

        private void Log(string url, string httpMethod, string contentType, string postData,
            string responseCode, string responseData, string responseType) {
            string message = string.Format("URL: {1}{0}Http Method: {2}{0}Request Type: {3}{0}Post Data: {4}{0}Response Code: {5}{0}Response Data: {6}{0}Response Type: {7}{0}",
                Environment.NewLine + Environment.NewLine, url, httpMethod, contentType, postData, responseCode, responseData, responseType);
            MessageLogger.LogSystemRunning("Service Traffic", message);
        }

        private void CheckStatusCode(HttpResponseMessage response) {
            switch (response.StatusCode) {
                case HttpStatusCode.OK:
                    return;
                case HttpStatusCode.BadRequest:
                    throw new WebProtocolException(response.StatusCode,
                        response.Content.ReadAsString(), null);
                case HttpStatusCode.InternalServerError:
                    throw new WebProtocolException(response.StatusCode,
                        response.Content.ReadAsString(), null);
                case HttpStatusCode.Unauthorized:
                    throw new WebProtocolException(response.StatusCode,
                        string.Format("You have no permission to access {0}.", response.Uri), null);
                default:
                    throw new WebProtocolException(response.StatusCode,
                        string.Format("\"{0}\" returned from {1}.", response.StatusCode, response.Uri), null);
            }
        }

        private string GetAuthHeader(string username, string password) {
            var encodedCred = Convert.ToBase64String(Encoding.UTF8.GetBytes(
                string.Format("{0}:{1}", username, password)));
            return string.Format("Basic {0}", encodedCred);
        }

        #endregion
    }
}

