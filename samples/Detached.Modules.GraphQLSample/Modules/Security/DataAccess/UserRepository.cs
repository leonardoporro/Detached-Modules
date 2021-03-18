﻿using Detached.Modules.GraphQLSample.Modules.Security.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detached.Modules.GraphQLSample.Modules.Security.DataAccess
{
    public class UserRepository
    {
        readonly MainDbContext _dbContext;

        public UserRepository(MainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetAsync()
        {
            return await _dbContext.Set<User>().ToListAsync();
        }

        public async Task<User> CreateAsync(User user)
        {
            _dbContext.Set<User>().Add(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public void ConfigureModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>();
        }   
    }
}