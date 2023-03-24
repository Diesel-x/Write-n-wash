using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write_Wash.Models.DBContext
{
    public class PointContext
    {
        [Key]
        public int idpoints_of_issue { get; set; }
        public int index { get; set; }
        public string city { get; set; }
        public string street { get; set; }
        public int house_num { get; set; }
    }
}
