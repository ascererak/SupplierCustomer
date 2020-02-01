using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SupplierCustomer.Contracts.Models;

namespace SupplierCustomer.Contracts.BLL
{
    public interface IAccountService
    {
        Task<AuthorizationResponseModel> RegisterAsync(AuthorizationRequestModel registrationRequestModel);

        Task<AuthorizationResponseModel> LogInAsync(AuthorizationRequestModel logInRequestModel);

        Task<UserData> GetClientAccountAsync(string token);

        Task LogoutAsync(SessionData data);
    }
}
