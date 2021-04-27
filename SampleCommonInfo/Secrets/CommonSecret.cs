using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleCommonInfo.Secrets
{
    public class CommonSecret
    {
        [JsonProperty("common_info_url")]
        public string CommonInfoHost { get; set; }
    }
}
