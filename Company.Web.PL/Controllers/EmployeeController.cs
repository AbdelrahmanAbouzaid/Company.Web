using AutoMapper;
using Company.Web.BLL.Interfaces;
using Company.Web.DAL.Models;
using Company.Web.PL.Dtos;
using Company.Web.PL.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Company.Web.PL.Controllers
{
    public class EmployeeController : Controller
    {
        //private readonly IEmployeeRepository _repository;
        //private readonly IDepartmentRepository _deptRepo;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public EmployeeController(
            //IEmployeeRepository repository,
            //IDepartmentRepository deptRepo,
            IUnitOfWork _unitOfWork,
            IMapper mapper)
        {
            //_repository = repository;
            //_deptRepo = deptRepo;
            unitOfWork = _unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(string? searchInput)
        {
            IEnumerable<Employee> employees;
            if (!string.IsNullOrEmpty(searchInput))
            {
                employees = unitOfWork.EmployeeRepository.GetByName(searchInput);
            }
            else
                employees = unitOfWork.EmployeeRepository.GetAll();

            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var departments = unitOfWork.DepartmentRepository.GetAll();
            ViewData["departments"] = departments;
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                if (model.Image is not null)
                {
                    model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                }
                var employee = mapper.Map<Employee>(model);
                unitOfWork.EmployeeRepository.Add(employee);
                var count = unitOfWork.SaveChanges();
                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Details([FromRoute] int? id)
        {
            if (id is null) return BadRequest("Invalid Id !");
            var employee = unitOfWork.EmployeeRepository.Get(id.Value);
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
        public IActionResult Edit([FromRoute] int? id)
        {
            if (id is null) return BadRequest("Invalid Id !");
            var employee = unitOfWork.EmployeeRepository.Get(id.Value);
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
            var dto = mapper.Map<CreateEmployeeDto>(employee);
            return View(dto);
        }
        [HttpPost]
        public IActionResult Edit([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {   
                if(model.ImageName is not null && model.Image is not null)
                {
                    DocumentSettings.Delete(model.ImageName,"images");
                }
                if (model.Image is not null)
                {
                    model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                }
               
                var employee = mapper.Map<Employee>(model);
                employee.Id = id;
                unitOfWork.EmployeeRepository.Update(employee);
                var count = unitOfWork.SaveChanges();

                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id is null) return BadRequest("Invalid Id !");
            var employee = unitOfWork.EmployeeRepository.Get(id.Value);
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
            var dto = mapper.Map<CreateEmployeeDto>(employee);

            return View(dto);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete([FromRoute] int? id)
        {
            if (id is null)
                return BadRequest("Invalid Id");
            var employee = unitOfWork.EmployeeRepository.Get(id.Value);
            if (employee is null)
                return NotFound($"Employee With Id {id} Is Not Found");
            DocumentSettings.Delete(employee.ImageName,"images");
            unitOfWork.EmployeeRepository.Delete(employee);
            unitOfWork.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
