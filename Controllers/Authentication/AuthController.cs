using FoodDonationWebApp.Models;
using FoodDonationWebApp.Services.Interfaces;
using FoodDonationWebApp.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodDonationWebApp.Controllers.Authentication
{
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IFileServices _fileServices;

        public AuthController(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              RoleManager<IdentityRole> roleManager,
                              IFileServices services)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _fileServices = services;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            var roles = _roleManager.Roles.ToList();
            var model = new RegisterVM
            {
                Roles = roles.Select(role => new SelectListItem
                {
                    Value = role.Name,
                    Text = role.Name
                })
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var chkEmail = await _userManager.FindByEmailAsync(model.Email);
                    if (chkEmail != null)
                    {
                        ModelState.AddModelError(string.Empty, "Email is already exists");
                        return View(model);
                    }
                    var folderName = "FoodAppImage/ProfilePicture";
                    var userName = $"{model.FirstName}.{model.LastName}".ToLower();
                    
                    string profilePicture = null;
                    if(model.Profile != null)
                    {
                        profilePicture = await _fileServices.SaveFileAsync(model.Profile, folderName);
                    }
                        
                    var user = new ApplicationUser
                    {
                        UserName = userName,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Address = model.Address,
                        PhoneNumber = model.PhoneNo,
                        Profile =  profilePicture
                    };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        if (!await _roleManager.RoleExistsAsync(model.SelectedRole))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(model.SelectedRole));
                        }
                        await _userManager.AddToRoleAsync(user, model.SelectedRole);
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                    if (result.Errors.Count() > 0)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        // Log the error message (this can be done via a logging service)
                        var errorMessage = error.ErrorMessage;
                        var exception = error.Exception; // Useful if there was an exception during model binding
                        Console.WriteLine($"Error: {errorMessage} | Exception: {exception?.Message}");
                    }
                }

                model.Roles = _roleManager.Roles.Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                });
            }
            catch (Exception)
            {
                throw;
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ListUser()
        {
            var users = _userManager.Users.ToList();

            var userListWithRoles = new List<UserWithRolesVM>();

            foreach(var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userListWithRoles.Add(new UserWithRolesVM
                {
                    User = user,
                    Roles = roles
                });

            }
            return View(userListWithRoles);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var checkEmail = await _userManager.FindByEmailAsync(model.Email.ToLower());
                    if (checkEmail == null)
                    {
                        ModelState.AddModelError(string.Empty, "Email is not found");
                        return View(model);
                    }
                    if (await _userManager.CheckPasswordAsync(checkEmail, model.Password) == false)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid Credentials");
                        return View(model);
                    }
                    var result = await _signInManager.PasswordSignInAsync(checkEmail, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempts");
                    return View(model);
                }

            }
            catch (Exception)
            {
                throw;
            }
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Auth");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var rolesAsSelectList = userRoles.Select(role => new SelectListItem
            {
                Value = role,
                Text = role 
            }).ToList();

            var model = new EditUserVM
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Roles = rolesAsSelectList,
                ProilePicture = user.Profile
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    return NotFound();
                }

                // Update the user's properties
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.Address = model.Address;
                user.Profile = model.ProilePicture;
               
                // Update the user in the database
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUser");
                }

                // If there were errors during the update, display them
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        public IActionResult Delete(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            if (user == null)
            {
                return NotFound();
            }

            var result = _userManager.DeleteAsync(user).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("ListUser");
            }
            // Handle deletion failure
            return View("Error");
        }
        public IActionResult View(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            if (user == null)
            {
                return NotFound();
            }
            var roles = _userManager.GetRolesAsync(user).Result;
            var role = roles.FirstOrDefault();
            // Map ApplicationUser to UserProfileVM
            var model = new EditUserVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                ProilePicture = user.Profile, // Assuming user has this property
                SelectedRole = role // If you store roles in the user model
            };

            return View(model); // Pass the ViewModel to the view
        }

    }
}
