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
        private readonly IClientRepository clientRepository;
        private readonly IApplicationRoleRepository applicationRoleRepository;
        private readonly ISessionHandler sessionHandler;

        public AccountService(
            IApplicationUserRepository applicationUserRepository,
            ISessionRepository sessionRepository,
            IClientRepository clientRepository,
            IJavascriptWebTokenFactory javascriptWebTokenFactory,
            IApplicationRoleRepository applicationRoleRepository,
            ISessionHandler sessionHandler,
            ICreditCardRepository creditCardRepository)
        {
            this.applicationUserRepository = applicationUserRepository;
            this.sessionRepository = sessionRepository;
            this.javascriptWebTokenFactory = javascriptWebTokenFactory;
            this.clientRepository = clientRepository;
            this.applicationRoleRepository = applicationRoleRepository;
            this.sessionHandler = sessionHandler;
            this.creditCardRepository = creditCardRepository;
        }

        public async Task<AuthorizationResponseModel> RegisterAsync(AuthorizationRequestModel registrationModel)
        {
            RoleData role = applicationRoleRepository.Get(registrationModel.Role);
            RegistrationResponseModel response = new RegistrationResponseModel() { IsSuccessful = false, Message = string.Empty };
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
            ClientData client = new ClientData()
            {
                Name = registrationModel.UserName,
                Surname = registrationModel.Surname,
                PhotoPath = "default/profile.png",
                UserId = userData.Id
            };
            ClientData addedClient = await clientRepository.AddAsync(client);
            if (addedClient == null)
            {
                response.Message = "Client not added";
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

        public async Task<AuthorizationRequestModel> GetClientAccountAsync(string token)
        {
            SessionData session = await sessionRepository.GetByTokenAsync(token);
            User user = await applicationUserRepository.FindByIdAsync(session.UserId);
            ClientData client = clientRepository.FindByUser(user);
            var account = new ClientAccountModel()
            {
                Email = user.Email,
                Passport = client.Passport,
                Telephone = client.Telephone,
                Name = client.Name,
                Surname = client.Surname,
                PhotoPath = client.PhotoPath,
                Role = applicationRoleRepository.Get(user.RoleId).Name,
                CreditCards = await creditCardRepository.GetByClientAsync(client.Id)
            };
            return account;
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
