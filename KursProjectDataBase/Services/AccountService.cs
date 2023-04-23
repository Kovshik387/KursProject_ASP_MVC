using DataBaseModel;
using DataBaseModel.Entity;
using KursProjectDataBase.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KursProjectDataBase.Services
{


    public class AccountService
    {
        //private readonly ILogger<AccountService> _logger;
        private readonly KursProjectDataBaseContext _dataBaseModelContext;

        private enum Role
        {
            Tenant = 0,
            Renter
        }

        public AccountService(KursProjectDataBaseContext dataBaseModelContext)
        {
            _dataBaseModelContext = dataBaseModelContext;
        }

        public Task<ClaimsIdentity> Register(TemporalEntity model)
        {
            try
            {
                var check = _dataBaseModelContext.Authorizations.FirstOrDefault(x =>
                    x.Loginuser == model.Loginuser);
                if (check != null)
                {
                    return Task.FromResult(new ClaimsIdentity());
                }

                var user = new User()
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    Sex = model.Sex,
                    Contact = model.Contact,
                };

                _dataBaseModelContext.Users.Add(user);
                _dataBaseModelContext.SaveChanges();

                var authorization = new Authorization()
                {
                    Loginuser = model.Loginuser,
                    IdType = 1,
                    Passworduser = model.Passworduser,
                    IdU = user.IdU,
                    
                };
                

                _dataBaseModelContext.Authorizations.Add(authorization);


                if ((Role)model.TypeUser == Role.Tenant)
                {
                    var type = new Tenant()
                    {
                        IdU = user.IdU,
                        Rating = 5
                    };
                    _dataBaseModelContext.Tenants.Add(type);
                }
                else
                {
                    var type = new Renter()
                    {
                        IdU = user.IdU,
                        License = new Random().Next(0, 1000000000)
                    };
                    _dataBaseModelContext.Renters.Add(type);
                }

                _dataBaseModelContext.SaveChanges();

                var result = Authenticate(authorization);
                result.Label = user.IdU.ToString();
                return Task.FromResult(new ClaimsIdentity());
            }

            catch (Exception)
            {

            }
            return Task.FromResult(new ClaimsIdentity());
        }

        public bool Login(Authorization authorization)
        {
            try
            {
                var check = _dataBaseModelContext.Authorizations.FirstOrDefault(x =>
                    x.Loginuser == authorization.Loginuser && x.Passworduser == authorization.Passworduser);
                if (check != null)
                {
                    var result = Authenticate(authorization);
                    return true;
                }
            }
            catch (Exception) { }
            return false;
        }

        private ClaimsIdentity Authenticate(Authorization user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Loginuser),
                new Claim(ClaimsIdentity.DefaultNameClaimType, "Пользователь"),
            };
            return new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType); 
        }
    }
}
