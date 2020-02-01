using SupplierCustomer.Contracts.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SupplierCustomer.Contracts.BLL
{
    public interface IAccounService
    {
        Task<RegistrationResponseModel> RegisterAsync(RegistrationRequestModel registrationRequestModel);

        Task<LogInResponseModel> LogInAsync(LogInRequestModel logInRequestModel);

        Task<LogInRequestModel> GetClientAccountAsync(string token);

        Task<UpdateClientResponseModel> UpdateClientAccountAsync(string token, UpdateClientRequestModel model);

        Task LogoutAsync(SessionData data);
    }
}
