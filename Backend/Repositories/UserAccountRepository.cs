using MongoDB.Driver;
using Core;

namespace Backend.Repositories
{
    public class UserAccountRepository
    {
        private readonly IMongoCollection<UserAccount> _useraccounts;

        /*Constructor:
        * 1. Opretter en klient med conneectionString
        * 2. Vælger databasen
        * 3. Vælger collection og gemmer den i feltet _useraccounts
        */
        public UserAccountRepository(IMongoDatabase database)
        {
            _useraccounts = database.GetCollection<UserAccount>("UserAccounts");
        }

        /* Søger efter en bruger med det angivne username && password.
         * Hvis der findes en bruger er resultatet af .Find().FirstOrDefault ikke null og der returneres et UserAccount objekt
         * Ellers returneres null
         */
        public UserAccount? UserLogin(string username, string password)
        {
            UserAccount? userFound = _useraccounts.Find(useraccount => useraccount.Username == username && useraccount.Password == password).FirstOrDefault();
            return userFound;
        }
    }
}
