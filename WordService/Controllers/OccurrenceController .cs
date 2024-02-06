using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WordService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OccurrenceController : ControllerBase
    {
        private Database database = Database.GetInstance();

        [HttpPost]
        public async Task<IActionResult> PostAsync(int docId, [FromBody] ISet<int> wordIds)
        {
            await database.InsertAllOccAsync(docId, wordIds);
            return Ok();
        }
    }
}
