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
using System.Text;
using DIS.Data.DataContract;
using DIS.Common.Utility;
using Microsoft.ServiceModel.Web;
using DIS.Data.DataAccess;

namespace DIS.Business.Proxy
{
    public partial class KeyProxy
    {
        //re-send return failed keys in the background
        public void SendGeneratedReturnReports()
        {
            base.SendReturnReports((l) => msClient.ReportReturn(l));
        }

        //get Return.Ack from Ms 
        public void RetrieveReturnReportAcks()
        {
            List<ReturnReport> returnReports = GetReportedReturnReports()
                .Where(c => c.ReturnUniqueId.HasValue).ToList();
            if (returnReports.Count > 0)
            {
                Guid[] readyReturnReportIds = msClient.RetrieveReturnReportAcks();
                returnReports = returnReports.Where(c => readyReturnReportIds.Contains(c.ReturnUniqueId.Value))
                    .Union(GetFailedReturnReports()).ToList();
                UpdateReturnsAfterAckReady(returnReports);
            }
            returnReports = GetReadyReturnReports();
            foreach (var returnReport in returnReports)
            {
                try
                {
                    ReturnReport returnWithAck = msClient.RetrievReturnReportAck(returnReport);
                    using (KeyStoreContext context = KeyStoreContext.GetContext())
                    {
                        ReturnReport dbReturnReport = UpdateReturnAfterAckRetrieved(returnWithAck, context);
                        var result = base.UpdateKeysAfterRetrieveReturnReportAck(dbReturnReport, context);
                        if (GetIsCarbonCopy())
                            base.UpdateKeysToCarbonCopy(result.Where(r => !r.Failed && r.KeyInDb.KeyState == KeyState.Returned).Select(r => r.Key).ToList(), true, context);
                        context.SaveChanges();
                    }
                }
                catch (WebProtocolException)
                {
                    UpdateReturnWhenAckFailed(returnReport);
                }
            }
        }
    }
}
