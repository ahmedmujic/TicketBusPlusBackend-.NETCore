using BookingManagement.Models.Bulk;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Helpers
{
    public static class BulkInsert
    {
        public static async Task BulkInsertAsync<TEntity, TInsertEntity>(BulkInsertConfig<TEntity, TInsertEntity> config) where TInsertEntity : class
        {
            try
            {
                SqlTransaction transaction = null;
                using (var connection = new SqlConnection(config.ConnectionString))
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    using (var sqlCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                    {
                        sqlCopy.DestinationTableName = config.DestinationTable;
                        sqlCopy.BatchSize = config.BatchSize;

                        foreach (var mapping in config.ColumnMappings)
                        {
                            sqlCopy.ColumnMappings.Add(mapping.Key, mapping.Value);
                        }

                        await sqlCopy.WriteToServerAsync(config.DataTable).ConfigureAwait(false);
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
