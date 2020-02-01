using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SupplierCustomer.Contracts.Models;

namespace SupplierCustomer.WebApi.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountApiController : Controller
    {
        private readonly IAccountService accountService;

        public AccountApiController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestModel userData)
            => Json(await accountService.RegisterAsync(userData));

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(LogInRequestModel logInRequestModel)
            => Json(await accountService.LogInAsync(logInRequestModel));

        [Route("logout")]
        [HttpPost]
        public async Task<IActionResult> Logout(SessionData sessionData)
        {
            await accountService.LogoutAsync(sessionData);
            return Ok();
        }

        [Route("get/{token}")]
        [HttpGet]
        public async Task<IActionResult> GetClientAccount(string token)
            => Json(await accountService.GetClientAccountAsync(token));
    }
}
