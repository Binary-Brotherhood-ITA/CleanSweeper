using MongoDB.Driver;
using Core;

namespace Backend.Repositories
{
    public class UserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("Users");
        }

        // Finder bruger ud fra ID
        public User? GetUserByID(int userID)
        {
            User? userFound = _users.Find(user => user.Id == userID).FirstOrDefault();

            return userFound;
        }
    }
}