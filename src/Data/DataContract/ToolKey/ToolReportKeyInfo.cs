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

namespace DIS.Data.DataContract.OA3ToolReportKeyInfo
{
    public class Key
    {
        public long ProductKeyID { get; set; }
        public byte ProductKeyState { get; set; }
        public string HardwareHash { get; set; }
        public List<Field> OEMOptionalInfo { get; set; }
        public string TrackingInfo { get; set; }
    }
}
