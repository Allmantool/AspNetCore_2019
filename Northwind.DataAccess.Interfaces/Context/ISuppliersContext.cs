﻿using Northwind.DataAccess.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.DataAccess.Interfaces.Context
{
    public interface ISuppliersContext : IEntityStorage
    {
        public IEntitySet<Suppliers> Suppliers { get; }
    }
}
