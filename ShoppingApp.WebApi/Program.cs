using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShoppingApp.Business.DataProtection;
using ShoppingApp.Business.Operations.Order;
using ShoppingApp.Business.Operations.Product;
using ShoppingApp.Business.Operations.Setting;
using ShoppingApp.Business.Operations.User;
using ShoppingApp.Data.Context;
using ShoppingApp.Data.Entities;
using ShoppingApp.Data.Repositories;
using ShoppingApp.Data.UnitOfWork;
using ShoppingApp.WebApi.Middlewares;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        var jwtSecurityScheme = new OpenApiSecurityScheme
        {
            Scheme = "Bearer",
            BearerFormat = "JWT",
            Name = "Jwt Authentication",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Description = "Put **_ONLY_** your JWT Bearer Token on Texbox below!",

            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };

        options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {jwtSecurityScheme, Array.Empty<string>() },
    });
});


builder.Services.AddScoped<IDataProtection, DataProtection>();
var keysDirectory = new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "App-Data", "Keys"));
builder.Services.AddDataProtection()
    .SetApplicationName("ShoppingApp")
    .PersistKeysToFileSystem(keysDirectory);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
        };
    });



var connectionStrings = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ShoppingAppDbContext>(options => options.UseSqlServer(connectionStrings));
builder.Services.AddScoped(typeof(IRepository<>),typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<IProductService, ProductManager>();
builder.Services.AddScoped<IOrderService, OrderManager>();
builder.Services.AddScoped<ISettingService, SettingManager>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseMaintenanceMode();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
