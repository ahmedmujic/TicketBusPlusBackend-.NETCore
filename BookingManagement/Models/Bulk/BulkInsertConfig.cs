using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Models.Bulk
{
    public abstract class BulkInsertConfig<TEntity, TInsertEntity> where TInsertEntity : class
    {
        public BulkInsertConfig(IEnumerable<TEntity> entities)
        {
            ColumnMappings = SetColumnMappings();
            DataTable = SetDataTableColumns();
            MapToDataTable(entities);
        }

        public DataTable DataTable { get; private set; }
        public string ConnectionString { get; set; }
        public string DestinationTable { get; set; }
        public int BatchSize { get; set; }
        public Dictionary<string, string> ColumnMappings { get; private set; }
        protected abstract void MapToDataTable(IEnumerable<TEntity> entities);
        private Dictionary<string, string> SetColumnMappings()
        {
            Dictionary<string, string> columnMappings = new();

            foreach (var property in typeof(TInsertEntity).GetProperties())
            {
                columnMappings.Add(property.Name, property.Name);
            }

            return columnMappings;
        }
        private DataTable SetDataTableColumns()
        {
            DataTable dataTable = new();

            foreach (var property in typeof(TInsertEntity).GetProperties())
            {
                dataTable.Columns.Add(property.Name, property.PropertyType);
            }

            return dataTable;
        }
    }
}
