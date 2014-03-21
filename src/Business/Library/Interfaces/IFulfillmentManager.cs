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
using DIS.Data.DataContract;
using DIS.Data.DataAccess;

namespace DIS.Business.Library {
    public interface IFulfillmentManager {
        FulfillmentInfo GetFirstFulfilledFulfillment();
        List<FulfillmentInfo> GetFulfillments(List<string> fulfillmentNumbers);
        List<FulfillmentInfo> GetFailedFulfillments(bool shouldIncludeExpired);
        void SaveAvailableFulfillments(List<FulfillmentInfo> infoes);
        void RetrieveFulfilment(List<FulfillmentInfo> infoes, Func<FulfillmentInfo, List<KeyInfo>> getKeys);
        void UpdateFulfillmentToCompleted(FulfillmentInfo info, KeyStoreContext context);
        void UpdateFulfillmentFailedWhenDiskIsFull();
    }

}
