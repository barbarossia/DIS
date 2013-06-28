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
using System.ComponentModel;

namespace DIS.Presentation.KMT.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class KeyInfoModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        public KeyInfo keyInfo { get; set; }
        
        /// <summary>
        /// selection for KMT.UI
        /// </summary>
        private bool isSelected = false;

        /// <summary>
        /// Only Item Selection
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
