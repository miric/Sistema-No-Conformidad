using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using Dapper;
using FINNINGWEB.Models;
using Microsoft.AspNetCore.Authorization;
using FINNINGWEB.Entities;
using System.Dynamic;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using ReflectionIT.Mvc.Paging;
using OfficeOpenXml;

namespace FINNINGWEB.Models
{
    [Authorize]
    public class HomeController : Controller
    {
        private IHostingEnvironment _enviroment;
        private UserManager<AppUser> userManager;
        private IUserValidator<AppUser> userValidator;
        private IPasswordValidator<AppUser> passwordValidator;
        private IPasswordHasher<AppUser> passwordHasher;

        public HomeController(UserManager<AppUser> userMgr, IHostingEnvironment enviroment, IUserValidator<AppUser> userValid,
        IPasswordValidator<AppUser> passValid,
        IPasswordHasher<AppUser> passwordHash)
        {
            userManager = userMgr;
            _enviroment = enviroment;
            userValidator = userValid;
            passwordValidator = passValid;
            passwordHasher = passwordHash;
        }

        private AdventureWorksContext db = new AdventureWorksContext();

        [Authorize]
        public async Task<IActionResult> Index(
        string sortOrder,
        string currentFilter,
        int? currentFilterCriticidad,
        int? currentFilterDias,
        int? currentFilterHoras,
        string searchString,
        int? searchIntCriticidad,
        int? searchIntDias,
        int? searchIntHoras,
        int? page,
        string TipoEventoSearch,
        int? EstadoSearch,
        string AnioSearch,
        string mesSearch,
        string AsignacionSearch,
        string AplicaRCASearch,
        string optradio)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["IdSortParm"] = sortOrder == "id_desc" ? "Id" : "id_desc";
            ViewData["TipoEventoSortParm"] = sortOrder == "tipo_evento" ? "tipo_evento_desc" : "tipo_evento";
            ViewData["EstadoSortParm"] = String.IsNullOrEmpty(sortOrder) ? "estado_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewData["ClienteSortParm"] = sortOrder == "Cliente" ? "cliente_desc" : "Cliente";
            ViewData["EquipoSortParm"] = sortOrder == "Equipo" ? "equipo_desc" : "Equipo";
            ViewData["ModeloSortParm"] = sortOrder == "Modelo" ? "modelo_desc" : "Modelo";
            ViewData["ComponenteSortParm"] = sortOrder == "Componente" ? "componente_desc" : "Componente";
            ViewData["HorasComponenteSortParm"] = sortOrder == "HorasComponente" ? "horascomponente_desc" : "HorasComponente";
            ViewData["ParteSortParm"] = sortOrder == "Parte" ? "parte_desc" : "Parte";
            ViewData["DescripcionSortParm"] = sortOrder == "Descripcion" ? "descripcion_desc" : "Descripcion";
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentFilterCriticidad"] = searchIntCriticidad;
            ViewData["CurrentFilterDias"] = searchIntDias;
            ViewData["currentFilterHoras"] = searchIntHoras;

            ViewData["TipoEventoFilter"] = TipoEventoSearch;
            ViewData["AñoFilter"] = AnioSearch;

            string CapturandoUsuario = null;
            ClaimsPrincipal currentUser = User;
            PruebaUser prueba;

            if (currentUser.IsInRole("Admins"))
            {
                CapturandoUsuario = "Administrador";

                prueba = new PruebaUser()
                {
                    ListaUsuarios = db.AspNetUsers.ToList(),
                    Usuario = new AspNetUsers()
                    {
                        Id = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value,

                    }
                };
            }
            else
            {
                prueba = new PruebaUser()
                {
                    ListaUsuarios = db.AspNetUsers.ToList(),
                    Usuario = new AspNetUsers()
                    {
                        Id = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value,

                    }
                };

                CapturandoUsuario = db.Persona.Select(c => new { nombreCompleto = c.Nombre + c.ApellidoPaterno + c.ApellidoMaterno, rut = c.Rut }).FirstOrDefault(c => c.rut.Equals(db.AspNetUsers.FirstOrDefault(d => d.Id.Equals(prueba.Usuario.Id)).RutPersona)).nombreCompleto;
            }

            var eventos = from s in db.Evento where s.Estado!=10
                          select s;

            if (currentUser.IsInRole("Calidad"))
            {
                Persona persona = new Persona()
                {
                    Rut = db.AspNetUsers.FirstOrDefault(c => c.Id.Equals(currentUser.getUserId())).RutPersona,

                };

                eventos = eventos.Where(c => c.Estado != 11 && c.Removed != true);

            }
            else
            {
                if (currentUser.IsInRole("Estandar"))
                {
                    Persona persona = new Persona()
                    {
                        Rut = db.AspNetUsers.FirstOrDefault(c => c.Id.Equals(currentUser.getUserId())).RutPersona,

                    };
                    eventos = (from e in db.Evento
                               join pe in db.PersonaEvento on e.Id equals pe.IdEvento
                               where ((pe.RutPersona == persona.Rut) && !e.Removed == true && e.Estado != 10 && pe.Estado == e.Estado)
                               select e);
                }
                else
                {
                    if (currentUser.IsInRole("Cliente"))
                    {
                        Persona persona = new Persona()
                        {
                            Rut = db.AspNetUsers.FirstOrDefault(c => c.Id.Equals(currentUser.getUserId())).RutPersona,

                        };

                        eventos = from e in db.Evento
                                  join pe in db.PersonaEvento on e.Id equals pe.IdEvento
                                  where (pe.RutPersona == persona.Rut) && !e.Removed == true && e.Estado != 7 && pe.Estado >= e.Estado
                                  select e;
                    }
                    else
                    {
                        if (currentUser.IsInRole("Admins"))
                        {
                            eventos = from e in db.Evento
                                      where (!e.Removed == true && e.Estado != 10)
                                      select e;
                        }
                    }
                }
            }

            if (!String.IsNullOrEmpty(AnioSearch))
            {
                int Anio = Convert.ToInt32(AnioSearch);

                eventos = eventos.Where(e => e.FechaRegistro.Value.Year.Equals(Anio));

            }


            if (!String.IsNullOrEmpty(mesSearch))
            {
                int mes = Convert.ToInt32(mesSearch);

                eventos = eventos.Where(e => e.FechaRegistro.Value.Month.Equals(mes));

            }


            if (!String.IsNullOrEmpty(TipoEventoSearch))
            {
                eventos = eventos.Where(s => s.TipoEvento == db.TipoEvento.FirstOrDefault(c => c.Tipo == TipoEventoSearch).Id
                                       );
            }


            if (!String.IsNullOrEmpty(AsignacionSearch))
            {
                if (AsignacionSearch == "Creados" && CapturandoUsuario != "Administrador")
                {
                    eventos = eventos.Where(s => s.Creador == db.AspNetUsers.FirstOrDefault(c => c.Id == prueba.Usuario.Id).RutPersona);
                }
                else
                {
                    if (AsignacionSearch == "Creados" && CapturandoUsuario == "Administrador")
                    {
                        eventos = eventos.Where(s => s.Creador == "Administrador");
                    }
                }

                if (AsignacionSearch == "Responsable" && CapturandoUsuario != "Administrador")
                {

                    eventos = (from e in eventos
                               join pe in db.PersonaEvento on e.Id equals pe.IdEvento
                               where pe.RutPersona == db.AspNetUsers.FirstOrDefault(d => d.Id == prueba.Usuario.Id).RutPersona
                               select e).Distinct();

                }

            }

            if (!String.IsNullOrEmpty(AplicaRCASearch))
            {
                if (AplicaRCASearch.Equals("Aceptada"))
                {
                    eventos = eventos.Where(s => s.Ncaceptada == true);
                }
                if (AplicaRCASearch.Equals("Rechazada"))
                {
                    eventos = eventos.Where(s => s.Ncaceptada == false);
                }
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                eventos = eventos.Where(s => s.Id.Contains(searchString)
                                       || db.TipoEvento.FirstOrDefault(c => c.Id == s.TipoEvento).Tipo.Contains(searchString.TrimEnd())
                                       || db.Estado.FirstOrDefault(c => c.Id == s.Estado).Nombre.Contains(searchString.TrimEnd())
                                       || db.Cliente.FirstOrDefault(c => c.Id == s.Cliente).Nombre.Contains(searchString.TrimEnd())
                                       || db.Equipo.FirstOrDefault(c => c.Id == s.Equipo).Nombre.Contains(searchString.TrimEnd())
                                       || db.Modelo.FirstOrDefault(c => c.Id == s.Modelo).Nombre.Contains(searchString.TrimEnd())
                                       || db.Componente.FirstOrDefault(c => c.Id == s.Componente).Nombre.Contains(searchString.TrimEnd())
                                       || db.Parte.FirstOrDefault(c => c.Id == s.Parte).Nombre.Contains(searchString.TrimEnd())
                                       || s.Descripcion.Contains(searchString)
                                       || db.Persona.FirstOrDefault(c => c.Rut == s.Creador).Nombre.Contains(searchString)
                                       || db.Area.FirstOrDefault(c => c.Id == s.Area).Nombre.Contains(searchString)
                                       || db.SubArea.FirstOrDefault(c => c.Id == s.SubArea).Nombre.Contains(searchString)


                                       );
            }

            if (searchIntHoras != null)
            {
                eventos = eventos.Where(s => s.HorasComponente > searchIntHoras

                                       );
            }


            if (searchIntDias != null)
            {

                eventos = eventos.Where(s => (Convert.ToInt32((DateTime.Now - s.FechaRegistro).Value.Days) > searchIntDias));
            }

            if (EstadoSearch != null)
            {
                if (EstadoSearch != 11)
                {
                    eventos = eventos.Where(s => s.Estado == EstadoSearch);
                }
                else
                {
                    eventos = eventos.Where(s => s.Estado != 10);
                }

            }

            switch (sortOrder)
            {
                case "Id":
                    eventos = eventos.OrderBy(s => s.Id);
                    break;
                case "id_desc":
                    eventos = eventos.OrderByDescending(s => s.Id);
                    break;
                case "tipo_evento":
                    eventos = eventos.OrderBy(s => s.TipoEvento);
                    break;
                case "tipo_evento_desc":
                    eventos = eventos.OrderByDescending(s => s.TipoEvento);
                    break;
                case "estado_desc":
                    eventos = eventos.OrderBy(s => s.Estado);
                    break;
                case "Date":
                    eventos = eventos.OrderBy(s => s.FechaRegistro);
                    break;
                case "date_desc":
                    eventos = eventos.OrderByDescending(s => s.FechaRegistro);
                    break;
                case "Cliente":
                    eventos = eventos.OrderBy(s => s.Cliente);
                    break;
                case "cliente_desc":
                    eventos = eventos.OrderByDescending(s => s.Cliente);
                    break;
                case "Equipo":
                    eventos = eventos.OrderBy(s => s.Equipo);
                    break;
                case "equipo_desc":
                    eventos = eventos.OrderByDescending(s => s.Equipo);
                    break;
                case "Modelo":
                    eventos = eventos.OrderBy(s => s.Modelo);
                    break;
                case "modelo_desc":
                    eventos = eventos.OrderByDescending(s => s.Modelo);
                    break;
                case "Componente":
                    eventos = eventos.OrderBy(s => s.Componente);
                    break;
                case "componente_desc":
                    eventos = eventos.OrderByDescending(s => s.Componente);
                    break;
                case "HorasComponente":
                    eventos = eventos.OrderBy(s => s.HorasComponente);
                    break;
                case "horascomponente_desc":
                    eventos = eventos.OrderByDescending(s => s.HorasComponente);
                    break;
                case "Parte":
                    eventos = eventos.OrderBy(s => s.Parte);
                    break;
                case "parte_desc":
                    eventos = eventos.OrderByDescending(s => s.Parte);
                    break;
                case "Descripcion":
                    eventos = eventos.OrderBy(s => s.Descripcion);
                    break;
                case "descripcion_desc":
                    eventos = eventos.OrderByDescending(s => s.Descripcion);
                    break;
                default:
                    eventos = eventos.OrderBy(s => s.Id);
                    break;
            }

            ListaEvento ep = new ListaEvento()
            {

            };
            int pageSize = 25;
            PaginatedList<FINNINGWEB.Entities.Evento> pagina = await PaginatedList<Evento>.CreateAsync(eventos.AsNoTracking(), page ?? 1, pageSize);
            ep.ListEvento = pagina;
            ep.probando = pagina;
            ep.Cliente = db.Cliente.ToList();
            ep.Equipo = db.Equipo.ToList();
            ep.Modelo = db.Modelo.ToList();
            ep.Componente = db.Componente.ToList();
            ep.Parte = db.Parte.ToList();
            ep.TipoEvento = db.TipoEvento.ToList();
            ep.search = searchString;
            ep.HorasSearch = searchIntHoras;
            ep.TipoEventoSearch = TipoEventoSearch;
            ep.EstadoSearch = EstadoSearch;
            ep.AnioSearch = AnioSearch;
            ep.MesSearch = mesSearch;
            ep.AsignacionSearch = AsignacionSearch;
            ep.optradio = optradio;
            ep.ListEstado = db.Estado.ToList();
            ep.HorasSearch = searchIntHoras;
            ep.FechaActual = DateTime.Now;
            ep.DiasSearch = searchIntDias;
            ep.AplicaRCASearch = AplicaRCASearch;
            return View(ep);

        }

        [Authorize]
        public async Task<IActionResult> DownloadExcel(
        string sortOrder,
        string currentFilter,
        int? currentFilterCriticidad,
        int? currentFilterDias,
        int? currentFilterHoras,
        string searchString,
        int? searchIntCriticidad,
        int? searchIntDias,
        int? searchIntHoras,
        int? page,
        string TipoEventoSearch,
        int? EstadoSearch,
        string AnioSearch,
        string mesSearch,
        string AsignacionSearch,
        string AplicaRCASearch,
        string optradio)
            {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["IdSortParm"] = sortOrder == "id_desc" ? "Id" : "id_desc";
            ViewData["TipoEventoSortParm"] = sortOrder == "tipo_evento" ? "tipo_evento_desc" : "tipo_evento";
            ViewData["EstadoSortParm"] = String.IsNullOrEmpty(sortOrder) ? "estado_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewData["ClienteSortParm"] = sortOrder == "Cliente" ? "cliente_desc" : "Cliente";
            ViewData["EquipoSortParm"] = sortOrder == "Equipo" ? "equipo_desc" : "Equipo";
            ViewData["ModeloSortParm"] = sortOrder == "Modelo" ? "modelo_desc" : "Modelo";
            ViewData["ComponenteSortParm"] = sortOrder == "Componente" ? "componente_desc" : "Componente";
            ViewData["HorasComponenteSortParm"] = sortOrder == "HorasComponente" ? "horascomponente_desc" : "HorasComponente";
            ViewData["ParteSortParm"] = sortOrder == "Parte" ? "parte_desc" : "Parte";
            ViewData["DescripcionSortParm"] = sortOrder == "Descripcion" ? "descripcion_desc" : "Descripcion";
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentFilterCriticidad"] = searchIntCriticidad;
            ViewData["CurrentFilterDias"] = searchIntDias;
            ViewData["currentFilterHoras"] = searchIntHoras;
            ViewData["TipoEventoFilter"] = TipoEventoSearch;
            ViewData["AñoFilter"] = AnioSearch;

            IEnumerable<Evento> eventos = db.Evento.ToList().Where(c => c.Removed != true);

            string CapturandoUsuario = null;
            ClaimsPrincipal currentUser = User;
            PruebaUser prueba;

            if (currentUser.IsInRole("Admins"))
            {
                CapturandoUsuario = "Administrador";

                prueba = new PruebaUser()
                {
                    ListaUsuarios = db.AspNetUsers.ToList(),
                    Usuario = new AspNetUsers()
                    {
                        Id = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value,

                    }
                };
            }
            else
            {
                prueba = new PruebaUser()
                {
                    ListaUsuarios = db.AspNetUsers.ToList(),
                    Usuario = new AspNetUsers()
                    {
                        Id = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value,

                    }
                };

                CapturandoUsuario = db.Persona.Select(c => new { nombreCompleto = c.Nombre + c.ApellidoPaterno + c.ApellidoMaterno, rut = c.Rut }).FirstOrDefault(c => c.rut.Equals(db.AspNetUsers.FirstOrDefault(d => d.Id.Equals(prueba.Usuario.Id)).RutPersona)).nombreCompleto;
            }


            if (!String.IsNullOrEmpty(AnioSearch))
            {
                int Anio = Convert.ToInt32(AnioSearch);

                eventos = eventos.Where(e => e.FechaRegistro.Value.Year.Equals(Anio));

            }


            if (!String.IsNullOrEmpty(mesSearch))
            {
                int mes = Convert.ToInt32(mesSearch);

                eventos = eventos.Where(e => e.FechaRegistro.Value.Month.Equals(mes));

            }


            if (!String.IsNullOrEmpty(TipoEventoSearch))
            {
                eventos = eventos.Where(s => s.TipoEvento == db.TipoEvento.FirstOrDefault(c => c.Tipo == TipoEventoSearch).Id
                                       );
            }


            if (!String.IsNullOrEmpty(AsignacionSearch))
            {
                if (AsignacionSearch == "Creados" && CapturandoUsuario != "Administrador")
                {
                    eventos = eventos.Where(s => s.Creador == db.AspNetUsers.FirstOrDefault(c => c.Id == prueba.Usuario.Id).RutPersona);
                }
                else
                {
                    if (AsignacionSearch == "Creados" && CapturandoUsuario == "Administrador")
                    {
                        eventos = eventos.Where(s => s.Creador.TrimEnd().Equals("Administrador"));
                    }
                }

                if (AsignacionSearch == "Responsable" && CapturandoUsuario != "Administrador")
                {

                    eventos = (from e in eventos
                               join pe in db.PersonaEvento on e.Id equals pe.IdEvento
                               where pe.RutPersona == db.AspNetUsers.FirstOrDefault(d => d.Id == prueba.Usuario.Id).RutPersona
                               select e).Distinct();

                }

            }

            if (!String.IsNullOrEmpty(AplicaRCASearch))
            {
                if (AplicaRCASearch.Equals("Aceptada"))
                {
                    eventos = eventos.Where(s => s.Ncaceptada == true);
                }
                if (AplicaRCASearch.Equals("Rechazada"))
                {
                    eventos = eventos.Where(s => s.Ncaceptada == false);
                }
            }
            int contadorsearch = 0;
            if (!String.IsNullOrEmpty(searchString))
            {


                if (eventos.Where(s => s.Id.Contains(searchString.TrimEnd())).ToList().Count()>0)
                {
                    eventos = eventos.Where(s => s.Id.Contains(searchString));
                    contadorsearch++;
                }
                if (db.TipoEvento.FirstOrDefault(c => c.Tipo.TrimEnd().Contains(searchString.TrimEnd())) != null)
                {
                    if (eventos.Where(s => s.TipoEvento == db.TipoEvento.FirstOrDefault(c => c.Tipo.Contains(searchString.TrimEnd())).Id).ToList().Count()>0)
                    {
                        eventos = eventos.Where(s => s.TipoEvento == db.TipoEvento.FirstOrDefault(c => c.Tipo.Contains(searchString.TrimEnd())).Id).ToList();
                        contadorsearch++;
                    }           
                }
                if (db.Estado.FirstOrDefault(c => c.Nombre.TrimEnd().Contains(searchString.TrimEnd())) != null)
                {
                    if (eventos.Where(s => s.Estado == db.Estado.FirstOrDefault(c => c.Nombre.TrimEnd().Contains(searchString.TrimEnd())).Id).ToList().Count()>0)
                    {
                        eventos = eventos.Where(s => s.Estado == db.Estado.FirstOrDefault(c => c.Nombre.TrimEnd().Contains(searchString.TrimEnd())).Id).ToList();
                        contadorsearch++;
                    }
                 
                }
                if (db.Cliente.FirstOrDefault(c => c.Nombre.TrimEnd().Contains(searchString.TrimEnd())) != null)
                {
                    if (eventos.Where(s => s.Cliente == db.Cliente.FirstOrDefault(c => c.Nombre.TrimEnd().Contains(searchString.TrimEnd())).Id).ToList().Count() > 0)
                    {
                        eventos = eventos.Where(s => s.Cliente == db.Cliente.FirstOrDefault(c => c.Nombre.TrimEnd().Contains(searchString.TrimEnd())).Id).ToList();
                        contadorsearch++;
                    }

                }
                if (db.Equipo.FirstOrDefault(c => c.Nombre.TrimEnd().Contains(searchString.TrimEnd())) != null)
                {
                    if (eventos.Where(s => s.Equipo == db.Equipo.FirstOrDefault(c => c.Nombre.TrimEnd().Contains(searchString.TrimEnd())).Id).ToList().Count() > 0)
                    {
                        eventos = eventos.Where(s => s.Equipo == db.Equipo.FirstOrDefault(c => c.Nombre.TrimEnd().Contains(searchString.TrimEnd())).Id).ToList();
                        contadorsearch++;
                    }

                }
                if (db.Modelo.FirstOrDefault(c => c.Nombre.TrimEnd().Contains(searchString.TrimEnd())) != null)
                {
                    if (eventos.Where(s => s.Modelo == db.Modelo.FirstOrDefault(c => c.Nombre.TrimEnd().Contains(searchString.TrimEnd())).Id).ToList().Count() > 0)
                    {
                        eventos = eventos.Where(s => s.Modelo == db.Modelo.FirstOrDefault(c => c.Nombre.TrimEnd().Contains(searchString.TrimEnd())).Id).ToList();
                        contadorsearch++;
                    }

                }
                if (db.Componente.FirstOrDefault(c => c.Nombre.TrimEnd().Contains(searchString.TrimEnd())) != null)
                {
                    if (eventos.Where(s => s.Componente == db.Componente.FirstOrDefault(c => c.Nombre.TrimEnd().Contains(searchString.TrimEnd())).Id).ToList().Count() > 0)
                    {
                        eventos = eventos.Where(s => s.Componente == db.Componente.FirstOrDefault(c => c.Nombre.TrimEnd().Contains(searchString.TrimEnd())).Id).ToList();
                        contadorsearch++;
                    }

                }
                if (db.Parte.FirstOrDefault(c => c.Nombre.TrimEnd().Contains(searchString.TrimEnd())) != null)
                {
                    if (eventos.Where(s => s.Parte == db.Parte.FirstOrDefault(c => c.Nombre.TrimEnd().Contains(searchString.TrimEnd())).Id).ToList().Count() > 0)
                    {
                        eventos = eventos.Where(s => s.Parte == db.Parte.FirstOrDefault(c => c.Nombre.TrimEnd().Contains(searchString.TrimEnd())).Id).ToList();
                        contadorsearch++;
                    }

                }
                if (db.Persona.FirstOrDefault(c => c.Nombre.TrimEnd().Contains(searchString.TrimEnd())) != null)
                {
                    if (eventos.Where(s => s.Creador == db.Persona.FirstOrDefault(c => c.Nombre.TrimEnd().Contains(searchString.TrimEnd())).Rut).ToList().Count() > 0)
                    {
                        eventos = eventos.Where(s => s.Creador == db.Persona.FirstOrDefault(c => c.Nombre.TrimEnd().Contains(searchString.TrimEnd())).Rut).ToList();
                        contadorsearch++;
                    }

                }
                if (db.Area.FirstOrDefault(c => c.Nombre.TrimEnd().Contains(searchString.TrimEnd())) != null)
                {
                    if (eventos.Where(s => s.Area == db.Area.FirstOrDefault(c => c.Nombre.TrimEnd().Contains(searchString.TrimEnd())).Id).ToList().Count() > 0)
                    {
                        eventos = eventos.Where(s => s.Area == db.Area.FirstOrDefault(c => c.Nombre.TrimEnd().Contains(searchString.TrimEnd())).Id).ToList();
                        contadorsearch++;
                    }

                }
                if (db.SubArea.FirstOrDefault(c => c.Nombre.TrimEnd().Contains(searchString.TrimEnd())) != null)
                {
                    if (eventos.Where(s => s.SubArea == db.SubArea.FirstOrDefault(c => c.Nombre.TrimEnd().Contains(searchString.TrimEnd())).Id).ToList().Count() > 0)
                    {
                        eventos = eventos.Where(s => s.SubArea == db.SubArea.FirstOrDefault(c => c.Nombre.TrimEnd().Contains(searchString.TrimEnd())).Id).ToList();
                        contadorsearch++;
                    }

                }
                if (contadorsearch==0)
                {
                    eventos = eventos.Where(s => s.Id.Contains(searchString));
                }
            }


            if (searchIntHoras != null)
            {
                eventos = eventos.Where(s => s.HorasComponente > searchIntHoras

                                       );
            }


            if (searchIntDias != null)
            {

                eventos = eventos.Where(s => (Convert.ToInt32((DateTime.Now - s.FechaRegistro).Value.Days) > searchIntDias));
            }

            if (EstadoSearch != null)
            {
                if (EstadoSearch != 11)
                {
                    eventos = eventos.Where(s => s.Estado == EstadoSearch);
                }
                else
                {
                    eventos = eventos.Where(s => s.Estado != 10);
                }

            }

            switch (sortOrder)
            {
                case "Id":
                    eventos = eventos.OrderBy(s => s.Id);
                    break;
                case "id_desc":
                    eventos = eventos.OrderByDescending(s => s.Id);
                    break;
                case "tipo_evento":
                    eventos = eventos.OrderBy(s => s.TipoEvento);
                    break;
                case "tipo_evento_desc":
                    eventos = eventos.OrderByDescending(s => s.TipoEvento);
                    break;
                case "estado_desc":
                    eventos = eventos.OrderBy(s => s.Estado);
                    break;
                case "Date":
                    eventos = eventos.OrderBy(s => s.FechaRegistro);
                    break;
                case "date_desc":
                    eventos = eventos.OrderByDescending(s => s.FechaRegistro);
                    break;
                case "Cliente":
                    eventos = eventos.OrderBy(s => s.Cliente);
                    break;
                case "cliente_desc":
                    eventos = eventos.OrderByDescending(s => s.Cliente);
                    break;
                case "Equipo":
                    eventos = eventos.OrderBy(s => s.Equipo);
                    break;
                case "equipo_desc":
                    eventos = eventos.OrderByDescending(s => s.Equipo);
                    break;
                case "Modelo":
                    eventos = eventos.OrderBy(s => s.Modelo);
                    break;
                case "modelo_desc":
                    eventos = eventos.OrderByDescending(s => s.Modelo);
                    break;
                case "Componente":
                    eventos = eventos.OrderBy(s => s.Componente);
                    break;
                case "componente_desc":
                    eventos = eventos.OrderByDescending(s => s.Componente);
                    break;
                case "HorasComponente":
                    eventos = eventos.OrderBy(s => s.HorasComponente);
                    break;
                case "horascomponente_desc":
                    eventos = eventos.OrderByDescending(s => s.HorasComponente);
                    break;
                case "Parte":
                    eventos = eventos.OrderBy(s => s.Parte);
                    break;
                case "parte_desc":
                    eventos = eventos.OrderByDescending(s => s.Parte);
                    break;
                case "Descripcion":
                    eventos = eventos.OrderBy(s => s.Descripcion);
                    break;
                case "descripcion_desc":
                    eventos = eventos.OrderByDescending(s => s.Descripcion);
                    break;
                default:
                    eventos = eventos.OrderBy(s => s.Id);
                    break;
            }

            string sWebRootFolder = _enviroment.WebRootPath;
            string sFileName = @"datos.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            using (ExcelPackage package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Eventos");
                //First add the headers
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "WorkOrder";
                worksheet.Cells[1, 3].Value = "TipoEvento";
                worksheet.Cells[1, 4].Value = "Fecha Registro";
                worksheet.Cells[1, 5].Value = "Fecha Identificación";
                worksheet.Cells[1, 6].Value = "Creador";
                worksheet.Cells[1, 7].Value = "Cliente";
                worksheet.Cells[1, 8].Value = "Equipo";
                worksheet.Cells[1, 9].Value = "Modelo";
                worksheet.Cells[1, 10].Value = "Serie Equipo";
                worksheet.Cells[1, 11].Value = "Componente";
                worksheet.Cells[1, 12].Value = "Posicion Componente";
                worksheet.Cells[1, 13].Value = "Parte";
                worksheet.Cells[1, 14].Value = "Numero Parte Componente";
                worksheet.Cells[1, 15].Value = "Serie Componente";
                worksheet.Cells[1, 16].Value = "Area";
                worksheet.Cells[1, 17].Value = "Sub Area";
                worksheet.Cells[1, 18].Value = "¿No Conformidad Aceptada?";
                worksheet.Cells[1, 19].Value = "Descripción";
                worksheet.Cells[1, 20].Value = "HorasComponente";
                worksheet.Cells[1, 20].Value = "HorasComponente";
                worksheet.Cells[1, 21].Value = "IDRCA";
                worksheet.Cells[1, 22].Value = "CreadorRCA";
                
                worksheet.Cells[1, 23].Value = "Origen Falla";
                worksheet.Cells[1, 24].Value = "Falla Primaria";
                worksheet.Cells[1, 25].Value = "Falla Secundaria";
                worksheet.Cells[1, 26].Value = "Costos";
                worksheet.Cells[1, 27].Value = "Causa";

                //Add values
                int contador = 2;
                string A;
                string B;
                string C;
                string D;
                string E;
                string F;
                string G;
                string H;
                string I;
                string J;
                string K;
                string L;
                string M;
                string N;
                string O;
                string P;
                string Q;
                string R;
                string S;
                string T;
                string U;
                string V;
                string W;
                string X;
                string Y;
                string Z;
                string AA;
                string AB;
                string AC;
                string AD;

                if (eventos!=null)
                {
                    foreach (Evento evento in eventos)
                    {
                        A = "A" + contador.ToString();
                        B = "B" + contador.ToString();
                        C = "C" + contador.ToString();
                        D = "D" + contador.ToString();
                        E = "E" + contador.ToString();
                        F = "F" + contador.ToString();
                        G = "G" + contador.ToString();
                        H = "H" + contador.ToString();
                        I = "I" + contador.ToString();
                        J = "J" + contador.ToString();
                        K = "K" + contador.ToString();
                        L = "L" + contador.ToString();
                        M = "M" + contador.ToString();
                        N = "N" + contador.ToString();
                        O = "O" + contador.ToString();
                        P = "P" + contador.ToString();
                        Q = "Q" + contador.ToString();
                        R = "R" + contador.ToString();
                        S = "S" + contador.ToString();
                        T = "T" + contador.ToString();
                        U = "U" + contador.ToString();
                        V = "V" + contador.ToString();
                        W = "W" + contador.ToString();
                        X = "X" + contador.ToString();
                        Y = "Y" + contador.ToString();
                        Z = "Z" + contador.ToString();
                        AA = "AA" + contador.ToString();
                        AB = "AB" + contador.ToString();
                        AC = "AC" + contador.ToString();
                        AD = "AD" + contador.ToString();

                        string TipoEvento = null;
                        string Creador = null;
                        string Cliente = null;
                        string Equipo = null;
                        string Modelo = null;
                        string Componente = null;
                        string Parte = null;
                        string Area = null;
                        string SubArea = null;
                        string FechaRegistro = null;
                        string FechaIdentificacion = null;
                        string IDRCA = null;
                        string CreadorRCA = null;
                        string aceptasn = null;
                        string OrigenFalla = null;
                        string FallaPrimaria = null;
                        string FallaSecundaria = null;
                        string Costos = null;
                        string Causa = null;
                        AnalisisCausaRaiz acr=null;
                        worksheet.Cells[A].Value = evento.Id;
                        worksheet.Cells[B].Value = evento.Woanterior;
                        if (db.TipoEvento.FirstOrDefault(c => c.Id == evento.TipoEvento) != null)
                        {
                            TipoEvento = db.TipoEvento.FirstOrDefault(c => c.Id == evento.TipoEvento).Tipo;
                        }
                        if (db.Persona.FirstOrDefault(c => c.Rut == evento.Creador) != null)
                        {
                            Creador = db.TipoEvento.FirstOrDefault(c => c.Id == evento.TipoEvento).Tipo;
                        }
                        else
                        {
                            if (evento.Creador != null)
                            {
                                Creador = evento.Creador;
                            }
                        }
                        if (db.Cliente.FirstOrDefault(c => c.Id == evento.Cliente) != null)
                        {
                            Cliente = db.Cliente.FirstOrDefault(c => c.Id == evento.Cliente).Nombre;
                        }
                        if (db.Equipo.FirstOrDefault(c => c.Id == evento.Equipo) != null)
                        {
                            Equipo = db.Equipo.FirstOrDefault(c => c.Id == evento.Equipo).Nombre;
                        }
                        if (db.Modelo.FirstOrDefault(c => c.Id == evento.Modelo) != null)
                        {
                            Modelo = db.Modelo.FirstOrDefault(c => c.Id == evento.Modelo).Nombre;
                        }
                        if (db.Componente.FirstOrDefault(c => c.Id == evento.Componente) != null)
                        {
                            Componente = db.Componente.FirstOrDefault(c => c.Id == evento.Componente).Nombre;
                        }
                        if (db.Parte.FirstOrDefault(c => c.Id == evento.Parte) != null)
                        {
                            Parte = db.Parte.FirstOrDefault(c => c.Id == evento.Parte).Nombre;
                        }
                        if (db.Area.FirstOrDefault(c => c.Id == evento.Area) != null)
                        {
                            Area = db.Area.FirstOrDefault(c => c.Id == evento.Area).Nombre;
                        }
                        if (db.SubArea.FirstOrDefault(c => c.Id == evento.SubArea) != null)
                        {
                            SubArea = db.SubArea.FirstOrDefault(c => c.Id == evento.SubArea).Nombre;
                        }
                        if (evento.FechaRegistro != null)
                        {
                            FechaRegistro = evento.FechaRegistro.ToString();
                        }
                        if (evento.FechaIdentificacion != null)
                        {
                            FechaIdentificacion = evento.FechaIdentificacion.ToString();
                        }
                        if (db.AnalisisCausaRaiz.FirstOrDefault(c=>c.IdEvento.TrimEnd().Equals(evento.Id.TrimEnd())) != null)
                        {
                            IDRCA = db.AnalisisCausaRaiz.FirstOrDefault(c => c.IdEvento.TrimEnd().Equals(evento.Id.TrimEnd())).Id;
                            acr = db.AnalisisCausaRaiz.FirstOrDefault(c => c.IdEvento.TrimEnd().Equals(evento.Id.TrimEnd()));
                        }
                        if (acr!=null)
                        {
                            CreadorRCA = acr.Creador;
                            if (db.OrigenFalla.FirstOrDefault(c=>c.Id== acr.IdOrigenFalla)!=null)
                            {
                                OrigenFalla = db.OrigenFalla.FirstOrDefault(c => c.Id == acr.IdOrigenFalla).Nombre;
                            }
                            if (db.FallaPrimaria.FirstOrDefault(c => c.Id == acr.IdFallaPrimaria) != null)
                            {
                                FallaPrimaria = db.FallaPrimaria.FirstOrDefault(c => c.Id == acr.IdFallaPrimaria).Nombre;
                            }
                            if (db.FallaSecundaria.FirstOrDefault(c => c.Id == acr.IdFallaSecundaria) != null)
                            {
                                FallaSecundaria = db.FallaSecundaria.FirstOrDefault(c => c.Id == acr.IdFallaSecundaria).Nombre;
                            }
                            int Costos2 = Convert.ToInt32(acr.Costo);
                            Costos = Costos2.ToString();
                            Causa = acr.Descripcion;
                        }

                        worksheet.Cells[C].Value = TipoEvento;
                        worksheet.Cells[D].Value = FechaRegistro;
                        worksheet.Cells[E].Value = FechaIdentificacion;
                        worksheet.Cells[F].Value = Creador;
                        worksheet.Cells[G].Value = Cliente;
                        worksheet.Cells[H].Value = Equipo;
                        worksheet.Cells[I].Value = Modelo;
                        worksheet.Cells[J].Value = evento.SerieEquipo;
                        worksheet.Cells[K].Value = Componente;
                        worksheet.Cells[L].Value = evento.PosicionComponente;
                        worksheet.Cells[M].Value = Parte;
                        worksheet.Cells[N].Value = evento.NumeroParteComponente;
                        worksheet.Cells[O].Value = evento.SerieComponente;
                        worksheet.Cells[P].Value = Area;
                        worksheet.Cells[Q].Value = SubArea;
                        worksheet.Cells[R].Value = evento.Ncaceptada;
                        worksheet.Cells[S].Value = evento.Descripcion;
                        worksheet.Cells[T].Value = evento.HorasComponente;
                        worksheet.Cells[U].Value = IDRCA;
                        worksheet.Cells[V].Value = CreadorRCA;
                        worksheet.Cells[W].Value = OrigenFalla;
                        worksheet.Cells[X].Value = FallaPrimaria;
                        worksheet.Cells[Y].Value = FallaSecundaria;
                        worksheet.Cells[Z].Value = Costos;
                        worksheet.Cells[AA].Value = Causa;

                        contador++;
                    }
                }
          
                package.Save(); //Save the workbook.
            }
            //-------------------------Descarga--------------------------
            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot", "Datos.xlsx");

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }
        [HttpGet]
        [Route("Export")]
        public string Export()
        {
            string sWebRootFolder = _enviroment.WebRootPath;
            string sFileName = @"demo.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            using (ExcelPackage package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Employee");
                //First add the headers
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "WorkOrder";
                worksheet.Cells[1, 3].Value = "TipoEvento";
                worksheet.Cells[1, 4].Value = "Fecha Registro";
                worksheet.Cells[1, 5].Value = "Fecha Identificación";
                worksheet.Cells[1, 6].Value = "Creador";
                worksheet.Cells[1, 7].Value = "Cliente";
                worksheet.Cells[1, 8].Value = "Equipo";
                worksheet.Cells[1, 9].Value = "Modelo";
                worksheet.Cells[1, 10].Value = "Serie Equipo";
                worksheet.Cells[1, 11].Value = "Componente";
                worksheet.Cells[1, 12].Value = "Posicion Componente";
                worksheet.Cells[1, 13].Value = "Parte";
                worksheet.Cells[1, 14].Value = "Numero Parte Componente";
                worksheet.Cells[1, 15].Value = "Serie Componente";
                worksheet.Cells[1, 16].Value = "Area";
                worksheet.Cells[1, 17].Value = "Sub Area";
                worksheet.Cells[1, 18].Value = "¿No Conformidad Aceptada?";
                worksheet.Cells[1, 19].Value = "Descripción";
                worksheet.Cells[1, 20].Value = "HorasComponente";



                //Add values
                int contador = 2;
                string A;
                string B;
                string C;
                string D;
                string E;
                string F;
                string G;
                string H;
                string I;
                string J;
                string K;
                string L;
                string M;
                string N;
                string O;
                string P;
                string Q;
                string R;
                string S;
                string T;
                A = "A" + "2";
                IEnumerable<Evento> le = db.Evento.ToList().Where(c => c.Removed != true);
                foreach (Evento evento in le)
                {
                    A = "A" + contador.ToString();
                    B = "B" + contador.ToString();
                    C = "C" + contador.ToString();
                    D = "D" + contador.ToString();
                    E = "E" + contador.ToString();
                    F = "F" + contador.ToString();
                    G = "G" + contador.ToString();
                    H = "H" + contador.ToString();
                    I = "I" + contador.ToString();
                    J = "J" + contador.ToString();
                    K = "K" + contador.ToString();
                    L = "L" + contador.ToString();
                    M = "M" + contador.ToString();
                    N = "N" + contador.ToString();
                    O = "O" + contador.ToString();
                    P = "P" + contador.ToString();
                    Q = "Q" + contador.ToString();
                    R = "R" + contador.ToString();
                    S = "S" + contador.ToString();
                    T = "T" + contador.ToString();

                    string TipoEvento = null;
                    string Creador = null;
                    string Cliente = null;
                    string Equipo = null;
                    string Modelo = null;
                    string Componente = null;
                    string Parte = null;
                    string Area = null;
                    string SubArea = null;
                    string FechaRegistro = null;
                    string FechaIdentificacion = null;
                    worksheet.Cells[A].Value = evento.Id;
                    worksheet.Cells[B].Value = evento.Woanterior;
                    if (db.TipoEvento.FirstOrDefault(c=>c.Id==evento.TipoEvento)!=null)
                    {
                        TipoEvento = db.TipoEvento.FirstOrDefault(c => c.Id == evento.TipoEvento).Tipo;
                    }
                    if (db.Persona.FirstOrDefault(c => c.Rut == evento.Creador) != null)
                    {
                        Creador = db.TipoEvento.FirstOrDefault(c => c.Id == evento.TipoEvento).Tipo;
                    }
                    else
                    {
                        if (evento.Creador!=null)
                        {
                            Creador = evento.Creador;
                        }
                    }
                    if (db.Cliente.FirstOrDefault(c => c.Id == evento.Cliente) != null)
                    {
                        Cliente = db.Cliente.FirstOrDefault(c => c.Id == evento.Cliente).Nombre;
                    }
                    if (db.Equipo.FirstOrDefault(c => c.Id == evento.Equipo) != null)
                    {
                        Equipo = db.Equipo.FirstOrDefault(c => c.Id == evento.Equipo).Nombre;
                    }
                    if (db.Modelo.FirstOrDefault(c => c.Id == evento.Modelo) != null)
                    {
                        Modelo = db.Modelo.FirstOrDefault(c => c.Id == evento.Modelo).Nombre;
                    }
                    if (db.Componente.FirstOrDefault(c => c.Id == evento.Componente) != null)
                    {
                        Componente = db.Componente.FirstOrDefault(c => c.Id == evento.Componente).Nombre;
                    }
                    if (db.Parte.FirstOrDefault(c => c.Id == evento.Parte) != null)
                    {
                        Parte = db.Parte.FirstOrDefault(c => c.Id == evento.Parte).Nombre;
                    }
                    if (db.Area.FirstOrDefault(c => c.Id == evento.Area) != null)
                    {
                        Area = db.Area.FirstOrDefault(c => c.Id == evento.Area).Nombre;
                    }
                    if (db.SubArea.FirstOrDefault(c => c.Id == evento.SubArea) != null)
                    {
                        SubArea = db.SubArea.FirstOrDefault(c => c.Id == evento.SubArea).Nombre;
                    }
                    if (evento.FechaRegistro != null)
                    {
                        FechaRegistro = evento.FechaRegistro.ToString();
                    }
                    if (evento.FechaIdentificacion != null)
                    {
                        FechaIdentificacion = evento.FechaIdentificacion.ToString();
                    }
                    worksheet.Cells[C].Value = TipoEvento;
                    worksheet.Cells[D].Value = FechaRegistro;
                    worksheet.Cells[E].Value = FechaIdentificacion;
                    worksheet.Cells[F].Value = Creador;
                    worksheet.Cells[G].Value = Cliente;
                    worksheet.Cells[H].Value = Equipo;
                    worksheet.Cells[I].Value = Modelo;
                    worksheet.Cells[J].Value = evento.SerieEquipo;
                    worksheet.Cells[K].Value = Componente;
                    worksheet.Cells[L].Value = evento.PosicionComponente;
                    worksheet.Cells[M].Value = Parte;
                    worksheet.Cells[N].Value = evento.NumeroParteComponente;
                    worksheet.Cells[O].Value = evento.SerieComponente;
                    worksheet.Cells[P].Value = Area;
                    worksheet.Cells[Q].Value = SubArea;
                    worksheet.Cells[R].Value = evento.Ncaceptada;
                    worksheet.Cells[S].Value = evento.Descripcion;
                    worksheet.Cells[T].Value = evento.HorasComponente;

                    contador++;
                }
                package.Save(); //Save the workbook.
            }
            return URL;
        }
        [Authorize(Roles = "Users")]
        public IActionResult OtherAction() => View("Index",
        GetData(nameof(OtherAction)));

        private Dictionary<string, object> GetData(string actionName) =>
        new Dictionary<string, object>
        {
            ["Action"] = actionName,
            ["User"] = HttpContext.User.Identity.Name,
            ["Authenticated"] = HttpContext.User.Identity.IsAuthenticated,
            ["Auth Type"] = HttpContext.User.Identity.AuthenticationType,
            ["In Users Role"] = HttpContext.User.IsInRole("Users")
        };
        //--------------------------

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
        public IActionResult CrearRespuestaCliente(string eventoID)
        {
            RespuestaClienteModel rc = new RespuestaClienteModel(){
            EventoId = eventoID,
            };
            return View(rc);
        }
        [HttpPost]
        public IActionResult CrearRespuestaCliente(RespuestaClienteModel model, List<IFormFile> files)
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
                            ModelState.AddModelError("Error", "Error en el formato del archivo" + " " + "." + filename2.Last());
                            return CrearRespuestaCliente(model.EventoId);
                        }
                    }
                }      

            }
            if (ModelState.IsValid)
            {
                RespuestaCliente rc = new RespuestaCliente()
                {
                    IdEvento = model.EventoId,
                    Descripcion = model.Descripcion,
                    FechaRegistro = model.FechaRegistro,
                    Removed = false,
                };
                db.RespuestaCliente.Add(rc);
                db.SaveChanges();

               

                long size = files.Sum(f => f.Length);
                // full path to file in temp location
                var filePath = Path.GetTempFileName();

                string direccionEventoId = model.EventoId.TrimEnd()+"/RespuestaCliente";
                foreach (var formFile in files)
                {

                    if (formFile.Length > 0)
                    {
                        string uploadPath = Path.Combine(_enviroment.WebRootPath, "uploads");
                        Directory.CreateDirectory(Path.Combine(uploadPath, direccionEventoId));
                        string filename = formFile.FileName;
                        using (FileStream fs = new FileStream(Path.Combine(uploadPath, direccionEventoId, filename), FileMode.Create))
                        {
                            formFile.CopyToAsync(fs);
                        }

                        Archivo ar = new Archivo()
                        {
                            Identificador = model.EventoId.TrimEnd(),
                            Nombre = filename,
                            Tipo = "RespuestaCliente",
                            Removed = false,
                            FechaRegistro = DateTime.Now,
                        };
                        db.Archivo.Add(ar);
                        db.SaveChanges();

                    }
                }


                return RedirectToAction("Index", "EventoSecuencia", new { EventoID = model.EventoId });


            }
            else
            {
                return CrearRespuestaCliente(model.EventoId);
            }

           
        }

        public IActionResult EditarRespuestaCliente(string eventoID)
        {
            RespuestaCliente respuesta = db.RespuestaCliente.FirstOrDefault(c=>c.IdEvento.TrimEnd().Equals(eventoID.TrimEnd()));
            string descripcion = null;
            if (respuesta.Descripcion!=null)
            {
                descripcion = respuesta.Descripcion.TrimEnd();
            }
            RespuestaClienteModel rc = new RespuestaClienteModel()
            {
                EventoId = eventoID,
                FechaRegistro = respuesta.FechaRegistro,
                Descripcion = descripcion,

            };
            return View(rc);
        }
        [HttpPost]
        public IActionResult EditarRespuestaCliente(RespuestaClienteModel model, List<IFormFile> files)
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
                            ModelState.AddModelError("Error", "Error en el formato del archivo" + " " + "." + filename2.Last());
                            return CrearRespuestaCliente(model.EventoId);
                        }

                    }
                }

            }
            if (ModelState.IsValid)
            {

                RespuestaCliente UpdateRespuestaCliente = db.RespuestaCliente.FirstOrDefault(c=>c.IdEvento.TrimEnd().Equals(model.EventoId));
                UpdateRespuestaCliente.Descripcion = model.Descripcion;
                UpdateRespuestaCliente.FechaRegistro = model.FechaRegistro;
                db.RespuestaCliente.Update(UpdateRespuestaCliente);
                db.SaveChanges();


                long size = files.Sum(f => f.Length);

                // full path to file in temp location
                var filePath = Path.GetTempFileName();

                string direccionEventoId = model.EventoId.TrimEnd() + "/RespuestaCliente";
                foreach (var formFile in files)
                {

                    if (formFile.Length > 0)
                    {
                        string uploadPath = Path.Combine(_enviroment.WebRootPath, "uploads");
                        Directory.CreateDirectory(Path.Combine(uploadPath, direccionEventoId));
                        string filename = formFile.FileName;
                        using (FileStream fs = new FileStream(Path.Combine(uploadPath, direccionEventoId, filename), FileMode.Create))
                        {
                            formFile.CopyToAsync(fs);
                        }

                        Archivo ar = new Archivo()
                        {
                            Identificador = model.EventoId.TrimEnd(),
                            Nombre = filename,
                            Tipo = "RespuestaCliente",
                            Removed = false,
                            FechaRegistro = DateTime.Now,
                        };
                        db.Archivo.Add(ar);
                        db.SaveChanges();

                    }
                }


                return RedirectToAction("Index", "EventoSecuencia", new { EventoID = model.EventoId });


            }
            else
            {
                return CrearRespuestaCliente(model.EventoId);
            }

        }

            public IActionResult CrearEventoPrimeraEtapa()
        {

            AddEvento addEvento = new AddEvento()
            {

                DevuelveComponente = null,
                InformeFalla = null,
                AntesDespuesDespacho = null,
                EventoUnico = new Evento(),

            };

            return View(addEvento);
        }

        [HttpGet]
        public async Task<IActionResult> CrearEventoSegundaEtapa(AddEvento e)
        {
            if (e.AntesDespuesDespacho == true)
            {
                e.Tipo = db.TipoEvento.FirstOrDefault(c => c.Tipo.TrimEnd().Equals("Desviacion"));
                e.IdTipo = db.TipoEvento.FirstOrDefault(c => c.Tipo.TrimEnd().Equals("Desviacion")).Id;
            }
            else
            {

                if (e.DevuelveComponente == true)
                {
                    e.Tipo = db.TipoEvento.FirstOrDefault(c => c.Tipo.TrimEnd().Equals("Reclamo Cliente"));
                    e.IdTipo = db.TipoEvento.FirstOrDefault(c => c.Tipo.TrimEnd().Equals("Reclamo Cliente")).Id;
                }
                else
                {
                    if (e.DevuelveComponente == false && e.InformeFalla == true)
                    {
                        e.Tipo = db.TipoEvento.FirstOrDefault(c => c.Tipo.TrimEnd().Equals("Queja Cliente"));
                        e.IdTipo = db.TipoEvento.FirstOrDefault(c => c.Tipo.TrimEnd().Equals("Queja Cliente")).Id;
                    }
                }

            }

            AddEvento addevento = new AddEvento()
            {
                Tipo = e.Tipo,
                IdTipo = e.IdTipo,
                ListaAreas = db.Area.ToList().Where(c=>c.Removed!=true),
                ListaSubAreas = db.SubArea.ToList().Where(c => c.Removed != true),
                EventoUnico = new Evento(),
                TipoEvento = db.TipoEvento.ToList(),
                Cliente = db.Cliente.ToList().Where(c =>c.Removed!=true),
                equipo = db.Equipo.ToList().Where(c => c.Removed != true)

            };

            return View(addevento);

        }

        [HttpPost]
        public async Task<IActionResult> CrearEventoSegundaEtapa(AddEvento addevento, List<IFormFile> files)
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
                            return await CrearEventoSegundaEtapa(addevento);
                        }

                    }
                }
                if (contador == 0 && addevento.IdTipo == 2)
                {
                    ModelState.AddModelError("Error3", "Debe Ingresar el Informe de Falla para continuar");
                    return await CrearEventoSegundaEtapa(addevento);
                }

            }

            if (ModelState.IsValid)
            {
                string[] DescripcionTamanio = new string[] { addevento.EventoUnico.Descripcion };
                if (addevento.EventoUnico.Descripcion != null)
                {
                    if (addevento.EventoUnico.Descripcion.Length > 2000)
                    {
                        ModelState.AddModelError("Error4", "Descripcion excede los 2000 caracteres");
                        return await CrearEventoSegundaEtapa(addevento);
                    }
                }


                string inicio = "CSAN";
                string FechaRegistro = DateTime.Now.ToString("yyyy");
                string Cadena = inicio + FechaRegistro;
                string ultimo = "";
                int suma;
                if (db.Evento.Count() == 0)
                {
                    ultimo = "00001";
                }
                else
                {
                    ultimo = db.Evento.OrderByDescending(t => t.FechaRegistro).FirstOrDefault().Id;
                    ultimo = ultimo.Substring(8);
                    suma = Convert.ToInt32(ultimo);
                    suma = suma + 1;
                    ultimo = suma.ToString();
                    if (ultimo.Length == 1)
                    {
                        ultimo = "0000" + ultimo;
                    }
                    else
                    {
                        if (ultimo.Length == 2)
                        {
                            ultimo = "000" + ultimo;
                        }
                        else
                        {
                            if (ultimo.Length == 3)
                            {
                                ultimo = "00" + ultimo;
                            }
                            else
                            {
                                if (ultimo.Length == 4)
                                {
                                    ultimo = "0" + ultimo;
                                }

                            }
                        }
                    }
                }

                Cadena = Cadena + ultimo;

                ClaimsPrincipal currentUser = User;

                if (db.AspNetUsers.FirstOrDefault(c => c.Id.Equals(currentUser.getUserId())).RutPersona == null)
                {
                    Evento evento = new Evento()
                    {
                        Id = Cadena,
                        FechaRegistro = DateTime.Now,
                        TipoEvento = addevento.IdTipo,
                        Woanterior = addevento.WorkOrderAnterior,
                        WorkOrder = addevento.WorkOrderNueva,
                        Creador = "Administrador",
                        Cliente = addevento.EventoUnico.Cliente,
                        Equipo = addevento.EventoUnico.Equipo,
                        SerieEquipo = addevento.SerieEquipo,
                        Modelo = addevento.EventoUnico.Modelo,
                        Componente = addevento.EventoUnico.Componente,
                        Parte = addevento.EventoUnico.Parte,
                        NumeroParteComponente = addevento.NumeroParteComponente,
                        SerieComponente = addevento.SerieComponente,
                        HorasComponente = addevento.HorasComponente,
                        Area = addevento.IdArea,
                        SubArea = addevento.IdSubArea,
                        Descripcion = addevento.EventoUnico.Descripcion,
                        Removed = false,
                        Estado = 2,
                        Probabilidad = addevento.Probabilidad,
                        Consecuencia = addevento.Consecuencia,

                    };
                    db.Evento.Add(evento);
                    db.SaveChanges();

                }
                else
                {

                    Evento evento = new Evento()
                    {
                        Id = Cadena,
                        FechaRegistro = DateTime.Now,
                        TipoEvento = addevento.IdTipo,
                        Woanterior = addevento.WorkOrderAnterior,
                        WorkOrder = addevento.WorkOrderNueva,
                        Creador = db.AspNetUsers.FirstOrDefault(c => c.Id.Equals(currentUser.getUserId())).RutPersona,
                        Cliente = addevento.EventoUnico.Cliente,
                        Equipo = addevento.EventoUnico.Equipo,
                        SerieEquipo = addevento.SerieEquipo,
                        Modelo = addevento.EventoUnico.Modelo,
                        Componente = addevento.EventoUnico.Componente,
                        Parte = addevento.EventoUnico.Parte,
                        NumeroParteComponente = addevento.NumeroParteComponente,
                        SerieComponente = addevento.SerieComponente,
                        HorasComponente = addevento.HorasComponente,
                        Area = addevento.IdArea,
                        SubArea = addevento.IdSubArea,
                        Descripcion = addevento.EventoUnico.Descripcion,
                        Removed = false,

                        Estado = 2,
                        Probabilidad = addevento.Probabilidad,
                        Consecuencia = addevento.Consecuencia,
                    };
                    db.Evento.Add(evento);
                    db.SaveChanges();

                    if (!currentUser.IsInRole("Cliente"))
                    {
                        if (db.TipoEvento.FirstOrDefault(c => c.Id == addevento.IdTipo).Tipo.TrimEnd().Equals("Desviacion"))
                        {
                            string rutresponsable = db.AspNetUsers.FirstOrDefault(c => c.Id == currentUser.getUserId()).RutPersona.TrimEnd();
                            PersonaEvento pe = new PersonaEvento()
                            {

                                RutPersona = rutresponsable,
                                IdEvento = Cadena,
                                Removed = false,
                                FechaRegistro = DateTime.Now,
                                Estado = 2,


                            };
                            db.PersonaEvento.Add(pe);
                            db.SaveChanges();
                        }
                    }
                }
                long size = files.Sum(f => f.Length);

                // full path to file in temp location
                var filePath = Path.GetTempFileName();

                string direccionEventoId = Cadena;
                foreach (var formFile in files)
                {

                    if (formFile.Length > 0)
                    {
                        string uploadPath = Path.Combine(_enviroment.WebRootPath, "uploads");
                        Directory.CreateDirectory(Path.Combine(uploadPath, direccionEventoId));
                        string filename = formFile.FileName;
                        using (FileStream fs = new FileStream(Path.Combine(uploadPath, direccionEventoId, filename), FileMode.Create))
                        {
                            await formFile.CopyToAsync(fs);
                        }

                        Archivo ar = new Archivo()
                        {
                            Identificador = Cadena.TrimEnd(),
                            Nombre = filename,
                            Tipo = "Evento",
                            Removed = false,
                            FechaRegistro = DateTime.Now,
                        };
                        db.Archivo.Add(ar);
                        db.SaveChanges();

                    }
                }

                // return RedirectToAction("Index","EventoSecuencia");
                return RedirectToAction("Index", "EventoSecuencia", new { EventoID = Cadena });
            }
            else
            {
                return await CrearEventoSegundaEtapa(addevento);
            }


        }

        public IActionResult RegistroAuditoria()
        {

            AddEvento addEvento = new AddEvento()
            {

                DevuelveComponente = null,
                InformeFalla = null,
                AntesDespuesDespacho = null,
                EventoUnico = new Evento(),

            };

            return View(addEvento);
        }


        public JsonResult getAreaId(int id)
        {
            List<SubArea> list = new List<SubArea>();
            list = db.SubArea.Where(c => c.IdArea == id && c.Removed!=true).ToList();
            //list.Insert(0, new SubArea { Id = 0, Nombre = "Please Select SubArea" });
            return Json(new SelectList(list, "Id", "Nombre"));
        }

        public JsonResult BuscarCoincidencia(string wo)
        {
            List<Evento> list = new List<Evento>();
            if (db.Evento.FirstOrDefault(c => c.Woanterior.TrimEnd().Equals(wo) && c.Removed != true)!=null)
            {
                IEnumerable<Evento> le = db.Evento.Where(c => c.Woanterior.TrimEnd().Equals(wo) && c.Removed != true).ToList();


             
                int contador = 0;
                foreach (Evento evento in le)
                {


                    list.Insert(contador, evento);


                    contador++;
                }
                return Json(list.Last());
            }



            //list = db.Evento.Where(c => c.Woanterior.TrimEnd().Equals(wo) && c.Removed != true).ToList();
            return Json(list);
        }
        //public JsonResult BuscarCoincidencia(string workorder)
        //{
        //    IEnumerable<Evento> le = db.Evento.ToList().Where(c => c.Woanterior.TrimEnd().Equals(workorder.TrimEnd()));
        //    List<Evento> list = new List<Evento>();
        //    int contador = 0;
        //    foreach (Evento evento in le)
        //    {
        //        if (contador==0)
        //        {
        //            list.Insert(contador, evento);
        //        }

        //        contador++;
        //    }
        //    return Json(le);
        //}
        //public JsonResult getCoincidencia(string workorder)
        //{


        //    List<Evento> listEventos = new List<Evento>();
        //    int contador = 0;
        //    foreach (Evento evento in listEventos.Where(c => c.Woanterior == workorder))
        //    {

        //            list.Insert(contador, new Persona { Rut = persona.Rut, Nombre = persona.Nombre });
        //            contador++;


        //    }
        //    //list = db.Persona.Where(c => c.IdSubAreaJefeSubArea==idSubArea).ToList();
        //    //list.Insert(0, new Modelo { Id = 0, Nombre = "Please Select Modelo" });
        //    return Json(new SelectList(list));
        //}

        public async Task<IActionResult> DownloadInformeFalla()
        {
            var filename = "Informe de Falla";
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot", "uploads","InformeSugerido", "Informe de Fallas.docx");

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
            db.SaveChanges();

            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = es.EventoUnico.Id });
        }


        public async Task<IActionResult> FormularioEditEvento(string eventoid)
        {
            AddEvento AddEvento = new AddEvento()
            {

                componenteJOINmodelo = (from c in db.Componente
                                        join mc in db.ModeloComponente on c.Id equals mc.IdComponente
                                        where mc.IdModelo == db.Evento.FirstOrDefault(c => c.Id == eventoid).Modelo
                                        select c).ToList(),

                componenteJOINparte = (from c in db.Parte
                                       join cp in db.ComponenteParte on c.Id equals cp.IdParte
                                       where cp.IdComponente == db.Evento.FirstOrDefault(c => c.Id == eventoid).Componente && !c.Removed == true && !cp.Removed == true
                                       select c).ToList(),


                ListaComponente = db.Componente.ToList().Where(c => !c.Removed == true),
                modelo = db.Modelo.ToList().Where(c=>c.Removed!=true),
                equipo = db.Equipo.ToList().Where(c=>c.Removed!=true),
                ListaParte = db.Parte.ToList().Where(c=>c.Removed!=true),
                EventoUnico = db.Evento.FirstOrDefault(c => c.Id == eventoid && !c.Removed==true),
                TipoEvento = db.TipoEvento.ToList(),
                Cliente = db.Cliente.ToList().Where(c => !c.Removed == true),
                ListaAreas = db.Area.ToList(),
                ListaSubAreas = db.SubArea.ToList(),
        };
            return View(AddEvento);
        }
        [HttpPost]
        public async Task<IActionResult> FormularioEditEvento(AddEvento clase, List<IFormFile> files)
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
                            return await FormularioEditEvento(clase.EventoUnico.Id);
                        }

                    }
                }
            



            }

            if (ModelState.IsValid)
            {

                Evento updateEvento = db.Evento.FirstOrDefault(c => c.Id == clase.EventoUnico.Id);
                updateEvento.Woanterior = clase.WorkOrderAnterior;
                updateEvento.WorkOrder = clase.WorkOrderNueva;
                updateEvento.SerieEquipo = clase.SerieEquipo;
                updateEvento.TipoEvento = clase.EventoUnico.TipoEvento;
                updateEvento.Cliente = clase.ClienteId;
                updateEvento.Equipo = clase.EquipoId;
                updateEvento.Modelo = clase.ModeloId;
                updateEvento.Componente = clase.ComponenteId;
                updateEvento.NumeroParteComponente = clase.NumeroParteComponente;
                updateEvento.SerieComponente = clase.SerieComponente;
                updateEvento.HorasComponente = clase.HorasComponente;
                updateEvento.Descripcion = clase.Descripcion;
                updateEvento.Parte = clase.ParteId;

                updateEvento.Probabilidad = clase.Probabilidad;
                updateEvento.Consecuencia = clase.Consecuencia;
                updateEvento.Area = clase.IdArea;
                updateEvento.SubArea = clase.IdSubArea;


                updateEvento.FechaIdentificacion = clase.FechaIdentificacion;


                db.Evento.Update(updateEvento);
                db.SaveChanges();



                long size = files.Sum(f => f.Length);

                // full path to file in temp location
                var filePath = Path.GetTempFileName();

                string direccionEventoId = clase.EventoUnico.Id.TrimEnd();



                foreach (var formFile in files)
                {

                    if (formFile.Length > 0)
                    {
                        string uploadPath = Path.Combine(_enviroment.WebRootPath, "uploads");
                        Directory.CreateDirectory(Path.Combine(uploadPath, direccionEventoId));
                        string filename = formFile.FileName;
                        using (FileStream fs = new FileStream(Path.Combine(uploadPath, direccionEventoId, filename), FileMode.Create))
                        {
                            await formFile.CopyToAsync(fs);
                        }

                        Archivo ar = new Archivo()
                        {
                            Identificador = clase.EventoUnico.Id.TrimEnd(),
                            Nombre = filename,
                            Tipo = "Evento",
                            Removed = false,
                            FechaRegistro = DateTime.Now,
                        };
                        db.Archivo.Add(ar);
                        db.SaveChanges();



                    }
                }

                return RedirectToAction("Index", "EventoSecuencia", new { EventoID = clase.EventoUnico.Id.TrimEnd() });
            }
            else
            {

                return await FormularioEditEvento(clase.EventoUnico.Id.TrimEnd());
            }

        }

        public IActionResult DeleteEvento(string eventoid)
        {
            Evento DeleteEvento = db.Evento.FirstOrDefault(c => c.Id == eventoid);
            DeleteEvento.Removed = true;
            db.Evento.Update(DeleteEvento);
            db.SaveChanges();


            return RedirectToAction("Index");

        }
        public IActionResult DeleteRespuestaCliente(string eventoid)
        {
            RespuestaCliente DeleteRespuesta = db.RespuestaCliente.FirstOrDefault(c => c.IdEvento == eventoid);
            DeleteRespuesta.Removed = true;
            db.RespuestaCliente.Update(DeleteRespuesta);
            db.SaveChanges();


            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = eventoid });

        }
        public IActionResult AprobarRegistroEvento(string eventoid)
        {
            Evento UpdateEvento = db.Evento.FirstOrDefault(c => c.Id == eventoid);
            UpdateEvento.Estado = 2;
            db.Evento.Update(UpdateEvento);
            db.SaveChanges();


            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = eventoid });

        }

        public IActionResult AprobarAccionInmediata(string eventoid)
        {
            Evento UpdateEvento = db.Evento.FirstOrDefault(c => c.Id == eventoid);
            UpdateEvento.Estado = 3;
            db.Evento.Update(UpdateEvento);
            db.SaveChanges();


            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = eventoid });

        }

        public IActionResult AprobarAnalisis(string eventoid)
        {
            Evento UpdateEvento = db.Evento.FirstOrDefault(c => c.Id == eventoid);
            UpdateEvento.Estado = 4;
            db.Evento.Update(UpdateEvento);
            db.SaveChanges();


            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = eventoid });

        }

        public IActionResult AprobarAnalisisCausaRaiz(string eventoid)
        {
            Evento UpdateEvento = db.Evento.FirstOrDefault(c => c.Id == eventoid);
            if (UpdateEvento.Ncaceptada==true)
            {
                UpdateEvento.Estado = 5;
                db.Evento.Update(UpdateEvento);
                db.SaveChanges();
            }
            else
            {
                UpdateEvento.Estado = 9;
                db.Evento.Update(UpdateEvento);
                db.SaveChanges();
            }




            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = eventoid });

        }

        public IActionResult AprobarRegistroInvolucrados(string eventoid)
        {
            Evento UpdateEvento = db.Evento.FirstOrDefault(c => c.Id == eventoid);
            UpdateEvento.Estado = 6;
            db.Evento.Update(UpdateEvento);
            db.SaveChanges();


            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = eventoid });

        }

        public IActionResult AprobarAccionCorrectiva(string eventoid)
        {
            Evento UpdateEvento = db.Evento.FirstOrDefault(c => c.Id == eventoid);
            UpdateEvento.Estado = 7;
            db.Evento.Update(UpdateEvento);
            db.SaveChanges();


            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = eventoid });

        }

        public IActionResult EliminarEventoModal(ListaEvento le)
        {
            Evento DeleteEvento = db.Evento.FirstOrDefault(c => c.Id == le.Evento.Id);
            DeleteEvento.Removed = true;
            db.Evento.Update(DeleteEvento);
            db.SaveChanges();


            return RedirectToAction("IndiceEventos");

        }
        public IActionResult EliminarEventoModalHome(ListaEvento le)
        {
            Evento DeleteEvento = db.Evento.FirstOrDefault(c => c.Id == le.Evento.Id);
            DeleteEvento.Removed = true;
            db.Evento.Update(DeleteEvento);
            db.SaveChanges();


            return RedirectToAction("Index");

        }


        public async Task<List<Evento>> DeleteEventoModalAjax(string id)
        {
            List<Evento> le = new List<Evento>();
            var evento = db.Evento.FirstOrDefault(c => c.Id == id);
            le.Add(evento);
            return le;

        }

        public async Task<List<Evento>> DeleteEventoModalAjax2(string id)
        {

            List<Evento> le = new List<Evento>();
            var evento = db.Evento.FirstOrDefault(c => c.Id == id);
            le.Add(evento);

            return le;

        }

        public IActionResult PruebaDatosEventos()
        {



            ClaimsPrincipal currentUser = User;



            Persona persona = new Persona()
            {
                Rut = db.AspNetUsers.FirstOrDefault(c => c.Id.Equals(currentUser.getUserId())).RutPersona,



            };



            var prueba = (from e in db.Evento
                          join pe in db.PersonaEvento on e.Id equals pe.IdEvento
                          where pe.RutPersona == persona.Rut
                          select e.Id).ToList();




            AddEvento classEventoTipoEvento = new AddEvento()
            {

                personaEvento = db.PersonaEvento.ToList(),
                Evento = (from e in db.Evento
                          join pe in db.PersonaEvento on e.Id equals pe.IdEvento
                          where pe.RutPersona == persona.Rut
                          select e).ToList(),

                TipoEvento = db.TipoEvento.ToList(),
                Cliente = db.Cliente.ToList()
            };

            return View(prueba);

        }


        public IActionResult Perfil()
        {

            ClaimsPrincipal currentUser = User;
            var Usuario = db.AspNetUsers.FirstOrDefault(c => c.Id.TrimEnd() == currentUser.FindFirst(ClaimTypes.NameIdentifier).Value.TrimEnd());
            var rolID = db.AspNetUserRoles.FirstOrDefault(c => c.UserId == Usuario.Id).RoleId;
            var RolName = db.AspNetRoles.FirstOrDefault(c=>c.Id==rolID).NormalizedName;
            var persona = db.Persona.FirstOrDefault(c => c.Rut.Equals(Usuario.RutPersona));
            PruebaUser prueba = new PruebaUser()
            {
                Usuario = Usuario,
                RolName = RolName,
                Persona = persona,  
            };


            return View(prueba);
        }

        public async Task<IActionResult> CambiarContraseña()
        {
            string id = User.getUserId();
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
                
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CambiarContraseña(CreateModel model)
        {
            AppUser user = await userManager.FindByIdAsync(model.id);
            if (user != null)
            {
                model.ListaPersonas = db.Persona.ToList();
                model.ListaRoles = db.AspNetRoles.ToList();
                user.Email = model.Email;
                user.UserName = model.Name;
      
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

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

      

        public JsonResult getModeloId(int id)
        {
            List<Modelo> list = new List<Modelo>();
            list = db.Modelo.Where(c => c.IdEquipo == id && c.Removed!=true).ToList();
            //list.Insert(0, new Modelo { Id = 0, Nombre = "Please Select Modelo" });
            return Json(new SelectList(list, "Id", "Nombre"));
        }

        public JsonResult getComponenteId(int id)
        {
            List<Componente> list = new List<Componente>();
            list = (from c in db.Componente
                    join mc in db.ModeloComponente on c.Id equals mc.IdComponente

                    //join mc2 in db.ModeloComponente on id equals mc2.IdModelo
                    where mc.IdModelo == id && mc.Removed!=true && c.Removed!=true
                    select c).ToList();
            //list.Insert(0, new Modelo { Id = 0, Nombre = "Please Select Modelo" });
            return Json(new SelectList(list, "Id", "Nombre"));
        }
        public JsonResult getParteId(int id)
        {
            List<Parte> list = new List<Parte>();
            list = (from p in db.Parte
                    join cp in db.ComponenteParte on p.Id equals cp.IdParte

                    //join mc2 in db.ModeloComponente on id equals mc2.IdModelo
                    where cp.IdComponente == id && !cp.Removed==true && !p.Removed==true
                    select p).ToList();
            //list.Insert(0, new Modelo { Id = 0, Nombre = "Please Select Modelo" });
            return Json(new SelectList(list, "Id", "Nombre"));
        }

        public async Task<IActionResult> Subida()
        {
        
            return View();
        }

      
        [HttpPost]
        public async Task<IActionResult> Subida(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            string direccion = "InformeSugerido";



            foreach (var formFile in files)
            {

                if (formFile.Length > 0)
                {
                    string uploadPath = Path.Combine(_enviroment.WebRootPath, "uploads");
                    Directory.CreateDirectory(Path.Combine(uploadPath, direccion));
                    string filename = formFile.FileName;
                    string[] filename2 = filename.Split('.');


                    if (!filename2[0].ToString().Equals("Informe de Fallas"))
                    {
                        ModelState.AddModelError("Error", "El nombre del archivo debe ser: Informe de Fallas ");
                        return await Subida();
                    }
                    if (filename2.Last().Equals("doc") || filename2.Last().Equals("docx") || filename2.Last().Equals("txt") || filename2.Last().Equals("csv") || filename2.Last().Equals("pdf") || filename2.Last().Equals("xls") || filename2.Last().Equals("xlsx") || filename2.Last().Equals("odp") || filename2.Last().Equals("ods") || filename2.Last().Equals("odt") || filename2.Last().Equals("vsd") || filename2.Last().Equals("pptx") || filename2.Last().Equals("ppt") || filename2.Last().Equals("potx") || filename2.Last().Equals("png") || filename2.Last().Equals("jpg") || filename2.Last().Equals("jpeg") || filename2.Last().Equals("ico") || filename2.Last().Equals("webp") || filename2.Last().Equals("avi") || filename2.Last().Equals("mp4") || filename2.Last().Equals("mpeg") || filename2.Last().Equals("mpg") || filename2.Last().Equals("ogv") || filename2.Last().Equals("webm") || filename2.Last().Equals("mp3") || filename2.Last().Equals("wav") || filename2.Last().Equals("oga") || filename2.Last().Equals("aac") || filename2.Last().Equals("mid") || filename2.Last().Equals("midi") || filename2.Last().Equals("weba") || filename2.Last().Equals("rar") || filename2.Last().Equals("zip") || filename2.Last().Equals("bz") || filename2.Last().Equals("bz2"))
                    {
                
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "El formato del archivo es invalido");
                        return await Subida();
                    }


                        using (FileStream fs = new FileStream(Path.Combine(uploadPath, direccion, filename), FileMode.Create))
                    {
                        await formFile.CopyToAsync(fs);
                    }

             



                }
            }
            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return RedirectToAction("CrearEventoPrimeraEtapa");
        }

        public ViewResult IFormFile()
        {
            return View();
        }

 
        [HttpPost]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            string direccionEventoId = "Prueba";



            foreach (var formFile in files)
            {
               
                if (formFile.Length > 0)
                {
                    string uploadPath = Path.Combine(_enviroment.WebRootPath, "uploads");
                    Directory.CreateDirectory(Path.Combine(uploadPath, direccionEventoId));
                    string filename = formFile.FileName;
                    using (FileStream fs = new FileStream(Path.Combine(uploadPath, direccionEventoId, filename), FileMode.Create))
                    {
                        await formFile.CopyToAsync(fs);
                    }
                }
            }

            return Ok(new { count = files.Count, size, filePath });
        }



        public async Task<IActionResult> PostArchivoEvento(string EventoID,List<IFormFile> Files)
        {
            long size = Files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            string direccionEventoId = EventoID;



            foreach (var formFile in Files)
            {

                if (formFile.Length > 0)
                {
                    string uploadPath = Path.Combine(_enviroment.WebRootPath, "uploads");
                    Directory.CreateDirectory(Path.Combine(uploadPath, direccionEventoId));
                    string filename = formFile.FileName;
                    using (FileStream fs = new FileStream(Path.Combine(uploadPath, direccionEventoId, filename), FileMode.Create))
                    {
                        await formFile.CopyToAsync(fs);
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = Files.Count, size, filePath });
        }

        public FileResult Download()
        {
            // Obtener contenido del archivo
            string text = "El texto para mi archivo.";

            return File(Encoding.ASCII.GetBytes(text), "text/plain", "Prueba.txt");
        }
        public ActionResult ModalAction(string Id)
        {
            ViewBag.Prueba = 2;

            AddEvento classEventoTipoEvento = new AddEvento()
            {

                ListaComponente = db.Componente.ToList(),
                equipo = db.Equipo.ToList(),
                modelo = db.Modelo.ToList(),
                personaEvento = db.PersonaEvento.ToList(),

                EventoUnico = db.Evento.FirstOrDefault(c => c.Id.Equals(Id.TrimEnd())),
                TipoEvento = db.TipoEvento.ToList(),
                Cliente = db.Cliente.ToList(),
            };



            return PartialView("ModalContent", classEventoTipoEvento);
        }

        public ActionResult AgregarResponsableAnalisis(EventoSecuenciaModels es)
        {
            ViewBag.Prueba = 2;


            PersonaEvento pe = new PersonaEvento()
            {

                RutPersona = es.Responsable,
                IdEvento = es.EventoUnico.Id.TrimEnd(),
                Removed = false,
                FechaRegistro = DateTime.Now,
                //Etapa = "Analisis",
                Estado = 1,


            };
            db.PersonaEvento.Add(pe);
            db.SaveChanges();




            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = es.EventoUnico.Id });
        }

        public ViewResult PruebaPanel()
        {
            return View();
        }

        public ViewResult ModalAjax(string id, string msj)
        {
            ViewBag.Prueba = 2;
            PersonaEvento pe = new PersonaEvento()
            {

                RutPersona = msj,
                IdEvento = id,
                Removed = false,
                FechaRegistro = DateTime.Now,
 
            };
            db.PersonaEvento.Add(pe);
            db.SaveChanges();


            EventoSecuenciaModels EventoSecuenciaModels = new EventoSecuenciaModels()
            {

            };


            return View(EventoSecuenciaModels);

        }

        [Authorize]
        public async Task<IActionResult> IndiceEventos(
        string sortOrder,
        string currentFilter,
        int? currentFilterCriticidad,
        int? currentFilterDias,
        int? currentFilterHoras,
        string searchString,
        int? searchIntCriticidad,
        int? searchIntDias,
        int? searchIntHoras,
        int? page,
        string TipoEventoSearch,
        int? EstadoSearch,
        string AnioSearch,
        string mesSearch,    
        string AsignacionSearch,
        string AplicaRCASearch,
        string optradio)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["IdSortParm"] = sortOrder == "id_desc" ? "Id" : "id_desc";
            ViewData["TipoEventoSortParm"] = sortOrder == "tipo_evento" ? "tipo_evento_desc" : "tipo_evento";
            ViewData["EstadoSortParm"] = String.IsNullOrEmpty(sortOrder) ? "estado_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewData["ClienteSortParm"] = sortOrder == "Cliente" ? "cliente_desc" : "Cliente";
            ViewData["EquipoSortParm"] = sortOrder == "Equipo" ? "equipo_desc" : "Equipo";
            ViewData["ModeloSortParm"] = sortOrder == "Modelo" ? "modelo_desc" : "Modelo";
            ViewData["ComponenteSortParm"] = sortOrder == "Componente" ? "componente_desc" : "Componente";
            ViewData["HorasComponenteSortParm"] = sortOrder == "HorasComponente" ? "horascomponente_desc" : "HorasComponente";
            ViewData["ParteSortParm"] = sortOrder == "Parte" ? "parte_desc" : "Parte";
            ViewData["DescripcionSortParm"] = sortOrder == "Descripcion" ? "descripcion_desc" : "Descripcion";



            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentFilterCriticidad"] = searchIntCriticidad;
            ViewData["CurrentFilterDias"] = searchIntDias;
            ViewData["currentFilterHoras"] = searchIntHoras;

            ViewData["TipoEventoFilter"] = TipoEventoSearch;
            ViewData["AñoFilter"] = AnioSearch;

            string CapturandoUsuario = null;
            ClaimsPrincipal currentUser = User;
            PruebaUser prueba;

            if (currentUser.IsInRole("Admins"))
            {
                CapturandoUsuario = "Administrador";

                prueba = new PruebaUser()
                {
                    ListaUsuarios = db.AspNetUsers.ToList(),
                    Usuario = new AspNetUsers()
                    {
                        Id = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value,

                    }
                };
            }
            else
            {
                prueba = new PruebaUser()
                {
                    ListaUsuarios = db.AspNetUsers.ToList(),
                    Usuario = new AspNetUsers()
                    {
                        Id = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value,

                    }
                };

                CapturandoUsuario = db.Persona.Select(c => new { nombreCompleto = c.Nombre + c.ApellidoPaterno + c.ApellidoMaterno, rut = c.Rut }).FirstOrDefault(c => c.rut.Equals(db.AspNetUsers.FirstOrDefault(d => d.Id.Equals(prueba.Usuario.Id)).RutPersona)).nombreCompleto;
            }

            var eventos = from s in db.Evento
                          select s;

            if (currentUser.IsInRole("Admins") || currentUser.IsInRole("Calidad") || currentUser.IsInRole("Estandar"))
            {
                eventos = from s in db.Evento
                          where s.Removed == false
                          select s;
            }
            else
            {
                Persona persona = new Persona()
                {
                    Rut = db.AspNetUsers.FirstOrDefault(c => c.Id.Equals(currentUser.getUserId())).RutPersona,

                };

                eventos = from e in db.Evento
                              //join pe in db.PersonaEvento on e.Id equals pe.IdEvento
                          where e.Creador == persona.Rut && !e.Removed == true
                          select e;
            }


            if (!String.IsNullOrEmpty(AnioSearch))
            {
                int Anio = Convert.ToInt32(AnioSearch);

                eventos = eventos.Where(e => e.FechaRegistro.Value.Year.Equals(Anio));

            }


            if (!String.IsNullOrEmpty(mesSearch))
            {
                int mes = Convert.ToInt32(mesSearch);

                eventos = eventos.Where(e => e.FechaRegistro.Value.Month.Equals(mes));

            }


            if (!String.IsNullOrEmpty(TipoEventoSearch))
            {
                eventos = eventos.Where(s => s.TipoEvento == db.TipoEvento.FirstOrDefault(c => c.Tipo == TipoEventoSearch).Id
                                       );
            }

            if (!String.IsNullOrEmpty(AsignacionSearch))
            {
                if (AsignacionSearch == "Creados" && CapturandoUsuario != "Administrador")
                {
                    eventos = eventos.Where(s => s.Creador == db.AspNetUsers.FirstOrDefault(c => c.Id == prueba.Usuario.Id).RutPersona);
                }
                else
                {
                    if (AsignacionSearch == "Creados" && CapturandoUsuario == "Administrador")
                    {
                        eventos = eventos.Where(s => s.Creador == "Administrador");
                    }
                }

                if (AsignacionSearch == "Responsable" && CapturandoUsuario != "Administrador")
                {

                    eventos = (from e in eventos
                               join pe in db.PersonaEvento on e.Id equals pe.IdEvento
                               where pe.RutPersona == db.AspNetUsers.FirstOrDefault(d => d.Id == prueba.Usuario.Id).RutPersona
                               select e).Distinct();

                }

            }

            if (!String.IsNullOrEmpty(AplicaRCASearch))
            {
                if (AplicaRCASearch.Equals("Aceptada"))
                {
                    eventos = eventos.Where(s => s.Ncaceptada == true);
                }
                if (AplicaRCASearch.Equals("Rechazada"))
                {
                    eventos = eventos.Where(s => s.Ncaceptada == false);
                }
            }




            if (!String.IsNullOrEmpty(searchString))
            {
                eventos = eventos.Where(s => s.Id.Contains(searchString)
                                       || db.TipoEvento.FirstOrDefault(c => c.Id == s.TipoEvento).Tipo.Contains(searchString.TrimEnd())
                                       || db.Estado.FirstOrDefault(c => c.Id == s.Estado).Nombre.Contains(searchString.TrimEnd())
                                       || db.Cliente.FirstOrDefault(c => c.Id == s.Cliente).Nombre.Contains(searchString.TrimEnd())
                                       || db.Equipo.FirstOrDefault(c => c.Id == s.Equipo).Nombre.Contains(searchString.TrimEnd())
                                       || db.Modelo.FirstOrDefault(c => c.Id == s.Modelo).Nombre.Contains(searchString.TrimEnd())
                                       || db.Componente.FirstOrDefault(c => c.Id == s.Componente).Nombre.Contains(searchString.TrimEnd())
                                       || db.Parte.FirstOrDefault(c => c.Id == s.Parte).Nombre.Contains(searchString.TrimEnd())
                                       || s.Descripcion.Contains(searchString)
                                       || db.Persona.FirstOrDefault(c=>c.Rut==s.Creador).Nombre.Contains(searchString)
                                       || db.Area.FirstOrDefault(c => c.Id == s.Area).Nombre.Contains(searchString)
                                       || db.SubArea.FirstOrDefault(c => c.Id == s.SubArea).Nombre.Contains(searchString)


                                       );
            }

            if (searchIntHoras!=null)
            {
                eventos = eventos.Where(s => s.HorasComponente>searchIntHoras
                           
                                       );
            }

            if (searchIntDias != null)
            {
             
                eventos = eventos.Where(s =>(Convert.ToInt32((DateTime.Now - s.FechaRegistro).Value.Days)> searchIntDias));
            }

            if (EstadoSearch != null)
            {
                if (EstadoSearch != 11)
                {
                    eventos = eventos.Where(s => s.Estado == EstadoSearch);
                }
                else
                {
                    eventos = eventos.Where(s => s.Estado != 10);
                }
                
            }

            switch (sortOrder)
            {
                case "Id":
                    eventos = eventos.OrderBy(s => s.Id);
                    break;
                case "id_desc":
                    eventos = eventos.OrderByDescending(s => s.Id);
                    break;
                case "tipo_evento":
                    eventos = eventos.OrderBy(s => s.TipoEvento);
                    break;
                case "tipo_evento_desc":
                    eventos = eventos.OrderByDescending(s => s.TipoEvento);
                    break;
                case "estado_desc":
                    eventos = eventos.OrderBy(s => s.Estado);
                    break;
                case "Date":
                    eventos = eventos.OrderBy(s => s.FechaRegistro);
                    break;
                case "date_desc":
                    eventos = eventos.OrderByDescending(s => s.FechaRegistro);
                    break;
                case "Cliente":
                    eventos = eventos.OrderBy(s => s.Cliente);
                    break;
                case "cliente_desc":
                    eventos = eventos.OrderByDescending(s => s.Cliente);
                    break;
                case "Equipo":
                    eventos = eventos.OrderBy(s => s.Equipo);
                    break;
                case "equipo_desc":
                    eventos = eventos.OrderByDescending(s => s.Equipo);
                    break;
                case "Modelo":
                    eventos = eventos.OrderBy(s => s.Modelo);
                    break;
                case "modelo_desc":
                    eventos = eventos.OrderByDescending(s => s.Modelo);
                    break;
                case "Componente":
                    eventos = eventos.OrderBy(s => s.Componente);
                    break;
                case "componente_desc":
                    eventos = eventos.OrderByDescending(s => s.Componente);
                    break;
                case "HorasComponente":
                    eventos = eventos.OrderBy(s => s.HorasComponente);
                    break;
                case "horascomponente_desc":
                    eventos = eventos.OrderByDescending(s => s.HorasComponente);
                    break;
                case "Parte":
                    eventos = eventos.OrderBy(s => s.Parte);
                    break;
                case "parte_desc":
                    eventos = eventos.OrderByDescending(s => s.Parte);
                    break;
                case "Descripcion":
                    eventos = eventos.OrderBy(s => s.Descripcion);
                    break;
                case "descripcion_desc":
                    eventos = eventos.OrderByDescending(s => s.Descripcion);
                    break;
                default:
                    eventos = eventos.OrderBy(s => s.Id);
                    break;
            }

            ListaEvento ep = new ListaEvento()
            {

            };
            int pageSize = 25;
            PaginatedList<FINNINGWEB.Entities.Evento> pagina = await PaginatedList<Evento>.CreateAsync(eventos.AsNoTracking(), page ?? 1, pageSize);
            ep.ListEvento = pagina;
            ep.probando = pagina;
            ep.Cliente = db.Cliente.ToList();
            ep.Equipo = db.Equipo.ToList();
            ep.Modelo = db.Modelo.ToList();
            ep.Componente = db.Componente.ToList();
            ep.Parte = db.Parte.ToList();
            ep.TipoEvento = db.TipoEvento.ToList();
            ep.search = searchString;
            ep.HorasSearch = searchIntHoras;
            ep.TipoEventoSearch = TipoEventoSearch;
            ep.EstadoSearch = EstadoSearch;
            ep.AnioSearch = AnioSearch;
            ep.MesSearch = mesSearch;
            ep.AsignacionSearch = AsignacionSearch;
            ep.optradio = optradio;
            ep.ListEstado = db.Estado.ToList();
            ep.HorasSearch = searchIntHoras;
            ep.FechaActual = DateTime.Now;
            ep.DiasSearch = searchIntDias;
            ep.AplicaRCASearch = AplicaRCASearch;
            return View(ep);

        }





    }
}

