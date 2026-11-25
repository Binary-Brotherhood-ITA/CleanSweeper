using Core;
using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("/api/login")]
    public class UserLoginController : ControllerBase
    {
        private readonly UserAccountRepository uar;

        public UserAccountController(UserAccountRepository userAccountRepository)
        {
            uar = userAccountRepository;
        }

        /* HttpPost:
         * Kalder UserAccountRepository.UserLogin
         * Modtager User-objekt eller null, som indikerer om brugerlogin er gyldigt eller ej
         * returnerer User-objekt eller null
         */
        [HttpPost]
        public IActionResult UserLogin(UserAccount user)
        {
            UserAccount userFound = uar.UserLogin(user.Username, user.Password);
            if (userFound != null)
            {
                return Ok(userFound);
            }
            else
            {
                return Unauthorized();
            }
        }

    }
}
