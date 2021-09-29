using BadOrder.Library.Abstractions.DataAccess;
using BadOrder.Library.Models;
using BadOrder.Library.Settings;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Repositories
{
    public class MongoCrudRepository<T> : ICrudRepository<T> where T : ModelBase
    {
        protected readonly string _collectionName;
        protected readonly IMongoCollection<T> _mongoCollection;
        protected readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;
      //  protected readonly FilterDefinition<T> filterById<S> where S: String = filterBuilder.Eq(model => model.Id, id);
        public MongoCrudRepository(MongoDbSettings settings, IMongoClient mongoClient)
        {
            if (string.IsNullOrWhiteSpace(settings.DatabaseName)) 
                throw new ArgumentException("DatabaseName is not set");

            _collectionName = $"{typeof(T).Name}s".ToLower();

            IMongoDatabase database = mongoClient.GetDatabase(settings.DatabaseName);
            _mongoCollection = database.GetCollection<T>(_collectionName);
        }

        public async Task<T> CreateAsync(T model)
        {
            await _mongoCollection.InsertOneAsync(model);
            return model;
        }

        public async Task DeleteAsync(string id)
        {
            var filter = filterBuilder.Eq(model => model.Id, id);
            await _mongoCollection.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<T>> GetAllAsync() =>
            await _mongoCollection.Find(new BsonDocument()).ToListAsync();

        public async Task<T> GetAsync(string id)
        {
            var filter = filterBuilder.Eq(model => model.Id, id);
            return await _mongoCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task UpdateAsync(T model)
        {
            var filter = filterBuilder.Eq(existing => existing.Id, model.Id);
            await _mongoCollection.ReplaceOneAsync(filter, model);
        }
    }
}
