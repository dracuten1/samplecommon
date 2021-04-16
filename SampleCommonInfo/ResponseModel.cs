using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleCommonInfo
{
    public class ResponseModel
    {        
        public int id_worker { get; set; }
        public string file_name { get; set; }
        public string version_name { get; set; }
        public string checksum { get; set; }
        public int type { get; set; }
        public string location { get; set; }
        public int id { get; set; }
        public int quantity { get; set; }
        public int id_company { get; set; }
        public bool deleted { get; set; }
    }
}
