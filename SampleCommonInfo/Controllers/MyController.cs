using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SampleCommonInfo.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        [JsonProperty("id_company")] public int id_company { get; set; }
        [JsonProperty("name")] public string name { get; set; }
    }
    public class MyController : ControllerBase
    {
        private CompanyInstanceRepository _companyInstanceRepository = new CompanyInstanceRepository();
        public MyController()
        {

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
        [HttpGet("/companies")]
        public async Task<JsonResult> GetCompany()
        {
            var data = new ResponsePattern
            {
                Data = new List<Company>()
                {
                    new Company
                    {
                        id_company = 2193,
                        name = "Nguyen Kim"
                    },
                    new Company
                    {
                        id_company = 1706,
                        name = "W-demo"
                    }
                }
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
