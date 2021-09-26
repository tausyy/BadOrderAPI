using BadOrder.Library.Abstractions.DataAccess;
using BadOrder.Library.Models.Orders;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Repositories
{
    public class MongoOrderRepository : IOrderRepository
    {
        private const string databaseName = "baddb";
        private const string collectionName = "orders";
        private readonly IMongoCollection<Order> ordersCollection;
        private readonly FilterDefinitionBuilder<Order> filterBuilder = Builders<Order>.Filter;

        public MongoOrderRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            ordersCollection = database.GetCollection<Order>(collectionName);
        }

        public Task<Order> CreateOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public Task DeleteOrderAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrderAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> GetOrderAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
