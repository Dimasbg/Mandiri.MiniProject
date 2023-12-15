using Dapper;
using EventManagementApp.Data.Dao;
using EventManagementApp.Data.Db;
using EventManagementApp.Domain.EventService.Models;
using Mandiri.MiniProject.Utilities.Base;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EventManagementApp.Domain.EventService.Impl
{
    public class EventServiceQueryImpl : IEventService
    {
        private readonly IConfiguration _config;
        private static string ConnectionString;
        private readonly MiniProjectDbContext _db;

        public EventServiceQueryImpl(MiniProjectDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
            ConnectionString = _config.GetValue<string>("ConnectionStrings:Dev");
        }

        public async Task<Event> Create(Event d, CancellationToken c)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var exist = await connection.QueryFirstOrDefaultAsync<User>($"SELECT * FROM Events WHERE Name = '{d.Name}'");
            if (exist != null)
                throw new DomainLayerException($"'{d.Name}' already used!");

            var insertQuery = "INSERT INTO Events (Name, Description, Location, StartDateTime, EndDateTime) VALUES (@Name, @Description, @Location, @StartDateTime, @EndDateTime); SELECT * FROM Events WHERE EventId= CAST(SCOPE_IDENTITY() AS INT)";
            d = await connection.QueryFirstOrDefaultAsync<Event>(insertQuery, d);
            return d;
        }

        public async Task Delete(int id, CancellationToken c)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var result = await connection.QueryFirstOrDefaultAsync<Event>($"SELECT e.* FROM Events e WHERE EventId = {id}");
            if (result == null)
                return;

            await connection.ExecuteAsync($"DELETE FROM Events WHERE EventId = {id}");
        }

        public async Task<List<Event>> List(EventListParamDomainModel p, CancellationToken c)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var builder = new SqlBuilder();

            var query = builder.AddTemplate("SELECT e.* FROM Events e /**where**/");

            IQueryable<Event> rawQuery = _db.Events;

            if (!string.IsNullOrWhiteSpace(p.Name))
                builder.Where($"Name LIKE '%{p.Name}%'", p.Name);

            if (!string.IsNullOrWhiteSpace(p.Description))
                builder.Where($"Description LIKE '%{p.Description}%'", p.Description);

            if (!string.IsNullOrWhiteSpace(p.Location))
                builder.Where($"Location LIKE '%{p.Location}%'", p.Location);

            return (await connection.QueryAsync<Event>(query.RawSql)).ToList();
        }

        public async Task<Event> Read(int id, CancellationToken c)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var result = await connection.QueryFirstOrDefaultAsync<Event>($"SELECT * FROM Events WHERE EventId = {id}");
            return result;
        }

        public async Task<Event> Update(Event d, CancellationToken c)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var result = await connection.QueryFirstOrDefaultAsync<Event>($"SELECT * FROM Events WHERE EventId = {d.EventId}")
                ?? throw new DomainLayerException($"Event with id '{d.EventId}' not found!");

            var exist = await connection.QueryFirstOrDefaultAsync<User>($"SELECT * FROM Events WHERE EventId != {d.EventId} AND Name = '{d.Name}'");
            if (exist != null)
                throw new DomainLayerException($"'{d.Name}' already used!");

            result.Update(d);
            var updateQuery = $"UPDATE Events SET Name=@Name, Description=@Description, Location=@Location, StartDateTime=@StartDateTime, EndDateTime=@EndDateTime WHERE EventId= {d.EventId}; SELECT * FROM Events WHERE EventId= {d.EventId}";
            return await connection.QueryFirstOrDefaultAsync<Event>(updateQuery, result);
        }
    }
}
