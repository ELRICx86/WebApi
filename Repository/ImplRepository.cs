using Backend.Models;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Emit;
using Backend.Repository;
using System.ComponentModel;


public class ImplRepository : IRepository
{

    private readonly string _connectionString;

    public ImplRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    private async Task<SqlConnection> GetOpenConnectionAsync()
    {
        var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }


    public TodoList MapToEntity(IDataRecord record)
    {
        var entity = Activator.CreateInstance<TodoList>();

        // Map properties from the SqlDataReader to the model properties
        entity.Id = (int)record["Id"];
        entity.Label = record["Label"] as string;
        entity.Title = record["Title"] as string;
        entity.Description = record["Description"] as string;
        entity.CreationDate = record["CreationDate"] as string;
        entity.FinishDate = record["FinishDate"] as string;
        entity.Status = record["Status"] as string;
        entity.Priority = record["Priority"] as string;
        return entity;
    }


    //Add 

    public async Task<Message> Add(TodoList todo)
    {
        using (var connection = await GetOpenConnectionAsync())
        {
            string query = "INSERT INTO TODO (Label, Title, Description, CreationDate, FinishDate, Status, Priority) VALUES" +
                " (@Label, @Title, @Description, GETDATE(), @FinishDate, @Status, @Priority)";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Label", todo.Label);
                command.Parameters.AddWithValue("@Title", todo.Title);
                command.Parameters.AddWithValue("@Description", todo.Description);
                command.Parameters.AddWithValue("@FinishDate", todo.FinishDate);
                command.Parameters.AddWithValue("@Status", todo.Status);
                command.Parameters.AddWithValue("@Priority", todo.Priority);
                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    return new Message
                    {
                        StatusCode = 200,
                        StatusMessage = "Successfully Inserted Into Database",
                        TodoList = todo,
                        lists = null
                    };
                }
                else
                {
                    return new Message
                    {
                        StatusCode = 100,
                        StatusMessage = "Failed to Inserted Into Database",
                        TodoList = todo,
                        lists = null
                    };
                }
            }
        }
    }

    //Delete By Id
    public async Task<Message> Delete(int id)
    {
        Message response = new Message();
        using (var connection = await GetOpenConnectionAsync())
        {
            using (var command = new SqlCommand($"DELETE FROM TODO WHERE Id = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Todo item deleted successfully";
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "No Todo item found with the given ID";
                }
            }
            return response;
        }
    }


    //Get ALL
    public async Task<Message> GetAll()
    {
        var response = new Message();
        var entities = new List<TodoList>();
        using (var connection = await GetOpenConnectionAsync())
        {
            using (var command = new SqlCommand($"SELECT * FROM TODO", connection))
            {
                var reader = await command.ExecuteReaderAsync();
                while (await reader!.ReadAsync())
                {
                    // Implement mapping logic to convert SqlDataReader to Message instance
                    if (reader != null)
                    {
                        var entity = MapToEntity(reader);
                        entities.Add(entity);
                    }
                }
            }
        }
        if (entities.Count > 0)
        {
                response.StatusCode = 200;
                response.StatusMessage = " Data Found";
                response.TodoList = null;
                response.lists = entities;
            

        }
        else
        {
            // No data found
            response.StatusCode = 200;
            response.StatusMessage = " Data Found";
            response.TodoList = null;
            response.lists = entities;

            
        }
        return response;
    }





    /*    public async Task<List<Message>> GetAll()
        {
            Message response = new Message();
            var entities = new List<Message>();
            using (var connection = await GetOpenConnectionAsync())
            {
                using (var command = new SqlCommand($"SELECT * FROM TODO", connection))
                {
                    var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        // Implement mapping logic to convert SqlDataReader to TEntity instance
                        var entity = MapToEntity(reader);
                        if(entity != null)
                        {
                            response.StatusCode = 200;
                            response.StatusMessage = "Data Found";
                            response.TodoList = entity;
                            entities.Add(response);
                        }
                        else
                        {
                            response.StatusCode = 100;
                            response.StatusMessage = "No Data Found";
                            response.TodoList = null;
                            response.lists = null;
                            break;
                        }

                    }
                }
            }
            return entities;
        }*/


    //Get by Id
    public async Task<Message> GetById(int id)
    {
        Message response = new Message();
        using (var connection = await GetOpenConnectionAsync())
        {
            using (var command = new SqlCommand($"SELECT * FROM TODO WHERE Id = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync()) // Check if there are rows to read
                    {
                        var entity = MapToEntity(reader);
                        response.StatusCode = 200;
                        response.StatusMessage = "Data Found";
                        response.TodoList = entity;
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "No Data Found";
                    }
                }
            }
            return response;
        }
    }





    public async Task<Message> Update(TodoList todo, int id)
    {
        Message response = new Message();
        using (var connection = await GetOpenConnectionAsync())
        {
            string query = "UPDATE TODO SET Label = @Label, Title = @Title, Description = @Description, " +
                           "FinishDate = @FinishDate, Status = @Status, Priority = @Priority " +
                           "WHERE Id = @Id";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Label", todo.Label);
                command.Parameters.AddWithValue("@Title", todo.Title);
                command.Parameters.AddWithValue("@Description", todo.Description);
                command.Parameters.AddWithValue("@FinishDate", todo.FinishDate);
                command.Parameters.AddWithValue("@Status", todo.Status);
                command.Parameters.AddWithValue("@Priority", todo.Priority);
                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Successfully updated todo item in the database";
                    response.TodoList = todo;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "No todo item found with the given ID";
                }
            }
            return response;
        }
    }
}

