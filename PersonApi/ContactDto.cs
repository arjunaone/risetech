﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonApi
{
    public class ContactDto
    {
        public int ReportId { get; set; }

        public List<Contact> contactList { get; set; }
    }
}
