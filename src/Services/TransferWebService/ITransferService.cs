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
using System.ServiceModel;
using System.ServiceModel.Web;
using DIS.Data.DataContract;

namespace DIS.Services.TransferWebService
{
    [ServiceContract]
    public interface ITransferService
    {
        #region Microsoft web services

        [OperationContract]
        [WebGet(UriTemplate = "/fulfillments/?status=ready")]
        List<FulfillmentInfo> GetFulfillments();

        [OperationContract]
        [WebGet(UriTemplate = "/fulfillments/{fulfillmentid}")]
        List<KeyInfo> FulfillKeys(string fulfillmentId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/computerbuildreport/", Method = "POST")]
        Guid ReportCbr(Cbr request);

        [OperationContract]
        [WebGet(UriTemplate = "/computerbuildreport/acknowledgements")]
        Guid[] RetrieveCbrAcks();

        [OperationContract]
        [WebInvoke(UriTemplate = "/computerbuildreport/acknowledgements", Method = "POST")]
        Cbr RetrieveCbrAck(Cbr cbr);

        [OperationContract]
        [WebInvoke(UriTemplate = "/return/", Method = "POST")]
        Guid ReportReturn(ReturnReport request);

        [OperationContract]
        [WebGet(UriTemplate = "/return/acknowledgements")]
        Guid[] RetrieveReturnAcks();

        [OperationContract]
        [WebInvoke(UriTemplate = "/return/acknowledgements", Method = "POST")]
        ReturnReport RetrieveReturnAck(ReturnReport request);

        #endregion

        #region ULS/DLS web services

        [OperationContract]
        [WebGet(UriTemplate = "/Diagnostic/Internal")]
        void TestInternal();

        [OperationContract]
        [WebGet(UriTemplate = "/Diagnostic/External")]
        void TestExternal();

        [OperationContract]
        [WebGet(UriTemplate = "/Diagnostic/DPS")]
        void TestDataPollingService();

        [OperationContract]
        [WebGet(UriTemplate = "/Diagnostic/KPS")]
        void TestKeyProviderService();

        [OperationContract]
        [WebGet(UriTemplate = "/Diagnostic/DPS/Report")]
        void DataPollingServiceReport();

        [OperationContract]
        [WebGet(UriTemplate = "/Diagnostic/KPS/Report")]
        void KeyProviderServiceReport();


        [OperationContract]
        [WebGet(UriTemplate = "/Keys/Get")]
        List<KeyInfo> GetKeys();

        // Sync keys between OEM/TPI, OEM/FF and TPI/FF.
        [OperationContract]
        [WebInvoke(UriTemplate = "/Keys/Sync", Method = "POST")]
        void SyncKeys(List<KeyInfo> request);
                
        [OperationContract]
        [WebInvoke(UriTemplate = "/Keys/Report", Method = "POST")]
        List<KeyInfo> ReportKeys(List<KeyInfo> request);

        [OperationContract]
        [WebInvoke(UriTemplate = "/Keys/Recall", Method = "POST")]
        void RecallKeys(List<KeyInfo> request);

        [OperationContract]
        [WebInvoke(UriTemplate = "/Keys/CarbonCopy/Fulfilled", Method = "POST")]
        void CarbonCopyFulfilledKeys(List<KeyInfo> request);

        [OperationContract]
        [WebInvoke(UriTemplate = "/Keys/CarbonCopy/Reported", Method = "POST")]
        void CarbonCopyReportedKeys(List<KeyInfo> request);

        [OperationContract]
        [WebInvoke(UriTemplate = "/Keys/CarbonCopy/Returned", Method = "POST")]
        void CarbonCopyReturnReportedKeys(List<KeyInfo> request);

        [OperationContract]
        [WebInvoke(UriTemplate = "/Keys/CarbonCopy/ReturnReport", Method = "POST")]
        void CarbonCopyReturnReport(ReturnReport request);

        [OperationContract]
        [WebInvoke(UriTemplate = "/Sync", Method = "POST")]
        long[] SendKeySyncNotifications(List<KeySyncNotification> request);

        #endregion
    }
}
