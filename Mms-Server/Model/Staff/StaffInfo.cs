using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Mms_Server.Model.Staff
{
    public class StaffInfo
    {
        public int ID { get; set; }

        public string Account { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Phone { get; set; }

        public decimal Salary { get; set; }

        public DateTime EntryDate { get; set; }

        public string Creater { get; set; }

        public DateTime? CreateTime { get; set; }

        public string Updater { get; set; }

        public DateTime? UpdateTime { get; set; }
    }
}
