using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Amazon.S3;
using Journey.Api;
using Journey.Api.Auth;
using Journey.Api.Middleware;
using Journey.Api.Models;
using Journey.Api.Settings;
using Journey.Core.Providers;
using Journey.Core.Repository;
using Journey.Core.Services.Blobs;
using Journey.Core.Services.Chapters;
using Journey.Core.Services.Communication;
using Journey.Core.Services.Communication.SendGrid;
using Journey.Core.Services.Quote;
using Journey.Core.Services.Quote.ApiNinja;
using Journey.Core.Services.Quote.ZenQuote;
using Journey.Core.Utilities;
using Journey.Repository.Memory;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
});

//LOGGING
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

////CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCORS", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var aws = builder.Configuration.GetSection("AwsSettings").Get<AwsSettings>();
var awsCreds = new BasicAWSCredentials(aws.AccessKey, aws.SecretKey);
builder.Services.AddMediatR(typeof(Program), typeof(SaveChapterCommand));
builder.Services.AddSingleton<IAmazonDynamoDB, AmazonDynamoDBClient>(c =>
{
    return new AmazonDynamoDBClient(awsCreds, RegionEndpoint.USEast2);
});
builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
builder.Services.AddSingleton<ICacheProvider, CacheProvider>();
builder.Services.AddHttpClient();
builder.Services.Configure<Journey.Core.Services.Quote.ApiNinja.ApiNinjaSettings>(builder.Configuration.GetSection("ApiNinjaSettings"));
builder.Services.Configure<Journey.Core.Services.Communication.SendGrid.SendGridSettings>(builder.Configuration.GetSection("SendGridSettings"));

builder.Services.Configure<DatabaseSettings>(config.GetSection(DatabaseSettings.KeyName));
builder.Services.AddScoped<IRepository, Journey.Repository.DynamoDb.Repository>();
builder.Services.AddScoped<IReadRepository, InMemoryRepository>();
builder.Services.AddSingleton<IAmazonS3, AmazonS3Client>(c =>
{
    return new AmazonS3Client(awsCreds, RegionEndpoint.USEast1);
});
builder.Services.AddScoped<IBlobStorageService, AwsBlobStorage>();
builder.Services.AddScoped <IBlobKeyProvider, BlobObjectKeyProvider>();
builder.Services.AddSingleton<IZenQuoteClientHandler, ZenQuoteClientHandler>();
builder.Services.AddSingleton<IZenQuoteClient, ZenQuoteClient>();
builder.Services.AddSingleton<INinjaQuoteClient, NinjaQuoteClient>();
builder.Services.AddSingleton<IApiNinjaClientHandler, ApiNinjaClientHandler>();
builder.Services.AddScoped<IQuoteProvider, RandomQuoteProvider>();
builder.Services.AddTransient<IEmailSender, SendGridEmailSender>();
builder.Services.AddScoped<IIndexedRepository, Journey.Repository.DynamoDb.IndexedRepository>();
builder.Services.AddAutoMapper(typeof(SaveChapterCommand), typeof(Program));
builder.Services.AddScoped<IEntityKeyProvider, DefaultEntityKeyProvider>();
builder.Services.AddScoped<IBlobUriResolver, AwsBlobUriResolver>();
builder.Services.AddScoped<IApiAuthenticationTokenProvider, JwtTokenProvider>(c =>
{
    return new JwtTokenProvider(config.GetSection("SigningKey").Value);
});
builder.Services.AddMemoryCache();

//Identity
var identityCnn = config.GetConnectionString("Identity");
var dbPassword = config["Authentication:DbPassword"];
var pgConnectionBuilder = new NpgsqlConnectionStringBuilder(identityCnn)
{
    Password = dbPassword
};
builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseNpgsql(pgConnectionBuilder.ConnectionString));
builder.Services.AddIdentity<AppUser, Microsoft.AspNetCore.Identity.IdentityRole>(opt =>
{
    opt.SignIn.RequireConfirmedEmail = true;
})
                .AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();

//builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
//                .AddEntityFrameworkStores<AppIdentityDbContext>();

//JWT Auth
var authority = builder.Environment.EnvironmentName == "Production" ? "https://iter-meum-api.azurewebsites.net/" : "https://localhost:7030";
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = "https://dev-2mb38pu2.us.auth0.com/";
    options.Audience = "iter-meum-api";
    //options.Authority = authority;
    //options.Audience = builder.Environment.EnvironmentName == "Production" ? "https://iter-meum-api.azurewebsites.net/" : "https://localhost:7030";
    //options.SaveToken = true;
    //options.Configuration = new OpenIdConnectConfiguration();
    //options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    //{
    //    ValidateIssuer = false,
    //    ValidateAudience = false,
    //    ValidateLifetime = true,
    //    ValidateIssuerSigningKey = true,
    //    ValidIssuer = authority,
    //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("SigningKey").Value))
    //};
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("EnableCORS");

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.Run();


