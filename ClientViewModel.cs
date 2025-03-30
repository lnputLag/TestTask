using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask
{
    public class ClientViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Inn { get; set; }
        public int? ActivitySphereId { get; set; }
        public string ActivitySphereName { get; set; }
        public int RequestCount { get; set; }
        public string LastRequestDate { get; set; }
        public string Notes { get; set; }
    }
}
