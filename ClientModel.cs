using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask
{
    public class ClientModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Inn { get; set; }
        public int? ActivitySphereId { get; set; }
        public string Notes { get; set; }
    }
}
