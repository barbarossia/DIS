//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace WcfService
{
    public partial class ReturnReportKey
    {
        #region Primitive Properties
    
        public virtual System.Guid ReturnUniqueID
        {
            get { return _returnUniqueID; }
            set
            {
                if (_returnUniqueID != value)
                {
                    if (ReturnReport != null && ReturnReport.ReturnUniqueID != value)
                    {
                        ReturnReport = null;
                    }
                    _returnUniqueID = value;
                }
            }
        }
        private System.Guid _returnUniqueID;
    
        public virtual int OEMRMALineNumber
        {
            get;
            set;
        }
    
        public virtual string ReturnTypeID
        {
            get;
            set;
        }
    
        public virtual long ProductKeyID
        {
            get { return _productKeyID; }
            set
            {
                if (_productKeyID != value)
                {
                    if (ProductKeyInfo != null && ProductKeyInfo.ProductKeyID != value)
                    {
                        ProductKeyInfo = null;
                    }
                    _productKeyID = value;
                }
            }
        }
        private long _productKeyID;
    
        public virtual Nullable<int> MSReturnLineNumber
        {
            get;
            set;
        }
    
        public virtual string LicensablePartNumber
        {
            get;
            set;
        }
    
        public virtual string ReturnReasonCode
        {
            get;
            set;
        }
    
        public virtual string ReturnReasonCodeDescription
        {
            get;
            set;
        }

        #endregion
        #region Navigation Properties
    
        public virtual ProductKeyInfo ProductKeyInfo
        {
            get { return _productKeyInfo; }
            set
            {
                if (!ReferenceEquals(_productKeyInfo, value))
                {
                    var previousValue = _productKeyInfo;
                    _productKeyInfo = value;
                    FixupProductKeyInfo(previousValue);
                }
            }
        }
        private ProductKeyInfo _productKeyInfo;
    
        public virtual ReturnReport ReturnReport
        {
            get { return _returnReport; }
            set
            {
                if (!ReferenceEquals(_returnReport, value))
                {
                    var previousValue = _returnReport;
                    _returnReport = value;
                    FixupReturnReport(previousValue);
                }
            }
        }
        private ReturnReport _returnReport;

        #endregion
        #region Association Fixup
    
        private void FixupProductKeyInfo(ProductKeyInfo previousValue)
        {
            if (previousValue != null && previousValue.ReturnReportKeys.Contains(this))
            {
                previousValue.ReturnReportKeys.Remove(this);
            }
    
            if (ProductKeyInfo != null)
            {
                if (!ProductKeyInfo.ReturnReportKeys.Contains(this))
                {
                    ProductKeyInfo.ReturnReportKeys.Add(this);
                }
                if (ProductKeyID != ProductKeyInfo.ProductKeyID)
                {
                    ProductKeyID = ProductKeyInfo.ProductKeyID;
                }
            }
        }
    
        private void FixupReturnReport(ReturnReport previousValue)
        {
            if (previousValue != null && previousValue.ReturnReportKeys.Contains(this))
            {
                previousValue.ReturnReportKeys.Remove(this);
            }
    
            if (ReturnReport != null)
            {
                if (!ReturnReport.ReturnReportKeys.Contains(this))
                {
                    ReturnReport.ReturnReportKeys.Add(this);
                }
                if (ReturnUniqueID != ReturnReport.ReturnUniqueID)
                {
                    ReturnUniqueID = ReturnReport.ReturnUniqueID;
                }
            }
        }

        #endregion
    }
}
