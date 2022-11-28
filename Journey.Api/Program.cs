using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Amazon.S3;
using Journey.Api.Identity;
using Journey.Api.Middleware;
using Journey.Api.Models;
using Journey.Api.Settings;
using Journey.Core.Providers;
using Journey.Core.Repository;
using Journey.Core.Services.Blobs;
using Journey.Core.Services.Chapters;
using Journey.Core.Services.Quote;
using Journey.Core.Utilities;
using Journey.Repository.Memory;
using MediatR;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

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

//CORS
builder.Services.AddCors(c => c.AddPolicy("CorsPolicy", builder =>
{
    builder.AllowAnyOrigin();
    builder.AllowAnyHeader();
    builder.AllowAnyMethod();
}));

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

builder.Services.Configure<DatabaseSettings>(config.GetSection(DatabaseSettings.KeyName));
builder.Services.AddScoped<IRepository, Journey.Repository.DynamoDb.Repository>();
builder.Services.AddScoped<IReadRepository, InMemoryRepository>();
builder.Services.AddSingleton<IAmazonS3, AmazonS3Client>(c =>
{
    return new AmazonS3Client(awsCreds, RegionEndpoint.USEast1);
});
builder.Services.AddScoped<IBlobStorageService, AwsBlobStorage>();
builder.Services.AddScoped <IBlobKeyProvider, BlobObjectKeyProvider>();
builder.Services.AddScoped<IZenQuoteClientHandler, ZenQuoteClientHandler>();
builder.Services.AddScoped<IZenQuoteClient, ZenQuoteClient>();
builder.Services.AddScoped<IQuoteProvider, RandomQuoteProvider>();
builder.Services.AddScoped<IIndexedRepository, Journey.Repository.DynamoDb.IndexedRepository>();
builder.Services.AddAutoMapper(typeof(SaveChapterCommand), typeof(Program));
builder.Services.AddScoped<IEntityKeyProvider, DefaultEntityKeyProvider>();
builder.Services.AddScoped<IBlobUriResolver, AwsBlobUriResolver>();
builder.Services.AddMemoryCache();

//Identity
var identityCnn = config.GetConnectionString("Identity");
builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(identityCnn));
builder.Services.AddIdentity<AppUser, Microsoft.AspNetCore.Identity.IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();

//builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
//                .AddEntityFrameworkStores<AppIdentityDbContext>();

//JWT Auth
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = builder.Environment.EnvironmentName == "Production" ? "https://dev-2mb38pu2.us.auth0.com/" : "https://localhost:5001";
    options.Audience = builder.Environment.EnvironmentName == "Production" ? "https://beyourhero.journey.com/api" : "https://localhost:5001";
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("SigningKey").Value))
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.MapControllers();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.Run();


