using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Models.Domain
{
    public class Station
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Towns Town { get; set; }
        public int TownId { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }

        public ICollection<Routes> StartStations { get; set; }

        public ICollection<Routes> EndStations { get; set; }
    }
}
