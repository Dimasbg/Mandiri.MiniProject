using Dapper;
using Mandiri.MiniProject.Utilities.Base;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingApp.Data.Api.EventApiRepo;
using TicketingApp.Data.Dao;
using TicketingApp.Domain.TicketService.Models;

namespace TicketingApp.Domain.TicketService.Imp
{
    public class TicketServiceQueryImpl : ITicketService
    {
        private readonly IConfiguration _config;
        private static string ConnectionString;
        private readonly IEventApiRepo _eventApiRepo;

        public TicketServiceQueryImpl(IConfiguration config, IEventApiRepo eventApiRepo)
        {
            _config = config;
            _eventApiRepo = eventApiRepo;
            ConnectionString = _config.GetValue<string>("ConnectionStrings:Dev");
        }

        public async Task<Ticket> Create(Ticket d, CancellationToken c)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            Event dataEvent = await _eventApiRepo.Read(d.EventId, c)
                ?? throw new DomainLayerException($"Event with id '{d.EventId}' not found!");

            Ticket exist = await connection.QueryFirstOrDefaultAsync<Ticket>($"SELECT * FROM Tickets WHERE TicketType = '{d.TicketType}' AND EventId = {d.EventId}");

            if (exist != null)
                throw new DomainLayerException($"Ticket with type '{d.TicketType}' for event '{dataEvent.Name}' already exist!");

            var insertQuery = "INSERT INTO Tickets (EventId, TicketType,Price) VALUES (@EventId, @TicketType,@Price); SELECT * FROM Tickets WHERE TicketId= CAST(SCOPE_IDENTITY() AS INT)";
            d = await connection.QueryFirstOrDefaultAsync<Ticket>(insertQuery, new {d.EventId,d.TicketType,d.Price});
            return d;
        }

        public async Task Delete(int id, CancellationToken c)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var result = await connection.QueryFirstOrDefaultAsync<Ticket>($"SELECT u.* FROM Tickets u WHERE TicketId = {id}");
            if (result == null)
                return;

            await connection.ExecuteAsync($"DELETE FROM Tickets WHERE TicketId = {id}");
        }

        public async Task<List<Ticket>> List(TicketListParamDomainModel p, CancellationToken c)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var builder = new SqlBuilder();

            var query = builder.AddTemplate("SELECT t.* FROM Tickets t /**where**/");

            if (p.Price.HasValue)
                builder.Where($"Price = {p.Price}", p.Price);

            if (!string.IsNullOrWhiteSpace(p.TicketType))
                builder.Where($"TicketType LIKE '%{p.TicketType}%'", p.TicketType);

            return (await connection.QueryAsync<Ticket>(query.RawSql)).ToList();
        }

        public async Task<Ticket> Read(int id, CancellationToken c)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var result = await connection.QueryFirstOrDefaultAsync<Ticket>($"SELECT * FROM Tickets WHERE TicketId = {id}");
            return result;
        }

        public async Task<Ticket> Update(Ticket d, CancellationToken c)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            var result = await connection.QueryFirstOrDefaultAsync<Ticket>($"SELECT t.* FROM Tickets t WHERE TicketId = {d.TicketId}")
                ?? throw new DomainLayerException($"Ticket with id '{d.TicketId}' not found!");

            Event dataEvent = await _eventApiRepo.Read(d.EventId, c)
                ?? throw new DomainLayerException($"Event with id '{d.EventId}' not found!");

            Ticket exist = await connection.QueryFirstOrDefaultAsync<Ticket>($"SELECT * FROM Tickets WHERE TicketId != {d.TicketId} AND TicketType = '{d.TicketType}' AND EventId = {d.EventId}");

            if (exist != null)
                throw new DomainLayerException($"Ticket with type '{d.TicketType}' for event '{dataEvent.Name}' already exist!");

            result.Update(d);

            var updateQuery = $"UPDATE Tickets SET EventId = @EventId,TicketType=@TicketType,Price=@Price WHERE TicketId= {d.TicketId}; SELECT * FROM Tickets WHERE TicketId= {d.TicketId}";
            return await connection.QueryFirstOrDefaultAsync<Ticket>(updateQuery, new {result.EventId,result.TicketType,result.Price});
        }
    }
}
