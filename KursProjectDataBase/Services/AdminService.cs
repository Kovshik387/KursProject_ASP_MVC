using DataBaseModel;
using DataBaseModel.Entity;
using KursProjectDataBase.Helpers;
using KursProjectDataBase.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace KursProjectDataBase.Services
{
    public class AdminService
    {
        private readonly KursProjectDataBaseContext _dbContext;
        private readonly HashHelper _hashHelper;
        public AdminService(KursProjectDataBaseContext context) 
        {
            _dbContext = context;
            _hashHelper = new HashHelper();
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

        public UserModelView UserSolution(string id, string type)
        {
            int id_user = default(int);
            foreach (var item in _dbContext.Renters) if (_hashHelper.HashString(item.IdU) == id) id_user = item.IdU;
            if (id_user == 0) foreach (var item0 in _dbContext.Tenants) if (_hashHelper.HashString(item0.IdU) == id) id_user = item0.IdU;



            if (type == "Renter")
            {
                var renter = _dbContext.Renters.Include(u => u.IdUNavigation).Where(u => u.IdU == id_user).First();
                return new UserModelView()
                {
                    Id = renter.IdU,
                    Contact = renter.IdUNavigation.Contact,
                    Name = renter.IdUNavigation.Name!,
                    Surname = renter.IdUNavigation.Surname!,
                    License = renter.License,
                    Type = type,
                    IdHash = id
                };
            }
            else
            {
                var tenant = _dbContext.Tenants.Include(u => u.IdUNavigation).Where(u => u.IdU == id_user).First();
                return new UserModelView()
                {
                    Id = tenant.IdU,
                    Contact = tenant.IdUNavigation.Contact,
                    Name = tenant.IdUNavigation.Name!,
                    Surname = tenant.IdUNavigation.Surname!,
                    Rating = tenant.Rating,
                    Type = type,
                    IdHash = id
                };
            }

        }

        public void DeleteUser(string id,string type)
        {
            int id_user = default(int);
            if (type == "Renter")
            {
                foreach (var item in _dbContext.Renters) if (_hashHelper.HashString(item.IdU) == id) id_user = item.IdU;
                _dbContext.Users.Where(u => u.IdU == id_user).ExecuteDelete();
            }
            else
            {
                int id_tenant = -1;
                foreach (var item0 in _dbContext.Tenants) if (_hashHelper.HashString(item0.IdU) == id) { id_user = item0.IdU; id_tenant = item0.IdT; }
                Console.WriteLine(id_user);

                if (_dbContext.Solutions.FirstOrDefault(t => t.IdT == id_tenant) != null) _dbContext.Solutions.FirstOrDefault(t => t.IdT == id_tenant).IdT = null;
                _dbContext.SaveChanges();


                var deleted = _dbContext.Users.Where(u => u.IdU == id_user).First(); _dbContext.Remove(deleted);
                _dbContext.SaveChanges();
            }
        }

        public void UpdateUser(UserModelView user)
        {
            Console.WriteLine(user.Id);
            if (user.Type == "Renter")
            {
                _dbContext.Renters.
                    Where(id => id.IdU == user.Id).ExecuteUpdate(prop => prop.
                        SetProperty(p1 => p1.License, p1 => user.License)
                );
                var user_update = _dbContext.Users.Where(u => u.IdU == user.Id);
                user_update.ExecuteUpdate(prop => prop.
                    SetProperty(p1 => p1.Name, p1 => user.Name).
                    SetProperty(p2 => p2.Surname, p2 => user.Surname).
                    SetProperty(p3 => p3.Contact, p3 => user.Contact)
                );
                EmailService emailService = new EmailService();
                emailService.To_Message = user_update.First().Contact;
                emailService.MessageSend();
            }
            else
            {
                _dbContext.Tenants.
                    Where(id => id.IdU == user.Id).ExecuteUpdate(prop => prop.
                        SetProperty(p1 => p1.Rating, p1 => user.Rating)
                );
                var user_update = _dbContext.Users.Where(u => u.IdU == user.Id);
                user_update.ExecuteUpdate(prop => prop.
                    SetProperty(p1 => p1.Name, p1 => user.Name).
                    SetProperty(p2 => p2.Surname, p2=> user.Surname).
                    SetProperty(p3 => p3.Contact, p3 => user.Contact)
                );
                EmailService emailService = new EmailService();
                emailService.To_Message = user_update.First().Contact;
                emailService.MessageSend();
            }
        }
    }
}
