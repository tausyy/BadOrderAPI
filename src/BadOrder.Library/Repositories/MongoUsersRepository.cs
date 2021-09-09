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
    public class MongoUsersRepository : IUsersRepository
    {
        private const string databaseName = "baddb";
        private const string collectionName = "users";
        private readonly IMongoCollection<User> usersCollection;
        private readonly FilterDefinitionBuilder<User> filterBuilder = Builders<User>.Filter;
        public MongoUsersRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            usersCollection = database.GetCollection<User>(collectionName);
        }
        public async Task CreateUserAsync(User user) =>
           await usersCollection.InsertOneAsync(user);

        public async Task DeleteUserAsync(int id)
        {
            var filter = filterBuilder.Eq(user => user.Id, id);
            await usersCollection.DeleteOneAsync(filter);
        }

        public async Task<User> GetUserAsync(int id)
        {
            var filter = filterBuilder.Eq(user => user.Id, id);
            return await usersCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetUsersAsync() =>
           await usersCollection.Find(new BsonDocument()).ToListAsync();

        public async Task UpdateItemAsync(User user)
        {
            var filter = filterBuilder.Eq(existingUser => existingUser.Id, user.Id);
            await usersCollection.ReplaceOneAsync(filter, user);
        }
    }
}
