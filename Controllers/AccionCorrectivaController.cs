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
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FINNINGWEB.Controllers
{
    [Authorize]
    public class AccionCorrectivaController : Controller
    {

        private IHostingEnvironment _enviroment;
        private UserManager<AppUser> userManager;
        public AccionCorrectivaController(UserManager<AppUser> userMgr, IHostingEnvironment enviroment)
        {
            userManager = userMgr;
            _enviroment = enviroment;
        }
        private AdventureWorksContext db = new AdventureWorksContext();
        // GET: /<controller>/


        public IActionResult IndexAccionInmediata(string eventoID)
        {
            AccionInmediataModels FormularioCrearAcccionInmediata = new AccionInmediataModels()
            {
                EventoId = eventoID,
                ListaPersonas = db.Persona.ToList(),

            };

            return View(FormularioCrearAcccionInmediata);
        }

       
        public async Task<IActionResult> CrearFormularioAccionCorrectiva(string EventoID)
        {
            AccionCorrectivaModels ac = new AccionCorrectivaModels()
            {
               //ListaPersonas = db.Persona.ToList(),
               EventoId = EventoID,

            };   
            return View(ac);
        }



        [HttpPost]
        public async Task<IActionResult> CrearFormularioAccionCorrectiva(AccionCorrectivaModels AC)
        {
            if (ModelState.IsValid)
            {
                AccionCorrectiva Accion = new AccionCorrectiva()
                {
                    FechaRegistro = DateTime.Now,
                    FechaLimite = AC.FechaLImite,
                    EventoId = AC.EventoId,
                    RutPersona = AC.RutPersona,
                    Causa = AC.Causa,
                    AccionCorrectiva1 = AC.AccionCorrectiva,
                    Estado = "Pendiente",
                    Removed = false,
                };
                db.AccionCorrectiva.Add(Accion);
                db.SaveChanges();

                PersonaEvento pe = new PersonaEvento()
                {

                    RutPersona = AC.RutPersona,
                    IdEvento = AC.EventoId.TrimEnd(),
                    Removed = false,
                    FechaRegistro = DateTime.Now,
                    Estado = 6,
                    IdAccionCorrectiva = Accion.Id,


                };
                db.PersonaEvento.Add(pe);
                db.SaveChanges();





                return RedirectToAction("Index", "EventoSecuencia", new { EventoID = AC.EventoId });

            }
            else
            {
                return await CrearFormularioAccionCorrectiva(AC.EventoId.TrimEnd());
            }
        }

        public async Task<IActionResult> FormularioEditAccionCorrectiva(string EventoID, string AccionCorrectivaID)
        {
            AccionCorrectiva AC = db.AccionCorrectiva.FirstOrDefault(c => c.Id == AccionCorrectivaID);
            AccionCorrectivaModels Accion = new AccionCorrectivaModels()
            {
                AccionUnica = AC,
                //ListaPersonas = db.Persona.ToList().Where(c => !c.Removed == true),
                RutPersona = AC.RutPersona,
                EventoId = (db.AccionCorrectiva.FirstOrDefault(c => c.Id == AccionCorrectivaID).EventoId),
                ListaArchivos = db.Archivo.ToList().Where(c => !c.Removed == true),

            };

            string nombrecompleto = null;
            if (AC.RutPersona != null)
            {
                nombrecompleto = db.Persona.Select(c => new { nombreCompleto = c.Nombre.TrimEnd() + " " + c.ApellidoPaterno.TrimEnd() + " " + c.ApellidoMaterno.TrimEnd(), rut = c.Rut }).FirstOrDefault(c => c.rut.TrimEnd().Equals(Accion.AccionUnica.RutPersona.TrimEnd())).nombreCompleto;
            }
            Accion.NombreCompleto = nombrecompleto;
            return View(Accion);
        }

        [HttpPost]
        public async Task<IActionResult> FormularioEditAccionCorrectiva(AccionCorrectivaModels accion)
        {

            if (ModelState.IsValid)
            {
                AccionCorrectiva updateAccionCorrectiva = db.AccionCorrectiva.FirstOrDefault(c => c.Id == accion.AccionUnica.Id);
                updateAccionCorrectiva.RutPersona = accion.RutPersona;
                updateAccionCorrectiva.FechaLimite = accion.FechaLImite;
                updateAccionCorrectiva.Causa = accion.Causa;
                updateAccionCorrectiva.AccionCorrectiva1 = accion.AccionCorrectiva;
                updateAccionCorrectiva.Descripcion = accion.Descripcion;

                db.AccionCorrectiva.Update(updateAccionCorrectiva);
                db.SaveChanges();
                return RedirectToAction("Index", "EventoSecuencia", new { EventoID = accion.AccionUnica.EventoId.TrimEnd() });
            }
            else
            {
                return await FormularioEditAccionCorrectiva(accion.AccionUnica.EventoId.TrimEnd(), accion.AccionUnica.Id.TrimEnd());
            }
        }

        [HttpPost]
        public async Task<IActionResult> FormularioEditarArchivoAccionCorrectiva(AccionCorrectivaModels ac, List<IFormFile> files)
        {

           
            AccionCorrectiva updateAccionCorrectiva = db.AccionCorrectiva.FirstOrDefault(c => c.Id == ac.AccionUnica.Id);
            updateAccionCorrectiva.Estado = "Cerrado";
            updateAccionCorrectiva.Descripcion = ac.Descripcion;
            updateAccionCorrectiva.FechaEjecucion = ac.FechaEjecucion;
            db.AccionCorrectiva.Update(updateAccionCorrectiva);
            db.SaveChanges();


            var filePath = Path.GetTempFileName();
            string direccionACId = ac.AccionUnica.Id.TrimEnd();
            string concatenar = ac.AccionUnica.EventoId.TrimEnd() + "/" + "AccionCorrectiva" + "/" + direccionACId;

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
                        Identificador = ac.AccionUnica.EventoId.TrimEnd(),
                        Identificador2 = ac.AccionUnica.Id.TrimEnd(),
                        Nombre = filename,
                        Tipo = "AccionCorrectiva",
                        Removed = false,
                        FechaRegistro = DateTime.Now,
                    };
                    db.Archivo.Add(ar);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("FormularioEditAccionCorrectiva", "AccionCorrectiva", new { EventoID = ac.AccionUnica.EventoId.TrimEnd(), AccionCorrectivaID = ac.AccionUnica.Id.TrimEnd() });

        }


        public ViewResult FormularioDetalleAccionCorrectiva(string EventoID, string AccionCorrectivaID)
        {
            AccionCorrectiva AC = db.AccionCorrectiva.FirstOrDefault(c => c.Id == AccionCorrectivaID);
            AccionCorrectivaModels Accion = new AccionCorrectivaModels()
            {
                AccionUnica = AC,
                //ListaPersonas = db.Persona.ToList().Where(c => !c.Removed == true),
                EventoId = (db.AccionCorrectiva.FirstOrDefault(c => c.Id == AccionCorrectivaID).EventoId),
                ListaArchivos = db.Archivo.ToList().Where(c => !c.Removed == true),

            };
            string nombrecompleto = null;
            if (AC.RutPersona != null)
            {
                nombrecompleto = db.Persona.Select(c => new { nombreCompleto = c.Nombre.TrimEnd() + " " + c.ApellidoPaterno.TrimEnd() + " " + c.ApellidoMaterno.TrimEnd(), rut = c.Rut }).FirstOrDefault(c => c.rut.TrimEnd().Equals(Accion.AccionUnica.RutPersona.TrimEnd())).nombreCompleto;
            }
            Accion.NombreCompleto = nombrecompleto;

            return View(Accion);
        }



        public async Task<IActionResult> FormularioCerrarAccionCorrectiva(string EventoID, string AccionCorrectivaID)
        {
            AccionCorrectiva AC = db.AccionCorrectiva.FirstOrDefault(c => c.Id == AccionCorrectivaID);
            AccionCorrectivaModels Accion = new AccionCorrectivaModels()
            {
                AccionUnica = AC,
                //ListaPersonas = db.Persona.ToList().Where(c => !c.Removed == true),
                ListaArchivos = db.Archivo.ToList().Where(c=>!c.Removed==true),

            };
            string nombrecompleto = null;
            if (AC.RutPersona != null)
            {
                nombrecompleto = db.Persona.Select(c => new { nombreCompleto = c.Nombre.TrimEnd() + " " + c.ApellidoPaterno.TrimEnd() + " " + c.ApellidoMaterno.TrimEnd(), rut = c.Rut }).FirstOrDefault(c => c.rut.TrimEnd().Equals(Accion.AccionUnica.RutPersona.TrimEnd())).nombreCompleto;
            }
            Accion.NombreCompleto = nombrecompleto;
            Accion.RutPersona = AC.RutPersona;
            return View(Accion);
        }

      

        [HttpPost]
        public async Task<IActionResult> FormularioCerrarAccionCorrectiva(AccionCorrectivaModels ac, List<IFormFile> files)
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
                           
                        }
                        else
                        {
                            ModelState.AddModelError("Error2", "Error en el formato del archivo" + " " + "." + filename2.Last());
                            return await FormularioCerrarAccionCorrectiva(ac.AccionUnica.EventoId.TrimEnd(), ac.AccionUnica.Id);
                        }

                    }
                }

            }

            if (ModelState.IsValid)
            {
                AccionCorrectiva updateAccionCorrectiva = db.AccionCorrectiva.FirstOrDefault(c => c.Id == ac.AccionUnica.Id);
                updateAccionCorrectiva.Estado = "Cerrado";

                updateAccionCorrectiva.Descripcion = ac.Descripcion;
                updateAccionCorrectiva.FechaEjecucion = ac.FechaEjecucion;

                db.AccionCorrectiva.Update(updateAccionCorrectiva);
                db.SaveChanges();


                var filePath = Path.GetTempFileName();

                string direccionACId = ac.AccionUnica.Id.TrimEnd();

                string concatenar = ac.AccionUnica.EventoId.TrimEnd() + "/" + "AccionCorrectiva" + "/" + direccionACId;

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
                            Identificador = ac.AccionUnica.EventoId.TrimEnd(),
                            Identificador2 = ac.AccionUnica.Id.TrimEnd(),
                            Nombre = filename,
                            Tipo = "AccionCorrectiva",
                            Removed = false,
                            FechaRegistro = DateTime.Now,
                        };
                        db.Archivo.Add(ar);
                        db.SaveChanges();



                    }
                }
                return RedirectToAction("Index", "EventoSecuencia", new { EventoID = ac.AccionUnica.EventoId.TrimEnd() });
            }
            else
            {
                return await FormularioCerrarAccionCorrectiva(ac.AccionUnica.EventoId.TrimEnd(),ac.AccionUnica.Id);
            }
        }

        



        public async Task<IActionResult> DownloadACFile(string filename, string Identificador, string tipo, string IdAC)
        {
            if (filename == null)
                return Content("filename not present");

            //string uploadPath = Path.Combine(_enviroment.WebRootPath, "uploads");
            //Directory.CreateDirectory(Path.Combine(uploadPath, Id));



            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot", "uploads", Identificador.TrimEnd() + "/ACCIONCORRECTIVA" + "/" + IdAC, filename.TrimEnd());

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        public IActionResult DeleteArchivo(string EventoId, int FileId)
        {
            Archivo DeleteArchivo = db.Archivo.FirstOrDefault(c => c.FileId == FileId);
            DeleteArchivo.Removed = true;
            db.Archivo.Update(DeleteArchivo);
            db.SaveChanges();

            AccionCorrectiva ac = db.AccionCorrectiva.FirstOrDefault(c=>c.EventoId.TrimEnd().Equals(EventoId));
        

          return RedirectToAction("FormularioCerrarAccionCorrectiva", "AccionCorrectiva", new {EventoID= EventoId,AccionCorrectivaID=ac.Id } );
            

        }

        public IActionResult DeleteArchivoEdit(string EventoId, int FileId)
        {
            Archivo DeleteArchivo = db.Archivo.FirstOrDefault(c => c.FileId == FileId);
            DeleteArchivo.Removed = true;
            db.Archivo.Update(DeleteArchivo);
            db.SaveChanges();

            AccionCorrectiva ac = db.AccionCorrectiva.FirstOrDefault(c => c.EventoId.TrimEnd().Equals(EventoId));


            return RedirectToAction("FormularioEditAccionCorrectiva", "AccionCorrectiva", new { EventoID = EventoId, AccionCorrectivaID = ac.Id });


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



        [HttpPost]
        public IActionResult FormularioEditAccionCorrectivaEtapa2(AccionCorrectivaModels accion)
        {


            AccionCorrectiva updateAccionCorrectiva = db.AccionCorrectiva.FirstOrDefault(c => c.Id == accion.AccionUnica.Id);
           
            updateAccionCorrectiva.FechaEjecucion = accion.FechaEjecucion;
      
            updateAccionCorrectiva.Descripcion = accion.Descripcion;

            db.AccionCorrectiva.Update(updateAccionCorrectiva);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = accion.AccionUnica.EventoId.TrimEnd() });

        }


        public IActionResult DeleteAccionInmediata(string AccioninmediataID)
        {


            AccionInmediata updateAccionInmediata = db.AccionInmediata.FirstOrDefault(c => c.Id == AccioninmediataID);
            updateAccionInmediata.Removed = true;


            db.AccionInmediata.Update(updateAccionInmediata);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = updateAccionInmediata.EventoId });

        }

        public IActionResult DeleteAccionCorrectiva(string AccionCorrectivaID)
        {


            AccionCorrectiva updateAccionCorrectiva = db.AccionCorrectiva.FirstOrDefault(c => c.Id == AccionCorrectivaID);
            updateAccionCorrectiva.Removed = true;


            db.AccionCorrectiva.Update(updateAccionCorrectiva);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return Json("Eliminado");

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

            return Json(Datoslist);
        }
    }
}