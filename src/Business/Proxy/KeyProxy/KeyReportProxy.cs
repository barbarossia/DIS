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
using DIS.Common.Utility;
using DIS.Data.DataContract;
using Microsoft.ServiceModel.Web;
using DIS.Business.Library;
using DIS.Data.DataAccess;

namespace DIS.Business.Proxy
{
    public partial class KeyProxy
    {
        #region Report keys

        public List<KeyOperationResult> SendKeysForRecalling(List<KeyInfo> keys)
        {
            return base.SendKeysForRecalling(keys,
                k => ulsClient.RecallKeys(keys));
        }

        public List<KeyOperationResult> SendKeysForRecalling(List<KeyGroup> groupKeys)
        {
            List<KeyInfo> keys = base.SearchRecallKeys(groupKeys);
            return SendKeysForRecalling(keys);
        }

        //report keys to ULS or MS
        public List<KeyOperationResult> SendBoundKeys(List<KeyInfo> keys)
        {
            if (keys.Any(k => k.KeyState != KeyState.Bound || k.KeyInfoEx.HqId != CurrentHeadQuarterId))
                throw new ApplicationException("Keys to be reported are invalid.");
            List<KeyInfo> keysIndb = base.GetKeysInDb(keys);
            if (keysIndb.Any(k => k.KeyState != KeyState.Bound))
                throw new DisException("ExportKeys_DataChangeError");
            return ReportKeys(keys);
        }

        public List<KeyOperationResult> SendBoundKeys(List<KeyGroup> groupKeys)
        {
            List<KeyInfo> keys = base.SearchBoundKeysToReport(groupKeys);
            if (keys.Any(k => k.KeyState != KeyState.Bound || k.KeyInfoEx.HqId != CurrentHeadQuarterId))
                throw new ApplicationException("Keys to be reported are invalid.");
            return SendBoundKeys(keys);
        }

        public void AutomaticReportKeys()
        {
            if (configManager.GetCanAutoReport())
            {
                List<KeyInfo> keysToReport = SearchBoundKeysToReport(new KeySearchCriteria()
                {
                    HasHardwareHash = true,
                    PageSize = Constants.BatchLimit,
                });
                if (keysToReport.Count == 0)
                    return;
                ReportKeys(keysToReport);
            }
        }

        //Re-send faild CBR to ms in the background
        public void SendGeneratedCbrs()
        {
            List<Cbr> cbrs = cbrManager.GetCbrsNotBeenSent();
            foreach (var cbr in cbrs)
            {
                try
                {
                    cbr.MsReportUniqueId = msClient.ReportCbr(cbr);
                    cbrManager.UpdateCbrAfterReported(cbr);
                    base.UpdateKeysAfterReportBinding(cbr);
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                }
            }
        }

        //get CBR.Ack from MS in the background 
        public void RetrieveCbrAcks()
        {
            List<Cbr> cbrs = cbrManager.GetReportedCbrs()
                .Where(c => c.MsReportUniqueId.HasValue).ToList();
            if (cbrs.Count > 0)
            {
                Guid[] readyCbrIds = msClient.RetrieveCbrAcks();
                cbrs = cbrs.Where(c => readyCbrIds.Contains(c.MsReportUniqueId.Value)).Union(cbrManager.GetFailedCbrs()).ToList();
                cbrManager.UpdateCbrsAfterAckReady(cbrs);
            }

            cbrs = cbrManager.GetReadyCbrs();
            foreach (Cbr cbr in cbrs)
            {
                try
                {
                    Cbr cbrWithAck = msClient.RetrieveCbrAck(cbr);
                    using (KeyStoreContext context = KeyStoreContext.GetContext())
                    {
                        cbrManager.UpdateCbrAfterAckRetrieved(cbrWithAck, false, context);
                        var result = base.UpdateKeysAfterRetrieveCbrAck(cbrWithAck, false, context);
                        if (GetIsCarbonCopy())
                            base.UpdateKeysToCarbonCopy(result.Where(r => !r.Failed).Select(r => r.KeyInDb).ToList(), true, context);
                        context.SaveChanges();
                    }
                }
                catch (WebProtocolException)
                {
                    cbrManager.UpdateCbrWhenAckFailed(cbr);
                }
            }
        }

        public void DoRecurringTasks()
        {
            switch (Constants.InstallType)
            {
                case InstallType.Oem:
                    if (configManager.GetIsMsServiceEnabled())
                    {
                        // CKI send failed CBR report to Microsoft.
                        IgnoreException(SendGeneratedCbrs);
                        IgnoreException(RetrieveCbrAcks);
                        // CKI send failed return report to Microsoft
                        IgnoreException(SendGeneratedReturnReports);
                        IgnoreException(RetrieveReturnReportAcks);
                    }
                    IgnoreException(SendKeySyncNotifications);
                    break;
                case InstallType.Tpi:
                    if (configManager.GetIsMsServiceEnabled())
                    {
                        // FKI send failed CBR to Microsoft.
                        IgnoreException(SendGeneratedCbrs);
                        IgnoreException(RetrieveCbrAcks);

                        if (GetIsCarbonCopy())
                        {
                            IgnoreException(SendCarbonCopyFulfilledKeys);
                            IgnoreException(SendCarbonCopyReportedKeys);
                            IgnoreException(SendCarbonCopyReturnReportedKeys);
                            IgnoreException(SendCarbonCopyReturnReport);
                        }
                    }
                    IgnoreException(SendKeySyncNotifications);
                    break;
            }
        }

        #endregion

        private List<KeyOperationResult> ReportKeys(List<KeyInfo> keysToReport)
        {
            List<KeyOperationResult> result = new List<KeyOperationResult>();
            switch (Constants.InstallType)
            {
                case InstallType.Oem:
                    result = ReportBindings(keysToReport);
                    break;
                case InstallType.Tpi:
                    if (GetIsCentralizeMode())
                        result = ReportKeysToUls(keysToReport);
                    else
                        result = ReportBindings(keysToReport);
                    break;
                case InstallType.FactoryFloor:
                    result = ReportKeysToUls(keysToReport);
                    break;
            }
            return result;
        }

        private List<KeyOperationResult> ReportKeysToUls(List<KeyInfo> keys)
        {
            if (!GetIsCentralizeMode())
                throw new ApplicationException("system is decentralize mode and it's invalid.");
            List<KeyOperationResult> results = new List<KeyOperationResult>();

            base.UpdateSyncState(keys, true);
            KeyInfoComparer comparer = new KeyInfoComparer();

            List<KeyInfo> failedKeys = ulsClient.ReportKeys(keys.Select(key => new KeyInfo(key.KeyState)
            {
                KeyId = key.KeyId,
                HardwareHash = key.HardwareHash,
                OemOptionalInfo = key.OemOptionalInfo,
                TrackingInfo = key.TrackingInfo,
            }).ToList());
            if (failedKeys != null && failedKeys.Count > 0)
                MessageLogger.LogSystemError("Report Failed",
                    string.Format("{0} cannot be reported.", string.Join(", ", failedKeys.Select(k => k.ProductKey).ToArray())));

            base.UpdateSyncState(keys, false);

            results.AddRange(failedKeys.Select(k => new KeyOperationResult()
            {
                Key = k,
                Failed = true,
                FailedType = KeyErrorType.SsIdInvalid
            }));

            List<KeyInfo> succeedKeys = keys.Where(k => !failedKeys.Contains(k, comparer)).ToList();
            base.UpdateKeysAfterReporting(succeedKeys);

            results.AddRange(succeedKeys.Select(k => new KeyOperationResult()
            {
                Key = k,
                Failed = false,
                FailedType = KeyErrorType.None
            }));

            return results;
        }

        private List<KeyOperationResult> ReportBindings(List<KeyInfo> keys)
        {
            if (!configManager.GetIsMsServiceEnabled())
                return keys.Select(k => new KeyOperationResult()
                {
                    Failed = true,
                    FailedType = KeyErrorType.Invalid,
                    Key = k
                }).ToList();

            using (KeyStoreContext context = KeyStoreContext.GetContext())
            {
                base.UpdateSyncState(keys, true, context);
                cbrManager.GenerateCbr(keys, false, context);
                context.SaveChanges();
            }
            return keys.Select(k => new KeyOperationResult()
            {
                Failed = false,
                Key = k
            }).ToList();
        }

        //Send the sync notification to ULS/DLS
        public void SendKeySyncNotifications()
        {
            base.SendKeySyncNotifications((s, k) => SendKeySyncNotifications(s, k));
        }

        private long[] SendKeySyncNotifications(int? ssId, List<KeySyncNotification> keySyncNotification)
        {
            long[] result = new long[0];
            try
            {
                if (ValidateSubsidary(ssId))
                    result = GetDlsClient(ssId.Value).SendKeySyncNotifications(keySyncNotification);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
            return result;
        }

        private bool ValidateSubsidary(int? ssId)
        {
            if (ssId == null)
                return false;

            Subsidiary ss = subsidiaryManager.GetSubsidiary(ssId.Value);
            if (ss == null)
                return false;
            else if (string.IsNullOrEmpty(ss.ServiceHostUrl))
                return false;
            else
                return true;

        }

        private void IgnoreException(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
    }
}
