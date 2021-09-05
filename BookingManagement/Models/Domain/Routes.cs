using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Models.Domain
{
    public class Routes
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public int BusId { get; set; }
        public Bus Bus { get; set; }
        public string CompandyId { get; set; }
        
        public int EndStationId { get; set; }
        public Station EndStation { get; set; }

        public int StartStationId { get; set; }
        public Station StartStation { get; set; }
        
        public int SellCounter { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        public ICollection<Dates> Dates { get; set; }

    }
}
