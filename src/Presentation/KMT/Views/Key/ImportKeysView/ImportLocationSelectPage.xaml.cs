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

using DIS.Presentation.KMT.ViewModel;

namespace DIS.Presentation.KMT.ImportKeysView
{
    public partial class ImportLocationSelectPage
    {
        public ImportKeysViewModel VM { get; private set; }

        public ImportLocationSelectPage(ImportKeysViewModel vm)
        {
            this.InitializeComponent();
            this.VM = vm;
            DataContext = this.VM;
        }
    }
}
