using Company.Web.BLL.Interfaces;
using Company.Web.DAL.Models;
using Company.Web.PL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Company.Web.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeController(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var employees = _repository.GetAll();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee()
                {
                    Name = model.Name,
                    Age = model.Age,
                    Address = model.Address,
                    Phone = model.Phone,
                    Email = model.Email,
                    Salary = model.Salary,
                    CreateAt = model.CreateAt,
                    HiringDate = model.HiringDate,
                    IsActive = model.IsActive,
                    IsDelete = model.IsDelete,
                };
                var count = _repository.Add(employee);
                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Details([FromRoute] int? id)
        {
            if (id is null) return BadRequest("Invalid Id !");
            var employee = _repository.Get(id.Value);
            if (employee is null)
                return NotFound($"Employee With Id {id} Is Not Found");

            //var dto = new CreateEmployeeDto()
            //{
            //    Name = employee.Name,
            //    Age = employee.Age,
            //    Address = employee.Address,
            //    Phone = employee.Phone,
            //    Email = employee.Email,
            //    Salary = employee.Salary,
            //    CreateAt = employee.CreateAt,
            //    HiringDate = employee.HiringDate,
            //    IsActive = employee.IsActive,
            //    IsDelete = employee.IsDelete,
            //};
            return View(employee);
        }

        [HttpGet]
        public IActionResult Edit([FromRoute]int? id)
        {
            if (id is null) return BadRequest("Invalid Id !");
            var employee = _repository.Get(id.Value);
            if (employee is null)
                return NotFound($"Employee With Id {id} Is Not Found");
            var dto = new CreateEmployeeDto()
            {
                Name = employee.Name,
                Age = employee.Age,
                Address = employee.Address,
                Phone = employee.Phone,
                Email = employee.Email,
                Salary = employee.Salary,
                CreateAt = employee.CreateAt,
                HiringDate = employee.HiringDate,
                IsActive = employee.IsActive,
                IsDelete = employee.IsDelete,
            };
            return View(dto);
        }
        [HttpPost]
        public IActionResult Edit([FromRoute] int id,CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee()
                {
                    Name = model.Name,
                    Age = model.Age,
                    Address = model.Address,
                    Phone = model.Phone,
                    Email = model.Email,
                    Salary = model.Salary,
                    CreateAt = model.CreateAt,
                    HiringDate = model.HiringDate,
                    IsActive = model.IsActive,
                    IsDelete = model.IsDelete,
                    Id = id
                };
                var count = _repository.Update(employee);
                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id is null) return BadRequest("Invalid Id !");
            var employee = _repository.Get(id.Value);
            if (employee is null)
                return NotFound($"Employee With Id {id} Is Not Found");
            var dto = new CreateEmployeeDto()
            {
                Name = employee.Name,
                Age = employee.Age,
                Address = employee.Address,
                Phone = employee.Phone,
                Email = employee.Email,
                Salary = employee.Salary,
                CreateAt = employee.CreateAt,
                HiringDate = employee.HiringDate,
                IsActive = employee.IsActive,
                IsDelete = employee.IsDelete,
            };
            return View(dto);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete([FromRoute] int? id)
        {
            if (id is null)
                return BadRequest("Invalid Id");
            var employee = _repository.Get(id.Value);
            if (employee is null)
                return NotFound($"Employee With Id {id} Is Not Found");
            _repository.Delete(employee);
            return RedirectToAction(nameof(Index));
        }
    }
}
