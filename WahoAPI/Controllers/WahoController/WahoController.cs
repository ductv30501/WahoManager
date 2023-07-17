using BusinessObjects.WahoModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.WahoRepository;
using ViewModels.DashBoardViewModels;

namespace WahoAPI.Controllers.WahoController
{
    [Route("waho")]
    [ApiController]
    public class WahoController : ControllerBase
    {
        private static readonly IWahoRepositories repository = new WahoRepositories();
        [HttpGet]
        public ActionResult<List<WahoInformation>> getWaho()
        {
            List<WahoInformation> list = new List<WahoInformation>();
            list = repository.GetWaho();
            return Ok(list);
        }
        [HttpGet("byNameEmail")]
        public ActionResult<WahoInformation> getWahoByNameAndEmail(string? name, string? email)
        {
            WahoInformation waho = new WahoInformation();
            waho = repository.GetWahoByNameEmail(name, email);
            if(waho == null) { 
                return NotFound();
            }
            return Ok(waho);
        }
        [HttpPost]
        public IActionResult saveWaho(WahoPostVM wahoPost)
        {
            if(wahoPost == null)
            {
                return BadRequest();
            }
            repository.SaveWaho(wahoPost);
            return Ok();
        }
    }
}
