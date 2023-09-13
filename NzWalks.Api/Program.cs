using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using FluentValidation;
using FluentValidation.AspNetCore;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Models.Data;
using Models.DTO;
using NzWalks.Api.Validators;
using Repositories.Classes;
using Repositories.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
);
   
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<Program>());
//builder.Services.AddScoped<IValidator<AddRegionRequest>, AddRegionRequestValidator>();

//builder.Services.AddDbContext<NzWalksDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("NzWalks")));
builder.Services.AddDbContext<NzWalksDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("NzWalks")));
builder.Services.AddScoped<IRegionRepo, RegionRepo>();
builder.Services.AddScoped<IWalkRepo, WalkRepo>();
builder.Services.AddScoped<IWalkDifficultyRepo, WalkDifficultyRepo>();
//builder.Services.AddSingleton<IUserRepo, StaticUserRepo>();
//builder.Services.AddScoped<IUserRepo, UserRepo>();
//builder.Services.AddScoped<ITokenHandler, Repositories.Classes.TokenHandler>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
/*options.TokenValidationParameters = new TokenValidationParameters()
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidAudience = builder.Configuration["Jwt:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
});*/

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = builder.Configuration["Test"],
        Version = "v1"
    });
   /* c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter your token in the text input below.",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });*/
});
//key vault configuration
var clientid = builder.Configuration["KeyVault:ClientId"];
var tenantid = builder.Configuration["KeyVault:TenantId"];
var clientsecret = builder.Configuration["KeyVault:ClientSecret"];
var credentials = new ClientSecretCredential(tenantid,clientid,clientsecret);
var secretClient = new SecretClient(new Uri(builder.Configuration["KeyVault:Uri"]),
    credentials);
builder.Configuration.AddAzureKeyVault(secretClient, new AzureKeyVaultConfigurationOptions());

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
