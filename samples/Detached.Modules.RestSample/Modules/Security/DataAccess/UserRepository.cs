﻿using Detached.Modules.RestSample.Modules.Security.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detached.Modules.RestSample.Modules.Security.DataAccess
{
    public class UserRepository
    {
        readonly MainDbContext _dbContext;

        public UserRepository(MainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _dbContext.Set<User>().ToListAsync();
        }
    }
}