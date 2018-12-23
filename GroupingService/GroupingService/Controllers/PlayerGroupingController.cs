using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GroupingService.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        //// GET: api/PlayerGrouping
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/PlayerGrouping/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        [HttpGet()]
        [Route("get/")]
        public string Get()
        {
            return "Hello world!";
        }

        // POST: {appDomain}/addUser?name=[name]&skill=[double]&remoteness=[int]
        [HttpPost()]
        [Route("addUser/")]
        public void AddUser(
            [FromQuery(Name = "name")] string name,
            [FromQuery(Name = "skill")] double skill,
            [FromQuery(Name = "remoteness")] int remoteness)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
        }
    }
}
