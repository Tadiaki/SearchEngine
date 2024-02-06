using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WordService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatabaseController : ControllerBase
    {
        private Database database = Database.GetInstance();

        [HttpDelete]
        public async Task DeleteAsync()
        {
            await database.DeleteDatabaseAsync();
        }

        [HttpPost]
        public async Task PostAsync()
        {
            await database.RecreateDatabaseAsync();
        }
    }
}
