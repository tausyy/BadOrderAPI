using BadOrder.Library.Abstractions.DataAccess;
using BadOrder.Library.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Repositories
{
    public class MongoUserRepository : IUserRepository
    {
        private const string databaseName = "baddb";
        private const string collectionName = "users";
        private readonly IMongoCollection<User> usersCollection;
        private readonly FilterDefinitionBuilder<User> filterBuilder = Builders<User>.Filter;
    
        public MongoUserRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            usersCollection = database.GetCollection<User>(collectionName);
        }
        
        public async Task<User> CreateUserAsync(User user)
        {
            User secureUser = user with
            {
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password)
            };

            await usersCollection.InsertOneAsync(secureUser);
            return secureUser;
        }
        
        public async Task DeleteUserAsync(string id)
        {
            var filter = filterBuilder.Eq(user => user.Id, id);
            await usersCollection.DeleteOneAsync(filter);
        }

        public async Task<User> GetUserAsync(string id)
        {
            var filter = filterBuilder.Eq(user => user.Id, id);
            return await usersCollection.Find(filter).SingleOrDefaultAsync();
        }



        public async Task<IEnumerable<User>> GetUsersAsync() =>
           await usersCollection.Find(new BsonDocument()).ToListAsync();


        public async Task UpdateUserAsync(User user)
        {
            User secureUser = user with
            {
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password)
            };

            var filter = filterBuilder.Eq(existingUser => existingUser.Id, secureUser.Id);
            await usersCollection.ReplaceOneAsync(filter, secureUser);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var filter = filterBuilder.Eq(findUser => findUser.Email, email);
            return await usersCollection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
