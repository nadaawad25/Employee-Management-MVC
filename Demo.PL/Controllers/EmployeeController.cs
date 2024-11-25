using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.PL.Helper;
using Demo.PL.ViewModels;
using Deno.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
       private readonly IUnitOfWork _unitOfWork;
        private readonly  IMapper _mapper;

        
        public EmployeeController(
            IUnitOfWork unitOfWork,
               IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index( string SearchValue)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchValue))
                 employees = await _unitOfWork.employeeRepository.GetAllAsync();   
            else
                 employees = _unitOfWork.employeeRepository.GetEmplyeeByName(SearchValue);
           
            var MappedEmployees =
                   _mapper.Map<IEnumerable<Employee>,
                   IEnumerable<EmployeeViewModel>>(employees);
            return View(MappedEmployees);
        }

        public async Task<IActionResult> Details(int? id ,string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var employee =await _unitOfWork.employeeRepository.GetByIdAsync(id.Value);
            //var departments = _unitOfWork.departmentRepository.GetAll();
           
            if (employee is null)
                return NotFound();
            var MappedEmployee = _mapper.Map<Employee , EmployeeViewModel>(employee);
          

            return View(ViewName, MappedEmployee);

        }

        [HttpGet]
        public IActionResult Create()
        {
           
           // ViewBag.Departments = _unitOfWork.departmentRepository.GetAll();
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel  employeeVM)
        {
            //manual mapping 
           if (!ModelState.IsValid)
            {
                employeeVM.ImageName=DocumentSetting.UploadFile(employeeVM.Image, "Images");


                var MappedEmployee = _mapper.Map<EmployeeViewModel , Employee >(employeeVM);
               await _unitOfWork.employeeRepository.AddAsync(MappedEmployee);
                   
                int result =await _unitOfWork.CompleteAsync();
                //as change tacker take care of changes occers and make it in db 
                if (result > 0)
                    TempData["Message"] = "Employee Is Added";

                return RedirectToAction("Index");
            }
            return View(employeeVM);
        }

        [HttpGet]
        public  async Task<IActionResult> Edit(int? id)
        {
            //if (id == null)
            //    return BadRequest();

            //var employee =  _unitOfWork.employeeRepository.GetByIdAsync(id.Value).Result;
            //if (employee == null)
            //    return NotFound();

            //var mappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);


            //mappedEmployee.DepartmentId = employee.DepartmentId;


            //var departments = _unitOfWork.departmentRepository.GetAllAsync().Result
            //    .Select(department => new SelectListItem
            //    {
            //        Value = department.Id.ToString(),
            //        Text = department.Name
            //    }).ToList();

            //ViewBag.Departments = departments;

            //return View(mappedEmployee);
            return await Details( id, "Edit");
        }

        //[ValidateAntiForgeryToken]
        [HttpPost]
       
        public async Task<IActionResult> Edit(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            if (id != employeeVM.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                try
                {
                    if (employeeVM.Image is not null)
                    {
                        employeeVM.ImageName = DocumentSetting.UploadFile(employeeVM.Image, "Images");
                    }

                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.employeeRepository.Update(MappedEmployee);
                    int result = await _unitOfWork.CompleteAsync();

                    if (result > 0)
                        TempData["Message"] = "Employee is Updated";

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

         return View(employeeVM);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id,"Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id , EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
            {
                return BadRequest();
            }
            try
            {
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.employeeRepository.Delete(MappedEmployee);
                int result =await _unitOfWork.CompleteAsync();
                if (result > 0)
                { 
                    TempData["Message2"] = "Employee is Deleted";
                   if (employeeVM.ImageName is not null)
                    {
                        DocumentSetting.Delete(employeeVM.ImageName, "Images");
                    }
                }
                    return RedirectToAction(nameof(Index));
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(employeeVM);
            }
         
            
        }

    }
}
