using Company.Web.BLL.Interfaces;
using Company.Web.DAL.Models;
using Company.Web.PL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Company.Web.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _repository;

        public DepartmentController(IDepartmentRepository repository)
        {
            this._repository = repository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var departments = _repository.GetAll();
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                Department department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };
                int count = _repository.Add(department);
                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}
