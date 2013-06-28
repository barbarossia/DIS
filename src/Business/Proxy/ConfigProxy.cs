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
using System.Linq;
using System.Text;
using DIS.Business.Client;
using DIS.Business.Library;
using DIS.Data.DataContract;
using Microsoft.ServiceModel.Web;
using DIS.Common.Utility;

namespace DIS.Business.Proxy
{
    public class ConfigProxy : ConfigManager, IConfigProxy
    {
        private IFulfillmentManager fulfillmentManager;
        private User user;

        public ConfigProxy(User user)
            : this(user, null)
        { }

        public ConfigProxy(User user, IFulfillmentManager fulfillmentManager)
        {
            this.user = user;

            if (fulfillmentManager == null)
                this.fulfillmentManager = new FulfillmentManager();
            else
                this.fulfillmentManager = fulfillmentManager;
        }

        #region Diagnostic

        public DiagnosticResult TestInternalConnection()
        {
            ServiceClient internalClient = new ServiceClient(null, CallDirection.Internal, user);
            return DiagnosticConnectionState(() => internalClient.TestInternal());
        }

        public DiagnosticResult TestUpLevelSystemConnection(int hqId)
        {
            ServiceClient client = new ServiceClient(hqId, CallDirection.Internal | CallDirection.UpLevelSystem, user);
            return DiagnosticConnectionState(() => client.TestExternal());
        }

        public DiagnosticResult TestDownLevelSystemConnection(int ssId)
        {
            ServiceClient client = new ServiceClient(ssId, CallDirection.Internal | CallDirection.DownLevelSystem, user);
            return DiagnosticConnectionState(() => client.TestExternal());
        }

        public DiagnosticResult TestDataPollingService()
        {
            ServiceClient internalClient = new ServiceClient(null, CallDirection.Internal, user);
            return DiagnosticConnectionState(() => internalClient.TestDataPollingService());
        }

        public DiagnosticResult TestKeyProviderService()
        {
            ServiceClient internalClient = new ServiceClient(null, CallDirection.Internal, user);
            return DiagnosticConnectionState(() => internalClient.TestKeyProviderService());
        }

        public DiagnosticResult TestMsConnection(int? hqId)
        {
            ServiceClient client = new ServiceClient(hqId, CallDirection.Internal | CallDirection.Microsoft, user);
            return DiagnosticConnectionState(() =>
                {
                    List<FulfillmentInfo> fulfillments = client.GetFulfilments();
                    if (fulfillments.Count > 0)
                        fulfillmentManager.SaveAvailableFulfillments(fulfillments);
                });
        }

        public void DataPollingServiceReport()
        {
            ServiceClient internalClient = new ServiceClient(null, CallDirection.Internal, user);
            internalClient.DataPollingServiceReport();
        }

        public void KeyProviderServiceReport()
        {
            ServiceClient internalClient = new ServiceClient(null, CallDirection.Internal, user);
            internalClient.KeyProviderServiceReport();
        }

        #endregion

        private DiagnosticResult DiagnosticConnectionState(Action action)
        {
            DiagnosticResult diagnosticResult = new DiagnosticResult();
            try
            {
                action();
                diagnosticResult.DiagnosticResultType = DiagnosticResultType.Ok;
            }
            catch (Exception ex)
            {
                diagnosticResult.DiagnosticResultType = DiagnosticResultType.Error;
                diagnosticResult.Exception = ex;
            }
            return diagnosticResult;
        }
    }
}

