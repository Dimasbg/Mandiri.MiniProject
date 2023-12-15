using Microsoft.EntityFrameworkCore;
using TicketingApp.Data.Api.EventApiRepo;
using TicketingApp.Data.Api.EventApiRepo.Impl;
using TicketingApp.Data.Api.UserApiRepo;
using TicketingApp.Data.Api.UserApiRepo.Impl;
using TicketingApp.Data.Db;
using TicketingApp.Domain.PurchaseService;
using TicketingApp.Domain.PurchaseService.Impl;
using TicketingApp.Domain.TicketService;
using TicketingApp.Domain.TicketService.Imp;

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
//builder.Services.AddScoped<ITicketService, TicketServiceImpl>();
builder.Services.AddScoped<ITicketService, TicketServiceQueryImpl>();
builder.Services.AddScoped<IPurchaseService, PurchaseServiceQueryImpl>();

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
