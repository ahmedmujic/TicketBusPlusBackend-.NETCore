using BookingManagement.Models.Csv;
using BookingManagement.Models.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Models.Bulk
{
  
    public class BulkInserBusses : BulkInsertConfig<BusCsv, Bus>
    {
        private string CompanyId { get; set; }
        public BulkInserBusses(IEnumerable<BusCsv> entities, string companyId) : base(entities)
        {
            CompanyId = companyId;
        }

        protected override void MapToDataTable(IEnumerable<BusCsv> entities)
        {
            foreach (var bus in entities)
            {
                DataRow dr = DataTable.NewRow();
                dr["Name"] = bus.Name;
                dr["NumberOfSeats"] = bus.NumberOfSeats;
                dr["BusPicture"] = null;
                dr["CompanyId"] = CompanyId;
                DataTable.Rows.Add(dr);
            }
        }
    }
  
}
