using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using FINNINGWEB.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using FINNINGWEB.Entities;

namespace FINNINGWEB.Controllers
{
    namespace Users.Controllers
    {
        [Authorize(Roles = "Admins")]
        public class RoleAdminController : Controller
        {
            private RoleManager<IdentityRole> roleManager;
            private UserManager<AppUser> userManager;

            public RoleAdminController(RoleManager<IdentityRole> roleMgr,
            UserManager<AppUser> userMrg)
            {
                roleManager = roleMgr;
                userManager = userMrg;
            }
            private AdventureWorksContext db = new AdventureWorksContext();

            public ViewResult Index() => View(roleManager.Roles);

            public IActionResult Create() => View();
            [HttpPost]
            public async Task<IActionResult> Create([Required]string name)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result
                    = await roleManager.CreateAsync(new IdentityRole(name));
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
                return View(name);
            }
            [HttpPost]
            public async Task<IActionResult> Delete(string id)
            {
                IdentityRole role = await roleManager.FindByIdAsync(id);
                if (role != null)
                {
                    IdentityResult result = await roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "No role found");
                }
                return View("Index", roleManager.Roles);
            }
            private void AddErrorsFromResult(IdentityResult result)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            public async Task<IActionResult> Edit(string id)
            {
                IdentityRole role = await roleManager.FindByIdAsync(id);
                List<AppUser> members = new List<AppUser>();
                List<AppUser> nonMembers = new List<AppUser>();
                foreach (AppUser user in userManager.Users)
                {
                    var list = await userManager.IsInRoleAsync(user, role.Name)
                    ? members : nonMembers;
                    list.Add(user);
                }
                return View(new RoleEditModel
                {
                    Role = role,
                    Members = members,
                    NonMembers = nonMembers
                });
            }
            [HttpPost]
            public async Task<IActionResult> Edit(RoleModificationModel model)
            {
                IdentityResult result;
                IdentityResult result2;
                if (ModelState.IsValid)
                {
                    foreach (string userId in model.IdsToAdd ?? new string[] { })
                    {
                        AppUser user = await userManager.FindByIdAsync(userId);
                        if (user != null)
                        {
                            if (!user.NormalizedUserName.Equals("ADMIN"))
                            {


                                if (db.AspNetUserRoles.FirstOrDefault(c => c.UserId.TrimEnd().Equals(user.Id.TrimEnd())) != null)
                                {
                                    if (!user.NormalizedUserName.Equals("ADMIN"))
                                    {
                                    var rol = db.AspNetUserRoles.FirstOrDefault(c => c.UserId.Equals(user.Id)).RoleId;
                                    var rolename = db.AspNetRoles.FirstOrDefault(c => c.Id.Equals(rol)).NormalizedName;
                                        //result2 = await userManager.RemoveFromRoleAsync(user,
                                        // rolename);
                                        AspNetUserRoles DeleteUserRole = db.AspNetUserRoles.FirstOrDefault(c => c.UserId.TrimEnd() == user.Id);
                                        db.AspNetUserRoles.Remove(DeleteUserRole);
                                        db.SaveChanges();
                                    }
                                //var rol = db.AspNetUserRoles.FirstOrDefault(c => c.UserId.Equals(user.Id)).RoleId;
                                //var rolename = db.AspNetRoles.FirstOrDefault(c=>c.Id.Equals(rol)).NormalizedName;
                                //result2 = await userManager.RemoveFromRoleAsync(user,
                                // rolename);

                                }


                            result = await userManager.AddToRoleAsync(user,
                            model.RoleName);


                            if (!result.Succeeded)
                            {
                                AddErrorsFromResult(result);
                            }
                        }
                        }
                    }
                    foreach (string userId in model.IdsToDelete ?? new string[] { })
                    {
                        AppUser user = await userManager.FindByIdAsync(userId);
                        if (user != null)
                        {
                            if (!user.NormalizedUserName.Equals("ADMIN"))
                            {

                             result = await userManager.RemoveFromRoleAsync(user,
                             model.RoleName);
                            if (!result.Succeeded)
                            {
                                AddErrorsFromResult(result);
                            }
                        }
                        }
                    }
                }
                if (ModelState.IsValid)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return await Edit(model.RoleId);
                }
            }


        }
    }
}


