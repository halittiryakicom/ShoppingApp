using Microsoft.EntityFrameworkCore.Storage;
using ShoppingApp.Data.Context;
using ShoppingApp.Data.Entities;
using ShoppingApp.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShoppingAppDbContext _db;
        private IDbContextTransaction _dbTransaction;
        public UnitOfWork(ShoppingAppDbContext db)
        {
            _db = db;
        }

        public async Task BeginTransaction()
        {
            _dbTransaction = await _db.Database.BeginTransactionAsync();
        }

        public async Task CommitTransaction()
        {
            await _dbTransaction.CommitAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task RollBackTransaction()
        {
            await _dbTransaction.RollbackAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
