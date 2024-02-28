using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public ViewResult Index()
        {
            var model = _employeeRepository.GetAllEmployee();
            return View(model);
        }
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if(ModelState.IsValid)
            {
                Employee newEmployee = _employeeRepository.Add(employee);
                //return RedirectToAction("details", new {id = newEmployee.Id});
            }
            return View(employee);
        }

        public ViewResult Details(int? id) 
        {
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel() 
            { 
                Employee = _employeeRepository.GetEmployee(id??1),
                PageTitle = "Employee Details",
            };
            return View(homeDetailsViewModel);
            //ViewData["Employee"] = model;
            //ViewData["PageTitle"] = "Employee Details";

            //ViewBag.PageTitle = "The value";
            //ViewBag.Employee = model;
            //return View(model);
            //return View("MyViews/TestMyView.cshtml", model);
            //return View("~/MyViews/TestMyView.cshtml", model);
            //return View("~/MyViews/TestMyView.cshtml", model);
            //return View("../Test/Update", model);
        }
        //    private readonly ILogger<HomeController> _logger;

        //    public HomeController(ILogger<HomeController> logger)
        //    {
        //        _logger = logger;
        //    }

        //    public IActionResult Index()
        //    {
        //        return View();
        //    }

        //    public IActionResult Privacy()
        //    {
        //        return View();
        //    }

        //    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //    public IActionResult Error()
        //    {
        //        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //    }
        //}
    }
}
