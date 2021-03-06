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

using System.Windows.Controls;
using DIS.Presentation.KMT.ViewModel;

namespace DIS.Presentation.KMT.RevertKeysView
{
    /// <summary>
    /// Interaction logic for MarkFulfilledMsg.xaml
    /// </summary>
    public partial class RevertKeysOperateMsgView : Page
    {
        public RevertKeysViewModel VM { get; private set; }

        public RevertKeysOperateMsgView(RevertKeysViewModel vm)
        {
            this.InitializeComponent();
            this.VM = vm;
            DataContext = this.VM;
        }
    }
}
