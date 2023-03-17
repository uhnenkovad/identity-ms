using System.Security.Cryptography;
using Auth.Application.Ports.Repositories;
using Auth.Application.Ports.Services;
using Auth.Application.UseCases.CreateUser.Response;
using Auth.Application.UseCases.Login;
using Auth.Application.UseCases.RefreshToken;
using Auth.Application.UseCases.SignOut;
using Auth.Infrastructure.Repositories.MongoDB;
using Auth.Infrastructure.Services.Cryptography;
using Auth.Infrastructure.Services.Jwt;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;


var builder = WebApplication.CreateBuilder(args);

// Register services
var jwtSettingsConfiguration = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettingsConfiguration);
var jwtSettings = jwtSettingsConfiguration.Get<JwtSettings>();

builder.Services.AddSingleton<IAuthTokenService, JwtService>();
builder.Services.AddSingleton<ICryptographyService, CryptographyService>();

builder.Services.AddSingleton(provider =>
{
    var rsa = RSA.Create();
    rsa.ImportRSAPrivateKey(source: Convert.FromBase64String(jwtSettings.AccessTokenSettings.PrivateKey), bytesRead: out int _);
    return new RsaSecurityKey(rsa);
});

// Register repositories
builder.Services.Configure<MongoDbSettings>(options => builder.Configuration.GetSection("MongoDBSettings").Bind(options));
builder.Services.AddSingleton<IAuthRepository, AuthRepository>();

// Register use cases
builder.Services.AddSingleton<LoginUseCase>();
builder.Services.AddSingleton<RefreshTokenUseCase>();
builder.Services.AddSingleton<SignOutUseCase>();
builder.Services.AddSingleton<CreateUserUseCase>();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth.API v1"));
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();