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
    public partial class ComputerBuildReport
    {
        #region Primitive Properties
    
        public virtual Nullable<System.Guid> MSReportUniqueID
        {
            get;
            set;
        }
    
        public virtual System.Guid CustomerReportUniqueID
        {
            get;
            set;
        }
    
        public virtual Nullable<System.DateTime> MSReceivedDateUTC
        {
            get;
            set;
        }
    
        public virtual string SoldToCustomerID
        {
            get;
            set;
        }
    
        public virtual string ReceivedFromCustomerID
        {
            get;
            set;
        }
    
        public virtual Nullable<int> CBRAckFileTotal
        {
            get;
            set;
        }
    
        public virtual Nullable<int> CBRAckFileNumber
        {
            get;
            set;
        }
    
        public virtual Nullable<bool> Status
        {
            get;
            set;
        }

        #endregion
        #region Navigation Properties
    
        public virtual ICollection<HardwareBindingReport> HardwareBindingReports
        {
            get
            {
                if (_hardwareBindingReports == null)
                {
                    var newCollection = new FixupCollection<HardwareBindingReport>();
                    newCollection.CollectionChanged += FixupHardwareBindingReports;
                    _hardwareBindingReports = newCollection;
                }
                return _hardwareBindingReports;
            }
            set
            {
                if (!ReferenceEquals(_hardwareBindingReports, value))
                {
                    var previousValue = _hardwareBindingReports as FixupCollection<HardwareBindingReport>;
                    if (previousValue != null)
                    {
                        previousValue.CollectionChanged -= FixupHardwareBindingReports;
                    }
                    _hardwareBindingReports = value;
                    var newValue = value as FixupCollection<HardwareBindingReport>;
                    if (newValue != null)
                    {
                        newValue.CollectionChanged += FixupHardwareBindingReports;
                    }
                }
            }
        }
        private ICollection<HardwareBindingReport> _hardwareBindingReports;

        #endregion
        #region Association Fixup
    
        private void FixupHardwareBindingReports(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (HardwareBindingReport item in e.NewItems)
                {
                    item.ComputerBuildReport = this;
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (HardwareBindingReport item in e.OldItems)
                {
                    if (ReferenceEquals(item.ComputerBuildReport, this))
                    {
                        item.ComputerBuildReport = null;
                    }
                }
            }
        }

        #endregion
    }
}