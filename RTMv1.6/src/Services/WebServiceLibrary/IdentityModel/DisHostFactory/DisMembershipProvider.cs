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
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Security;
using DIS.Business.Proxy;
using DIS.Data.DataContract;

namespace DIS.Services.WebServiceLibrary.IdentityModel {
    public class DisMembershipProvider : IMembershipProvider {
        private readonly CallDirectionExtractor callDirectionExtractor;
        private ISubsidiaryProxy ssProxy;
        private IHeadQuarterProxy hqProxy;

        public DisMembershipProvider()
            : this(null, null) {
        }

        public DisMembershipProvider(ISubsidiaryProxy ssProxy, IHeadQuarterProxy hqProxy) {
            callDirectionExtractor = new CallDirectionExtractor();

            if (ssProxy == null)
                this.ssProxy = new SubsidiaryProxy();
            else
                this.ssProxy = ssProxy;

            if (hqProxy == null)
                this.hqProxy = new HeadQuarterProxy();
            else
                this.hqProxy = hqProxy;
        }

        public bool ValidateUser(string username, string password, InstallType installType) {
            switch (installType) {
                case InstallType.Dls:
                    return ssProxy.ValidateSubsidiary(username, password);
                case InstallType.Uls:
                    return hqProxy.ValidateHeadQuarter(username, password);
                default:
                    throw new NotSupportedException();
            }
        }

        public void SetCredentials(DisCredentials credentials, Message requestMessage) {
            credentials.CallDirection = callDirectionExtractor.ExtractDirection(requestMessage);
        }
    }
}