using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using WebApi.Models.Entities.Projects;
using WebApi.Models.ViewModel.Projects;

namespace WebApi.Repositories.Projects
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly string _connectionString;

        public ProjectRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddProject(Project project)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand(
                    "INSERT INTO Projetos (Titulo, DataCriacao, DataConclusao, IdUsuario) VALUES (@Title, @CreatedAt, @FinishedAt, @UserId)",
                    connection))
                {
                    command.Parameters.AddWithValue("@Title", project.Title);
                    command.Parameters.AddWithValue("@CreatedAt", project.CreatedAt);
                    command.Parameters.AddWithValue("@FinishedAt", project.FinishedAt.HasValue ? (object)project.FinishedAt.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@UserId", project.UserId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteProject(long projectId, long userId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "DELETE FROM Projetos WHERE Id = @ProjectId AND IdUsuario = @UserId";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProjectId", projectId);
                    command.Parameters.AddWithValue("@UserId", userId);

                    var rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected == 0)
                    {
                        throw new Exception("Projeto não encontrado ou você não tem permissão para excluí-lo.");
                    }
                }
            }
        }

        public async Task<Project> GetProjectById(long projectId)
        {
            Project project = null;

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM Projetos WHERE Id = @ProjectId";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProjectId", projectId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            project = new Project
                            {
                                Id = reader.GetInt64(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Titulo")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("DataCriacao")),
                                FinishedAt = reader.IsDBNull(reader.GetOrdinal("DataConclusao"))
                                    ? (DateTime?)null
                                    : reader.GetDateTime(reader.GetOrdinal("DataConclusao")),
                                UserId = reader.GetInt64(reader.GetOrdinal("IdUsuario")),
                            };
                        }
                    }
                }
            }
            return project;
        }


        public async Task<Project> GetProjectByIdAndUser(long projectId, long userId)
        {
            Project project = null;

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = new StringBuilder("SELECT * FROM Projetos WHERE Id = @ProjectId AND IdUsuario = @UserId");

                using (var command = new MySqlCommand(query.ToString(), connection))
                {
                    command.Parameters.AddWithValue("@ProjectId", projectId);
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            project = new Project
                            {
                                Id = reader.GetInt64(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Titulo")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("DataCriacao")),
                                FinishedAt = reader.IsDBNull(reader.GetOrdinal("DataConclusao")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DataConclusao")),
                                UserId = reader.GetInt64(reader.GetOrdinal("IdUsuario"))
                            };
                        }
                    }
                }
            }

            return project;
        }


        public async Task<List<Project>> GetProjects(FindProjectModel model, long userId)
        {
            var projects = new List<Project>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = new StringBuilder("SELECT * FROM Projetos WHERE 1=1");

                query.Append(" AND IdUsuario = @UserId");

                if (!string.IsNullOrEmpty(model.Title))
                    query.Append(" AND Titulo LIKE @Title");
                
                if (model.CreatedAt.HasValue)
                    query.Append(" AND DataCriacao >= @CreatedAt");

                using (var command = new MySqlCommand(query.ToString(), connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    if (!string.IsNullOrEmpty(model.Title))
                        command.Parameters.AddWithValue("@Title", "%" + model.Title + "%");
                    
                    if (model.CreatedAt.HasValue)
                        command.Parameters.AddWithValue("@CreatedAt", model.CreatedAt.Value);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var project = new Project
                            {
                                Id = reader.GetInt64(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Titulo")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("DataCriacao")),
                                FinishedAt = reader.IsDBNull(reader.GetOrdinal("DataConclusao")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DataConclusao")),
                                UserId = reader.GetInt64(reader.GetOrdinal("IdUsuario")),
                            };
                            projects.Add(project);
                        }
                    }
                }
            }

            return projects;
        }

        public async Task UpdateProject(Project project)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"UPDATE Projetos 
                      SET Titulo = @Title, 
                          DataCriacao = @CreatedAt, 
                          DataConclusao = @FinishedAt
                      WHERE Id = @ProjectId AND IdUsuario = @UserId";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", project.Title);
                    command.Parameters.AddWithValue("@CreatedAt", project.CreatedAt);
                    command.Parameters.AddWithValue("@FinishedAt", project.FinishedAt.HasValue ? (object)project.FinishedAt.Value : DBNull.Value); // Caso o campo seja nulo
                    command.Parameters.AddWithValue("@ProjectId", project.Id);
                    command.Parameters.AddWithValue("@UserId", project.UserId);

                    var rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected == 0)
                        throw new Exception("Projeto não encontrado ou você não tem permissão para atualizá-lo.");
                    
                }
            }
        }

    }
}