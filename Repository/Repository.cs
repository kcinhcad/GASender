using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Wallet.GASender.Repository
{
    internal class Repository : IRepository
    {
        private readonly string _connectionString;

        public Repository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool IsConnected
        {
            get
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        connection.Open();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
        }

        public List<Payment> GetInvoiceOperationsForGA(DateTime date)
        {
            var result = new List<Payment>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("Reporting.GetInvoiceOperationsForGA", connection))
            {
                connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 0;
                command.Parameters.AddWithValue("@DateFrom", date);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Payment(
                            (long)reader["UserId"],
                            (string)reader["Title"],
                            (long)reader["OperationId"],
                            (bool)reader["IsFirst"],
                            (decimal)reader["Amount"],
                            (decimal)reader["FeeFromMerchant"],
                            (decimal)reader["FeeFromPayer"],
                            (decimal)reader["FeeToMerchant"],
                            (decimal)reader["FeeToAggregator"],
                            (decimal)reader["FeeToAgent"],
                            (decimal)reader["FeeFromAgent"],
                            (decimal)reader["FeeToIpsp"],
                            (DateTime)reader["UpdateDate"]
                        ));
                    }
                }
            }
            return result;
        }
    }
}
