﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 2020/8/26 23:13:39
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
//------------------------------------------------------------------------------

using System;
using System.Data;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Common;
using System.Collections.Generic;

namespace EasyNow.Dal
{
    public partial class WxPusherAppUser {

        public WxPusherAppUser()
        {
            OnCreated();
        }

        public virtual System.Guid Id
        {
            get;
            set;
        }

        public virtual System.DateTime CreateTime
        {
            get;
            set;
        }

        public virtual System.DateTime UpdateTime
        {
            get;
            set;
        }

        public virtual System.Guid Creator
        {
            get;
            set;
        }

        public virtual System.Guid Updater
        {
            get;
            set;
        }

        public virtual WxPusherApp WxPusherApp
        {
            get;
            set;
        }

        public virtual WxPusherUser WxPusherUser
        {
            get;
            set;
        }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
