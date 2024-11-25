﻿using Demo.BLL.Interfaces;
using Deno.DAL.Context;
using Deno.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private MvcDbContext _dbContext;
        public GenericRepository(MvcDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(T entity)
        {
           await _dbContext.AddAsync(entity);
        }
        public void Delete(T entity)
        {
            _dbContext.Remove(entity);
        }
        public async Task <IEnumerable<T>> GetAllAsync()
        {
            
            if (typeof(T) == typeof(Employee))
            {
                return  (IEnumerable<T>) await _dbContext.Employees.Include(e => e.Department).ToListAsync();
            }

            return await _dbContext.Set<T>().ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id)
         => await _dbContext.Set<T>().FindAsync(id);
        public void Update(T entity)
        {
          _dbContext.Update(entity);
        }
    }
}
