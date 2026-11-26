using Core;
using Blazored.SessionStorage;
using System.Net.Http.Json;
// @using System.Net.Http.Json i _Imports.razor gælder Razor-filer, men ikke automatisk dine almindelige .cs-filer.
// Derfor skal vi selv tilføje <using System.Net.Http.Json>; i LoginService.cs, når vi bruger PostAsJsonAsync().


/* HOW TO USE: 
* Gør den tilgængelig i Imports.razor: @using GenbrugEaa.Services
* registrer servicen i program.cs - gør den global for projektet: builder.Services.AddScoped<LoginService>();
* Derfor kan sider kan gøre brug af: 
* * @inject LoginService login
* * Efter injektion kan sider kalde login.UserLogin(username, password) og gøre brug af metoden her for at tjekke et logins gyldighed hos DB. 
*/

/* LOGIC FLOW:
* * * LoginService: Til at autentificere brugerens login.
* 
* Når brugeren logger ind
1. LoginPage:    Sender Username og Password til api/login.
2. LoginController:   Modtager loginoplysningerne.
3. UserAccountRepository:   Finder den matchende konto i UserAccounts.
4. UserRepository:   Bruger kontoens UserID til at finde medarbejderen i Users.
5. LoginController:   Samler oplysningerne i et LoginResponse-objekt og sender det tilbage.
6. LoginService:   Modtager svaret og gemmer brugeroplysningerne i SessionStorage.
 */

namespace Frontend.Services
{
    public class LoginService
    {
        private readonly HttpClient Http; // opretter et felt, som klassen kan bruge
        private readonly ISessionStorageService sessionStorage; // opretter felt, som klassen kan bruge
        public event Action? OnUserStateChanged; // Opretter et event OnUserStateChanged. Action? er en type, der repræsenterer en metode uden parameter og returværdi.

        // Constructor
        public LoginService(HttpClient http, ISessionStorageService storage)
        {
            Http = http;
            sessionStorage = storage;
        }

        /* UserLogin():
         * Opretter nyt UserAccount objekt. 
         * Kalder Post API hos UserAccountController
         * Den får response som er et HTTP-svar fra UserAccountController.
         * Hvis der er success, så læses JSON-indholdet og det gemmes som loggedInUser og et LoginResponse objekt. 
         * Ellers returneres null.
         */
        public async Task<LoginResponse?> UserLogin(string username, string password)
        {
            UserAccount userAccount = new UserAccount { Username = username, Password = password };

            HttpResponseMessage response = await Http.PostAsJsonAsync("api/login", userAccount);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LoginResponse>();
            }
            else
            {
                return (null);
            }
        }

        /* SetUserSession():
         * Hvis et login er gyldigt skal LoginService.SetUserSession(username) kaldes. 
         * gemmer username og bool i sessionStorage - fortæller hvilken bruger og om bruger er logget ind.
         * OnUserStateChanged?.Invoke(): Fortæller resten af appen, når brugerstatus ændres. 
         * "Hvis nogen lytter på dette event, så kald alle metoder, der har tilmeldt sig"
         */
        public async Task SetUserSession(LoginResponse user)
        {
            await sessionStorage.SetItemAsync("userId", user.UserID);
            await sessionStorage.SetItemAsync("username", user.Username);
            await sessionStorage.SetItemAsync("firstName", user.FirstName);
            await sessionStorage.SetItemAsync("role", user.Role);
            await sessionStorage.SetItemAsync("isLoggedIn", true);

            OnUserStateChanged?.Invoke();
        }

        /* Logout():
         * Når bruger logger ud skal LoginService.Logout kaldes
         * Sletter data med userId, username, firstName og om bruger er logget ind fra sessionStorage.
         * OnUserStateChanged?.Invoke(): Fortæller resten af appen, når brugerstatus ændres. 
         * "Hvis nogen lytter på dette event, så kald alle metoder, der har tilmeldt sig"
         */
        public async Task Logout()
        {
            await sessionStorage.RemoveItemAsync("userId");
            await sessionStorage.RemoveItemAsync("username");
            await sessionStorage.RemoveItemAsync("firstName");
            await sessionStorage.RemoveItemAsync("isLoggedIn");
            OnUserStateChanged?.Invoke();
        }

        /* IsLoggedIn():
         * Når sider med begrænset adgang åbnes, skal LoginService.
         * Henter username og isLoggedIn bool fra sessionStorage.
         * Hvis værdier er gyldige returneres true. 
         * Ellers returneres false. 
         */
        public async Task<bool> IsLoggedIn()
        {
            int? userId = await sessionStorage.GetItemAsync<int?>("userId");
            string? username = await sessionStorage.GetItemAsync<string?>("username");
            bool isLoggedIn = await sessionStorage.GetItemAsync<bool>("isLoggedIn");
            if (userId != null && username != null && isLoggedIn)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /* GetUserId():
         * returnerer userId fra sessionStorage
         */
        public async Task<int> GetUserId()
        {
            return await sessionStorage.GetItemAsync<int>("userId");
        }

        /* GetFirstName():
         * Returnerer fornavnet på brugeren fra sessionStorage
         * Hvis det er null er det en tom string
         */
        public async Task<string> GetFirstName()
        {
            return await sessionStorage.GetItemAsync<string>("firstName") ?? "";
        }
    }
}
