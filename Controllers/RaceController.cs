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
        [HttpPost]
        public async Task<ActionResult> Post([FromBody]RaceParameters race)
        {
            if(race == null ||
                string.IsNullOrWhiteSpace(race.Start) ||
                string.IsNullOrEmpty(race.End))
                return BadRequest(new { error = "You must provide a race start and race end"});

            return Json(await _raceAlgo.FindPath(race.Start, race.End)); 
        }
    }
}
