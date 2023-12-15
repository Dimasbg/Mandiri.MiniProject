using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using UserAuthApp.Data.Db;
using UserAuthApp.Domain.RoleService;
using UserAuthApp.Domain.RoleService.Impl;
using UserAuthApp.Domain.UserRoleService;
using UserAuthApp.Domain.UserRoleService.Impl;
using UserAuthApp.Domain.UserService;
using UserAuthApp.Domain.UserService.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//    .AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
//});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
string connection = builder.Configuration.GetValue<string>("ConnectionStrings:Dev");

builder.Services.AddDbContext<MiniProjectDbContext>(options
    => options.UseSqlServer(connection));
builder.Services.AddScoped<IUserService, UserSeviceQueryImpl>();
builder.Services.AddScoped<IUserRoleService, UserRoleServiceImpl>();
builder.Services.AddScoped<IRoleService, RoleServiceImpl>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
