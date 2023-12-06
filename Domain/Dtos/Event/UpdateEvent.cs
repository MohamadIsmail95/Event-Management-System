using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos.Event
{
    public class UpdateEvent
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public DateTime PresentDate { get; set; }
        [Required]
        public int AvailableTikect { get; set; }
    }
}
