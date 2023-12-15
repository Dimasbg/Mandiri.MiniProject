using EventManagementApp.Data.Api.UserApiRepo;
using EventManagementApp.Data.Api.UserApiRepo.Impl;
using EventManagementApp.Data.Db;
using EventManagementApp.Domain.EventService;
using EventManagementApp.Domain.EventService.Impl;
using EventManagementApp.Domain.RegistrationService;
using EventManagementApp.Domain.RegistrationService.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region HttpFactory
builder.Services.AddHttpClient("UserAuth.App", o =>
{
    string mode = builder.Configuration.GetValue<string>("BaseUrl:UserAuth:Mode");
    o.BaseAddress =
        new Uri(builder.Configuration.GetValue<string>($"BaseUrl:UserAuth:Url:{mode}"));

    o.DefaultRequestHeaders.Add("X-API-KEY", builder.Configuration.GetValue<string>($"BaseUrl:CORE:UserAuth:{mode}"));
});
#endregion

#region Db
string connection = builder.Configuration.GetValue<string>("ConnectionStrings:Dev");

builder.Services.AddDbContext<MiniProjectDbContext>(options
    => options.UseSqlServer(connection));
#endregion

#region Repo
builder.Services.AddScoped<IUserApiRepo, UserApiRepoImpl>();
#endregion

#region Service
builder.Services.AddScoped<IEventService, EventServiceQueryImpl>();
builder.Services.AddScoped<IRegistrationService, RegistrationServiceImpl>();
#endregion

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
