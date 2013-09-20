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
    public class InternalMembershipProvider : IMembershipProvider {
        private IUserProxy userProxy;

        public InternalMembershipProvider()
            : this(null) {
        }

        public InternalMembershipProvider(IUserProxy userProxy) {
            if (userProxy == null)
                this.userProxy = new UserProxy();
            else
                this.userProxy = userProxy;
        }

        public bool ValidateUser(string username, string password, InstallType installType) {
            return userProxy.ValidateUser(username, password);
        }

        public void SetCredentials(DisCredentials credentials, Message requestMessage) {
            // Do nothing
        }
    }
}