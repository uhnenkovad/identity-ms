using Auth.Application.Exceptions;
using Auth.Application.Ports.Repositories;
using Auth.Domain;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
namespace Auth.Infrastructure.Repositories.MongoDB;
using Auth.Domain;

public class AuthRepository : IAuthRepository
{
    private readonly IMongoCollection<User> _users;
    private readonly MongoDbSettings _settings;

        public AuthRepository(IOptions<MongoDbSettings> settings)
        {
            BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            _settings = settings.Value;
            var database = new MongoClient(_settings.ConnectionString).GetDatabase(_settings.DatabaseName);
            _users = database.GetCollection<User>(_settings.CollectionName);
        }

        public async Task<User> GetUserByUserId(Guid userId)
        {
            var filter = new FilterDefinitionBuilder<User>().Where(u => u.Id == userId);
            
            var result = (await _users.FindAsync(filter)).SingleOrDefault(); 
            return result;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var filter = new FilterDefinitionBuilder<User>().Where(u => u.Email == email);

            var result = (await _users.FindAsync(filter)).SingleOrDefault();

            return result;
        }

        public async Task UpdateUser(User user)
        {
            var filter = new FilterDefinitionBuilder<User>().Where(u => u.Id == user.Id);

            user.UpdateDate = DateTime.UtcNow;

            var result = await _users.ReplaceOneAsync(filter, user);

            if (!result.IsAcknowledged)
                throw new UpdateUserException("User could not be updated. Result is not acknowledged");
        }

        public async Task CreateUser(User user)
        {
            await _users.InsertOneAsync(user);
        }
}