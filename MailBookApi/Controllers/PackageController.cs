using Microsoft.AspNetCore;
using MailBookApi.Repos;
using Microsoft.AspNetCore.Mvc;
using MailBookApi.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MailBookApi.Controllers
{
    [Route("/api/package")]
    [ApiController]
    public class PackageController : Controller
    {
        private readonly IMailBookRepository _repo;

        public PackageController(IMailBookRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreatePackageModel model)
        {
            await _repo.CreatePackageEntryAsync(model);

            return Ok();
        }

        [HttpGet]
        public async Task<IEnumerable<PackageModel>> List()
        {
            var packageModelList = await _repo.ListPackageAsync();

            return packageModelList;
        }         
    }
}