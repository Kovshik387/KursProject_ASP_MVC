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

namespace KursProjectDataBase.Services
{


    public class AccountService
    {
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

        public BaseResponse<ClaimsIdentity> Register(TemporalEntity model)
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


                if ((Role)model.TypeUser == Role.Tenant)
                {
                    var type = new Tenant()
                    {
                        IdU = user.IdU,
                        Rating = default(int),
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
                //result.Label = user.IdU.ToString();
                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    Description = "Объект добавлен",
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

                var result = Authenticate(authorization);
                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
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



        private ClaimsIdentity Authenticate(Authorization user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Loginuser),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "Пользователь"),
            };
            return new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }
    }
}
