using GroupingService.Entities;
using GroupingService.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;

namespace GroupingService.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        public MatchCreatorService MatchCreatorService { get; }

        public UserController()
        {
            // TODO: Dependency Injection
            this.MatchCreatorService = new MatchCreatorService();
        }

        private const string FileName = "match_groups.txt";

        [HttpGet()]
        [Route("download/")]
        public ActionResult Download()
        {
            var file = MatchCreatorService.GenerateMatches(5); // TODO: Pass param
            
            string now = DateTime.Now.ToString("yyyy-dd-M-HH-mm-ss_");
            string fileName = now + FileName;

            return File(Encoding.UTF8.GetBytes(file.ToString()),
                "text/plain",
                fileName);
        }

        // POST: {appDomain}/addUser?name=[name]&skill=[double]&remoteness=[int]
        [HttpPost()]
        [Route("addUser/")]
        public JsonResult AddUser([FromQuery] Player player)
        {
            // TODO: Validate name and ranges...

            MatchCreatorService.AddPlayer(player);

            return new JsonResult(new
            {
                success = true
            });
        }
    }
}
