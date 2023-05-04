using DataBaseModel;
using DataBaseModel.Entity;
using DataBaseModel.Response;
using KursProjectDataBase.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using DataBaseModel.ViewEntity;
using System.Runtime.CompilerServices;

namespace KursProjectDataBase.Services
{
    public class AccountService
    {
        private readonly KursProjectDataBaseContext _dataBaseModelContext;

        private enum Role
        {
            Tenant = 1,
            Renter = 2
        }

        public AccountService(KursProjectDataBaseContext dataBaseModelContext)
        {
            _dataBaseModelContext = dataBaseModelContext;
        }
        public BaseResponse<ClaimsIdentity> Register(UserView model)
        {
            try
            {
                var check = _dataBaseModelContext.Authorizations.FirstOrDefault(x =>
                    x.Loginuser == model.Loginuser);
                if (check != null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Пользователь с таким именем уже есть",
                    };
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

                ClaimsIdentity result;

                if ((Role)model.TypeUser == Role.Renter)
                {
                    var type = new Renter()
                    {
                        IdU = user.IdU,
                        License = new Random().Next(0, 1000000000)
                    };
                    _dataBaseModelContext.Renters.Add(type);
                    result = Authenticate(authorization, Role.Renter);
                }
                else
                {
                    var type = new Tenant()
                    {
                        IdU = user.IdU,
                        Rating = default(int),
                    };
                    _dataBaseModelContext.Tenants.Add(type);
                    result = Authenticate(authorization,Role.Tenant);
                }

                _dataBaseModelContext.SaveChanges();


                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    StatusCode = DataBaseModel.Enum.StatusCode.OK
                };
            }

            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = DataBaseModel.Enum.StatusCode.InternalServerError,
                };
            }
        }

        public BaseResponse<String> Update(UserView user, string _id)
        {
            int id = int.Parse(_id);

            this._dataBaseModelContext.Authorizations.Where(a => a.IdU == id).ExecuteUpdate(p => p.
                SetProperty(l => l.Loginuser, l => user.Loginuser).
                SetProperty(l => l.Passworduser, l=> user.Passworduser)
            );

            this._dataBaseModelContext.Users.Where(u => u.IdU == id).ExecuteUpdate(u => 
                u.SetProperty(c => c.Contact, c => user.Contact)
            );
            
            return new BaseResponse<String>
            {
                Data = null,
                StatusCode = DataBaseModel.Enum.StatusCode.OK,
                Description = "Успешно"
            };

        }

        public BaseResponse<ClaimsIdentity> Login(Authorization authorization)
        {
            try
            {
                var check = _dataBaseModelContext.Authorizations.FirstOrDefault(x =>
                    x.Loginuser == authorization.Loginuser && x.Passworduser == authorization.Passworduser);
                if (check == null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Неверное имя пользователя или пароль"
                    };
                }

                var role = _dataBaseModelContext.Tenants.Where(u => u.IdU == check.IdU).ToList();
                ClaimsIdentity result;

                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = role.Count == 1 ? result = Authenticate(check, Role.Tenant): result = Authenticate(check, Role.Renter) ,
                    StatusCode = DataBaseModel.Enum.StatusCode.OK,
                };
            }
            catch (Exception ex) 
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    StatusCode = DataBaseModel.Enum.StatusCode.UserNotFound,
                    Description = ex.Message
                };
            }
            
        }

        private ClaimsIdentity Authenticate(Authorization user,Role role)
        {
            string userId = user.IdU.ToString()!;
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userId),
                new Claim("role", role.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role.ToString()),
            };
            Console.WriteLine($"{userId} + {role}");
            return new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }
    }
}
