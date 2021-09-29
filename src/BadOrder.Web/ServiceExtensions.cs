using BadOrder.Library.Abstractions.Authentication;
using BadOrder.Library.Abstractions.DataAccess;
using BadOrder.Library.Converters;
using BadOrder.Library.Models;
using BadOrder.Library.Repositories;
using BadOrder.Library.Services;
using BadOrder.Library.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Web
{
    public static class ServiceExtensions
    {

        public static void ConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.Converters.Add(new UnitTypeEnumConverter());
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var modelState = actionContext.ModelState;
                    return new BadRequestObjectResult(modelState.ToErrorResponseModel());
                };
            });
        }

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtTokenSettings = configuration.GetSection(nameof(JwtTokenSettings)).Get<JwtTokenSettings>();
            services.AddSingleton(jwtTokenSettings);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                //x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtTokenSettings.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenSettings.Secret)),
                    ValidAudience = jwtTokenSettings.Audience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                };
            });

            services.AddTransient<IAuthService, AuthService>();
        }

        public static void ConfigureMongo(this IServiceCollection services, IConfiguration configuration)
        {
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
            var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
            services.AddSingleton(mongoDbSettings);
            services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoDbSettings.ConnectionString));
            services.AddSingleton(typeof(ICrudRepository<>), typeof(MongoCrudRepository<>));
            services.AddSingleton<IUserRepository, MongoUserRepository>();
        }
    }
}
