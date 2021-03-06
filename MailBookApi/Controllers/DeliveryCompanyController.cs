using System.Collections.Generic;
using System.Threading.Tasks;
using MailBookApi.Models;
using MailBookApi.Repos;
using Microsoft.AspNetCore.Mvc;

namespace MailBookApi.Controllers
{
    [Route("/api/deliver-company")]
    [ApiController]
    public class DeliverCompanryController : Controller
    {
        private readonly IMailBookRepository _repo;

        public DeliverCompanryController(IMailBookRepository repo)
        {
            _repo = repo;
        }
        
        [HttpGet]
        public async Task<IEnumerable<DeliverCompanyModel>> GetAsync()
        {
            var models = await _repo.ListDeliverCompanyAsync();

            return models;
        }
    }
}