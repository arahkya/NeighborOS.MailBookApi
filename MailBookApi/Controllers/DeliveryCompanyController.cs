using System.Collections.Generic;
using System.Threading.Tasks;
using MailBookApi.Models;
using MailBookApi.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace MailBookApi.Controllers
{
    [Authorize]
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
            HttpContext.VerifyUserHasAnyAcceptedScope("MailBook.Read");

            var models = await _repo.ListDeliverCompanyAsync();

            return models;
        }
    }
}