using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos.Event
{
    public class CreateEvent
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public DateTime PresentDate { get; set; }
        [Required]
        public int AvailableTikect { get; set; }
        public string ? CreatedBy { get; set; }

    }
}
