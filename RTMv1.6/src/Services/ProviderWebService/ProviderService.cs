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
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using DIS.Business.Client;
using DIS.Business.Proxy;
using DIS.Common.Utility;
using DIS.Data.DataContract;
using DIS.Services.WebServiceLibrary;
using DIS.Services.WebServiceLibrary.IdentityModel;
using Microsoft.ServiceModel.Web;

namespace DIS.Services.ProviderWebService
{
    [ServiceBehavior(MaxItemsInObjectGraph = int.MaxValue)]
    public class ProviderService : ServiceBase, IProviderService
    {
        private DisIdentity identity;
        private IConfigProxy cfgProxy;
        private IKeyProxy keyProxy;
        private ISubsidiaryProxy ssProxy;

        public ProviderService()
        {
            identity = GetIdentity();
            cfgProxy = new ConfigProxy(null);
            var defaultHeadQuarter = new HeadQuarterProxy().GetHeadQuarters().FirstOrDefault();
            if (defaultHeadQuarter != null)
            {
                keyProxy = new KeyProxy(null, defaultHeadQuarter.HeadQuarterId);
            }
            else
            {
                keyProxy = new KeyProxy(null, null);
            }
            ssProxy = new SubsidiaryProxy();
        }

        public void TestExternal()
        {
        }

        /// <summary>
        /// FKI/FFKI gets keys from CKI.
        /// FFKI gets keys from FKI.
        /// </summary>       
        public List<KeyInfo> GetKeys()
        {
            return HandleException<List<KeyInfo>>(MessageLogger.GetMethodName(), () =>
                keyProxy.GetAssignedKeys(ssProxy.GetSubsidiary(identity.DlsName).SsId).ToList()); 
        }

        /// <summary>
        /// FKI/FFKI tells CKI it has received the keys successfully.
        /// FFKI tells FKI it has received the keys successfully.
        /// </summary>
        /// <param name="request"></param>
        public void SyncKeys(List<KeyInfo> request)
        {
            HandleException(MessageLogger.GetMethodName(), () =>
            {
                keyProxy.ReceiveSyncNotification(request);
            });
        }

        /// <summary>
        /// FKI/FFKI reports bound keys to CKI.
        /// FFKI reports bound keys to FKI.
        /// </summary>
        /// <param name="request"></param>
        public List<KeyInfo> ReportKeys(List<KeyInfo> request)
        {
            return HandleException<List<KeyInfo>>(MessageLogger.GetMethodName(), () =>
                keyProxy.ReceiveBoundKeys(request, ssProxy.GetSubsidiary(identity.DlsName).SsId)
                    .Where(r => r.Failed).Select(r => r.Key).ToList()); 
        }

        /// <summary>
        /// FKI/FFKI recalls keys to CKI.
        /// FFKI recalls keys to FKI.
        /// </summary>
        /// <param name="request"></param>
        public void RecallKeys(List<KeyInfo> request)
        {
            HandleException(MessageLogger.GetMethodName(), () =>
            {
                keyProxy.ReceiveKeysForRecalling(request, ssProxy.GetSubsidiary(identity.DlsName).SsId);
            });
        }

        /// <summary>
        /// FKI carbon copies fulfilled keys to CKI in Decentralized Mode.
        /// </summary>
        /// <param name="request"></param>
        public void CarbonCopyFulfilledKeys(List<KeyInfo> request)
        {
            HandleException(MessageLogger.GetMethodName(), () =>
            {
                keyProxy.GetAndSaveCarbonCopyFulfilledKeys(request, ssProxy.GetSubsidiary(identity.DlsName).SsId);
            });
        }

        /// <summary>
        /// FKI carbon copies ActivationEnabled/ActivatinDenied keys to CKI in Decentralized Mode.
        /// </summary>
        /// <param name="request"></param>
        public void CarbonCopyReportedKeys(List<KeyInfo> request)
        {
            HandleException(MessageLogger.GetMethodName(), () =>
            {
                keyProxy.GetAndSaveCarbonCopyReportedKeys(request);
            });
        }

        /// <summary>
        /// FKI carbon copies returned keys to CKI in Decentralized Mode.
        /// </summary>
        /// <param name="request"></param>
        public void CarbonCopyReturnReportedKeys(List<KeyInfo> request)
        {
            HandleException(MessageLogger.GetMethodName(), () =>
            {
                keyProxy.GetAndSaveCarbonCopyReturnReportedKeys(request);
            });
        }

        /// <summary>
        /// FKI carbon copies return report to CKI in Decentralized Mode.
        /// </summary>
        /// <param name="request"></param>
        public void CarbonCopyReturnReport(ReturnReport request)
        {
            HandleException(MessageLogger.GetMethodName(), () =>
            {
                keyProxy.GetAndSaveCarbonCopyReturnReport(request);
            });
        }

        /// <summary>
        /// CKI syncs keys state to FKI/FFKI.
        /// FKI syncs keys state to FFKI.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public long[] UpdateKeyStateAfterRecieveSyncNotification(List<KeySyncNotification> request)
        {
            return HandleException<long[]>(MessageLogger.GetMethodName(),() => 
                keyProxy.UpdateKeyStateAfterRecieveSyncNotification(request));
        }

        private void HandleException(string methodName, Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                throw new WebProtocolException(HttpStatusCode.InternalServerError, MessageLogger.GetMethodName(), ex);
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
                throw new WebProtocolException(HttpStatusCode.InternalServerError, MessageLogger.GetMethodName(), ex);
            }
        }

        private DisIdentity GetIdentity() {
            ServiceSecurityContext ssContext = ServiceSecurityContext.Current;
            if (ssContext != null)
                return ssContext.PrimaryIdentity as DisIdentity;
            else
                return null;
        }
    }
}