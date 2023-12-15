using Microsoft.EntityFrameworkCore;
using NotificationApp.Data.Api.EventApiRepo;
using NotificationApp.Data.Api.EventApiRepo.Impl;
using NotificationApp.Data.Api.UserApiRepo;
using NotificationApp.Data.Api.UserApiRepo.Impl;
using NotificationApp.Data.Db;
using NotificationApp.Domain.NotificationService;
using NotificationApp.Domain.NotificationService.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("UserAuth.App", o =>
{
    string mode = builder.Configuration.GetValue<string>("BaseUrl:UserAuth:Mode");
    o.BaseAddress =
        new Uri(builder.Configuration.GetValue<string>($"BaseUrl:UserAuth:Url:{mode}"));

    o.DefaultRequestHeaders.Add("X-API-KEY", builder.Configuration.GetValue<string>($"BaseUrl:UserAuth:UserAuth:{mode}"));
});
builder.Services.AddHttpClient("EventManagement.App", o =>
{
    string mode = builder.Configuration.GetValue<string>("BaseUrl:EventManagement:Mode");
    o.BaseAddress =
        new Uri(builder.Configuration.GetValue<string>($"BaseUrl:EventManagement:Url:{mode}"));

    o.DefaultRequestHeaders.Add("X-API-KEY", builder.Configuration.GetValue<string>($"BaseUrl:EventManagement:UserAuth:{mode}"));
});

#region Db
string connection = builder.Configuration.GetValue<string>("ConnectionStrings:Dev");

builder.Services.AddDbContext<MiniProjectDbContext>(options
    => options.UseSqlServer(connection));
#endregion

builder.Services.AddScoped<IEventApiRepo, EventApiRepoImpl>();
builder.Services.AddScoped<IUserApiRepo, UserApiRepoImpl>();

builder.Services.AddScoped<INotificationService, NotificationServiceImpl>();

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
