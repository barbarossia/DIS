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

namespace DIS.Data.DataContract
{
    /// <summary>
    /// ImportResult class
    /// </summary>
    public class KeyOperationResult
    {
        /// <summary>
        /// Indicate if import failed
        /// </summary>
        public bool Failed { get; set; }

        /// <summary>
        /// Import failed reason
        /// </summary>
        public KeyErrorType FailedType { get; set; }

        /// <summary>
        /// The key to import
        /// </summary>
        public KeyInfo Key { get; set; }

        /// <summary>
        /// The key in current DB
        /// </summary>
        public KeyInfo KeyInDb { get; set; }
    }
}
