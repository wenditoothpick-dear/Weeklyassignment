using EmployeeAppReloade2.Data;
using MySql.Data.MySqlClient;

namespace EmployeeAppReloade2.Repository;

public class EmployeeRepository : IEmployeeRepository
{
    private IDbConnectionFactory _connectionFactory;

    public EmployeeRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> AddEmployee(Employee employee)
    {
        using var connection = (MySqlConnection)await _connectionFactory.CreateConnectionAsync();

        string query = @"INSERT INTO Employees 
                            (Id, FirstName, LastName, Email, Department, HireDate, Salary) 
                            VALUES (@Id, @FirstName, @LastName, @Email, @Department, @HireDate, @Salary)";

        MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@Id", Guid.NewGuid());
        command.Parameters.AddWithValue("@FirstName", employee.FirstName);
        command.Parameters.AddWithValue("@LastName", employee.LastName);
        command.Parameters.AddWithValue("@Email", employee.Email);
        command.Parameters.AddWithValue("@Department", employee.Department);
        command.Parameters.AddWithValue("@HireDate", employee.HireDate);
        command.Parameters.AddWithValue("@Salary", employee.Salary);

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

    public async Task<bool> DeleteEmployee(Guid id)
    {
        using var connection = (MySqlConnection)await _connectionFactory.CreateConnectionAsync();
        using MySqlCommand cmd = new("DELETE FROM Employees WHERE Id=@Id", connection);
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

    public async Task<List<Employee>> GetAllEmployees()
    {
        var employees = new List<Employee>();
        using var connection = (MySqlConnection)await _connectionFactory.CreateConnectionAsync();
        string query = "SELECT * FROM Employees";
        MySqlCommand command = new(query, connection);

        try
        {
            var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                var employee = new Employee
                {
                    Id = reader.GetGuid(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3),
                    Department = reader.GetString(4),
                    HireDate = reader.GetDateTime(5),
                    Salary = reader.GetDecimal(6)
                };
                employees.Add(employee);
            }

            reader.Close();
            return employees;
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return [];
        }
    }

    public async Task<Employee> GetEmployeeById(Guid id)
    {
        using var connection = (MySqlConnection)await _connectionFactory.CreateConnectionAsync();

        string query = "SELECT * FROM Employees WHERE Id = @Id";
        MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@Id", id);
        var employee = new Employee();

        try
        {
            var reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                reader.Read();

                employee = new Employee
                {
                    Id = (Guid)reader["Id"],
                    FirstName = reader["FirstName"].ToString()!,
                    LastName = reader["LastName"].ToString()!,
                    Email = reader["Email"].ToString()!,
                    Department = reader["Department"].ToString()!,
                    HireDate = (DateTime)reader["HireDate"],
                    Salary = (decimal)reader["Salary"]
                };
            }
            reader.Close();

            return employee;
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return employee;
        }
    }

    public async Task<bool> IsEmployeeExist(string email)
    {
        using var connection = (MySqlConnection)await _connectionFactory.CreateConnectionAsync();
        string query = "SELECT COUNT(*) FROM Employees WHERE Email = @Email";
        MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@Email", email);

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

    public async Task<bool> EditEmployee(Employee employee)
    {
        using var connection = (MySqlConnection)await _connectionFactory.CreateConnectionAsync();
        string query = @"Edit Employees SET 
                         FirstName=@FirstName, LastName=@LastName, Email=@Email, 
                         Department=@Department, HireDate=@HireDate, Salary=@Salary
                         WHERE Id=@Id";
        using MySqlCommand cmd = new(query, connection);
        cmd.Parameters.AddWithValue("@Id", employee.Id);
        cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
        cmd.Parameters.AddWithValue("@LastName", employee.LastName);
        cmd.Parameters.AddWithValue("@Email", employee.Email);
        cmd.Parameters.AddWithValue("@Department", employee.Department);
        cmd.Parameters.AddWithValue("@HireDate", employee.HireDate);
        cmd.Parameters.AddWithValue("@Salary", employee.Salary);

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
