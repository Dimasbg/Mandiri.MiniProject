using Dapper;
using EventManagementApp.Data.Api.UserApiRepo;
using EventManagementApp.Data.Dao;
using EventManagementApp.Domain.RegistrationService.Models;
using Mandiri.MiniProject.Utilities.Base;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementApp.Domain.RegistrationService.Impl
{
    public class RegistrationServiceQueryImpl : IRegistrationService
    {
        private readonly IConfiguration _config;
        private static string ConnectionString;
        private readonly IUserApiRepo _userApiRepo;

        public RegistrationServiceQueryImpl(IUserApiRepo userApiRepo, IConfiguration config)
        {
            _config = config;
            ConnectionString = _config.GetValue<string>("ConnectionStrings:Dev");
            _userApiRepo = userApiRepo;
            _config = config;
        }

        public async Task<Registration> Create(Registration d, CancellationToken c)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var exist = await connection.QueryFirstOrDefaultAsync<Event>($"SELECT * Events Registrations WHERE EventId = '{d.Event.EventId}'")
                ?? throw new DomainLayerException($"Event with id '{d.Event.EventId}' not found!");

            User dataUser = await _userApiRepo.Read(d.UserId, c)
                ?? throw new DomainLayerException($"User with id '{d.UserId}' not found!");

            var insertQuery = "INSERT INTO Registrations (EventId, UserId, RegistrationDateTime) VALUES (@EventId, @UserId, @RegistrationDateTime); SELECT r.*,e.* FROM Registrations r LEFT JOIN Events e ON r.EventId = e.EventId WHERE RegistrationId = CAST(SCOPE_IDENTITY() AS INT)";
            d = await connection.QueryFirstOrDefaultAsync<Registration>(insertQuery, new { d.Event.EventId, d.UserId, d.RegistrationDateTime });
            return d;
        }

        public async Task Delete(int id, CancellationToken c)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var result = await connection.QueryFirstOrDefaultAsync<Event>($"SELECT r.* FROM Registrations r WHERE RegistrationId = {id}");
            if (result == null)
                return;

            await connection.ExecuteAsync($"DELETE FROM Registrations WHERE Registration = {id}");
        }

        public async Task<List<Registration>> List(RegistrationListParamDomainModel p, CancellationToken c)
        {
            IEnumerable<Registration> rawData = new List<Registration>();
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            string query = "SELECT r.*";

            if (p.IsIncludeEvent)
                query += @" ,r.* FROM Registration r  
                            LEFT JOIN Events e ON r.EventId = e.EventId ";
            else
                query += " FROM Registration r ";

            if (p.IsIncludeEvent)
                rawData = await connection.QueryAsync<Registration, Event, Registration>(query, (reg, ev) =>
                {
                    reg.Event = ev;
                    return reg;
                }, splitOn: "EventId");
            else
                rawData = await connection.QueryAsync<Registration>(query);


            return rawData.ToList();
        }

        public async Task<Registration> Read(int id, CancellationToken c, bool isIncludeEvent = false)
        {
            IEnumerable<Registration> rawData;
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            string query = "SELECT r.* FROM Registrations r";

            if (isIncludeEvent)
            {
                query = @"SELECT r.*,e.* FROM Registration r  
                            LEFT JOIN Events e ON r.EventId = e.EventId";
                rawData = await connection.QueryAsync<Registration, Event, Registration>($"{query} WHERE r.RegistrationId = '{id}'", (reg, ev) =>
                {
                    reg.Event = ev;
                    return reg;
                }, splitOn: "EventId");
            }
            else
                rawData = await connection.QueryAsync<Registration>($"{query} WHERE r.RegistrationId = '{id}'");


            if (!rawData.Any())
                return null;

            return rawData.FirstOrDefault();
        }

        public async Task<Registration> Update(Registration d, CancellationToken c)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var result = await connection.QueryFirstOrDefaultAsync<Registration>($"SELECT * FROM Registrations WHERE RegistrationId = {d.RegistrationId}")
                ?? throw new DomainLayerException($"Registration with id '{d.RegistrationId}' not found!");

            d.Event = await connection.QueryFirstOrDefaultAsync<Event>($"SELECT * FROM Events WHERE EventId = {d.Event.EventId}")
                ?? throw new DomainLayerException($"Event with id '{d.Event.EventId}' not found!");

            User dataUser = await _userApiRepo.Read(d.UserId, c)
                ?? throw new DomainLayerException($"User with id '{d.UserId}' not found!");

            result.Update(d);

            var updateQuery = $"UPDATE Registrations SET UserId=@UserId,Event=@EventId,RegistrationDateTime=@RegistrationDateTime WHERE RegistrationId= {d.RegistrationId}; SELECT * FROM Events WHERE RegistrationId= {d.RegistrationId}";
            return await connection.QueryFirstOrDefaultAsync<Registration>(updateQuery, new { d.UserId, d.Event.EventId, d.RegistrationDateTime });

        }
    }
}
