using BadOrder.Library.Abstractions.DataAccess;
using BadOrder.Library.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Settings
{
    public static class ConfigureMongoExtensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
        {
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
            var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
            services.AddSingleton(mongoDbSettings);
            services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoDbSettings.ConnectionString));
            services.AddSingleton(typeof(ICrudRepository<>), typeof(MongoCrudRepository<>));
            services.AddSingleton<IUserRepository, MongoUserRepository>();
            return services;
        }
    }
}
