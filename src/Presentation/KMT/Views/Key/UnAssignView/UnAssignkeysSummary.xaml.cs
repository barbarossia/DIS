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

namespace DIS.Presentation.KMT.UnAssignView
{
    /// <summary>
    /// Interaction logic for SummaryView.xaml
    /// </summary>
    public partial class UnAssignkeysSummary : Page
    {
        public UnAssignKeysViewModel VM { get; set; }

        public UnAssignkeysSummary(UnAssignKeysViewModel vm)
        {
            InitializeComponent();

            VM = vm;
            DataContext = vm;
            Title = DIS.Presentation.KMT.Properties.MergedResources.Common_Summary;
        }
    }
}
