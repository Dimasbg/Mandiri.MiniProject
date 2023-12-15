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
using TicketingApp.Data.Api.UserApiRepo;
using TicketingApp.Data.Dao;
using TicketingApp.Domain.PurchaseService.Models;

namespace TicketingApp.Domain.PurchaseService.Impl
{
    public class PurchaseServiceQueryImpl : IPurchaseService
    {
        private readonly IConfiguration _config;
        private static string ConnectionString;
        private readonly IUserApiRepo _userApiRepo;

        public PurchaseServiceQueryImpl(IConfiguration config, IUserApiRepo userApiRepo)
        {
            _config = config;
            _userApiRepo = userApiRepo;
            ConnectionString = _config.GetValue<string>("ConnectionStrings:Dev");
        }

        public async Task<Purchase> Create(Purchase d, CancellationToken c)
        {
            if (d.Quantity == 0)
                throw new DomainLayerException("At Least 1 ticket must be bought!");

            if (!d.PurchaseDateTime.HasValue)
                throw new DomainLayerException("Purchase date cannot be empty!");

            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            d.Ticket = await connection.QueryFirstOrDefaultAsync<Ticket>($"SELECT * FROM Tickets WHERE TicketId = {d.Ticket.TicketId}")
                ?? throw new DomainLayerException($"Ticket with id '{d.Ticket.TicketId}' not found!");

            User dataUser = await _userApiRepo.Read(d.UserId, c)
                ?? throw new DomainLayerException($"User with id '{d.UserId}' not found!");

            var insertQuery = "INSERT INTO Purchases (UserId, TicketId,PurchaseDateTime,Quantity) VALUES (@UserId, @TicketId,@PurchaseDateTime,@Quantity); SELECT * FROM Purchases WHERE PurchaseId= CAST(SCOPE_IDENTITY() AS INT)";
            d = await connection.QueryFirstOrDefaultAsync<Purchase>(insertQuery, new { d.UserId, d.Ticket.TicketId, d.PurchaseDateTime, d.Quantity });
            return d;
        }

        public async Task Delete(int id, CancellationToken c)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var result = await connection.QueryFirstOrDefaultAsync<Purchase>($"SELECT p.* FROM Purchases p WHERE PurchaseId = {id}");
            if (result == null)
                return;

            await connection.ExecuteAsync($"DELETE FROM Purchases WHERE PurchaseId = {id}");
        }

        public async Task<List<Purchase>> List(PurchaseListParamDomainModel p, CancellationToken c)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var builder = new SqlBuilder();

            var query = builder.AddTemplate("SELECT p.* FROM Purchases p /**where**/");

            if (p.PurchaseDateTime.HasValue)
                builder.Where($"PurchaseDateTime > {p.PurchaseDateTime.Value.AddSeconds(-1)} AND PurchaseDateTime < {p.PurchaseDateTime.Value.AddDays(1)}", p.PurchaseDateTime);

            if (p.Quantity.HasValue)
                builder.Where($"Quantity = {p.Quantity}", p.Quantity);

            if (p.UserId.HasValue)
                builder.Where($"UserId = {p.UserId}", p.UserId);

            if (p.EventId.HasValue)
                builder.Where($"EventId = {p.EventId}", p.EventId);

            return (await connection.QueryAsync<Purchase>(query.RawSql)).ToList();
        }

        public async Task<Purchase> Read(int id, CancellationToken c, bool isIncludeTicket = false)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var result = await connection.QueryFirstOrDefaultAsync<Purchase>($"SELECT * FROM Purchases WHERE PurchaseId = {id}");
            return result;
        }

        public async Task<Purchase> Update(Purchase d, CancellationToken c)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            if (d.Quantity == 0)
                throw new DomainLayerException("At Least 1 ticket must be bought!");

            d.Ticket = await connection.QueryFirstOrDefaultAsync<Ticket>($"SELECT * FROM Tickets WHERE TicketId = {d.Ticket.TicketId}")
                ?? throw new DomainLayerException($"Ticket with id '{d.Ticket.TicketId}' not found!");

            User dataUser = await _userApiRepo.Read(d.UserId, c)
                ?? throw new DomainLayerException($"User with id '{d.UserId}' not found!");

            Purchase result = await connection.QueryFirstOrDefaultAsync<Purchase>($"SELECT * FROM Purchases WHERE PurchaseId = {d.PurchaseId}")
                ?? throw new DomainLayerException($"Purchase with id '{d.PurchaseId}' not found!");

            result.Update(d);
            var updateQuery = $"UPDATE Purchases SET UserId = @UserId,TicketId=@TicketId,PurchaseDateTime=@PurchaseDateTime,Quantity=@Quantity WHERE PurchaseId= {d.PurchaseId}; SELECT * FROM Purchases WHERE PurchaseId= {d.PurchaseId}";
            return await connection.QueryFirstOrDefaultAsync<Purchase>(updateQuery, new { result.UserId, result.Ticket.TicketId, result.PurchaseDateTime, result.Quantity });
        }

    }
}
