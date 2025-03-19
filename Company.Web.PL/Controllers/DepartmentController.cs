using Company.Web.BLL.Interfaces;
using Company.Web.DAL.Models;
using Company.Web.PL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Company.Web.PL.Controllers
{
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepository _repository;
        private readonly IUnitOfWork unitOfWork;

        public DepartmentController(/*IDepartmentRepository repository*/IUnitOfWork _unitOfWork)
        {
            //this._repository = repository;
            unitOfWork = _unitOfWork;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var departments = unitOfWork.DepartmentRepository.GetAll();
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                unitOfWork.DepartmentRepository.Add(department);
                var count = unitOfWork.SaveChanges();

                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int? id , string viewName="Details")
        {
            if (id is null)
                return BadRequest("Invalid Id");
            var department = unitOfWork.DepartmentRepository.Get(id.Value);
            if (department is null)
                return NotFound($"DEpartment With Id {id} Is Not Found");
            return View(viewName,department);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            //if (id is null)
            //    return BadRequest("Invalid Id");
            //var department = _repository.Get(id.Value);
            //if (department is null)
            //    return NotFound($"DEpartment With Id {id} Is Not Found");
            return Details(id,"Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Department model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest();
                unitOfWork.DepartmentRepository.Update(model);
                var count = unitOfWork.SaveChanges();

                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            //if (id is null)
            //    return BadRequest("Invalid Id");
            //var department = _repository.Get(id.Value);
            //if (department is null)
            //    return NotFound($"DEpartment With Id {id} Is Not Found");
            //return View(department);
            return Details(id,"Delete");
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete([FromRoute] int? id)
        {
            if (id is null)
                return BadRequest("Invalid Id");
            var department = unitOfWork.DepartmentRepository.Get(id.Value);
            if (department is null)
                return NotFound($"DEpartment With Id {id} Is Not Found");
            unitOfWork.DepartmentRepository.Delete(department);
            return RedirectToAction(nameof(Index));
        }


    }
}
