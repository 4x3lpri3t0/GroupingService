using GroupingService.Entities;
using GroupingService.Services;
using Microsoft.Extensions.Hosting;
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
        private readonly IMatchCreatorService _matchCreatorService;

        public UserController(
            IHostingEnvironment hostingEnvironment,
            IMatchCreatorService matchCreatorService)
        {
            _hostingEnvironment = hostingEnvironment;
            _matchCreatorService = matchCreatorService;
        }

        private const string FileName = "all_match_groups_for_browser.txt";

        [HttpGet()]
        [Route("download/")]
        public ActionResult Download()
        {
            var file = _matchCreatorService.DownloadAllMatches();

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
                _matchCreatorService.AddPlayer(player);
            });

            return new JsonResult(new
            {
                success = true
            });
        }
    }
}
