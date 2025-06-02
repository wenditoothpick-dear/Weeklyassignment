using EmployeeAppReloade2.Data;
using MySql.Data.MySqlClient;

namespace EmployeeAppReloade2.Repository;

public class DepartmentRepository : IDepartmentRepository
{
    private IDbConnectionFactory _connectionFactory;

    public DepartmentRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> AddDepartment(Department department)
    {
        using var connection = (MySqlConnection)await _connectionFactory.CreateConnectionAsync();

        string query = @"INSERT INTO Departments
                            (Id, Name,Description) 
                            VALUES (@Id, @Name, @Description)";

        MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@Id", Guid.NewGuid());
        command.Parameters.AddWithValue("@Name", department.Name);
        command.Parameters.AddWithValue("@Description", department.Description);
        try
        {
            int rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteDepartment(Guid id)
    {
        using var connection = (MySqlConnection)await _connectionFactory.CreateConnectionAsync();
        using MySqlCommand cmd = new("DELETE FROM Departmet WHERE Id=@Id", connection);
        cmd.Parameters.AddWithValue("@Id", id);

        try
        {
            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }

    public async Task<List<Department>> GetAllDepartment()
    {
        var departments = new List<Department>();
        using var connection = (MySqlConnection)await _connectionFactory.CreateConnectionAsync();
        string query = "SELECT * FROM Departments";
        MySqlCommand command = new(query, connection);

        try
        {
            var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                var department = new Department
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    
                };
                departments.Add(department);
            }

            reader.Close();
            return departments;
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return [];
        }
    }

    public async Task<Department> GetDepartmentById(Guid id)
    {
        using var connection = (MySqlConnection)await _connectionFactory.CreateConnectionAsync();

        string query = "SELECT * FROM Departments WHERE Id = @Id";
        MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@Id", id);
        var department = new Department();

        try
        {
            var reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                reader.Read();

                department = new Department
                {
                    Id = (Guid)reader["Id"],
                    Name = reader["Name"].ToString()!,
                    Description = reader["Description"].ToString()!,
                };
            }
            reader.Close();

            return department;
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return department;
        }
    }

    public async Task<bool> IsDepartmentExist(string Name)
    {
        using var connection = (MySqlConnection)await _connectionFactory.CreateConnectionAsync();
        string query = "SELECT COUNT(*) FROM Departments WHERE Name = @Name";
        MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@Name", name);

        try
        {
            object? result = await command.ExecuteScalarAsync();
            int count = Convert.ToInt32(result);
            return count == 1;
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> EditDepartment(Department department)
    {
        using var connection = (MySqlConnection)await _connectionFactory.CreateConnectionAsync();
        string query = @"Edit Departments SET 
                         Name=@Name, Description=@Description
                         WHERE Id=@Id";
        using MySqlCommand cmd = new(query, connection);
        cmd.Parameters.AddWithValue("@Id", department.Id);
        cmd.Parameters.AddWithValue("@Name", department.Name);
        cmd.Parameters.AddWithValue("@Description", department.Description);
        try
        {
            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }
}
