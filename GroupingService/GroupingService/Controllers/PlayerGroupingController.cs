using System;
using System.Collections.Generic;
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

        //// POST: api/PlayerGrouping
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        [HttpGet()]
        [Route("get/")]
        public string Get()
        {
            return "Hello world!";
        }

        // PUT: api/PlayerGrouping/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            
        }
    }
}
