using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReportApi
{
    public class Report
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [Required]
        public string Status { get; set; } = "Preparing";

        public string Content { get; set; }
    }
}
