using AutoMapper;
using Demo.PL.ViewModels;
using Deno.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class UserController :Controller 
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public UserController (UserManager<ApplicationUser> userManager , IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
            
        }

        public async Task<IActionResult> Index(string SearchValue)
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                var Users = await _userManager.Users.Select(
                     U => new UserViewModel() {
                        Id = U.Id,
                        FName = U.FName,
                        LName =U.LName,
                        Email = U.Email,
                        PhoneNumber = U.PhoneNumber,
                        Roles =  _userManager.GetRolesAsync(U).Result,
                    }).ToListAsync();

                return View(Users);
            }
            else
            {
                var User = await _userManager.FindByEmailAsync(SearchValue);
                var MappedUser = new UserViewModel()
                {
                    Id = User.Id,
                    FName= User.FName,
                    LName = User.LName,
                    Email = User.Email,
                    PhoneNumber = User.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(User).Result,

                };
                return View(new List<UserViewModel> { MappedUser});
            }

        }

        public async Task<IActionResult> Details(string id ,string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
             var User =await _userManager.FindByIdAsync(id);
            if(User is null)
                return NotFound();
            var MappedUser = _mapper.Map<ApplicationUser, UserViewModel>(User);
            return View( ViewName,MappedUser );

        }

        public async Task<IActionResult> Edit (string id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model , [FromRoute] string Id )
        {
            if(Id != model.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    //can't mapped this user as i wanna update object from db not mapped 
                    //var MappedUser = _mapper.Map<UserViewModel, ApplicationUser>(model);
                    var User = await _userManager.FindByIdAsync(Id); //get it to use change tracker 
                    User.FName = model.FName;
                    User.LName = model.LName;
                    User.PhoneNumber = model.PhoneNumber;
                    await _userManager.UpdateAsync(User);
                    return RedirectToAction(nameof(Index));
                }catch(Exception ex)
                {
                    ModelState.AddModelError(String.Empty ,ex.Message);
                }
               
            }

            return View(model);

        }
        [HttpGet]
        public async Task<IActionResult> Delete(string Id )
        {

            return await Details(Id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(string Id)
        {
            try
            {
                var User = await _userManager.FindByIdAsync(Id);
                await _userManager.DeleteAsync(User);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError (String.Empty ,ex.Message);
                return RedirectToAction("Error", "Home");
            }
            
        }

    }
}
