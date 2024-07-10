using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace work_01.Shared.DTO
{
    public class SpotEditDTO
    {
        public int SpotID { get; set; }
        [Required, StringLength(50), Display(Name = "Spot Name")]
        public string SpotName { get; set; } = default!;
        [Required, DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal Price { get; set; }
        [StringLength(150)]
        public string Picture { get; set; } = default!;
        public bool IsAvailable { get; set; }
    }
}
