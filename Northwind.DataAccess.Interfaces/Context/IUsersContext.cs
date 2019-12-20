using System;
using System.Collections.Generic;
using System.Text;
using Northwind.DataAccess.Interfaces.Models;

namespace Northwind.DataAccess.Interfaces.Context
{
    public interface IUsersContext : IEntityStorage
    {
        IEntitySet<User> Users { get; }
    }
}
