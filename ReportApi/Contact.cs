using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportApi
{
    public class Contact
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Type { get; set; }

        public string Content { get; set; }
    }
}
