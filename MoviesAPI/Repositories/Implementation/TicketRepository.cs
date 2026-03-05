using Dapper;
using Microsoft.Extensions.Options;
using MoviesAPI.Models;
using MoviesAPI.Models.System;
using MoviesAPI.Repositories.Interface;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MoviesAPI.Repositories.Implementation
{
    public class TicketRepository : ITicketRepository
    {
        private readonly DBSettings _dbSettings;

        public TicketRepository(IOptions<DBSettings> dbSettings)
        {
            _dbSettings = dbSettings.Value;
        }

        public async Task<IEnumerable<TicketResponse>> GetTicketsAsync()
        {
            using var conn = new SqlConnection(_dbSettings.SqlServerDB);
          

            string sql = @"SELECT 
                        t.id, 
                        m.name AS moviename,
                        m.poster_path AS posterpath,
                        u.username AS username, 
                        t.watch_movie, 
                        t.price,
                        H.id AS hallname,
                        s.row_number AS row,
                        s.seat_number AS column
                    FROM ticket t
                    INNER JOIN movie m ON t.movie_id = m.id
                    INNER JOIN users u ON t.user_id = u.id
                    INNER JOIN hall_seat s ON t.hall_seat_id = s.id
                    INNER JOIN hall h ON s.hall_id = h.id;";

            System.Diagnostics.Debug.WriteLine(sql);

            var result = await conn.QueryAsync<TicketResponse>(sql);
            System.Diagnostics.Debug.WriteLine("result" + result);


            foreach (var ticket in result)
            {

                System.Diagnostics.Debug.WriteLine("ticket" + ticket);
            }

            return result;
        }

        public async Task<TicketResponse> GetTicketAsync(long id)
        {
            using var conn = new SqlConnection(_dbSettings.SqlServerDB);
            string sql = @"SELECT 
                        t.id, 
                        m.name AS moviename,
                        m.poster_path AS posterpath,
                        u.username AS username, 
                        t.watch_movie, 
                        t.price,
                        H.id AS hallname,
                        s.row_number AS row,
                        s.seat_number AS column
                    FROM ticket t
                    INNER JOIN movie m ON t.movie_id = m.id
                    INNER JOIN users u ON t.user_id = u.id
                    INNER JOIN hall_seat s ON t.hall_seat_id = s.id
                    INNER JOIN hall h ON s.hall_id = h.id
                    WHERE t.id = @id;";

            var ticket = await conn.QueryFirstOrDefaultAsync<TicketResponse>(sql, new { id });
            return ticket;
        }

        //public async Task<UpdateTicket> GetTicketForUpdateAsync(long id)
        //{
        //    using var conn = new SqlConnection(_dbSettings.SqlServerDB);
        //    string sql = "SELECT watch_movie, amount FROM ticket WHERE id = @id";

        //    var updateTicket = await conn.QueryFirstOrDefaultAsync<UpdateTicket>(sql, new { id });
        //    return updateTicket;

        //}

        public async Task<int> CreateTicketAsync(CreateTicket ticket)
        {
            using var conn = new SqlConnection(_dbSettings.SqlServerDB);

            string sql = @"INSERT INTO ticket(movie_id, user_id, watch_movie, price)
                   VALUES(@Movie_Id, @User_Id, @Watch_Movie, @Price);
                   SELECT CAST(SCOPE_IDENTITY() AS INT);";

            var id = await conn.ExecuteScalarAsync<int>(sql, ticket);

            return id;
        }

        //public async Task<int> UpdateTicketAsync(long id, UpdateTicket updateTicket)
        //{
        //    using var conn = new SqlConnection(_dbSettings.SqlServerDB);

        //    string sql = @"UPDATE ticket
        //           SET watch_movie = @Watch_Movie,
        //               price = @Price
        //           WHERE id = @Id";

        //    return await conn.ExecuteAsync(sql, new
        //    {
        //        Id = id,
        //        updateTicket.Watch_Movie,
        //        updateTicket.Price
        //    });
        //}

        public async Task<int> DeleteTicketAsync(long id)
        {
            using var conn = new SqlConnection(_dbSettings.SqlServerDB);

            string sql = "DELETE FROM ticket WHERE id = @id";

            return await conn.ExecuteAsync(sql, new { id });

        }

        public async Task<int> GetPurchasedTicketsAsync(long movieId, DateTime showTime)
        {
            using var conn = new SqlConnection(_dbSettings.SqlServerDB);
            var sql = @"
                SELECT COUNT(*) 
                FROM ticket 
                WHERE movie_id = @MovieId AND watch_movie = @ShowTime"; 
            return await conn.ExecuteScalarAsync<int>(sql, new { MovieId = movieId, ShowTime = showTime });
        }

      


    }
}
