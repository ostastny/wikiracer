using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace wikiracer.Controllers
{
    [Route("api/[controller]")]
    public class RaceController : Controller
    {
        private IRace _raceAlgo;

        public RaceController (IRace race)
        {
            _raceAlgo = race;
        }


        // GET api/race
        [HttpGet]
        public async Task<Models.Race> Get(string start, string end)
        {
            return await _raceAlgo.FindPath(start, end); 
        }
    }
}
