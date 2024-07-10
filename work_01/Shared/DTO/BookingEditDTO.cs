using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using work_01.Shared.Models;

namespace work_01.Shared.DTO
{
    public class BookingEditDTO
    {
        [Key]
        public int BookingID { get; set; }
        [Required, Column(TypeName = "date"),
        Display(Name = "Order Date"),
            DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
            ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "date"),
            Display(Name = "Delivery Date"),
            DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
            ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }
        [Required, EnumDataType(typeof(Status))]
        public Status Status { get; set; }
        [Required, ]
        public int CustomerID { get; set; }
        //public Customer Customer { get; set; } = default!;
        public virtual ICollection<BookingItemEditDTO> BookingItems { get; set; } = new List<BookingItemEditDTO>();
    }
}
