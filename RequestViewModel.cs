using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask
{
    public class RequestViewModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string RequestDate { get; set; }
        public string WorkName { get; set; }
        public string WorkDescription { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
    }
}
