using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace work_01.Shared.DTO
{
    public class BookingItemDTO
    {
        [ForeignKey("Booking")]
        public int BookingID { get; set; }
        [ForeignKey("Spot")]
        public int SpotID { get; set; }
        [Required]
        public int Travelers { get; set; }
    }
}
