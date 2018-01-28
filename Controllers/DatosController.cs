using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FINNINGWEB.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using FINNINGWEB.Entities;
using System.Linq;
using System;
using System.Collections.Generic;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FINNINGWEB.Controllers
{
    [Authorize(Roles = "Admins,Calidad")]
    public class DatosController : Controller
    {
        private UserManager<AppUser> userManager;
        public DatosController(UserManager<AppUser> userMgr)
        {
            userManager = userMgr;
        }

        private AdventureWorksContext db = new AdventureWorksContext();

        [Authorize]
        public IActionResult Index()
        {
            List<Persona> listaJefeArea = new List<Persona>();
            foreach (PersonaArea pa in db.PersonaArea.ToList())
            {
                Persona persona2 = db.Persona.FirstOrDefault(c => c.Rut.TrimEnd().Equals(pa.RutPersona.TrimEnd()));
                listaJefeArea.Add(persona2);
            }

            ListaArea Area = new ListaArea()
            {
                ListaPersonas = listaJefeArea,
                ListaAreas = db.Area.ToList().Where(c=>c.Removed!=true),
                ListaPersonaArea = db.PersonaArea.ToList(),
            };

        


            return View(Area);
        }


        [Authorize]
        public IActionResult IndiceCliente()
        {

            ListaCliente cliente = new ListaCliente()
            {

                ListaClientes = db.Cliente.ToList().Where(c => !c.Removed == true),
            };

            return View(cliente);
        }

        [Authorize]
        public IActionResult IndiceProcesos()
        {

            ListaProceso1 lp = new ListaProceso1()
            {

                ListaProcesos1 = db.Proceso1.ToList(),
            };

            return View(lp);
        }



        public IActionResult FormularioCrearCliente()
        {

            ListaCliente cliente = new ListaCliente()
            {

                ListaClientes = db.Cliente.ToList(),

            };


            return View(cliente);
        }
        [HttpPost]
        public async Task<IActionResult> FormularioCrearCliente(ListaCliente model)
        {

            if (ModelState.IsValid)
            {
                Cliente c = new Cliente()
                {
                    Nombre = model.nombre,
                    Removed = false,

                };
                db.Cliente.Add(c);
                db.SaveChanges();
                return RedirectToAction("IndiceCliente");
            }
            else
            {
                return FormularioCrearCliente();
            }

           
        }



        public IActionResult DeleteCliente(int ClienteId)
        {

            Cliente DeleteCliente = db.Cliente.FirstOrDefault(c => c.Id == ClienteId);
            DeleteCliente.Removed = true;

            db.Cliente.Update(DeleteCliente);
            db.SaveChanges();

            return RedirectToAction("IndiceCliente");

        }

        public IActionResult FormularioEditCliente(int ClienteId)
        {

            ListaCliente lc = new ListaCliente()
            {
                //Id = AreaId,
                //Nombre = db.Area.FirstOrDefault(c => c.Id == AreaId).Nombre,
                //RutJefe = db.Area.FirstOrDefault(c => c.Id == AreaId).RutJefe,
                //ListaPersonas = db.Persona.ToList(),
                ClienteUnico = db.Cliente.FirstOrDefault(c => c.Id == ClienteId),
            };

            return View(lc);
        }

        [HttpPost]
        public async Task<IActionResult> FormularioEditCliente(ListaCliente model)
        {
            if (ModelState.IsValid)
            {
                Cliente updateCliente = db.Cliente.FirstOrDefault(c => c.Id == model.ClienteUnico.Id);
                updateCliente.Nombre = model.nombre;

                db.Cliente.Update(updateCliente);
                db.SaveChanges();

                return RedirectToAction("IndiceCliente");
            }
            else
            {
                return FormularioEditCliente(model.ClienteUnico.Id);
            }
    

        }




        public IActionResult FormularioCrearArea()
        {

            ListaArea Area = new ListaArea()
            {

                //ListaPersonas = db.Persona.ToList(),

            };


            return View(Area);
        }
        [HttpPost]
        public IActionResult FormularioCrearArea(ListaArea model)
        {

            if (ModelState.IsValid)
            {
                Area area = new Area()
                {
                    Nombre = model.NombreArea,
                    //RutJefe = model.RutPersona,
                    //RutJefe = model.AreaUnica.RutJefe
                    Removed = false,
                };
                db.Area.Add(area);
                db.SaveChanges();

                if (model.RutPersona!=null)
                {
                    PersonaArea pa = new PersonaArea()
                    {
                        RutPersona = model.RutPersona.TrimEnd(),
                        IdArea = area.Id,

                    };
                    db.PersonaArea.Add(pa);
                    db.SaveChanges();
                }


               



                //if (db.Persona.FirstOrDefault(c => c.Rut.Equals(model.RutPersona)) != null)
                //{
                //    Persona updatePersona = db.Persona.FirstOrDefault(c => c.Rut.TrimEnd().Equals(model.RutPersona.TrimEnd()));
                //    updatePersona.IdAreaJefeArea = area.Id;
                //    db.Persona.Update(updatePersona);
                //    db.SaveChanges();
                //}
                return RedirectToAction("Index");
            }
            else
            {
                return FormularioCrearArea();
            }

     
        }


        public IActionResult FormularioEditArea(int AreaId)
        {
            string rutJefe = null;
         
            if (db.PersonaArea.FirstOrDefault(c => c.IdArea == AreaId) != null)
            {
                rutJefe = db.PersonaArea.FirstOrDefault(c => c.IdArea == AreaId).RutPersona;
            };

            Area AU = db.Area.FirstOrDefault(c => c.Id == AreaId);
            ListaArea Area1 = new ListaArea()
            {

                AreaUnica = AU,
                RutJefeArea = rutJefe,
                ListaPersonas = db.Persona.ToList(),
                RutPersona = rutJefe,
            };

            string nombrecompleto = null;
            if (rutJefe != null)
            {
                nombrecompleto = db.Persona.Select(c => new { nombreCompleto = c.Nombre.TrimEnd() + " " + c.ApellidoPaterno.TrimEnd() + " " + c.ApellidoMaterno.TrimEnd(), rut = c.Rut }).FirstOrDefault(c => c.rut.TrimEnd().Equals(rutJefe.TrimEnd())).nombreCompleto;
            }
            Area1.NombreCompleto = nombrecompleto;


            return View(Area1);
        }
        [HttpPost]
        public IActionResult FormularioEditArea(ListaArea model)
        {
            if (ModelState.IsValid)
            {
                Area updateArea = db.Area.FirstOrDefault(c => c.Id == model.AreaUnica.Id);
                updateArea.Nombre = model.NombreArea;
                //updateArea.RutJefe = model.AreaUnica.RutJefe;
                db.Area.Update(updateArea);
                db.SaveChanges();

               
                    if (db.PersonaArea.FirstOrDefault(c=> c.IdArea == model.AreaUnica.Id)!=null)
                    {
                        PersonaArea DeletePersonaArea = db.PersonaArea.FirstOrDefault(c => c.IdArea == model.AreaUnica.Id);
                        db.PersonaArea.Remove(DeletePersonaArea);
                        db.SaveChanges();

                        PersonaArea pa = new PersonaArea()
                        {
                            RutPersona = model.RutPersona.TrimEnd(),
                            IdArea = model.AreaUnica.Id

                        };
                        db.PersonaArea.Add(pa);
                        db.SaveChanges();

                    }
                    else
                    {
                    if (model.RutPersona != null)
                    {
                        PersonaArea pa = new PersonaArea()
                        {
                            RutPersona = model.RutPersona.TrimEnd(),
                            IdArea = model.AreaUnica.Id

                        };
                        db.PersonaArea.Add(pa);
                        db.SaveChanges();
                    }
      

                    }

                //if (db.Persona.FirstOrDefault(c => c.Rut.Equals(model.RutPersona)) != null)
                //{
                //    if (db.Persona.FirstOrDefault(c => c.IdAreaJefeArea == model.AreaUnica.Id) != null)
                //    {
                //        Persona updatePersona2 = db.Persona.FirstOrDefault(c => c.IdAreaJefeArea == model.AreaUnica.Id);
                //        updatePersona2.IdAreaJefeArea = null;
                //        db.Persona.Update(updatePersona2);
                //        db.SaveChanges();
                //    }


                //    Persona updatePersona = db.Persona.FirstOrDefault(c => c.Rut.TrimEnd().Equals(model.RutPersona.TrimEnd()));
                //    updatePersona.IdAreaJefeArea = model.AreaUnica.Id;
                //    db.Persona.Update(updatePersona);
                //    db.SaveChanges();
                //}


                return RedirectToAction("Index");
            }
            else
            {
                return FormularioEditArea(model.AreaUnica.Id);
            }


        }
        public ViewResult IndiceSubAreas(int AreaId)
        {
            ListaArea SubArea = new ListaArea()
            {
                //ListaPersonas = db.Persona.ToList(),
                //ListaSupervisores = db.Persona.ToList().Where(c=>c.IdSubAreaJefeSubArea!=null),
                ListaAreas = db.Area.ToList(),
                ListaSubAreas = db.SubArea.ToList().Where(c => c.IdArea == AreaId && c.Removed!=true),
                AreaUnica = db.Area.FirstOrDefault(c => c.Id == AreaId),
            };


            return View(SubArea);
        }

        public IActionResult FormularioCrearSubArea(int AreaId)
        {


            ListaArea SubArea1 = new ListaArea()
            {
                AreaUnica = db.Area.FirstOrDefault(c => c.Id == AreaId),
                SubAreaUnica = new SubArea()
                {
                    IdArea = AreaId,
                }


            };


            return View(SubArea1);
        }
        [HttpPost]
        public IActionResult FormularioCrearSubArea(ListaArea model)
        {
            if (ModelState.IsValid)
            {
                SubArea subarea = new SubArea()
                {
                    Nombre = model.NombreSubArea,
                    IdArea = model.AreaUnica.Id
                   
                };
                db.SubArea.Add(subarea);
                db.SaveChanges();
                return RedirectToAction("IndiceSubAreas", new { AreaId = model.AreaUnica.Id });
            }
            else
            {
                return FormularioCrearSubArea(model.AreaUnica.Id);
            }

           
        }

        public IActionResult FormularioEditSubArea(int SubAreaId)
        {


            ListaArea Area1 = new ListaArea()
            {

                SubAreaUnica = db.SubArea.FirstOrDefault(c => c.Id == SubAreaId),
                AreaUnica = db.Area.FirstOrDefault(c => c.Id == db.SubArea.FirstOrDefault(d => d.Id == SubAreaId).IdArea),
                ListaAreas = db.Area.ToList(),
                ListaPersonas = db.Persona.ToList(),
            };

            return View(Area1);
        }

        [HttpPost]
        public IActionResult FormularioEditSubArea(ListaArea model)
        {
            if (ModelState.IsValid)
            {
                SubArea updateSubArea = db.SubArea.FirstOrDefault(c => c.Id == model.SubAreaUnica.Id);
                updateSubArea.Nombre = model.NombreSubArea;
                //updateArea.RutJefe = model.AreaUnica.RutJefe;
                db.SubArea.Update(updateSubArea);
                db.SaveChanges();

                return RedirectToAction("IndiceSubAreas", new { AreaId = model.AreaUnica.Id });
            }
            else
            {
                return FormularioEditSubArea(model.SubAreaUnica.Id);
            }
      

        }



        public async Task<List<SubArea>> getSubAreaEventoModalAjax(int id)
        {
            List<SubArea> le = new List<SubArea>();
            var subarea = db.SubArea.FirstOrDefault(c => c.Id == id);
            le.Add(subarea);
            return le;

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
                nombreCompleto = persona.Nombre.TrimEnd()+" " + persona.ApellidoPaterno.TrimEnd()+" " + persona.ApellidoMaterno.TrimEnd();
                Datoslist.Insert(contador, new DatosPersona { rut = persona.Rut, Nombre = nombreCompleto, Area = areaVariable, SubArea = SubAreaVariable, Cargo = cargoVariable });
                contador++;
            }

            //list.Insert(0, new SubArea { Id = 0, Nombre = "Please Select SubArea" });
            //return Json(new SelectList(list,"Rut", "ApellidoPaterno"));
            return Json(Datoslist);
        }



        public JsonResult ComprobarEliminarSubArea(int SubArea)
        {
            int? comprobar = 0;

            //if (db.Persona.FirstOrDefault(c =>c.IdSubAreaJefeSubArea == SubArea) == null)
            //{
            //    comprobar = 0;
            //}
            //else
            //{
            //    if (db.Persona.FirstOrDefault(c => c.IdSubAreaJefeSubArea == SubArea) != null)
            //    {
            //        comprobar = 1;
            //    }
            //}


            if (db.PersonaSubArea.FirstOrDefault(c => c.IdSubArea == SubArea) == null)
            {
                comprobar = 0;
            }
            else
            {
                if (db.PersonaSubArea.FirstOrDefault(c => c.IdSubArea == SubArea) != null)
                {
                    comprobar = 1;
                }
            }

            return Json(comprobar);


        }

        //public JsonResult CargarDatosSupervisores(int elemento)
        //{
        //    IEnumerable<Area> ListaArea = db.Area.ToList();
        //    IEnumerable<SubArea> ListaSubArea = db.SubArea.ToList();

        //    List<Persona> list = new List<Persona>();
        //    list = db.Persona.Where(c => c.IdSubAreaJefeSubArea == elemento).ToList();
        //    List<DatosPersona> Datoslist2 = new List<DatosPersona>();
        //    int contador = 0;
        //    foreach (Persona persona in list)
        //    {
        //        string areaVariable = "";
        //        string SubAreaVariable = "";
        //        string nombreCompleto = "";
        //        string cargoVariable = "";

        //        if (ListaArea.FirstOrDefault(c => c.Id == persona.IdArea) != null)
        //        {
        //            areaVariable = ListaArea.FirstOrDefault(c => c.Id == persona.IdArea).Nombre;
        //        }
        //        if (ListaSubArea.FirstOrDefault(c => c.Id == persona.IdSubArea) != null)
        //        {
        //            SubAreaVariable = ListaSubArea.FirstOrDefault(c => c.Id == persona.IdSubArea).Nombre;
        //        }
        //        if (persona.Cargo != null)
        //        {
        //            cargoVariable = persona.Cargo;
        //        }
        //        nombreCompleto = persona.Nombre + persona.ApellidoPaterno + persona.ApellidoMaterno;
        //        Datoslist2.Insert(contador, new DatosPersona { rut = persona.Rut, Nombre = nombreCompleto, Area = areaVariable, SubArea = SubAreaVariable, Cargo = cargoVariable });
        //        contador++;
        //    }
        //    return Json(Datoslist2);
        //}

        public JsonResult CargarDatosSupervisores(int elemento)
        {
            IEnumerable<Area> ListaArea = db.Area.ToList();
            IEnumerable<SubArea> ListaSubArea = db.SubArea.ToList();

            List<PersonaSubArea> list = new List<PersonaSubArea>();
            list = db.PersonaSubArea.Where(c => c.IdSubArea == elemento).ToList();
            List<DatosPersona> Datoslist2 = new List<DatosPersona>();
            int contador = 0;
            foreach (PersonaSubArea ps in list)
            {
                Persona p = db.Persona.FirstOrDefault(c=>c.Rut.TrimEnd().Equals(ps.RutPersona.TrimEnd()));

                string areaVariable = "";
                string SubAreaVariable = "";
                string nombreCompleto = "";
                string cargoVariable = "";

                if (ListaArea.FirstOrDefault(c => c.Id == p.IdArea) != null)
                {
                    areaVariable = ListaArea.FirstOrDefault(c => c.Id == p.IdArea).Nombre;
                }
                if (ListaSubArea.FirstOrDefault(c => c.Id == p.IdSubArea) != null)
                {
                    SubAreaVariable = ListaSubArea.FirstOrDefault(c => c.Id == p.IdSubArea).Nombre;
                }
                if (p.Cargo != null)
                {
                    cargoVariable = p.Cargo;
                }
                nombreCompleto = p.Nombre + p.ApellidoPaterno + p.ApellidoMaterno;
                Datoslist2.Insert(contador, new DatosPersona { rut = p.Rut, Nombre = nombreCompleto, Area = areaVariable, SubArea = SubAreaVariable, Cargo = cargoVariable });
                contador++;
            }
            return Json(Datoslist2);
        }

        public JsonResult ComprobarSupervisor(string Rut, int IdSubArea)
        {
            int? comprobar = 0;
            string comprobar2 = null;


            if (db.PersonaSubArea.FirstOrDefault(c => c.RutPersona.Equals(Rut) && c.IdSubArea == IdSubArea) != null)
            {
                comprobar2 = "NoAsignar";
            }
            else
            {
                comprobar2 = "Asignar";

            }

            //    if (db.Persona.FirstOrDefault(c => c.Rut.Equals(Rut) && c.IdSubAreaJefeSubArea==null)!=null)
            //{
            //    comprobar = 0;
            //    comprobar2 = "Asignar";

            //}
            //else
            //{


            //    if (db.Persona.FirstOrDefault(c => c.Rut.Equals(Rut) && c.IdSubAreaJefeSubArea != null) !=null)
            //    {
            //        comprobar = db.Persona.FirstOrDefault(c => c.Rut.Equals(Rut)).IdSubAreaJefeSubArea;
            //        comprobar2 = db.SubArea.FirstOrDefault(c=>c.Id==comprobar).Nombre;
            //    }


            //}
            return Json(comprobar2);


        }

        public IActionResult AsignarSupervisor(string Rut, int subArea, int original)
        {

            PersonaSubArea ps = new PersonaSubArea()
            {
                RutPersona = Rut,
                IdSubArea = subArea,
                Removed = false,


            };
            db.PersonaSubArea.Add(ps);
            db.SaveChanges();

            //if (db.Persona.FirstOrDefault(c => c.Rut.Equals(Rut)) != null)
            //{
            //    Persona updatePersona = db.Persona.FirstOrDefault(c => c.Rut.TrimEnd().Equals(Rut));
            //    updatePersona.IdSubAreaJefeSubArea = subArea;
            //    db.Persona.Update(updatePersona);
            //    db.SaveChanges();
            //}

            return RedirectToAction("IndiceSubAreas", new { AreaId = original });

        }

        public IActionResult DeleteArea(int AreaId)
        {
            //Area DeleteArea = db.Area.FirstOrDefault(c => c.Id == AreaId);

            //db.Area.Remove(DeleteArea);
            //db.SaveChanges();

            //Area updateArea = db.Area.FirstOrDefault(c => c.Id == AreaId);
            //updateArea.Removed = true;
            //db.Area.Update(updateArea);
            //db.SaveChanges();

            Area updateArea = db.Area.FirstOrDefault(c => c.Id == AreaId);
            updateArea.Removed = true;
            db.Area.Update(updateArea);
            db.SaveChanges();

            return RedirectToAction("Index");

        }

        //public IActionResult DeleteSubArea(int SubAreaId)
        //{
        //    SubArea area2 = new SubArea()
        //    {
        //        Id = SubAreaId,
        //        IdArea = db.SubArea.FirstOrDefault(c => c.Id == SubAreaId).IdArea,
        //    };


        //    SubArea DeleteSubArea = db.SubArea.FirstOrDefault(c => c.Id == SubAreaId);

        //    db.SubArea.Remove(DeleteSubArea);
        //    db.SaveChanges();


        //    return RedirectToAction("IndiceSubAreas", new { AreaId = area2.IdArea });

        //}
        public IActionResult EliminarSubArea(int Area,int SubArea)
        {
            SubArea updateSubArea = db.SubArea.FirstOrDefault(c => c.Id==SubArea);
            updateSubArea.Removed = true;
            db.SubArea.Update(updateSubArea);
            db.SaveChanges();

            List<PersonaSubArea> listaPSA = new List<PersonaSubArea>();
            foreach (PersonaSubArea psa in db.PersonaSubArea.ToList())
            {
                if (psa.IdSubArea==SubArea)
                {
                    PersonaSubArea DeletePersonaSubArea = db.PersonaSubArea.FirstOrDefault(c => c.IdSubArea==SubArea);
                    db.PersonaSubArea.Remove(DeletePersonaSubArea);
                    db.SaveChanges();
                }
              
            }

         

            return RedirectToAction("IndiceSubAreas", new { AreaId = Area });

        }


        public ViewResult IndiceEquipos()
        {
            ListaEquipo equipo = new ListaEquipo()
            {

                ListaEquipos = db.Equipo.ToList().Where(c=>c.Removed!=true),

            };


            return View(equipo);
        }

        public IActionResult FormularioCrearEquipo()
        {

            CreateEquipo Equipo = new CreateEquipo();


            return View(Equipo);
        }
        [HttpPost]
        public IActionResult FormularioCrearEquipo(CreateEquipo model)
        {
            if (ModelState.IsValid)
            {
                Equipo equipo = new Equipo()
                {
                    Nombre = model.Nombre,


                };
                db.Equipo.Add(equipo);
                db.SaveChanges();
                return RedirectToAction("IndiceEquipos");
            }
            else
            {
                return FormularioCrearEquipo();
            }
        
        }

        public ViewResult FormularioEditEquipo(int EquipoId)
        {


            CreateEquipo Equipo = new CreateEquipo()
            {
                EquipoUnico = db.Equipo.FirstOrDefault(c => c.Id == EquipoId),
                ListaEquipos = db.Equipo.ToList(),
            };





            return View(Equipo);
        }
        [HttpPost]
        public IActionResult FormularioEditEquipo(CreateEquipo model)
        {

            if (ModelState.IsValid)
            {
                Equipo updateEquipo = db.Equipo.FirstOrDefault(c => c.Id == model.EquipoUnico.Id);
                updateEquipo.Nombre = model.Nombre;
                db.Equipo.Update(updateEquipo);
                db.SaveChanges();

                return RedirectToAction("IndiceEquipos");
            }
            else
            {
                return FormularioEditEquipo(model.EquipoUnico.Id);
            }




        }
        public IActionResult DeleteEquipo(int EquipoId)
        {
            //SubArea area2 = new SubArea()
            //{
            //    Id = SubAreaId,
            //    IdArea = db.SubArea.FirstOrDefault(c => c.Id == SubAreaId).IdArea,
            //};

         
            Equipo DeleteEquipo = db.Equipo.FirstOrDefault(c => c.Id == EquipoId);
     
            DeleteEquipo.Removed = true;
            db.Equipo.Update(DeleteEquipo);
            db.SaveChanges();


            return RedirectToAction("IndiceEquipos");

        }

        public ViewResult IndiceModelos(int EquipoId)
        {
            ListaModelo modelo = new ListaModelo()
            {

                ListaModelos = db.Modelo.ToList().Where(c=>c.Removed!=true),
                EquipoId = EquipoId,
                EquipoUnico = db.Equipo.FirstOrDefault(c => c.Id == EquipoId),
            };


            return View(modelo);
        }


        public IActionResult FormularioCrearModelo(int EquipoId)
        {

            CreateModelo modelo = new CreateModelo()
            {
                EquipoId = EquipoId,
                ListaEquipos = db.Equipo.ToList(),
            };


            return View(modelo);
        }
        [HttpPost]
        public IActionResult FormularioCrearModelo(CreateModelo model)
        {
            if (ModelState.IsValid)
            {
                Modelo modelo = new Modelo()
                {
                    Nombre = model.Nombre,
                    IdEquipo = model.EquipoId,

                };
                db.Modelo.Add(modelo);
                db.SaveChanges();
                return RedirectToAction("IndiceModelos", new { EquipoId = model.EquipoId });
            }
            else
            {
                return FormularioCrearModelo(model.EquipoId);
            }
       
        }

        public IActionResult FormularioEditModelo(int ModeloId)
        {


            CreateModelo modelo = new CreateModelo()
            {

                ListaEquipos = db.Equipo.ToList(),
                ModeloUnico = db.Modelo.FirstOrDefault(c => c.Id == ModeloId),
            };

            return View(modelo);
        }
        [HttpPost]
        public IActionResult FormularioEditModelo(CreateModelo model)
        {

            if (ModelState.IsValid)
            {
                Modelo updateModelo = db.Modelo.FirstOrDefault(c => c.Id == model.ModeloUnico.Id);
                updateModelo.Nombre = model.Nombre;
                db.Modelo.Update(updateModelo);
                db.SaveChanges();

                return RedirectToAction("IndiceModelos", new { EquipoId = model.ModeloUnico.IdEquipo });
            }
            else
            {
                return FormularioEditModelo(model.ModeloUnico.Id);
            }




        }
        public IActionResult DeleteModelo(int ModeloId)
        {
            //SubArea area2 = new SubArea()
            //{
            //    Id = SubAreaId,
            //    IdArea = db.SubArea.FirstOrDefault(c => c.Id == SubAreaId).IdArea,
            //};


            Modelo DeleteModelo = db.Modelo.FirstOrDefault(c => c.Id == ModeloId);
            DeleteModelo.Removed = true;
            db.Modelo.Update(DeleteModelo);
            db.SaveChanges();


           


            return RedirectToAction("IndiceModelos", new { EquipoId = DeleteModelo.IdEquipo });

        }

        public ViewResult IndiceComponentes(int ModeloId)
        {
            ListaModelo modelo = new ListaModelo()
            {

                ModeloId = ModeloId,
                ListaModelos = db.Modelo.ToList(),
                ListaEquipos = db.Equipo.ToList(),
                ListaComponentes = db.Componente.ToList(),
                EquipoId = db.Modelo.FirstOrDefault(c => c.Id == ModeloId).IdEquipo,
                ListaModeloComponentes = db.ModeloComponente.ToList().Where(c=>c.Removed!=true),
            };


            return View(modelo);
        }
        public ViewResult FormularioAsociarComponente(int ModeloId)
        {

            ListaModelo modelo = new ListaModelo()
            {

                ModeloId = ModeloId,
                ListaModelos = db.Modelo.ToList(),
                ListaEquipos = db.Equipo.ToList(),
                EquipoId = db.Modelo.FirstOrDefault(c => c.Id == ModeloId).IdEquipo,
                ListaComponentes = db.Componente.ToList(),
            };
            return View(modelo);

        }
        [HttpPost]
        public IActionResult FormularioAsociarComponente(ListaModelo ModeloComponente)
        {

            if (ModelState.IsValid)
            {
                ModeloComponente modelo = new ModeloComponente()
                {
                    IdComponente = Convert.ToInt32(ModeloComponente.ComponenteId),
                    IdModelo = Convert.ToInt32(ModeloComponente.ModeloId),


                };
                db.ModeloComponente.Add(modelo);
                db.SaveChanges();


                return RedirectToAction("IndiceComponentes", new { ModeloId = modelo.IdModelo });
            }

            return RedirectToAction("Index");
        }

        public IActionResult FormularioCreateComponente()
        {

            CreateComponente modelo = new CreateComponente()
            {
            };
            return View(modelo);

        }
        [HttpPost]
        public IActionResult FormularioCreateComponente(CreateComponente model)
        {

            if (ModelState.IsValid)
            {
                Componente modelo = new Componente()
                {
                    Nombre = model.Nombre,
                    NumeroSerie = model.NumeroSerie,
                    Posicion = model.Posicion,

                };
                db.Componente.Add(modelo);
                db.SaveChanges();
                return RedirectToAction("PrincipalComponentes");
            }
            else
            {
                return FormularioCreateComponente();
            }
       

        }


        public ViewResult PrincipalComponentes()
        {


            ListaComponente modelo = new ListaComponente()
            {
                ListaComponentes = db.Componente.ToList().Where(c=>c.Removed!=true),
            };
            return View(modelo);

        }

        public IActionResult FormularioEditComponente(int ComponenteId)
        {

            Componente comp = db.Componente.FirstOrDefault(c => c.Id == ComponenteId);
            string nombre = null;
            string numeroserie = null;
            string posicion = null;
            if (comp.Nombre != null)
            {
                nombre = comp.Nombre.TrimEnd();
            }
            if (comp.NumeroSerie != null)
            {
                numeroserie = comp.NumeroSerie.TrimEnd();
            }
            if (comp.Posicion != null)
            {
                posicion = comp.Posicion.TrimEnd();
            }
            ListaComponente componente = new ListaComponente()
            {

                ComponenteUnico = db.Componente.FirstOrDefault(c => c.Id == ComponenteId),
                Nombre = nombre,
                NumeroSerie= numeroserie,
                Posicion = posicion,
                
            };

            return View(componente);
        }
        [HttpPost]
        public IActionResult FormularioEditComponente(ListaComponente componente)
        {
            if (ModelState.IsValid)
            {
                Componente updateComponente = db.Componente.FirstOrDefault(c => c.Id == @componente.ComponenteUnico.Id);
                updateComponente.Nombre = componente.Nombre;
                updateComponente.NumeroSerie = componente.NumeroSerie;
                updateComponente.Posicion = componente.Posicion;
                db.Componente.Update(updateComponente);
                db.SaveChanges();

                return RedirectToAction("PrincipalComponentes");
            }
            else
            {
                return FormularioEditComponente(componente.ComponenteUnico.Id);
            }

        }
        public IActionResult DeleteComponente(int ComponenteId)
        {
            //SubArea area2 = new SubArea()
            //{
            //    Id = SubAreaId,
            //    IdArea = db.SubArea.FirstOrDefault(c => c.Id == SubAreaId).IdArea,
            //};

            Componente DeleteComponente = db.Componente.FirstOrDefault(c => c.Id == ComponenteId);
            DeleteComponente.Removed = true;
            db.Componente.Update(DeleteComponente);
            db.SaveChanges();


            return RedirectToAction("PrincipalComponentes");

        }

        public IActionResult DeleteModeloComponente(int ModeloComponenteId)
        {
            //SubArea area2 = new SubArea()
            //{
            //    Id = SubAreaId,
            //    IdArea = db.SubArea.FirstOrDefault(c => c.Id == SubAreaId).IdArea,
            //};


            Modelo DatosModelo = new Modelo()
            {
                Id = db.Modelo.FirstOrDefault(c => c.Id == db.ModeloComponente.FirstOrDefault(d => d.Id == ModeloComponenteId).IdModelo).Id
            };


            ModeloComponente DeleteModeloComponente = db.ModeloComponente.FirstOrDefault(c => c.Id == ModeloComponenteId);
            DeleteModeloComponente.Removed = true;
            db.ModeloComponente.Update(DeleteModeloComponente);
            db.SaveChanges();


            return RedirectToAction("IndiceComponentes", new { ModeloId = DatosModelo.Id });

        }

        public ViewResult FormularioEditModeloComponente(int ModeloComponenteId)
        {


            ListaModelo modelo = new ListaModelo()
            {

                ModeloId = db.Modelo.FirstOrDefault(c => c.Id == db.ModeloComponente.FirstOrDefault(d => d.Id == ModeloComponenteId).IdModelo).Id,
                ListaModelos = db.Modelo.ToList(),
                ListaEquipos = db.Equipo.ToList(),
                EquipoId = db.Modelo.FirstOrDefault(c => c.Id == db.Modelo.FirstOrDefault(d => d.Id == db.ModeloComponente.FirstOrDefault(e => e.Id == ModeloComponenteId).IdModelo).Id).IdEquipo,
                ListaComponentes = db.Componente.ToList(),
                ModeloComponenteId = ModeloComponenteId,
                ListaModeloComponentes = db.ModeloComponente.ToList(),
            };
            return View(modelo);

        }

        [HttpPost]
        public IActionResult FormularioEditModeloComponente(ListaModelo modeloComponente)
        {


            ModeloComponente updateModeloComponente = db.ModeloComponente.FirstOrDefault(c => c.Id == modeloComponente.ModeloComponenteId);
            updateModeloComponente.IdComponente = Convert.ToInt32(modeloComponente.ComponenteId);
            updateModeloComponente.IdModelo = Convert.ToInt32(modeloComponente.ModeloId);

            db.ModeloComponente.Update(updateModeloComponente);
            db.SaveChanges();

            return RedirectToAction("IndiceComponentes", new { ModeloId = modeloComponente.ModeloId });
        }

        public IActionResult FormularioCrearParte()
        {

            CreateParte parte = new CreateParte()
            {
            };
            return View(parte);

        }
        [HttpPost]
        public IActionResult FormularioCrearParte(CreateParte model)
        {
            if (ModelState.IsValid)
            {
                Parte parte = new Parte()
                {
                    Nombre = model.Nombre,
                    NumeroParte = model.NumeroParte,
                    Removed = false,
                };
                db.Parte.Add(parte);
                db.SaveChanges();
                return RedirectToAction("IndiceParte");

            }
            else
            {
                return FormularioCrearParte();
            }
          
        }
        public ViewResult IndiceParte()
        {
            ListaParte lista = new ListaParte()
            {
                ListaPartes = db.Parte.ToList(),
            };


            return View(lista);
        }

        public ViewResult FormularioEditParte(int ParteId)
        {
            Parte parte = db.Parte.FirstOrDefault(c => c.Id == ParteId);
            string nombre = null;
            string numeroparte = null;
            if (parte.Nombre!=null)
            {
                nombre = parte.Nombre.TrimEnd();
            }
            if (parte.NumeroParte != null)
            {
                numeroparte = parte.NumeroParte.TrimEnd();
            }

            CreateParte p = new CreateParte()
            {

                ParteId = parte.Id,
                Nombre = nombre,
                NumeroParte = numeroparte,

            };
            return View(p);

        }
        [HttpPost]
        public IActionResult FormularioEditParte(CreateParte parte)
        {

            if (ModelState.IsValid)
            {
                Parte updateParte = db.Parte.FirstOrDefault(c => c.Id == parte.ParteId);
                updateParte.Id = Convert.ToInt32(parte.ParteId);
                updateParte.Nombre = parte.Nombre;
                updateParte.NumeroParte = parte.NumeroParte;

                db.Parte.Update(updateParte);
                db.SaveChanges();

                return RedirectToAction("IndiceParte");
            }
            else
            {
                return FormularioEditParte(parte.ParteId);
            }

        }


        public IActionResult DeleteParte(int ParteId)
        {

            Parte updateParte = db.Parte.FirstOrDefault(c => c.Id == ParteId);
            updateParte.Removed = true;
            db.Parte.Update(updateParte);
            db.SaveChanges();
            return RedirectToAction("IndiceParte");



        }


        public ViewResult PrincipalPartes(int componenteId, int variable)
        {


            ListaParte listaparte = new ListaParte()
            {
                variable = variable,
                ListaModeloComponente = db.ModeloComponente.ToList(),
                idComponente = componenteId,
                ListaPartes = db.Parte.ToList(),
                ListaComponenteParte = db.ComponenteParte.ToList().Where(c=>c.Removed!=true),
                ListaComponente = db.Componente.ToList(),
            };
            return View(listaparte);

        }

        public ViewResult FormularioAsociarParte(int ComponenteId, int variable)
        {

            ListaComponente listacomponente = new ListaComponente()
            {
                variable = variable,
                ComponenteId = ComponenteId,
                ListaComponentes = db.Componente.ToList(),
                listaPartes = db.Parte.ToList().Where(c=>!c.Removed==true),

            };
            return View(listacomponente);

        }

        [HttpPost]
        public IActionResult FormularioAsociarParte(ListaComponente partecomponente)
        {

            if (ModelState.IsValid)
            {
                ComponenteParte cp = new ComponenteParte()
                {
                    IdComponente = Convert.ToInt32(partecomponente.ComponenteId),
                    IdParte = Convert.ToInt32(partecomponente.ParteId),
                    Removed = false,


                };
                db.ComponenteParte.Add(cp);
                db.SaveChanges();


                return RedirectToAction("PrincipalPartes", new { ComponenteId = partecomponente.ComponenteId, variable = partecomponente.variable });
            }

            return RedirectToAction("Index");
        }

        public IActionResult DeleteComponenteParte(int ComponenteParteId, int variable)
        {

            ComponenteParte updateComponenteParte = db.ComponenteParte.FirstOrDefault(c => c.Id == ComponenteParteId);
            updateComponenteParte.Removed = true;

            db.ComponenteParte.Update(updateComponenteParte);
            db.SaveChanges();

            return RedirectToAction("PrincipalPartes", new { ComponenteId = db.ComponenteParte.FirstOrDefault(c => c.Id == ComponenteParteId).IdComponente, variable = variable });
        }

        public IActionResult EliminarSupervisor(string rut,int area, int SubArea)
        {

            PersonaSubArea DeletePersonaSubArea = db.PersonaSubArea.FirstOrDefault(c => c.IdSubArea == SubArea && c.RutPersona == rut.TrimEnd());
            //DeleteResponsable.Removed = true;
            //db.PersonaEvento.Update(DeleteResponsable);
            //db.SaveChanges();
            db.PersonaSubArea.Remove(DeletePersonaSubArea);
            db.SaveChanges();


            //Persona updatePersona = db.Persona.FirstOrDefault(c => c.Rut.Equals(rut));
            //updatePersona.IdSubAreaJefeSubArea = null;

            //db.Persona.Update(updatePersona);
            //db.SaveChanges();


            return RedirectToAction("IndiceSubAreas", new { AreaId = area });
        }


        public ViewResult FormularioEditComponenteParte(int ComponenteParteId, int variable)
        {


            ListaComponente parte = new ListaComponente()
            {
                ComponenteParteId = ComponenteParteId,
                ParteId = db.ComponenteParte.FirstOrDefault(c => c.Id == ComponenteParteId).IdParte,
                ListaComponentes = db.Componente.ToList(),
                ComponenteId = db.ComponenteParte.FirstOrDefault(c => c.Id == ComponenteParteId).IdComponente,
                variable = variable,
                listaPartes = db.Parte.ToList(),

            };
            return View(parte);

        }
        [HttpPost]
        public IActionResult FormularioEditComponenteParte(ListaComponente lc)
        {

            ComponenteParte updateComponenteParte = db.ComponenteParte.FirstOrDefault(c => c.Id == lc.ComponenteParteId);
            updateComponenteParte.IdParte = Convert.ToInt32(lc.ParteId);

            db.ComponenteParte.Update(updateComponenteParte);
            db.SaveChanges();

            return RedirectToAction("PrincipalPartes", new { ComponenteId = db.ComponenteParte.FirstOrDefault(c => c.Id == lc.ComponenteParteId).IdComponente, variable = lc.variable });




        }




    }
}