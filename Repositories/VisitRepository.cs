using Microsoft.Data.SqlClient;
using webApiC.DTOs;
using webApiC.Models;

namespace webApiC.Repositories;

public class VisitRepository : IVisitRepository
{
    private readonly string _connectionString;

    public VisitRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<DTO> GetVisitById(int id, CancellationToken cancellationToken)
    {
        var visit = new DTO();
        // var visit = new VisitResponseDTO();
        visit = new DTO();
        await using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);

            var query = @"SELECT
                        [dbo].[Visit].[date],
                        [dbo].[Client].[first_name],
                        [dbo].[Client].[last_name],
                        [dbo].[Client].[date_of_birth],
                        [dbo].[Mechanic].[first_name] AS [firstname],
                        [dbo].[Mechanic].[last_name] AS [lastname],
                        [dbo].[Mechanic].[licence_number],
                        [dbo].[Service].[name],
                        [dbo].[Service].[base_fee]
--                         [dbo].[Visit_Service].[amount]
                        FROM [dbo].[Visit]
                        Inner Join [dbo].[Client] On [dbo].[Client].[client_id] = [dbo].[Visit].[client_id]
                        Inner Join [dbo].[Mechanic] On [dbo].[Mechanic].[mechanic_id] = [dbo].[Visit].[mechanic_id]
                        Inner Join [dbo].[Visit_Service] On [dbo].[Visit_Service].[visit_id] = [dbo].[Visit].[visit_id]
                        Inner Join [dbo].[Service] On Visit_Service.service_id = Service.service_id
--                         WHERE [dbo].[Visit].[visit_id] = @id";

            await using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                await using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        visit = new DTO()
                        {
                            date = reader.GetDateTime(0),
                            // name = reader.GetString(reader.GetOrdinal("first_name")),
                            // serviceFee = reader.GetFloat(reader.GetOrdinal("service_fee")),
                        };
                        visit.client = new Client()
                        {
                            customer_id = reader.GetInt32(reader.GetOrdinal("customer_id")),
                            firstName = reader.GetString(reader.GetOrdinal("name")),
                        };
                        var service = new VisitServiceDTO()
                        {
                            name = reader.GetString(reader.GetOrdinal("name")),
                            serviceFee = reader.GetFloat(reader.GetOrdinal("price")),
                            // visitId = reader.GetInt32(reader.GetOrdinal("visit_id")),
                        };
                        visit.visitServices.Add(service);
                        visit.date = reader.GetDateTime(reader.GetOrdinal("date_of_birth"));
                    }
                }
            }
        }

        return visit;
    }

    public async Task<bool> DoesVisitExistAsync(int id, CancellationToken cancellationToken)
    {
        await using (var connection = new SqlConnection(_connectionString))
        {

            await connection.OpenAsync(cancellationToken);

            var query = @"SELECT COUNT(1) FROM [dbo].[Visit] WHERE visit_id = @visit_id";

            await using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@visit_id", id);

                var result = await command.ExecuteScalarAsync(cancellationToken);

                return Convert.ToInt32(result) > 0;
            }
        }
    }
    
    public async Task<bool> DoesClientExistAsync(int id, CancellationToken cancellationToken)
    {
        await using (var connection = new SqlConnection(_connectionString))
        {

            await connection.OpenAsync(cancellationToken);

            var query = @"SELECT COUNT(1) FROM [dbo].[Client] WHERE client_id = @client_id";

            await using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@client_id", id);

                var result = await command.ExecuteScalarAsync(cancellationToken);

                return Convert.ToInt32(result) > 0;
            }
        }
    }

    public async Task<bool> DoesMechanicExistAsync(int id, CancellationToken cancellationToken)
    {
        await using (var connection = new SqlConnection(_connectionString))
        {

            await connection.OpenAsync(cancellationToken);

            var query = @"SELECT COUNT(1) FROM [dbo].[Mechanic] WHERE mechanic_id = @mechanic_id";

            await using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@mechanic_id", id);

                var result = await command.ExecuteScalarAsync(cancellationToken);

                return Convert.ToInt32(result) > 0;
            }
        }
    }

    public async Task<bool> DoesServiceNameExistAsync(String name, CancellationToken cancellationToken)
    {
        await using (var connection = new SqlConnection(_connectionString))
        {

            await connection.OpenAsync(cancellationToken);

            var query = @"SELECT COUNT(1) FROM [dbo].[Service] WHERE name = @name";

            await using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", name);

                var result = await command.ExecuteScalarAsync(cancellationToken);

                return Convert.ToInt32(result) > 0;
            }
        }
    }

    public async Task<int> GetDriverIdForLicenceNumber(string licenceNumber, CancellationToken cancellationToken)
    {
        await using (var connection = new SqlConnection(_connectionString))
        {

            await connection.OpenAsync(cancellationToken);

            var query = @"SELECT driver_id FROM Driver WHERE licence_number = @licence_number";

            await using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@licence_number", licenceNumber);
                var result = await command.ExecuteScalarAsync(cancellationToken);

                return (int)result;
            }
        }
    }

    

    // public async Task<Product> GetProductByProductName(string productName, CancellationToken cancellationToken)
    // {
    //     var product = new Product();
    //
    //     await using (var connection = new SqlConnection(_connectionString))
    //     {
    //
    //         await connection.OpenAsync(cancellationToken);
    //
    //
    //         var query = @"SELECT * FROM Product WHERE Product.name = @productName";
    //
    //         await using (var command = new SqlCommand(query, connection))
    //         {
    //             command.Parameters.AddWithValue("@productName", productName);
    //
    //             await using (var reader = await command.ExecuteReaderAsync(cancellationToken))
    //             {
    //                 while (await reader.ReadAsync(cancellationToken))
    //                 {
    //                     product = new Product()
    //                     {
    //                         name = reader.GetString(reader.GetOrdinal("name")),
    //                         price = reader.GetDecimal(reader.GetOrdinal("price")),
    //                         product_id = reader.GetInt32(reader.GetOrdinal("product_id")),
    //                     };
    //                 }
    //             }
    //         }
    //
    //         return product;
    //     }
    // }

//     public async Task<int> SaveDelivery(DeliverySaveDTO deliverySave, CancellationToken cancellationToken)
// {
//     int counter = 0;
//     await using (var connection = new SqlConnection(_connectionString))
//     {
//         await connection.OpenAsync(cancellationToken);
//         SqlTransaction transaction = connection.BeginTransaction();
//
//         try
//         {
//             var query1 = @"INSERT INTO Delivery(delivery_id, customer_id, driver_id, date) 
//                            VALUES(@delivery_id, @customer_id, @driver_id, @date)";
//
//             await using (var command = new SqlCommand(query1, connection, transaction))
//             {
//                 command.Parameters.AddWithValue("@delivery_id", deliverySave.delivery.delivery_id);
//                 command.Parameters.AddWithValue("@customer_id", deliverySave.delivery.customer_id);
//                 command.Parameters.AddWithValue("@driver_id", deliverySave.delivery.driver_id);
//                 command.Parameters.AddWithValue("@date", deliverySave.delivery.date);
//
//                 var num = await command.ExecuteNonQueryAsync(cancellationToken);
//                 counter++;
//             }
//
//             var query2 = @"
//                 INSERT INTO Product_Delivery 
//                 ([product_ID], [delivery_ID], [amount]) 
//                 VALUES
//                     (@product_ID, @delivery_ID, @amount)";
//
//             foreach (var productDelivery in deliverySave.productDeliveries)
//             {
//                 await using (var command = new SqlCommand(query2, connection, transaction))
//                 {
//                     command.Parameters.AddWithValue("@product_ID", productDelivery.product_id);
//                     command.Parameters.AddWithValue("@delivery_ID", productDelivery.delivery_id);
//                     command.Parameters.AddWithValue("@amount", productDelivery.amount);
//
//                     var num = await command.ExecuteNonQueryAsync(cancellationToken);
//                     counter++;
//                 }
//             }
//
//             await transaction.CommitAsync(cancellationToken);
//             return counter;
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"SQL error: {ex.Message}");
//             await transaction.RollbackAsync(cancellationToken);
//             return 0;
//         }
//     }
// }
//     
}
    

