using DutchTreat.Data;
using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update;
using System;
using System.Linq;
using System.Security.Permissions;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        private readonly INullMailService _mailService;
        private readonly IDutchRepository _repository;

        // don't want to use it directly => direct calls through repository
        // private readonly DutchContext _context;

        public AppController(INullMailService mailserv, IDutchRepository repository) {
            _mailService= mailserv;
            _repository = repository;
        }

        public IActionResult Index()
        {
            // throw new InvalidProgramException("shit happens");
            return View();
        }
         
        [HttpGet("contact")]
        public IActionResult Contact()
        {
            // throw new InvalidOperationException("it happens");
            return View();
        }

        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model) {

            _mailService.SendMessage("address@yahoo.com", model.Subject, $"From {model.Email}, Message: {model.Message}");
            ViewBag.UserMessage = "Mail Sent";
            ModelState.Clear(); // clear the fields
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [Authorize]
        public IActionResult Shop()
        {
            // go to db, get all prods and return them
            // var results = _context.Products.ToList();

            // method 1:
            /* var results = _context.Products
                .OrderBy(p => p.Category)
                .ToList(); */

            // method 2:
            /* var results = from p in _context.Products
                orderby p.Category
                select p; */

            // method 3: using a repo
            var results = _repository.GetAllProducts();
            return View(results.ToList());
        }
    }
}
