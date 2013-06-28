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
using System.Configuration;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;
using DIS.Business.Proxy;
using DIS.Common.Utility;
using DIS.Data.DataContract;

namespace DIS.Services.DataPolling {
    /// <summary>
    /// OEMDataPollingService class for Service code behind
    /// </summary>
    public partial class DataPollingService : ServiceBase {
        #region Private fields

        private IUserProxy userProxy;
        private IConfigProxy configProxy;
        private IHeadQuarterProxy hqProxy;

        private readonly int defaultInterval;

        private Timer pulseTimer = null;
        private Timer intervalTimer = null;
        private Timer fulfillmentTimer = null;
        private Timer reportTimer = null;
        private Timer miscTimer = null;

        private bool isFulfilling = false;
        private bool isReporting = false;
        private bool isMiscRunning = false;

        #endregion Private fields

        public IUserProxy UserProxy {
            get {
                if (userProxy == null)
                    userProxy = new UserProxy();
                return userProxy;
            }
        }

        public IConfigProxy ConfigProxy {
            get {
                if (configProxy == null)
                    configProxy = new ConfigProxy(UserProxy.GetFirstManager());
                return configProxy;
            }
        }

        public IHeadQuarterProxy HqProxy {
            get {
                if (hqProxy == null)
                    hqProxy = new HeadQuarterProxy();
                return hqProxy;
            }
        }

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public DataPollingService() {
            InitializeComponent();
            AutoLog = true;

            defaultInterval = Math.Max(int.Parse(ConfigurationManager.AppSettings["interval"]), 60000);
        }

        #endregion Consturctor

        #region Overriden Methods

        /// <summary>
        /// OnStart
        /// </summary>
        /// <param name="args">string[]</param>
        protected override void OnStart(string[] args) {
            try {
                intervalTimer = new Timer();
                intervalTimer.Enabled = true;
                intervalTimer.Interval = defaultInterval;
                intervalTimer.Elapsed += new ElapsedEventHandler(IntervalTimerElapsed);

                fulfillmentTimer = new Timer();
                fulfillmentTimer.Enabled = true;
                fulfillmentTimer.Interval = defaultInterval;
                fulfillmentTimer.Elapsed += new ElapsedEventHandler(FulfillmentTimerElapsed);

                reportTimer = new Timer();
                reportTimer.Enabled = true;
                reportTimer.Interval = defaultInterval;
                reportTimer.Elapsed += new ElapsedEventHandler(ReportTimerElapsed);

                miscTimer = new Timer();
                miscTimer.Enabled = true;
                miscTimer.Interval = defaultInterval;
                miscTimer.Elapsed += new ElapsedEventHandler(MiscTimerElapsed);

                pulseTimer = new Timer();
                pulseTimer.Enabled = true;
                pulseTimer.Interval = Constants.PulseInterval;
                pulseTimer.Elapsed += new ElapsedEventHandler(PulseTimerElapsed);
            }
            catch (Exception ex) {
                ExceptionHandler.HandleException(ex);
            }
        }

        /// <summary>
        /// OnShutdown
        /// </summary>
        protected override void OnShutdown() {
            intervalTimer.Elapsed -= new ElapsedEventHandler(IntervalTimerElapsed);
            intervalTimer.Dispose();

            fulfillmentTimer.Elapsed -= new ElapsedEventHandler(FulfillmentTimerElapsed);
            fulfillmentTimer.Dispose();

            reportTimer.Elapsed -= new ElapsedEventHandler(ReportTimerElapsed);
            reportTimer.Dispose();

            miscTimer.Elapsed -= new ElapsedEventHandler(MiscTimerElapsed);
            miscTimer.Dispose();

            pulseTimer.Elapsed -= new ElapsedEventHandler(PulseTimerElapsed);
            pulseTimer.Dispose();

            base.OnShutdown();
        }

        /// <summary>
        /// OnStop
        /// </summary>
        protected override void OnStop() {
            intervalTimer.Elapsed -= new ElapsedEventHandler(IntervalTimerElapsed);
            intervalTimer.Enabled = false;

            fulfillmentTimer.Elapsed -= new ElapsedEventHandler(FulfillmentTimerElapsed);
            fulfillmentTimer.Enabled = false;

            reportTimer.Elapsed -= new ElapsedEventHandler(ReportTimerElapsed);
            reportTimer.Enabled = false;

            miscTimer.Elapsed -= new ElapsedEventHandler(MiscTimerElapsed);
            miscTimer.Enabled = false;

            pulseTimer.Elapsed -= new ElapsedEventHandler(PulseTimerElapsed);
            pulseTimer.Enabled = false;

            base.OnStop();
        }

        #endregion Overriden Methods

        #region Timer Events

        private void IntervalTimerElapsed(object sender, ElapsedEventArgs e) {
            try {
                int fulfillmentInterval = GetTimerInterval(ConfigProxy.GetFulfillmentInterval());
                if (fulfillmentTimer.Interval != fulfillmentInterval)
                    fulfillmentTimer.Interval = fulfillmentInterval;

                int reportInterval = GetTimerInterval(ConfigProxy.GetReportInterval());
                if (reportTimer.Interval != reportInterval)
                    reportTimer.Interval = reportInterval;
            }
            catch (Exception ex) {
                ExceptionHandler.HandleException(ex);
            }
        }

        private void PulseTimerElapsed(object sender, ElapsedEventArgs e) {
            try {
                ConfigProxy.DataPollingServiceReport();
            }
            catch {
                //do nothing
            }
        }

        /// <summary>
        /// Invoked when the time is elapsed for the OrderFulfillment Timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FulfillmentTimerElapsed(object sender, ElapsedEventArgs e) {
            if (!isFulfilling) {
                isFulfilling = true;
                LoopForHeadQuarters((p) => {
                    p.AutomaticGetKeys();
                });
                isFulfilling = false;
            }
        }

        /// <summary>
        /// Invoked when the time is elapsed for the Getproductkeys Timer
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">ElapsedEventArgs</param>
        private void ReportTimerElapsed(object sender, ElapsedEventArgs e) {
            if (!isReporting) {
                isReporting = true;
                LoopForHeadQuarters((p) => {
                    p.AutomaticReportKeys();
                });
                isReporting = false;
            }
        }

        private void MiscTimerElapsed(object sender, ElapsedEventArgs e) {
            if (!isMiscRunning) {
                isMiscRunning = true;
                LoopForHeadQuarters((p) => {
                    p.DoRecurringTasks();
                });
                isMiscRunning = false;
            }
        }

        private void LoopForHeadQuarters(Action<IKeyProxy> action) {
            try {
                List<HeadQuarter> hqs = HqProxy.GetHeadQuarters();
                if (hqs == null || hqs.Count == 0)
                    action(new KeyProxy(UserProxy.GetFirstManager(), null));
                else
                    foreach (HeadQuarter hq in hqs) {
                        action(new KeyProxy(UserProxy.GetFirstManager(), hq.HeadQuarterId));
                    }
            }
            catch (Exception ex) {
                ExceptionHandler.HandleException(ex);
            }
        }

        #endregion Timer Events

        private int GetTimerInterval(double intervalInMinute) {
            return (int)(intervalInMinute * 60 * 1000);
        }
    }
}
