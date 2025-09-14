using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class DatabaseService
{
    private readonly string _connectionString;

    public DatabaseService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    /// <summary>
    /// Executes a stored procedure and returns a DbDataReader.
    /// </summary>
    /// <param name="storedProcedureName">The name of the stored procedure to execute.</param>
    /// <param name="parameters">An array of SqlParameter to pass to the stored procedure.</param>
    /// <returns>A DbDataReader containing the result set.</returns>
    public async Task<DbDataReader> ExecuteStoredProcedureAsync(string storedProcedureName, SqlParameter[] parameters)
    {
        var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand(storedProcedureName, connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        if (parameters != null)
        {
            command.Parameters.AddRange(parameters);
        }

        try
        {
            await connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            return reader; // The reader will be handled in the calling code
        }
        catch (Exception ex)
        {
            connection.Dispose();
            throw new ApplicationException($"Error executing stored procedure: {ex.Message}", ex);
        }
    }


    /// <summary>
    /// Executes a stored procedure that does not return a result set and returns the number of rows affected.
    /// </summary>
    /// <param name="storedProcedureName">The name of the stored procedure to execute.</param>
    /// <param name="parameters">An array of SqlParameter to pass to the stored procedure.</param>
    /// <returns>The number of rows affected by the stored procedure.</returns>
    public async Task<int> ExecuteNonQueryAsync(string storedProcedureName, SqlParameter[] parameters)
    {
        var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand(storedProcedureName, connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        if (parameters != null)
        {
            command.Parameters.AddRange(parameters);
        }

        try
        {
            await connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected;
        }
        catch (Exception ex)
        {
            connection.Dispose();
            throw new ApplicationException($"Error executing stored procedure: {ex.Message}", ex);
        }
    }

    public async Task<int> ExecuteStoredProcedureWithReturnAsync(string storedProcedureName, SqlParameter[] parameters)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            using (var command = new SqlCommand(storedProcedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Add parameters to the command
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                // Define a return value parameter
                var returnParameter = new SqlParameter
                {
                    ParameterName = "@ReturnVal",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.ReturnValue
                };
                command.Parameters.Add(returnParameter);

                // Open the connection and execute the stored procedure
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();

                // Retrieve the return value
                int result = (int)returnParameter.Value;
                return result;
            }
        }
    }
}
