using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FINNINGWEB.Models;
using Microsoft.AspNetCore.Identity;
using FINNINGWEB.Entities;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FINNINGWEB.Controllers
{
    [Authorize]
    public class AnalisisCausaRaizController : Controller
    {
        private IHostingEnvironment _enviroment;
        private UserManager<AppUser> userManager;
        public AnalisisCausaRaizController(UserManager<AppUser> userMgr, IHostingEnvironment enviroment)
        {
            userManager = userMgr;
            _enviroment = enviroment;
        }
        private AdventureWorksContext db = new AdventureWorksContext();

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult getFallaSecundaria(int id)
        {
            List<FallaSecundaria> list = new List<FallaSecundaria>();
            list = db.FallaSecundaria.Where(c => c.IdFallaPrimaria == id).ToList();
            //list.Insert(0, new Modelo { Id = 0, Nombre = "Please Select Modelo" });
            return Json(new SelectList(list, "Id", "Nombre"));
        }

        public JsonResult getProceso2(int id)
        {
            List<Proceso2> list = new List<Proceso2>();
            list = db.Proceso2.Where(c => c.IdProceso1 == id).ToList();
            //list.Insert(0, new Modelo { Id = 0, Nombre = "Please Select Modelo" });
            return Json(new SelectList(list, "Id", "Nombre"));
        }

        public JsonResult getProceso3(int id)
        {
            List<Proceso3> list = new List<Proceso3>();
            list = db.Proceso3.Where(c => c.IdProceso2 == id).ToList();
            //list.Insert(0, new Modelo { Id = 0, Nombre = "Please Select Modelo" });
            return Json(new SelectList(list, "Id", "Nombre"));
        }

        public JsonResult getCausa2Id(int id)
        {
            List<Causa2> list = new List<Causa2>();
            list = db.Causa2.Where(c => c.IdLevel1 == id).ToList();
            //list.Insert(0, new Modelo { Id = 0, Nombre = "Please Select Modelo" });
            return Json(new SelectList(list, "Id", "Nombre"));
        }

        public JsonResult getCausa3Id(int id)
        {
            List<Causa3> list = new List<Causa3>();
            list = db.Causa3.ToList();
            //list.Insert(0, new Modelo { Id = 0, Nombre = "Please Select Modelo" });
            return Json(new SelectList(list, "Id", "Nombre"));
        }
        public JsonResult getCausa4Id(int id)
        {
            List<Causa4> list = new List<Causa4>();
            list = db.Causa4.Where(c => c.IdLevel3 == id).ToList();
            //list.Insert(0, new Modelo { Id = 0, Nombre = "Please Select Modelo" });
            return Json(new SelectList(list, "Id", "Nombre"));
        }

        public JsonResult getCausa5Id(int id)
        {
            List<Causa5> list = new List<Causa5>();
            list = db.Causa5.Where(c => c.IdLevel4 == id).ToList();
            //list.Insert(0, new Modelo { Id = 0, Nombre = "Please Select Modelo" });
            return Json(new SelectList(list, "Id", "Nombre"));
        }

        public JsonResult getCausaIntermedia2Id(int id)
        {
            List<CausaIntermedia2> list = new List<CausaIntermedia2>();
            list = db.CausaIntermedia2.Where(c => c.IdLevel6 == id).ToList();
            //list.Insert(0, new Modelo { Id = 0, Nombre = "Please Select Modelo" });
            return Json(new SelectList(list, "Id", "Nombre"));
        }

        public async Task<IActionResult> FormularioCrearCausa(string eventoID)
        {
            AnalisisCausaRaizModels FormularioCrearAnalisis = new AnalisisCausaRaizModels()
            {
                EventoId = eventoID,
                ListaPersonas = db.Persona.ToList(),
                ListaOrigenFalla = db.OrigenFalla.ToList(),
                EventoUnico = db.Evento.FirstOrDefault(c => c.Id == eventoID),
                ListaCausa1 = db.Causa1.ToList(),
                ListaCausa2 = db.Causa2.ToList(),
                ListaCausa3 = db.Causa3.ToList(),
                ListaCausa4 = db.Causa4.ToList(),
                ListaCausa5 = db.Causa5.ToList(),
                ListaCausaIntermedia1 = db.CausaIntermedia1.ToList(),
                ListaCausaIntermedia2 = db.CausaIntermedia2.ToList(),
                ListaProceso1 = db.Proceso1.ToList(),
                ListaProceso2 = db.Proceso2.ToList(),
                ListaProceso3 = db.Proceso3.ToList(),
                ListaProceso4 = db.Proceso4.ToList(),
                ListaProceso5 = db.Proceso5.ToList(),
                ListaFallaPrimaria = db.FallaPrimaria.ToList(),
                ListaFallaSecundaria = db.FallaSecundaria.ToList(),

            };

            return View(FormularioCrearAnalisis);
        }

        [HttpPost]
        public async Task<IActionResult> FormularioCrearCausa(AnalisisCausaRaizModels rca, List<IFormFile> files)
        {


            int contador = 0;
            string[] filename2;
            string eventoid = rca.EventoId;
            if (ModelState.IsValid)
            {

                foreach (var formFile in files)
                {
                    contador++;

                    if (formFile.Length > 0)
                    {
                        string cadena = formFile.FileName;
                        filename2 = cadena.Split('.');
                        if (filename2.Last().Equals("doc") || filename2.Last().Equals("docx") || filename2.Last().Equals("txt") || filename2.Last().Equals("csv") || filename2.Last().Equals("pdf") || filename2.Last().Equals("xls") || filename2.Last().Equals("xlsx") || filename2.Last().Equals("odp") || filename2.Last().Equals("ods") || filename2.Last().Equals("odt") || filename2.Last().Equals("vsd") || filename2.Last().Equals("pptx") || filename2.Last().Equals("ppt") || filename2.Last().Equals("potx") || filename2.Last().Equals("png") || filename2.Last().Equals("jpg") || filename2.Last().Equals("jpeg") || filename2.Last().Equals("ico") || filename2.Last().Equals("webp") || filename2.Last().Equals("avi") || filename2.Last().Equals("mp4") || filename2.Last().Equals("mpeg") || filename2.Last().Equals("mpg") || filename2.Last().Equals("ogv") || filename2.Last().Equals("webm") || filename2.Last().Equals("mp3") || filename2.Last().Equals("wav") || filename2.Last().Equals("oga") || filename2.Last().Equals("aac") || filename2.Last().Equals("mid") || filename2.Last().Equals("midi") || filename2.Last().Equals("weba") || filename2.Last().Equals("rar") || filename2.Last().Equals("zip") || filename2.Last().Equals("bz") || filename2.Last().Equals("bz2"))
                        {
                            //ModelState.AddModelError("Error1", "no hay Error en el formato del archivo" + " " + filename2.Last());
                        }
                        else
                        {
                            ModelState.AddModelError("Error2", "Error en el formato del archivo" + " " + "." + filename2.Last());
                            return await FormularioCrearCausa(eventoid);
                        }

                    }
                }




                string creadorRCA = null;
               ClaimsPrincipal currentUser = User;
                if (db.AspNetUsers.FirstOrDefault(c => c.Id.Equals(currentUser.getUserId())).RutPersona != null)
                {
                    creadorRCA = db.AspNetUsers.FirstOrDefault(c => c.Id.Equals(currentUser.getUserId())).RutPersona;
                }

                AnalisisCausaRaiz acr = new AnalisisCausaRaiz()
                {
                IdEvento = rca.EventoId,
                Creador = creadorRCA,
                FechaRegistro = rca.FechaRegistro,
                IdOrigenFalla = rca.OrigenFalla,
                IdFallaPrimaria = rca.FallaPrimaria,
                IdFallaSecundaria = rca.FallaSecundaria,
                IdCausa1 = rca.Causa1,
                IdCausa2 = rca.Causa2,
                IdCausa3 = rca.Causa3,
                IdCausa4 = rca.Causa4,
                IdCausa5 = rca.Causa5,
                IdCausaIntermedia1 = rca.CausaIntermedia1,
                IdCausaIntermedia2 = rca.CausaIntermedia2,
                IdProceso1 = rca.Proceso1,
                IdProceso2 = rca.Proceso2,
                IdProceso3 = rca.Proceso3,
                IdProceso4 = rca.Proceso4,
                IdProceso5 = rca.Proceso5,
                Descripcion = rca.Descripcion,
                Costo = rca.Costo,
                Removed = false,

            };
            db.AnalisisCausaRaiz.Add(acr);
            db.SaveChanges();

            if (rca.EventoUnico.Ncaceptada == true)
            {
                Evento UpdateEvento = db.Evento.FirstOrDefault(c => c.Id == rca.EventoId);
                UpdateEvento.Ncaceptada = true;
                UpdateEvento.Estado = 5;
                db.Evento.Update(UpdateEvento);
                db.SaveChanges();
            }
            else
            {
                if (rca.EventoUnico.Ncaceptada == false)
                {
                    Evento UpdateEvento = db.Evento.FirstOrDefault(c => c.Id == rca.EventoId);
                    UpdateEvento.Ncaceptada = false;
                    UpdateEvento.Estado = 10;
                    db.Evento.Update(UpdateEvento);
                    db.SaveChanges();
                }
            }

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            string direccionRCAId = db.AnalisisCausaRaiz.FirstOrDefault(c => c.IdEvento == rca.EventoId).Id.TrimEnd();

            string concatenar = rca.EventoId.TrimEnd() + "/" + "AnalisisCausaRaiz" + "/" + direccionRCAId;

            foreach (var formFile in files)
            {

                if (formFile.Length > 0)
                {
                    string uploadPath = Path.Combine(_enviroment.WebRootPath, "uploads");
                    Directory.CreateDirectory(Path.Combine(uploadPath, concatenar));
                    string filename = formFile.FileName;
                    using (FileStream fs = new FileStream(Path.Combine(uploadPath, concatenar, filename), FileMode.Create))
                    {
                        await formFile.CopyToAsync(fs);
                    }

                    Archivo ar = new Archivo()
                    {
                        Identificador = rca.EventoId.TrimEnd(),
                        Nombre = filename,
                        Tipo = "AnalisisCausaRaiz",
                        Removed = false,
                        FechaRegistro = DateTime.Now,
                    };
                    db.Archivo.Add(ar);
                    db.SaveChanges();



                }
            }

            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = rca.EventoId });

            }
            else
            {
                return await FormularioCrearCausa(rca.EventoId.TrimEnd());
            }
        }

        public ActionResult AgregarResponsableAnalisisCausaRaiz(EventoSecuenciaModels es)
        {
            ViewBag.Prueba = 2;


            PersonaEvento pe = new PersonaEvento()
            {

                RutPersona = es.Responsable,
                IdEvento = es.EventoUnico.Id.TrimEnd(),
                Removed = false,
                FechaRegistro = DateTime.Now,
                //Etapa = "RCA",
                Estado = 4,


            };
            db.PersonaEvento.Add(pe);
            //db.SaveChanges();

            PersonaEvento pe2 = new PersonaEvento()
            {

                RutPersona = es.Responsable,
                IdEvento = es.EventoUnico.Id.TrimEnd(),
                Removed = false,
                FechaRegistro = DateTime.Now,
                //Etapa = "RCA",
                Estado = 5,


            };
            db.PersonaEvento.Add(pe2);
            //db.SaveChanges();

            PersonaEvento pe3 = new PersonaEvento()
            {

                RutPersona = es.Responsable,
                IdEvento = es.EventoUnico.Id.TrimEnd(),
                Removed = false,
                FechaRegistro = DateTime.Now,   
                //Etapa = "RCA",
                Estado = 6,


            };
            db.PersonaEvento.Add(pe3);
            db.SaveChanges();

            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = es.EventoUnico.Id });
        }


        public ActionResult AgregarInvolucrado(EventoSecuenciaModels es)
        {

            PersonaEventoInvolucrado pe = new PersonaEventoInvolucrado()
            {

                RutPersona = es.Involucrado,
                IdEvento = es.EventoUnico.Id.TrimEnd(),
                Removed = false,
                FechaRegistro = DateTime.Now,
                //Etapa = "RCA",
                Estado = 3,


            };
            db.PersonaEventoInvolucrado.Add(pe);
            db.SaveChanges();
            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = es.EventoUnico.Id });
        }

        public IActionResult DeleteResponsableRCA(string eventoID, string ResponsableRut)
        {

            PersonaEvento DeleteResponsable = db.PersonaEvento.FirstOrDefault(c => c.IdEvento.TrimEnd().Equals(eventoID.TrimEnd()) && c.RutPersona.TrimEnd().Equals(ResponsableRut.TrimEnd()) && c.Estado == 4);
            //DeleteResponsable.Removed = true;
            //db.PersonaEvento.Update(DeleteResponsable);
            //db.SaveChanges();
            db.PersonaEvento.Remove(DeleteResponsable);
            db.SaveChanges();

            PersonaEvento DeleteResponsable2 = db.PersonaEvento.FirstOrDefault(c => c.IdEvento.TrimEnd().Equals(eventoID.TrimEnd()) && c.RutPersona.TrimEnd().Equals(ResponsableRut.TrimEnd()) && c.Estado == 5);
            //DeleteResponsable.Removed = true;
            //db.PersonaEvento.Update(DeleteResponsable);
            //db.SaveChanges();
            db.PersonaEvento.Remove(DeleteResponsable2);
            db.SaveChanges();

            PersonaEvento DeleteResponsable3 = db.PersonaEvento.FirstOrDefault(c => c.IdEvento.TrimEnd().Equals(eventoID.TrimEnd()) && c.RutPersona.TrimEnd().Equals(ResponsableRut.TrimEnd()) && c.Estado == 6);
            //DeleteResponsable.Removed = true;
            //db.PersonaEvento.Update(DeleteResponsable);
            //db.SaveChanges();
            db.PersonaEvento.Remove(DeleteResponsable3);
            db.SaveChanges();

            //return RedirectToAction("Index", "EventoSecuencia", new { EventoID = eventoID.TrimEnd() });
            return Json("Eliminado");


        }

        public IActionResult DeleteInvolucrado(string eventoID, string InvolucradoRut)
        {

            PersonaEventoInvolucrado DeleteInvolucrado = db.PersonaEventoInvolucrado.FirstOrDefault(c => c.IdEvento.TrimEnd() == eventoID.TrimEnd() && c.RutPersona.TrimEnd() == InvolucradoRut.TrimEnd());
            //DeleteResponsable.Removed = true;
            //db.PersonaEvento.Update(DeleteResponsable);
            //db.SaveChanges();
            db.PersonaEventoInvolucrado.Remove(DeleteInvolucrado);
            db.SaveChanges();

            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = eventoID.TrimEnd() });


        }


        public async Task<IActionResult> FormularioEditCausa(string IdCausa, string EventoID)
        {
            int? Causa1 =null;
            int? Causa2 =null;
            int? Causa3 =null;
            int? Causa4 =null;
            int? Causa5 =null;
            int? CausaIntermedia1 =null;
            int? CausaIntermedia2 = null;

            int? Proceso1 = null;
            int? Proceso2 = null;
            int? Proceso3 = null;
            int? Proceso4 = null;
            int? Proceso5 = null;

            int? FallaPrimaria01 = null;
            int? FallaSecundaria01 = null;



            if (db.AnalisisCausaRaiz.FirstOrDefault(c => c.Id == IdCausa && !c.Removed == true) !=null)
            {

                AnalisisCausaRaiz acr = db.AnalisisCausaRaiz.FirstOrDefault(c => c.Id == IdCausa && !c.Removed == true);

                if (db.Causa1.FirstOrDefault(c => c.Id == acr.IdCausa1 ) !=null)
                {
                    Causa1 = db.Causa1.FirstOrDefault(c => c.Id == acr.IdCausa1).Id;
                };
                if (db.Causa2.FirstOrDefault(c => c.Id == acr.IdCausa2) != null)
                {
                    Causa2 = db.Causa2.FirstOrDefault(c => c.Id == acr.IdCausa2).Id;
                };
                if (db.Causa3.FirstOrDefault(c => c.Id == acr.IdCausa3) != null)
                {
                    Causa3 = db.Causa3.FirstOrDefault(c => c.Id == acr.IdCausa3).Id;
                };
                if (db.Causa4.FirstOrDefault(c => c.Id == acr.IdCausa4) != null)
                {
                    Causa4 = db.Causa4.FirstOrDefault(c => c.Id == acr.IdCausa4).Id;
                };
                if (db.Causa5.FirstOrDefault(c => c.Id == acr.IdCausa5) != null)
                {
                    Causa5 = db.Causa5.FirstOrDefault(c => c.Id == acr.IdCausa5).Id;
                };
                if (db.CausaIntermedia1.FirstOrDefault(c => c.Id == acr.IdCausaIntermedia1) != null)
                {
                    CausaIntermedia1 = db.CausaIntermedia1.FirstOrDefault(c => c.Id == acr.IdCausaIntermedia1).Id;
                };
                if (db.CausaIntermedia2.FirstOrDefault(c => c.Id == acr.IdCausaIntermedia2) != null)
                {
                    CausaIntermedia2 = db.CausaIntermedia2.FirstOrDefault(c => c.Id == acr.IdCausaIntermedia2).Id;
                };
                if (db.Proceso1.FirstOrDefault(c => c.Id == acr.IdProceso1) != null)
                {
                    Proceso1 = db.Proceso1.FirstOrDefault(c => c.Id == acr.IdProceso1).Id;
                };
                if (db.Proceso2.FirstOrDefault(c => c.Id == acr.IdProceso2) != null)
                {
                    Proceso2 = db.Proceso2.FirstOrDefault(c => c.Id == acr.IdProceso2).Id;
                };
                if (db.Proceso3.FirstOrDefault(c => c.Id == acr.IdProceso3) != null)
                {
                    Proceso3 = db.Proceso3.FirstOrDefault(c => c.Id == acr.IdProceso3).Id;
                };
                if (db.Proceso4.FirstOrDefault(c => c.Id == acr.IdProceso4) != null)
                {
                    Proceso4 = db.Proceso4.FirstOrDefault(c => c.Id == acr.IdProceso4).Id;
                };
                if (db.Proceso5.FirstOrDefault(c => c.Id == acr.IdProceso5) != null)
                {
                    Proceso5 = db.Proceso5.FirstOrDefault(c => c.Id == acr.IdProceso5).Id;
                };
                if (db.FallaPrimaria.FirstOrDefault(c => c.Id == acr.IdFallaPrimaria) != null)
                {
                    FallaPrimaria01 = db.FallaPrimaria.FirstOrDefault(c => c.Id == acr.IdFallaPrimaria).Id;
                };
                if (db.FallaSecundaria.FirstOrDefault(c => c.Id == acr.IdFallaSecundaria) != null)
                {
                    FallaSecundaria01 = db.FallaSecundaria.FirstOrDefault(c => c.Id == acr.IdFallaSecundaria).Id;
                };
            };
     
            AnalisisCausaRaizModels Edit = new AnalisisCausaRaizModels()
            {
                EventoId = EventoID,
                RCAUnica = db.AnalisisCausaRaiz.FirstOrDefault(c => c.Id == IdCausa),
                ListaOrigenFalla = db.OrigenFalla.ToList(),
                EventoUnico = db.Evento.FirstOrDefault(c => c.Id == EventoID),
                Causa1 = Causa1,
                Causa2 = Causa2,
                Causa3 = Causa3,
                Causa4 = Causa4,
                Causa5 = Causa5,
                CausaIntermedia1 = CausaIntermedia1,
                CausaIntermedia2 = CausaIntermedia2,
                ListaCausa1= db.Causa1.ToList(),
                ListaCausa2 = db.Causa2.ToList(),
                ListaCausa3 = db.Causa3.ToList(),
                ListaCausa4 = db.Causa4.ToList(),
                ListaCausa5 = db.Causa5.ToList(),
                ListaCausaIntermedia1 = db.CausaIntermedia1.ToList(),
                ListaCausaIntermedia2 = db.CausaIntermedia2.ToList(),
                Proceso1 = Proceso1,
                Proceso2 = Proceso2,
                Proceso3 = Proceso3,
                Proceso4 = Proceso4,
                Proceso5 = Proceso5,
                ListaProceso1 = db.Proceso1.ToList(),
                ListaProceso2 = db.Proceso2.ToList(),
                ListaProceso3 = db.Proceso3.ToList(),
                ListaProceso4 = db.Proceso4.ToList(),
                ListaProceso5 = db.Proceso5.ToList(),
                ListaPersonas = db.Persona.ToList(),
                ListaArchivos = db.Archivo.ToList().Where(c => c.Identificador.TrimEnd().Equals(EventoID)),
                ListaPersonaEvento = db.PersonaEvento.ToList().Where(c => c.IdEvento.TrimEnd().Equals(EventoID)),
                FallaPrimaria = FallaPrimaria01,
                FallaSecundaria=FallaSecundaria01,
                ListaFallaPrimaria = db.FallaPrimaria.ToList(),
                ListaFallaSecundaria = db.FallaSecundaria.ToList(),
            };
            return View(Edit);
        }

        [HttpPost]
        public async Task<IActionResult> FormularioEditCausa(AnalisisCausaRaizModels Edit, List<IFormFile> files)
        {
            int contador = 0;
            string[] filename2;
            if (ModelState.IsValid)
            {
                foreach (var formFile in files)
                {
                    contador++;

                    if (formFile.Length > 0)
                    {
                        string cadena = formFile.FileName;
                        filename2 = cadena.Split('.');
                        if (filename2.Last().Equals("doc") || filename2.Last().Equals("docx") || filename2.Last().Equals("txt") || filename2.Last().Equals("csv") || filename2.Last().Equals("pdf") || filename2.Last().Equals("xls") || filename2.Last().Equals("xlsx") || filename2.Last().Equals("odp") || filename2.Last().Equals("ods") || filename2.Last().Equals("odt") || filename2.Last().Equals("vsd") || filename2.Last().Equals("pptx") || filename2.Last().Equals("ppt") || filename2.Last().Equals("potx") || filename2.Last().Equals("png") || filename2.Last().Equals("jpg") || filename2.Last().Equals("jpeg") || filename2.Last().Equals("ico") || filename2.Last().Equals("webp") || filename2.Last().Equals("avi") || filename2.Last().Equals("mp4") || filename2.Last().Equals("mpeg") || filename2.Last().Equals("mpg") || filename2.Last().Equals("ogv") || filename2.Last().Equals("webm") || filename2.Last().Equals("mp3") || filename2.Last().Equals("wav") || filename2.Last().Equals("oga") || filename2.Last().Equals("aac") || filename2.Last().Equals("mid") || filename2.Last().Equals("midi") || filename2.Last().Equals("weba") || filename2.Last().Equals("rar") || filename2.Last().Equals("zip") || filename2.Last().Equals("bz") || filename2.Last().Equals("bz2"))
                        {
                            //ModelState.AddModelError("Error1", "no hay Error en el formato del archivo" + " " + filename2.Last());
                        }
                        else
                        {
                            ModelState.AddModelError("Error2", "Error en el formato del archivo" + " " + "." + filename2.Last());
                            return await FormularioEditCausa(Edit.RCAUnica.Id,Edit.EventoId);
                        }

                    }
                }

            


            AnalisisCausaRaiz updateRCA = db.AnalisisCausaRaiz.FirstOrDefault(c => c.Id == Edit.RCAUnica.Id);
            updateRCA.Descripcion = Edit.Descripcion;
            updateRCA.IdOrigenFalla = Edit.RCAUnica.IdOrigenFalla;
            updateRCA.FechaRegistro = Edit.FechaRegistro;
            updateRCA.IdCausa1 = Edit.Causa1;
            updateRCA.IdCausa2 = Edit.Causa2;
            updateRCA.IdCausa3 = Edit.Causa3;
            updateRCA.IdCausa4 = Edit.Causa4;
            updateRCA.IdCausa5 = Edit.Causa5;
            updateRCA.IdCausaIntermedia1 = Edit.CausaIntermedia1;
            updateRCA.IdCausaIntermedia2 = Edit.CausaIntermedia2;
            updateRCA.IdFallaPrimaria = Edit.FallaPrimaria;
            updateRCA.IdFallaSecundaria = Edit.FallaSecundaria;
            updateRCA.Costo = Convert.ToInt64(Edit.RCAUnica.Costo);
            updateRCA.IdProceso1 = Edit.Proceso1;
            updateRCA.IdProceso2 = Edit.Proceso2;
            updateRCA.IdProceso3 = Edit.Proceso3;
            updateRCA.IdProceso4 = Edit.Proceso4;
            updateRCA.IdProceso5 = Edit.Proceso5;

            db.AnalisisCausaRaiz.Update(updateRCA);
            db.SaveChanges();


            if (Edit.EventoUnico.Ncaceptada == true)
            {
                Evento UpdateEvento = db.Evento.FirstOrDefault(c => c.Id == Edit.EventoId);
                UpdateEvento.Ncaceptada = true;
                UpdateEvento.Estado = 5;
                db.Evento.Update(UpdateEvento);
                db.SaveChanges();
            }
            else
            {
                if (Edit.EventoUnico.Ncaceptada == false)
                {
                    Evento UpdateEvento = db.Evento.FirstOrDefault(c => c.Id == Edit.EventoId);
                    UpdateEvento.Ncaceptada = false;
                    UpdateEvento.Estado = 10;
                    db.Evento.Update(UpdateEvento);
                    db.SaveChanges();
                }
            }

            var filePath = Path.GetTempFileName();
            string direccionRCAId = db.AnalisisCausaRaiz.FirstOrDefault(c => c.IdEvento == updateRCA.IdEvento).Id.TrimEnd();
            string concatenar = updateRCA.IdEvento.TrimEnd() + "/" + "AnalisisCausaRaiz" + "/" + direccionRCAId;
            foreach (var formFile in files)
            {

                if (formFile.Length > 0)
                {
                    string uploadPath = Path.Combine(_enviroment.WebRootPath, "uploads");
                    Directory.CreateDirectory(Path.Combine(uploadPath, concatenar));
                    string filename = formFile.FileName;
                    using (FileStream fs = new FileStream(Path.Combine(uploadPath, concatenar, filename), FileMode.Create))
                    {
                        await formFile.CopyToAsync(fs);
                    }

                    Archivo ar = new Archivo()
                    {
                        Identificador = updateRCA.IdEvento.TrimEnd(),
                        Nombre = filename,
                        Tipo = "AnalisisCausaRaiz",
                        Removed = false,
                        FechaRegistro = DateTime.Now,
                    };
                    db.Archivo.Add(ar);
                    db.SaveChanges();



                }
            }
            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = Edit.EventoId });
            }
            else
            {
                return await FormularioEditCausa(Edit.RCAUnica.Id, Edit.EventoId.TrimEnd());
            }
        }

       
        public IActionResult DeleteCausa(int IdCausa, string EventoID)
        {
            Causa updateCausa = db.Causa.FirstOrDefault(c => c.Id == IdCausa);
            updateCausa.Removed =true;
            db.Causa.Update(updateCausa);
            db.SaveChanges();
            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = EventoID });
            
        }

        public IActionResult EliminarCausaModal(EventoSecuenciaModels esm)
        {
            AnalisisCausaRaiz updateRCA = db.AnalisisCausaRaiz.FirstOrDefault(c => c.Id == esm.RCA.Id);
            updateRCA.Removed = true;
            db.AnalisisCausaRaiz.Update(updateRCA);
            db.SaveChanges();

            Evento UpdateEvento = db.Evento.FirstOrDefault(c=>c.Id==esm.EventoUnico.Id);
            UpdateEvento.Ncaceptada = null;
            UpdateEvento.Estado = 4;
            db.Evento.Update(UpdateEvento);
            db.SaveChanges();
            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = esm.EventoUnico.Id.TrimEnd() });


        }

        public IActionResult EliminarCausa(string Id,string IdEvento)
        {
            AnalisisCausaRaiz updateRCA = db.AnalisisCausaRaiz.FirstOrDefault(c => c.Id == Id);
            updateRCA.Removed = true;
            db.AnalisisCausaRaiz.Update(updateRCA);
            db.SaveChanges();

            Evento UpdateEvento = db.Evento.FirstOrDefault(c => c.Id == IdEvento);
            UpdateEvento.Ncaceptada = null;
            UpdateEvento.Estado = 4;
            db.Evento.Update(UpdateEvento);
            db.SaveChanges();
            return Json("Eliminado");


        }
        public async Task<List<AnalisisCausaRaiz>> DeleteCausaModalAjax(string id)
        {
            List<AnalisisCausaRaiz> le = new List<AnalisisCausaRaiz>();
            var RCA = db.AnalisisCausaRaiz.FirstOrDefault(c => c.Id == id);
            le.Add(RCA);
            return le;

        }

        public async Task<IActionResult> SubidaRCAFile(EventoSecuenciaModels esm, IFormFile Nombre)
        {
            CreateParte parteFile = new CreateParte();
            var filePath = Path.GetTempFileName();
            string direccionRCAId = db.AnalisisCausaRaiz.FirstOrDefault(c => c.IdEvento == esm.EventoUnico.Id).Id.TrimEnd();
            string concatenar = esm.EventoUnico.Id.TrimEnd() + "/" + "AnalisisCausaRaiz" + "/" + direccionRCAId;
            if (Nombre != null)
            {
                string uploadPath = Path.Combine(_enviroment.WebRootPath, "uploads");
                Directory.CreateDirectory(Path.Combine(uploadPath, concatenar));
                string filename = Nombre.FileName;
                using (FileStream fs = new FileStream(Path.Combine(uploadPath, concatenar, filename), FileMode.Create))
                {
                    await Nombre.CopyToAsync(fs);
                }

                Archivo ar = new Archivo()
                {
                    Identificador = esm.EventoUnico.Id.TrimEnd(),
                    Nombre = filename,
                    Tipo = "AnalisisCausaRaiz",
                    Removed = false,
                    FechaRegistro = DateTime.Now,
                };
                db.Archivo.Add(ar);
                db.SaveChanges();

            }
            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = esm.EventoUnico.Id.TrimEnd() });
        }


        public async Task<IActionResult> DownloadRCAFile(string filename, string Identificador, string tipo,string IdRCA)
        {
            if (filename == null)
                return Content("filename not present");
            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot", "uploads", Identificador.TrimEnd() + "/AnalisisCausaRaiz"+"/"+ IdRCA, filename.TrimEnd());

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                //------Documentos------
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".txt", "text/plain"},
                {".csv", "text/csv"},
                {".pdf", "application/pdf"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".odp", "application/vnd.oasis.opendocument.presentation"},
                {".ods", "	application/vnd.oasis.opendocument.spreadsheet"},
                {".odt", "	application/vnd.oasis.opendocument.text"},
                {".vsd", "application/vnd.visio"},
                //---PPT----
                {".pptx","application/vnd.openxmlformats-officedocument.presentationml.presentation" },
                {".ppt","application/vnd.ms-powerpoint" },
                {".potx","application/vnd.openxmlformats-officedocument.presentationml.template" },

                //-------Imagen-------
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".ico", "image/x-icon"},
                {".webp", "image/webp"},
              
                //---Video-----
                {".avi","video/*" },
                {".mp4","video/*" },
                {".mpeg","video/*" },
                {".mpg","video/*" },
                {".ogv","video/ogg" },
                {".webm","video/webm" },
                {".gif","image/gif" },
               
               
                //---Audio
                {".mp3","audio/*" },
                {".wav","audio/x-wav" },
                {".oga","audio/ogg" },
                {".aac","audio/aac" },
                {".mid","audio/midi" },
                {".midi","audio/midi" },
                {".weba","audio/webm" },
      
                //----zip y rar----
                {".rar","application/x-rar-compressed" },
                {".zip","application/zip" },
                {".bz","application/x-bzip" },
                {".bz2","application/x-bzip2" },

            };
        }

    }
}