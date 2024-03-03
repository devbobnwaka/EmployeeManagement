using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HomeController(IEmployeeRepository employeeRepository,
                IWebHostEnvironment hostingEnvironment
            )
        {
            _employeeRepository = employeeRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        public ViewResult Index()
        {
            var model = _employeeRepository.GetAllEmployee();
            return View(model);
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel()
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department  = employee.Department,
                ExistingPhotoPath = employee.PhotoPath

            };
            return View(employeeEditViewModel);
        }



        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepository.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;
                if (model.Photo != null)
                {
                    //to delete file from the wwwroot 
                    if(model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    //End
                    employee.PhotoPath = ProcessUplodedFile(model);
                }

                _employeeRepository.Update(employee);
                return RedirectToAction("index");
            }
            return View(model);
        }

        private string ProcessUplodedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUplodedFile(model);
                //if (model.Photos != null && model.Photos.Count > 0)
                //{
                //    foreach (IFormFile photo in model.Photos)
                //    {
                //        string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                //        uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                //        string filePath = Path.Combine(uploadFolder, uniqueFileName);
                //        photo.CopyTo(new FileStream(filePath, FileMode.Create));
                //    }
                //}
                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                };
                _employeeRepository.Add(newEmployee);
                return RedirectToAction("details", new {id = newEmployee.Id});
            }
            return View();
        }

        /*
            ============INITIAL CREATE=============
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

        */

        public IActionResult Details(int? id) 
        {
            if (id == null)
            {
                return RedirectToAction("index"); ;
            }
            Employee employee = _employeeRepository.GetEmployee(id.Value);
            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel() 
            { 
                Employee = employee,
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
