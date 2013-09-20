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
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using DC = DIS.Data.DataContract;

namespace DIS.Data.ServiceContract
{
    /// <summary>
    /// This is ExterndedMethods class for ServiceClient layer  
    /// </summary>
    public static class TypeConversionExtension
    {
        public static List<DC.FulfillmentInfo> FromServiceContract(this FulfillmentInfoResponse response)
        {
            List<DC.FulfillmentInfo> fulfillmentInfoes = new List<DC.FulfillmentInfo>();
            response.FulfillmentInfos.ToList().ForEach(info => fulfillmentInfoes.Add(new DC.FulfillmentInfo()
            {
                FulfillmentNumber = info.FulfillmentNumber,
                OrderUniqueId = info.OrderUniqueID,
                SoldToCustomerId = info.SoldToCustomerID,
                FulfillmentStatus = DC.FulfillmentStatus.Ready,
            }));
            return fulfillmentInfoes;
        }

        /// <summary>
        /// Extension method to convert Tranfer Key DataContract to BindingReportRequest
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static ComputerBuildReportRequest ToBindingReport(this DC.Cbr cbr)
        {
            return new ComputerBuildReportRequest()
            {
                CustomerReportUniqueID = cbr.CbrUniqueId,
                SoldToCustomerID = cbr.SoldToCustomerId,
                ReceivedFromCustomerID = cbr.ReceivedFromCustomerId,
                TotalLineItems = cbr.CbrKeys.Count(),
                Bindings = (from k in cbr.CbrKeys
                            let info = new DC.OemOptionalInfo(k.OemOptionalInfo)
                            select new Binding()
                            {
                                ProductKeyID = k.KeyId,
                                HardwareHash = k.HardwareHash,
                                OEMOptionalInfo = info.Values.Any(p => !string.IsNullOrEmpty(p.Value)) ?
                                    info.Values.Where(p => !string.IsNullOrEmpty(p.Value)).Select(p => new OEMOptionalInfoExtendedProperty()
                                    {
                                        Name = p.Key,
                                        Value = p.Value
                                    }).ToArray() : null
                            }).ToArray(),
            };
        }

        public static ReturnRequest ToReturnReport(this DC.ReturnReport returnReport)
        {
            return new ReturnRequest()
            {
                OEMRMADate = returnReport.OemRmaDate,
                OEMRMANumber = returnReport.OemRmaNumber,
                ReturnNoCredit = returnReport.ReturnNoCredit,
                SoldToCustomerID = returnReport.SoldToCustomerId,
                ReturnLineItems = (from reportKey in returnReport.ReturnReportKeys
                                   select new ReturnLineItem()
                                   {
                                       ProductKeyID = reportKey.KeyId,
                                       ReturnTypeID = reportKey.ReturnTypeId,
                                       OEMRMALineNumber = reportKey.OemRmaLineNumber
                                   }
                                   ).ToArray(),
            };   
        }

        public static DC.Cbr FromServiceContract(this ComputerBuildReportAckResponse response)
        {
            ComputerBuildReportAck ack = response.ComputerBuildReportAcks.Single();
            return ack.FromServiceContract();
        }

        //offline CBR.ack
        public static DC.Cbr FromServiceContract(this ComputerBuildReportAck ack)
        {
            var cbr = new DC.Cbr()
            {
                MsReportUniqueId = ack.MSReportUniqueID,
                CbrUniqueId = ack.CustomerReportUniqueID,
                SoldToCustomerId = ack.SoldToCustomerID,
                ReceivedFromCustomerId = ack.ReceivedFromCustomerID,
                MsReceivedDateUtc = ack.MSReceivedDateUTC,
                CbrAckFileTotal = ack.CBRAckFileTotal,
                CbrAckFileNumber = ack.CBRAckFileNumber,
                CbrKeys = new List<DC.CbrKey>(),
            };

            ack.SuccessfulValidations.ToList().ForEach(r =>
                cbr.CbrKeys.Add(new DC.CbrKey()
                {
                    CustomerReportUniqueId = cbr.CbrUniqueId,
                    KeyId = r.ProductKeyID,
                    HardwareHash = r.HardwareHash,
                    ReasonCode = DC.Constants.CBRAckReasonCode.ActivationEnabled,
                }));

            ack.FailedValidations.ToList().ForEach(r =>
                cbr.CbrKeys.Add(new DC.CbrKey()
                {
                    CustomerReportUniqueId = cbr.CbrUniqueId,
                    KeyId = r.ProductKeyID,
                    HardwareHash = r.HardwareHash,
                    ReasonCode = r.ReasonCode,
                    ReasonCodeDescription = r.ReasonCodeDescription,
                }));

            return cbr;
        }

        /// <summary>
        /// Converts from Fulfillment ServiceContract to KeyInformation DataContract
        /// </summary>
        /// <param name="fulfillment"></param>
        /// <returns></returns>
        public static List<DC.KeyInfo> FromServiceContract(this FulfillmentResponse response)
        {
            List<DC.KeyInfo> keys = new List<DC.KeyInfo>();
            foreach (KeyFulfillment fulfillment in response.Fulfillments)
            {
                keys.AddRange(fulfillment.Keys.Select(k => new DC.KeyInfo
                {
                    SoldToCustomerName = fulfillment.SoldToCustomerName,
                    OrderUniqueId = fulfillment.OrderUniqueID,
                    OemPoNumber = fulfillment.OEMPONumber,
                    SoldToCustomerId = fulfillment.SoldToCustomerID,
                    OemPartNumber = fulfillment.OEMPartNumber,
                    CallOffReferenceNumber = fulfillment.CallOffReferenceNumber,
                    MsOrderNumber = fulfillment.MSOrderNumber,
                    MsOrderLineNumber = fulfillment.MSOrderLineNumber,
                    LicensablePartNumber = fulfillment.LicensablePartNumber,
                    Quantity = fulfillment.Quantity,
                    OemPoDateUtc = fulfillment.OEMPODateUTC,
                    ShipToCustomerId = fulfillment.ShipToCustomerID,
                    ShipToCustomerName = fulfillment.ShipToCustomerName,
                    LicensableName = fulfillment.LicensableName,
                    OemPoLineNumber = fulfillment.OEMPOLineNumber,
                    CallOffLineNumber = fulfillment.CallOffLineNumber,
                    FulfillmentResendIndicator = fulfillment.FulfillmentResendIndicator,
                    FulfillmentNumber = fulfillment.FulfillmentNumber,
                    FulfilledDateUtc = fulfillment.FulfilledDateUTC,
                    FulfillmentCreateDateUtc = fulfillment.FulfillmentCreateDateUTC,
                    KeyId = k.ProductKeyID,
                    ProductKey = k.ProductKey,
                    SkuId = k.SKUID,
                    OemOptionalInfo = new DC.OemOptionalInfo(),
                }));
            }
            return keys;
        }

        public static DC.Cbr FromServiceContract(this ComputerBuildReportRequest request)
        {
            var cbr = new DC.Cbr()
            {
                CbrUniqueId = request.CustomerReportUniqueID,
                SoldToCustomerId = request.SoldToCustomerID,
                ReceivedFromCustomerId = request.ReceivedFromCustomerID,
                CbrKeys = request.Bindings.Select(b=>b.FromServiceContract()).ToList(),
            };

            return cbr;
        }

        public static DC.CbrKey FromServiceContract(this Binding binding)
        {
            string strOptionInfo = string.Empty;
            if (binding.OEMOptionalInfo != null && binding.OEMOptionalInfo.Length > 0)
            {
                DC.OemOptionalInfo oemOptionInfo = new DC.OemOptionalInfo();
                if (binding.OEMOptionalInfo.SingleOrDefault(r => r.Name == "ZPC_MODEL_SKU") != null)
                    oemOptionInfo.ZPC_MODEL_SKU = binding.OEMOptionalInfo.SingleOrDefault(r => r.Name == "ZPC_MODEL_SKU").Value;
                if (binding.OEMOptionalInfo.SingleOrDefault(r => r.Name == "ZOEM_EXT_ID") != null)
                    oemOptionInfo.ZOEM_EXT_ID = binding.OEMOptionalInfo.SingleOrDefault(r => r.Name == "ZOEM_EXT_ID").Value;
                if (binding.OEMOptionalInfo.SingleOrDefault(r => r.Name == "ZMANUF_GEO_LOC") != null)
                    oemOptionInfo.ZMANUF_GEO_LOC = binding.OEMOptionalInfo.SingleOrDefault(r => r.Name == "ZMANUF_GEO_LOC").Value;
                if (binding.OEMOptionalInfo.SingleOrDefault(r => r.Name == "ZPGM_ELIG_VAL") != null)
                    oemOptionInfo.ZPGM_ELIG_VALUES = binding.OEMOptionalInfo.SingleOrDefault(r => r.Name == "ZPGM_ELIG_VAL").Value;
                if (binding.OEMOptionalInfo.SingleOrDefault(r => r.Name == "ZCHANNEL_REL_ID") != null)
                    oemOptionInfo.ZCHANNEL_REL_ID = binding.OEMOptionalInfo.SingleOrDefault(r => r.Name == "ZCHANNEL_REL_ID").Value;
                strOptionInfo = oemOptionInfo.ToString();
            }
            return new DC.CbrKey()
            {
                KeyId = binding.ProductKeyID,
                HardwareHash = binding.HardwareHash,
                OemOptionalInfo = strOptionInfo
            };
        }

        public static DC.ReturnReport FromServiceContract(this ReturnAck response)
        {
            DC.ReturnReport reportReturn = new DC.ReturnReport();

            reportReturn.ReturnUniqueId = response.ReturnUniqueID;
            reportReturn.MsReturnNumber = response.MSReturnNumber;
            reportReturn.OemRmaDateUTC = response.OEMRMADateUTC;
            reportReturn.OemRmaNumber = response.OEMRMANumber;
            reportReturn.ReturnDateUTC = response.ReturnDateUTC;
            reportReturn.SoldToCustomerId = response.SoldToCustomerID;
            reportReturn.SoldToCustomerName = response.SoldToCustomerName;

            reportReturn.ReturnReportKeys.AddRange(response.ReturnAckLineItems.Select(k => new DC.ReturnReportKey
            {
                MsReturnLineNumber = k.MSReturnLineNumber,
                OemRmaLineNumber = k.OEMRMALineNumber,
                ReturnTypeId = k.ReturnTypeID,
                KeyId = k.ProductKeyID,
                LicensablePartNumber = k.LicensablePartNumber,
                ReturnReasonCode = k.ReturnReasonCode,
                ReturnReasonCodeDescription = k.ReturnReasonCodeDescription
            }));

            return reportReturn;
        }
    }
}
