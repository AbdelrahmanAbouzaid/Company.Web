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
        public async Task<IActionResult> Index()
        {
            var departments = await unitOfWork.DepartmentRepository.GetAllAsync();
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                Department department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };
                await unitOfWork.DepartmentRepository.AddAsync(department);
                var count = await unitOfWork.SaveChangesAsync();

                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id , string viewName="Details")
        {
            if (id is null)
                return BadRequest("Invalid Id");
            var department = await unitOfWork.DepartmentRepository.GetAsync(id.Value);
            if (department is null)
                return NotFound($"DEpartment With Id {id} Is Not Found");
            return View(viewName,department);
        }

        [HttpGet]
        public Task<IActionResult> Edit(int? id)
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
        public async Task<IActionResult> Edit([FromRoute] int id, Department model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest();
                unitOfWork.DepartmentRepository.Update(model);
                var count = await unitOfWork.SaveChangesAsync();

                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public Task<IActionResult> Delete(int? id)
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
        public async Task<IActionResult> ConfirmDelete([FromRoute] int? id)
        {
            if (id is null)
                return BadRequest("Invalid Id");
            var department = await unitOfWork.DepartmentRepository.GetAsync(id.Value);
            if (department is null)
                return NotFound($"DEpartment With Id {id} Is Not Found");
            unitOfWork.DepartmentRepository.Delete(department);
            var count = await unitOfWork.SaveChangesAsync();
            if(count > 0)
                return RedirectToAction(nameof(Index));
            return View(id);
        }


    }
}
