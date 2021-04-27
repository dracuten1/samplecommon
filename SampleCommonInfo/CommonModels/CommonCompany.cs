using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleCommonInfo.CommonModels
{
    public class CommonCompany
    {
        [JsonProperty("id_company")]
        public long ID { get; set; }
        [JsonProperty("name_company")]
        public string Name { get; set; }
    }
    public class CommonCompanyMultiResponse
    {
        [JsonProperty("err")]
        public string Error { get; set; }
        [JsonProperty("data")]
        public CompanyCommonData Data { get; set; }
    }

    public class CompanyCommonData
    {
        [JsonProperty("companies")]
        public IEnumerable<CommonCompany> Companies { get; set; }
    }
}
