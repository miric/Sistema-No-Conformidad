using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FINNINGWEB.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using FINNINGWEB.Entities;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace FINNINGWEB.Controllers
{
    namespace Users.Controllers
    {
        [Authorize(Roles = "Admins")]
        public class AdminController : Controller
        {

            private AdventureWorksContext db = new AdventureWorksContext();
            private UserManager<AppUser> userManager;
            private IUserValidator<AppUser> userValidator;
            private IPasswordValidator<AppUser> passwordValidator;
            private IPasswordHasher<AppUser> passwordHasher;



            public AdminController(UserManager<AppUser> usrMgr,
            IUserValidator<AppUser> userValid,
            IPasswordValidator<AppUser> passValid,
            IPasswordHasher<AppUser> passwordHash)
            {
                userManager = usrMgr;
                userValidator = userValid;
                passwordValidator = passValid;
                passwordHasher = passwordHash;
            }

            //Hay que cambiarlo por la clase usuarioPersona
            //public ViewResult Index() => View(userManager.Users);
            public ViewResult Index()
            {

                usuariosPersonas usuariosPersonas = new usuariosPersonas()
                {
                    ListaPersonas = db.Persona.ToList(),
                    ListaUsuarios = db.AspNetUsers.ToList(),
                    ListaRoles = db.AspNetRoles.ToList(),
                    ListaUserRoles = db.AspNetUserRoles.ToList(),


                };


                return View(usuariosPersonas);
            }

            public ViewResult CuentasPersonas()
            {

       

                usuariosPersonas usuariosPersonas = new usuariosPersonas()
                {
                    
                    ListaPersonas = db.Persona.ToList().Where(c => !c.Removed.Equals(true)),
                    //ListaPersonas = db.Persona.ToList(),
                    ListaUsuarios = db.AspNetUsers.ToList(),
                    ListaAreas = db.Area.ToList(),
                    ListaSubAreas = db.SubArea.ToList(),

                };


                return View(usuariosPersonas);

            }

            //public ViewResult Create() => View();


            public async Task<IActionResult> FormularioCrearPersona()
            {

                CreatePersona model = new CreatePersona()
                {
                    //ListaPersonas = db.Persona.ToList(),
                    ListaAreas = db.Area.ToList(),
                    ListaSubAreas = db.SubArea.ToList(),
                };
                //ViewBag.Area = db.Area.ToList();

                return View(model);
            }
            [HttpPost]
            public async Task<IActionResult> FormularioCrearPersona(CreatePersona model)
            {
                if (ModelState.IsValid)
                {
                        string rut = model.rut.TrimEnd().Replace(".", "").ToUpper();
                        rut = rut.Replace(",", "");
                        rut = rut.Replace("-", "");
                        rut = rut.Replace("_", "");

                    if (db.Persona.FirstOrDefault(c => c.Rut.TrimEnd().Equals(rut)) != null)
                    {
                        ModelState.AddModelError("Error", "Ya existe una persona con ese rut, ingresar un rut distinto para continuar");
                        return await FormularioCrearPersona();
                    }
                    Persona persona = new Persona()
                    {
                        Rut = rut,
                        Nombre = model.Nombre,
                        ApellidoPaterno = model.ApellidoPaterno,
                        ApellidoMaterno = model.ApellidoMaterno,
                        Sexo = model.Sexo,
                        RutSupervisor = model.RutSupervisor,
                        IdArea = model.IdArea,
                        IdSubArea = model.IdSubArea,
                        Removed = false,
                        Cargo = model.Cargo,

                    };
                    db.Persona.Add(persona);
                    db.SaveChanges();

                    return RedirectToAction("CuentasPersonas");

                }
                else
                {
                    return await FormularioCrearPersona();
                }



            }
            public JsonResult getPersonaModalAjax(string valor)
            {
                IEnumerable<Area> ListaArea = db.Area.ToList();
                IEnumerable<SubArea> ListaSubArea = db.SubArea.ToList();

                List<Persona> list = new List<Persona>();
                list = db.Persona.Where(c => (c.Nombre.TrimEnd() + " " + c.ApellidoPaterno.TrimEnd() + " " + c.ApellidoMaterno.TrimEnd()).ToUpper().Contains(valor.ToUpper())).ToList();
                List<DatosPersona> Datoslist = new List<DatosPersona>();
                int contador = 0;
                foreach (Persona persona in list)
                {
                    string areaVariable = "";
                    string SubAreaVariable = "";
                    string nombreCompleto = "";
                    string cargoVariable = "";

                    if (ListaArea.FirstOrDefault(c => c.Id == persona.IdArea) != null)
                    {
                        areaVariable = ListaArea.FirstOrDefault(c => c.Id == persona.IdArea).Nombre;
                    }
                    if (ListaSubArea.FirstOrDefault(c => c.Id == persona.IdSubArea) != null)
                    {
                        SubAreaVariable = ListaSubArea.FirstOrDefault(c => c.Id == persona.IdSubArea).Nombre;
                    }
                    if (persona.Cargo != null)
                    {
                        cargoVariable = persona.Cargo;
                    }
                    nombreCompleto = persona.Nombre.TrimEnd() + " " + persona.ApellidoPaterno.TrimEnd() + " " + persona.ApellidoMaterno.TrimEnd();
                    Datoslist.Insert(contador, new DatosPersona { rut = persona.Rut, Nombre = nombreCompleto, Area = areaVariable, SubArea = SubAreaVariable, Cargo = cargoVariable });
                    contador++;
                }

                //list.Insert(0, new SubArea { Id = 0, Nombre = "Please Select SubArea" });
                //return Json(new SelectList(list,"Rut", "ApellidoPaterno"));
                return Json(Datoslist);
            }
            public JsonResult getAreaId(int id)
            {
                List<SubArea> list = new List<SubArea>();
                list = db.SubArea.Where(c => c.IdArea == id).ToList();
                list.Insert(0, new SubArea { Id = 0, Nombre = "Please Select SubArea" });
                return Json(new SelectList(list, "Id", "Nombre"));
            }



            public IActionResult DeletePersona(string rut)
            {

                Persona updatePersona = db.Persona.FirstOrDefault(c => c.Rut.Equals(rut));
                updatePersona.Removed = true;
                db.Persona.Update(updatePersona);
                db.SaveChanges();
                return RedirectToAction("CuentasPersonas");

            }

            public ViewResult FormularioEditPersona(string rut)
            {

                Persona persona = db.Persona.FirstOrDefault(c => c.Rut == rut);
                string nombrecompleto = "";
                if (persona.RutSupervisor!=null)
                {
                    nombrecompleto = db.Persona.Select(c => new { nombreCompleto = c.Nombre.TrimEnd() + " " + c.ApellidoPaterno.TrimEnd() + " " + c.ApellidoMaterno.TrimEnd(), rut = c.Rut }).FirstOrDefault(c => c.rut.TrimEnd().Equals(persona.RutSupervisor)).nombreCompleto;
                }
                string rutpersona = null;
                string nombrepersona = null;
                string appersona = null;
                string ampersona = null;
                string cargopersona = null;
                if (persona.Rut != null)
                {
                    rutpersona = persona.Rut.TrimEnd();
                }
                if (persona.Nombre!=null)
                {
                    nombrepersona = persona.Nombre.TrimEnd();
                }
                if (persona.ApellidoPaterno != null)
                {
                    appersona = persona.ApellidoPaterno.TrimEnd();
                }
                if (persona.ApellidoMaterno != null)
                {
                    ampersona = persona.ApellidoMaterno.TrimEnd();
                }
                if (persona.Cargo != null)
                {
                    cargopersona = persona.Cargo.TrimEnd();
                }
                CreatePersona model = new CreatePersona()
                {
                    rut = rutpersona,
                    Nombre = nombrepersona,
                    ApellidoPaterno = appersona,
                    ApellidoMaterno = ampersona,
                    NombreCompleto = nombrecompleto,
                    Sexo = persona.Sexo,
                    RutSupervisor = persona.RutSupervisor,
                    IdArea = Convert.ToInt32(persona.IdArea),
                    IdSubArea = Convert.ToInt32(persona.IdSubArea),
                    Cargo = cargopersona,
                    //ListaPersonas = db.Persona.ToList(),
                    ListaAreas = db.Area.ToList(),
                    ListaSubAreas = db.SubArea.ToList(),
                };


                return View(model);
            }


            [HttpPost]
            public IActionResult FormularioEditPersona(CreatePersona model)
            {
                if (ModelState.IsValid)
                {
                    Persona updatePersona = db.Persona.FirstOrDefault(c => c.Rut == model.rut);
                    updatePersona.Nombre = model.Nombre;
                    updatePersona.ApellidoPaterno = model.ApellidoPaterno;
                    updatePersona.ApellidoMaterno = model.ApellidoMaterno;
                    updatePersona.Sexo = model.Sexo;
                    updatePersona.RutSupervisor = model.RutSupervisor;
                    updatePersona.IdArea = model.IdArea;
                    updatePersona.IdSubArea = model.IdSubArea;
                    updatePersona.Cargo = model.Cargo;
                    db.Persona.Update(updatePersona);
                    db.SaveChanges();

                    return RedirectToAction("CuentasPersonas");
                }
                else
                {
                    return FormularioEditPersona(model.rut.TrimEnd());
                }





            }



            public ViewResult Create()
            {
                CreateModel model = new CreateModel()
                {
                    ListaRoles = db.AspNetRoles.ToList(),
                    ListaPersonas = db.Persona.ToList(),
                };

                return View(model);
            }

           

            [HttpPost]
            public async Task<IActionResult> Create(CreateModel model)
            {
                if (ModelState.IsValid)
                {
                    AppUser user = new AppUser
                    {
                        UserName = model.Name,
                        Email = model.Email,
                        rutPersona = model.rutPersona,


                    };
                    IdentityResult result
                    = await userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        AspNetUsers UserRol = db.AspNetUsers.FirstOrDefault(c => c.UserName == model.Name);
                        AspNetUserRoles ur = new AspNetUserRoles()
                        {
                            RoleId = model.rol,
                            UserId = UserRol.Id,
                        };
                        db.AspNetUserRoles.Add(ur);
                        db.SaveChanges();

                        return RedirectToAction("Index");
                        //return AgregarRolUser(ur);
                    }
                    else
                    {
                        foreach (IdentityError error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        model.ListaPersonas = db.Persona.ToList();
                        model.ListaRoles = db.AspNetRoles.ToList();
                    }
                    return View(model);
                }
                else
                {
                    return Create();
                }
               
            }

            public IActionResult AgregarRolUser(AspNetUserRoles ur)
            {

                db.AspNetUserRoles.Add(ur);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            [HttpPost]
            public async Task<IActionResult> Delete(string id)
            {
                AppUser user = await userManager.FindByIdAsync(id);
                if (user != null)
                {
                    IdentityResult result = await userManager.DeleteAsync(user);
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

                    ModelState.AddModelError("", "User Not Found");
                }
                return View("Index", userManager.Users);
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
                AppUser user = await userManager.FindByIdAsync(id);
                CreateModel model = new CreateModel()
                {
                    id = id,
                    Email = db.AspNetUsers.FirstOrDefault(c => c.Id == id).Email,
                    Name = db.AspNetUsers.FirstOrDefault(c => c.Id == id).UserName,
                    rutPersona = db.AspNetUsers.FirstOrDefault(c => c.Id == id).RutPersona,
                    rol = db.AspNetRoles.FirstOrDefault(d => d.Id == db.AspNetUserRoles.FirstOrDefault(c => c.UserId == id).RoleId).Id,


                    Password = null,
                    ListaPersonas = db.Persona.ToList(),
                    ListaRoles = db.AspNetRoles.ToList(),
                    usuarioUnico = user,
                };

                return View(model);
            }






            [HttpPost]
            public async Task<IActionResult> Edit(CreateModel model)
            {
                if (ModelState.IsValid)
                {

                    AppUser user = await userManager.FindByIdAsync(model.id);
                    if (user != null)
                    {
                        model.ListaPersonas = db.Persona.ToList();
                        model.ListaRoles = db.AspNetRoles.ToList();
                        user.Email = model.Email;
                        user.UserName = model.Name;
                        user.rutPersona = model.rutPersona;
                        IdentityResult validEmail
                        = await userValidator.ValidateAsync(userManager, user);



                        if (!validEmail.Succeeded)
                        {
                            AddErrorsFromResult(validEmail);
                        }
                        IdentityResult validPass = null;
                        if (!string.IsNullOrEmpty(model.Password))
                        {
                            validPass = await passwordValidator.ValidateAsync(userManager,
                            user, model.Password);
                            if (validPass.Succeeded)
                            {
                                user.PasswordHash = passwordHasher.HashPassword(user,
                                model.Password);
                            }
                            else
                            {
                                AddErrorsFromResult(validPass);
                            }
                        }
                        if ((validEmail.Succeeded && validPass == null)
                        || (validEmail.Succeeded
                        && model.Password != string.Empty && validPass.Succeeded))
                        {
                            IdentityResult result = await userManager.UpdateAsync(user);
                            if (result.Succeeded)
                            {
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                AddErrorsFromResult(result);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "User Not Found");

                    }
                    return View(model);
                }
                else
                {
                    return await Edit(model.id);
                }


            }

        }
    }
}