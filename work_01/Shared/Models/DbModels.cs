using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace work_01.Shared.Models
{
    public enum Status { Open=1,Pending,Cancelled}
    public class Customer
    {
        public int CustomerID { get; set; }
        [Required, StringLength(50), Display(Name = "Customer Name")]
        public string CustomerName { get; set; } = default!;
        [Required, StringLength(150)]
        public string Address { get; set; } = default!;


        [Required, StringLength(50), EmailAddress]
        public string Email { get; set; } = default!;
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    }
    public class Booking
    {
        public int BookingID { get; set; }
        [Required, Column(TypeName = "date"),
        Display(Name = "Start Date"),
            DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
            ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "date"),
            Display(Name = "End Date"),
            DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
            ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }
        [Required, EnumDataType(typeof(Status))]
        public Status Status { get; set; }
        [Required, ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public Customer Customer { get; set; } = default!;
        public virtual ICollection<BookingItem> BookingItems { get; set; } = new List<BookingItem>();

    }
    public class BookingItem
    {
        [ForeignKey("Booking")]
        public int BookingID { get; set; }
        [ForeignKey("Spot")]
        public int SpotID { get; set; }
        [Required]
        public int Travelers { get; set; }

        public virtual Booking Booking { get; set; } = default!;
        public virtual Spot Spot { get; set; } = default!;


    }
    public class Spot
    {
        public int SpotID { get; set; }
        [Required, StringLength(50), Display(Name = "Spot Name")]
        public string SpotName { get; set; } = default!;
        [Required, Column(TypeName = "money"), DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal Price { get; set; }
        [Required, StringLength(150)]
        public string Picture { get; set; } = default!;
        public bool IsAvailable { get; set; }
        public virtual ICollection<BookingItem> BookingItems { get; set; } = new List<BookingItem>();
    }
 
}
