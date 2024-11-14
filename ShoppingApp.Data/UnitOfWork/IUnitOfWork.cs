﻿using ShoppingApp.Data.Entities;
using ShoppingApp.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {

        Task<int> SaveChangesAsync();
        Task BeginTransaction();
        Task CommitTransaction();
        Task RollBackTransaction();
    }
}
