using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Auth.Domain;

[BsonIgnoreExtraElements]
public class User
{
    [BsonId]
    public Guid Id { get; set; }
    
    [BsonElement("active")]
    public bool Active { get; set; }
    
    [BsonElement("email")]
    public string Email { get; set; }
    
    [BsonElement("password")]
    public string Password { get; set; }
    
    [BsonElement("salt")]
    public string Salt { get; set; }
    
    [BsonElement("name")]
    public string Name { get; set; }
    
    [BsonElement("lastName")]
    public string LastName { get; set; }
    
    [BsonElement("claims")]
    public IList<Claim> Claims { get; set; }
    
    [BsonElement("refreshToken")]
    public RefreshToken RefreshToken { get; set; }
    
    [BsonElement("creationDate")]
    public DateTime CreationDate { get; set; }
    
    [BsonElement("updateDate")]
    public DateTime UpdateDate { get; set; }

    public User()
    {
        Claims = new List<Claim>();
        RefreshToken = new RefreshToken();
    }
}