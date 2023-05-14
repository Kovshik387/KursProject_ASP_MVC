using DataBaseModel;
using DataBaseModel.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;

namespace KursProjectDataBase.Services
{
    public class AdminService
    {
        private readonly KursProjectDataBaseContext _dbContext; 
        public AdminService(KursProjectDataBaseContext context) 
        {
            _dbContext = context;
        }

        public Tuple<List<Tenant>,List<Renter>> UsersGet()
        {
            var result = _dbContext.Solutions.
                Include(t => t.IdTNavigation).
                    ThenInclude(u => u.IdUNavigation).
                Include(r => r.IdRNavigation).
                    ThenInclude(u => u.IdUNavigation);

            var tenants = _dbContext.Tenants.Include(u => u.IdUNavigation).ToList();
            var renters = _dbContext.Renters.Include(u0 => u0.IdUNavigation).ToList();
            return new(tenants, renters);
        }
    }
}
