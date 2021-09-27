using BadOrder.Library.Abstractions.DataAccess;
using BadOrder.Library.Models;
using BadOrder.Library.Models.Users;
using BadOrder.Library.Services;
using BadOrder.Library.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Repositories
{
    public class MongoUserRepository: MongoCrudRepository<User>, IUserRepository
    {
        public MongoUserRepository(MongoDbSettings settings, IMongoClient mongoClient) 
            : base(settings, mongoClient) { }

        public async Task<User> GetByEmailAsync(string email)
        {    
            var filter = filterBuilder.Eq(findUser => findUser.Email, email);
            return await _mongoCollection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
