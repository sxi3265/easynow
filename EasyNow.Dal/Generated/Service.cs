﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 2020/8/22 21:34:31
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
    public partial class Service {

        public Service()
        {
            this.UserServices = new List<UserService>();
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

        public virtual string Code
        {
            get;
            set;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual ServiceCategory Category
        {
            get;
            set;
        }

        public virtual IList<UserService> UserServices
        {
            get;
            set;
        }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
