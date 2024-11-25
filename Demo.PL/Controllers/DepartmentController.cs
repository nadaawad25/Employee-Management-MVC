using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Deno.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
      private IUnitOfWork _unitOfWork;
        public DepartmentController(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()

        {
            var departments = await _unitOfWork.departmentRepository.GetAllAsync();
            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid) //property inside action(server side validation )
            {
            
                await _unitOfWork.departmentRepository.AddAsync(department);
                int result = await _unitOfWork.CompleteAsync();
                if (result>0)
                    TempData["Message"] = "Departmnet is created";


                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }
        public async Task<IActionResult> Details(int? id , string ViewName="Details")
        {
            if (id is null)
                return BadRequest();
            var department =await _unitOfWork.departmentRepository.GetByIdAsync(id.Value);
            if(department is null)
                return NotFound();
            return View( ViewName,department);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            //if(id is null)
            //    return BadRequest();
            //var department = _departmentRepository.GetById(id.Value);
            //if (department is null)
            //    return NotFound();
            //return View(department);
            return await Details(id ,"Edit");

        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(Department department, [FromRoute] int id )
        {
            if(id!= department.Id)
                return BadRequest();
            
            
            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.departmentRepository.Update(department);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex) {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(department);
        }

        [HttpGet]

        public async Task<IActionResult> Delete(int? id)
        {
            //if (id is null)
            //    return BadRequest();
            //var department = _departmentRepository.GetById(id.Value);
            //if (department is null)
            //    return NotFound();
            return await Details(id, "Delete");
        }

        // POST: Actually Delete the department
        
        [HttpPost]

        public async Task<IActionResult> Delete(Department department ,[FromRoute]int id)
        {
            if(department.Id!= id)
                return BadRequest();
            try
            {
                _unitOfWork.departmentRepository.Delete(department);
                
                var result = await _unitOfWork.CompleteAsync();
                if (result > 0)
                    TempData["Message2"] = "Department is Deleted";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            
            { 
                ModelState.AddModelError(string.Empty , ex.Message);
                return View(department);
            }
              

               
        }
    }
}
