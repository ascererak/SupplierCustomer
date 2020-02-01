using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SupplierCustomer.Contracts.BLL;
using SupplierCustomer.Contracts.Models;

namespace SupplierCustomer.BLL
{
    public class AccountService : IAccountService
    {
        private readonly IApplicationUserRepository applicationUserRepository;
        private readonly ISessionRepository sessionRepository;
        private readonly IJavascriptWebTokenFactory javascriptWebTokenFactory;
        private readonly IUserRepository userRepository;
        private readonly IApplicationRoleRepository applicationRoleRepository;
        private readonly ISessionHandler sessionHandler;
        
        public AccountService(
            IApplicationUserRepository applicationUserRepository,
            ISessionRepository sessionRepository,
            IUserRepository userRepository,
            IJavascriptWebTokenFactory javascriptWebTokenFactory,
            IApplicationRoleRepository applicationRoleRepository,
            ISessionHandler sessionHandler)
        {
            this.applicationUserRepository = applicationUserRepository;
            this.sessionRepository = sessionRepository;
            this.javascriptWebTokenFactory = javascriptWebTokenFactory;
            this.userRepository = userRepository;
            this.applicationRoleRepository = applicationRoleRepository;
            this.sessionHandler = sessionHandler;
        }

        public async Task<AuthorizationResponseModel> RegisterAsync(AuthorizationRequestModel registrationModel)
        {
            RoleData role = applicationRoleRepository.Get(registrationModel.Role);
            AuthorizationResponseModel response = new AuthorizationResponseModel() { IsSuccessful = false, Message = string.Empty };
            var userData = new UserData
            {
                Email = registrationModel.Email,
                Password = registrationModel.Password,
                RoleId = role.Id
            };

            IdentityResult userCreatingResult = await applicationUserRepository.CreateAsync(userData);
            if (!userCreatingResult.Succeeded)
            {
                // pushing message of first error in array
                response.Message = GetErrorMessage(userCreatingResult);
                return response;
            }

            userData = await applicationUserRepository.FindByEmailAsync(userData.Email);
            BasicUserData viewer = new BasicUserData()
            {
                Email = registrationModel.Email,
                UserId = userData.Id,
                RoleId = role.Id
            };
            ViewerData addedClient = await userRepository.AddAsync(viewer);
            if (addedClient == null)
            {
                response.Message = "User not added";
            }
            response.IsSuccessful = true;
            string token = javascriptWebTokenFactory.Create(userData.Id);
            var sessionData = new SessionData
            {
                UserId = userData.Id,
                Token = token,
            };
            await sessionRepository.CreateAsync(sessionData);
            response.Token = token;
            return response;
        }

        public async Task<AuthorizationResponseModel> LogInAsync(AuthorizationRequestModel logInRequestModel)
        {
            var response = new AuthorizationResponseModel { IsSuccessful = false };

            UserData userData = await applicationUserRepository.FindByEmailAsync(logInRequestModel.Email.Normalize());

            if (userData == null)
            {
                response.Message = "Account with this email doesn`t exists";
            }
            else if (!await applicationUserRepository.CheckPasswordAsync(
                logInRequestModel.Email,
                logInRequestModel.Password))
            {
                response.Message = "Wrong Password";
            }
            else
            {
                string token = javascriptWebTokenFactory.Create(userData.Id);
                var sessionDto = new SessionData
                {
                    UserId = userData.Id,
                    Token = token
                };
                await sessionRepository.CreateAsync(sessionDto);
                response.Token = token;
                response.IsSuccessful = true;
            }
            return response;
        }

        public async Task<UserData> GetClientAccountAsync(string token)
        {
            SessionData session = await sessionRepository.GetByTokenAsync(token);
            UserData user = await applicationUserRepository.FindByIdAsync(session.UserId);
            
            return user;
        }

        public async Task LogoutAsync(SessionData sessionData) => await sessionRepository.RemoveAsync(sessionData);

        private string GetErrorMessage(IdentityResult identityResult)
        {
            // return first error in list
            var errorsEnumarator = identityResult.Errors.GetEnumerator();
            errorsEnumarator.MoveNext();
            return errorsEnumarator.Current.Description;
        }
    }
}
