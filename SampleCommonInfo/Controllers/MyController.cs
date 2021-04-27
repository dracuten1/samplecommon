using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SampleCommonInfo.CommonModels;
using SampleCommonInfo.Contexts;
using SampleCommonInfo.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SampleCommonInfo.Controllers
{
    public class ResponsePattern
    {
        [JsonProperty("err")] public string ErrorMessage { get; set; } = null;
        [JsonProperty("data")] public object Data { get; set; } = null;
    }
    public class Company
    {
        [JsonProperty("id_company")] public long id_company { get; set; }
        [JsonProperty("name")] public string name { get; set; }
    }
    public class MyController : ControllerBase
    {
        private CompanyInstanceRepository _companyInstanceRepository = new CompanyInstanceRepository();
        private readonly CommonSecret commonSecret;

        public MyController(CommonSecret commonSecret)
        {
            this.commonSecret = commonSecret;
        }
        [HttpGet("/companies/by-id/{companyID}/workers/config")]
        public async Task<JsonResult> GetInstance([FromRoute(Name = "companyID")]int CompanyID)
        {
            var data = new ResponsePattern
            {
                Data = await _companyInstanceRepository.GetInstanceWorker(CompanyID)
            };
            return new JsonResult(data);
        }
        [HttpGet("/config/workers")]
        public async Task<JsonResult> GetWorkerConfig()
        {
            var data = new ResponsePattern
            {
                Data = await _companyInstanceRepository.GetWorker()
            };
            return new JsonResult(data);
        }
        [HttpGet("/companies/by-user")]
        public async Task<JsonResult> GetCompany([FromHeader] string authorization)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(commonSecret.CommonInfoHost+ "/companies/by-user"),
                Headers = {
                        { HttpRequestHeader.Accept.ToString(), "application/json" },
                        { "authorization", authorization }
                    },
            };
            using var client = new HttpClient();
            using var res = await client.SendAsync(httpRequestMessage, HttpContext.RequestAborted);
            if (res.StatusCode == HttpStatusCode.Unauthorized) throw new UnauthorizedAccessException("Unauthorized");
            var strResult = await res.Content.ReadAsStringAsync();
            if (res.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(strResult);
            }           
            
            var commonResult = JsonConvert.DeserializeObject<CommonCompanyMultiResponse>(strResult);
           
            var data = new ResponsePattern
            {
                Data = commonResult.Data.Companies.Select(c => new Company
                {
                    id_company = c.ID,
                    name = c.Name
                })
        };
            return new JsonResult(data);
        }
        [HttpPost("/companies/by-id/{companyID}/workers/config")]
        public async Task<JsonResult> UpdateWorkerConfig([FromRoute(Name = "companyID")]int CompanyID, [FromBody]IEnumerable<Company_Worker> workers)
        {
            try
            {

                var data = new ResponsePattern
                {
                    Data = await _companyInstanceRepository.AddOrUpdate(workers, CompanyID)
                };
                return new JsonResult(data);
            }
            catch (Exception e)
            {
                var data = new ResponsePattern
                {
                    ErrorMessage = "Duplicate type " + e.Message
                };
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new JsonResult(data);
            }
        }
    }
}
