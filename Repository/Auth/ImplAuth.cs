using Backend.Models;
using System.Data.SqlClient;

namespace Backend.Repository.Auth
{
    public class ImplAuth : IAuth
    {

        private readonly string _connectionString;

        public ImplAuth(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private async Task<SqlConnection> GetOpenConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }



        public async Task<bool> Login(string email, string password)
        {
            using (var connection = await GetOpenConnectionAsync())
            {
                using (var command = new SqlCommand($"SELECT * FROM [User] WHERE Email = @email and Password = @password", connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@password", password);
                    using (var reader = await command.ExecuteReaderAsync())
                    {

                        if (await reader.ReadAsync())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }



            }
        }

        public async Task<bool> Registration(User user)
        {
            using(var connection = await GetOpenConnectionAsync())
            {
                using (var command = new SqlCommand($"INSERT INTO [User] (Name, Email, Password) VALUES (@Name, @Email, @Password)", connection))
                {
                    command.Parameters.AddWithValue("@Name", user.Username);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
                
        
        }
    }
}
