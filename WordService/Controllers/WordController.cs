using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WordService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WordController : ControllerBase
    {
        private Database database = Database.GetInstance();

        [HttpGet]
        public async Task<Dictionary<string, int>> GetAsync()
        {
            return await database.GetAllWordsAsync();
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Dictionary<string, int> res)
        {
            await database.InsertAllWordsAsync(res);
            return Ok();
        }
    }
}
