using GroupingService.Entities;
using GroupingService.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using System.Threading.Tasks;

namespace GroupingService.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public MatchCreatorService MatchCreatorService { get; }

        public UserController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

            // TODO: Dependency Injection
            this.MatchCreatorService = new MatchCreatorService();
        }

        private const string FileName = "all_match_groups_for_browser.txt";

        [HttpGet()]
        [Route("download/")]
        public ActionResult Download()
        {
            var file = MatchCreatorService.DownloadAllMatches();

            return File(Encoding.UTF8.GetBytes(file.ToString()),
                "text/plain",
                FileName);
        }

        // POST: {appDomain}/addUser?name=[name]&skill=[double]&remoteness=[int]
        [HttpPost()]
        [Route("addUser/")]
        public JsonResult AddUser([FromQuery] Player player)
        {
            Task task = Task.Run(() => {
                MatchCreatorService.AddPlayer(player, _hostingEnvironment);
            });

            return new JsonResult(new
            {
                success = true
            });
        }
    }
}
