    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    namespace WordService.Controllers
    {
        [ApiController]
        [Route("[controller]")]
        public class DocumentController : ControllerBase
        {
            private Database database = Database.GetInstance();

            [HttpGet("GetByDocIds")]
            public async Task<List<string>> GetByDocIdsAsync([FromQuery] List<int> docIds)
            {
                return await database.GetDocDetailsAsync(docIds);
            }

            [HttpGet("GetByWordIds")]
            public async Task<Dictionary<int, int>> GetByWordIdsAsync([FromQuery] List<int> wordIds)
            {
                return await database.GetDocumentsAsync(wordIds);
            }

            [HttpPost]
            public async Task<IActionResult> PostAsync(int id, string url)
            {
                await database.InsertDocumentAsync(id, url);
                return Ok();
            }
        }
    }
