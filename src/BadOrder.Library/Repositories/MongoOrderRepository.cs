using BadOrder.Library.Abstractions.DataAccess;
using BadOrder.Library.Models.Orders;
using BadOrder.Library.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Repositories
{
    public class MongoOrderRepository : MongoCrudRepository<Order>, IOrderRepository
    {

        public MongoOrderRepository(MongoDbSettings settings, IMongoClient mongoClient)
            : base(settings, mongoClient) { }

        public async Task<Order> GetByOwnerEmailAsync(string email)
        {
            var filter = filterBuilder.Eq(findOrder => findOrder.OwnerEmail, email);
            return await _mongoCollection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
