//*********************************************************
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
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using DIS.Business.Client;
using DIS.Business.Proxy;
using DIS.Common.Utility;
using DIS.Data.DataContract;
using DIS.Services.WebServiceLibrary;
using Microsoft.ServiceModel.Web;

namespace DIS.Services.TransferWebService
{
    [ServiceBehavior(MaxItemsInObjectGraph = int.MaxValue)]
    public class TransferService : ServiceBase, ITransferService
    {
        private const int reportCheckInterval = Constants.PulseInterval * 2;
        private IServiceClient serviceClient;

        public TransferService()
        {
            try
            {
                string systemId = GetRequestHeader(ServiceClient.SystemIdHeaderName);
                CallDirection callDirection = (CallDirection)Enum.Parse(typeof(CallDirection),
                    GetRequestHeader(ServiceClient.DirectionHeaderName));
                if (callDirection != CallDirection.None)
                    serviceClient = new ServiceClient(string.IsNullOrEmpty(systemId) ? null :
                        (int?)int.Parse(systemId), callDirection, null) { ShouldLogResponseData = true };
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                throw;
            }
        }

        #region Microsoft web services

        public List<FulfillmentInfo> GetFulfillments()
        {
            return HandleException<List<FulfillmentInfo>>(MessageLogger.GetMethodName(), () =>
                serviceClient.GetFulfilments());
        }

        public List<KeyInfo> FulfillKeys(string fulfillmentId)
        {
            return HandleException<List<KeyInfo>>(MessageLogger.GetMethodName(), () =>
                serviceClient.FulfillKeys(fulfillmentId));
        }

        public Guid ReportCbr(Cbr request)
        {
            return HandleException<Guid>(MessageLogger.GetMethodName(), () =>
                serviceClient.ReportCbr(request));
        }

        public Cbr SearchSubmittedCbr(Cbr cbr)
        {
            return HandleException<Cbr>(MessageLogger.GetMethodName(), () =>
                serviceClient.SearchSubmittedCbr(cbr));
        }

        public Guid[] RetrieveCbrAcks()
        {
            return HandleException<Guid[]>(MessageLogger.GetMethodName(), () =>
                serviceClient.RetrieveCbrAcks());
        }

        public Cbr RetrieveCbrAck(Cbr cbr)
        {
            return HandleException<Cbr>(MessageLogger.GetMethodName(), () => 
                serviceClient.RetrieveCbrAck(cbr));
        }

        public Guid ReportReturn(ReturnReport request)
        {
            return HandleException<Guid>(MessageLogger.GetMethodName(), () =>
                serviceClient.ReportReturn(request));
        }

        public ReturnReport SearchSubmittedReturn(ReturnReport returnReport)
        {
            return HandleException<ReturnReport>(MessageLogger.GetMethodName(), () =>
                serviceClient.SearchSubmittedReturn(returnReport));
        }

        public Guid[] RetrieveReturnAcks()
        {
            return HandleException<Guid[]>(MessageLogger.GetMethodName(), () =>
                serviceClient.RetrieveReturnReportAcks());
        }

        public ReturnReport RetrieveReturnAck(ReturnReport request)
        {
            return HandleException<ReturnReport>(MessageLogger.GetMethodName(), () =>
                serviceClient.RetrievReturnReportAck(request));
        }

        #endregion

        #region ULS web services

        public void TestInternal()
        {
        }

        public void TestExternal()
        {
            HandleException(MessageLogger.GetMethodName(), () =>
            {
                serviceClient.TestExternal();
            });
        }

        public void TestDataPollingService()
        {
            CalculatePeriodOfTime(Global.DpsLastReportTime);
        }

        public void TestKeyProviderService()
        {
            CalculatePeriodOfTime(Global.KpsLastReportTime);
        }

        public void DataPollingServiceReport()
        {
            Global.DpsLastReportTime = DateTime.Now;
        }

        public void KeyProviderServiceReport()
        {
            Global.KpsLastReportTime = DateTime.Now;
        }

        /// <summary>
        /// Get Product Keys from OEM's
        /// </summary>
        /// <param name="shipToCustomNumber">Identity the TPI</param>
        public List<KeyInfo> GetKeys()
        {
            return HandleException<List<KeyInfo>>(MessageLogger.GetMethodName(), () =>
                serviceClient.GetKeys());
        }

        public void SyncKeys(List<KeyInfo> request)
        {
            HandleException(MessageLogger.GetMethodName(), () =>
            {
                serviceClient.SyncKeys(request);
            });
        }

        public List<KeyInfo> ReportKeys(List<KeyInfo> request)
        {
            return HandleException<List<KeyInfo>>(MessageLogger.GetMethodName(), () =>
                serviceClient.ReportKeys(request));
        }

        public void RecallKeys(List<KeyInfo> request)
        {
            HandleException(MessageLogger.GetMethodName(), () =>
            {
                serviceClient.RecallKeys(request);
            });
        }

        public void CarbonCopyFulfilledKeys(List<KeyInfo> request)
        {
            HandleException(MessageLogger.GetMethodName(), () =>
            {
                serviceClient.CarbonCopyFulfilledKeys(request);
            });
        }

        public void CarbonCopyReportedKeys(List<KeyInfo> request)
        {
            HandleException(MessageLogger.GetMethodName(), () =>
            {
                serviceClient.CarbonCopyReportedKeys(request);
            });
        }

        public void CarbonCopyReturnReportedKeys(List<KeyInfo> request)
        {
            HandleException(MessageLogger.GetMethodName(), () =>
            {
                serviceClient.CarbonCopyReturnReportedKeys(request);
            });
        }

        public void CarbonCopyReturnReport(ReturnReport request)
        {
            HandleException(MessageLogger.GetMethodName(), () =>
            {
                serviceClient.CarbonCopyReturnReport(request);
            });
        }

        public long[] SendKeySyncNotifications(List<KeySyncNotification> request)
        {
            return HandleException<long[]>(MessageLogger.GetMethodName(), () =>
                serviceClient.SendKeySyncNotifications(request));
        }

        #endregion

        private void HandleException(string methodName, Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                if (ex is WebProtocolException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = ((WebProtocolException)ex).StatusCode;
                    throw;
                }
                else
                    throw new WebProtocolException(HttpStatusCode.InternalServerError, methodName, ex);
            }
        }

        private T HandleException<T>(string methodName, Func<T> func)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                if (ex is WebProtocolException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = ((WebProtocolException)ex).StatusCode;
                    throw;
                }
                else
                    throw new WebProtocolException(HttpStatusCode.InternalServerError, methodName, ex);
            }
        }

        private void CalculatePeriodOfTime(DateTime period)
        {
            TimeSpan elplseTime = DateTime.Now.Subtract(period);
            if (elplseTime.TotalMilliseconds > reportCheckInterval)
                throw new WebProtocolException(HttpStatusCode.InternalServerError);
        }
    }

    public static class Global
    {
        /// <summary>
        /// Global variable storing important stuff.
        /// </summary>
        static DateTime dpsLastReportTime;
        static DateTime kpsLastReportTime;

        /// <summary>
        /// Get or set the static important data.
        /// </summary>
        public static DateTime DpsLastReportTime
        {
            get
            {
                return dpsLastReportTime;
            }
            set
            {
                dpsLastReportTime = value;
            }
        }

        /// <summary>
        /// Get or set the static important data.
        /// </summary>
        public static DateTime KpsLastReportTime
        {
            get
            {
                return kpsLastReportTime;
            }
            set
            {
                kpsLastReportTime = value;
            }
        }
    }
}
