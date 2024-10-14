using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using WebApi.Models.Entities.Users;

namespace WebApi.Repositories.Users
{
public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
  
        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var users = new List<User>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand("SELECT Id, Nome, Email FROM Usuarios", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            users.Add(new User
                            {
                                Id = reader.GetInt64(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Nome")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                            });
                        }
                    }
                }
            }

            return users;
        }

        public async Task AddUser(User user)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand(
                    "INSERT INTO Usuarios (Nome, Email, Senha, VersaoToken, Salt, DataCriacao) VALUES (@Name, @Email, @Password, @TokenVersion, @Salt, @CreatedAt)",
                    connection))
                {
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@TokenVersion", user.TokenVersion);
                    command.Parameters.AddWithValue("@Salt", user.Salt);
                    command.Parameters.AddWithValue("@CreatedAt", user.CreatedAt != DateTime.MinValue ? user.CreatedAt : DateTime.UtcNow);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<User?> GetUserByEmail(string username)
        {
            User user = new User();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand("SELECT * FROM Usuarios WHERE Email = @Email", connection))
                {
                    command.Parameters.AddWithValue("@Email", username);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user.Id = reader.GetInt64(reader.GetOrdinal("Id"));
                            user.Name = reader.GetString(reader.GetOrdinal("Nome"));
                            user.Email = reader.GetString(reader.GetOrdinal("Email"));
                            user.Password = reader.GetString(reader.GetOrdinal("Senha"));
                            user.TokenVersion = reader.GetInt64(reader.GetOrdinal("VersaoToken"));
                            user.Salt = reader.GetString(reader.GetOrdinal("Salt"));
                        }
                        else
                        {
                            user = null;
                        }
                    }
                }
            }

            return user;
        }

        public async Task<long> GetTokenVersion(long userId)
        {
            long tokenVersion = 0;

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand("SELECT VersaoToken FROM Usuarios WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", userId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            tokenVersion = reader.GetInt64(reader.GetOrdinal("VersaoToken"));
                        }
                    }
                }
            }

            return tokenVersion;
        }

        public async Task<bool> UpdateTokenVersion(long userId, long newTokenVersion)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand(
                    "UPDATE Usuarios SET VersaoToken = @TokenVersion WHERE Id = @UserId",
                    connection))
                {
                    command.Parameters.AddWithValue("@TokenVersion", newTokenVersion);
                    command.Parameters.AddWithValue("@UserId", userId);

                    var rowsAffected = await command.ExecuteNonQueryAsync();

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

        public async Task<User> GetUserById(long userId)
        {
            User user = null;
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM Usuarios WHERE Id = @UserId";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = new User
                            {
                                Id = reader.GetInt64(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Nome")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("DataCriacao")),
                            };
                        }
                    }
                }
            }

            return user;
        }

        public async Task<(User user, string error)> FindUserAuthenticated(long userId, string salt)
        {
            User user = null;
            string error = null;

            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    var query = "SELECT * FROM Usuarios WHERE Id = @UserId AND Salt = @Salt";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@Salt", salt);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                user = new User
                                {
                                    Id = reader.GetInt64(reader.GetOrdinal("Id")),
                                    Name = reader.GetString(reader.GetOrdinal("Nome")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("DataCriacao"))
                                };
                            }
                            else
                            {
                                error = "Usuário não encontrado ou salt inválido.";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    error = $"Erro ao buscar o usuário: {ex.Message}";
                }
            }

            return (user, error);
        }

    }
}