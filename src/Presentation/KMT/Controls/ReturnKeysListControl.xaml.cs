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
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace DIS.Presentation.KMT.Controls
{
    /// <summary>
    /// Interaction logic for ReturnKeysListControl.xaml
    /// </summary>
    public partial class ReturnKeysListControl : UserControl
    {
        public ReturnKeysListControl()
        {
            InitializeComponent();
        }

        #region Piblic Members

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler LoadNextPage;

        /// <summary>
        /// 
        /// </summary>
        public event SortingEventHandler SortingByColumn;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void SortingEventHandler(object sender, SortingEventArgs e);

        /// <summary>
        /// 
        /// </summary>
        public class SortingEventArgs : EventArgs
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="columnPath"></param>
            public SortingEventArgs(string columnPath)
            {
                this.SortMemberPath = columnPath;
            }

            /// <summary>
            /// 
            /// </summary>
            public string SortMemberPath { get; set; }
        }

        #endregion

        #region Private Members

        private void ReturnByKeys_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

            ScrollBar sb = (ScrollBar)e.OriginalSource;

            if (sb.Orientation == Orientation.Vertical
                && e.NewValue == sb.Maximum && e.ScrollEventType == ScrollEventType.ThumbTrack)
            {

                if (LoadNextPage != null)
                    LoadNextPage(this, new EventArgs());
            }

        }

        private void ReturnByKeys_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer sv = (ScrollViewer)e.OriginalSource;

            if (e.VerticalChange > 0
                && e.VerticalOffset == sv.ScrollableHeight)
            {

                if (LoadNextPage != null)
                    LoadNextPage(this, new EventArgs());
            }
        }

        private void ReturnByKeys_Sorting(object sender, DataGridSortingEventArgs e)
        {
            if (SortingByColumn != null)
                SortingByColumn(this, new SortingEventArgs(e.Column.SortMemberPath));
            e.Handled = true;
        }

        #endregion Private Members
    }
}
