using Core;
using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("/api/login")]
    public class LoginController : ControllerBase
    {
        private readonly UserAccountRepository uar;
        private readonly UserRepository ur;

        public LoginController(UserAccountRepository userAccountRepository, UserRepository userRepository)
        {
            uar = userAccountRepository;
            ur = userRepository;
        }

        /* HttpPost:
         * Kalder UserAccountRepository.UserLogin
         * Modtager UserAccount-objekt userFound eller null, som indikerer om brugerlogin er gyldigt eller ej
         * Hvis userFound != null, kaldes UserRepository.GetUserByID med det fundne ID for at få FirstName.
         * Hvis der returneres et User-objekt, gemmer vi værdierne i LoginResponse response, og response returneres som json i en http-response
         * 
         * Ellers returneres Unauthorized()
         */
        [HttpPost]
        public IActionResult UserLogin(UserAccount user)
        {
            UserAccount? userFound = uar.UserLogin(user.Username, user.Password);
            if (userFound != null)
            {
                User? userDetails = ur.GetUserByID(userFound.UserID);

                if(userDetails != null)
                {
                    LoginResponse response = new LoginResponse
                    {
                        UserID = userDetails.Id,
                        Username = userFound.Username,
                        FirstName = userDetails.FirstName,
                        Role = userFound.Role
                    };
                    return Ok(response);
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return Unauthorized();
            }
        }

    }
}
