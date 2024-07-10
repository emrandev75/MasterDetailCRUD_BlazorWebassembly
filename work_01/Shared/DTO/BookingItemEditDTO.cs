using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace work_01.Shared.DTO
{
    public class BookingItemEditDTO
    {
        [Key]
        public int BookingID { get; set; }
      
        public int SpotID { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]

        public int Travelers { get; set; }
    }
}
