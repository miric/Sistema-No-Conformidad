using FINNINGWEB.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FINNINGWEB.Models
{
    public class ListaArea
    {
        [StringLength(50, ErrorMessage = "El Campo Nombre excede los 50 caracteres permitidos.")]
        public string NombreArea { get; set; }
        [StringLength(50, ErrorMessage = "El Campo Nombre excede los 50 caracteres permitidos.")]
        public string NombreSubArea { get; set; }
        public string RutPersona { get; set; }
        public string NombreCompleto { get; set; }
        public Area AreaUnica { get; set; }
        public SubArea SubAreaUnica { get; set; }
        public IEnumerable<Area> ListaAreas { get; set; }
        public IEnumerable<SubArea> ListaSubAreas { get; set; }
        public IEnumerable<Persona> ListaPersonas { get; set; }
        public IEnumerable<Persona> ListaSupervisores { get; set; }
        public IEnumerable<PersonaArea> ListaPersonaArea { get; set; }
        public string RutJefeArea { get; set; }

    }

    public class ListaCliente
    {
        [StringLength(50, ErrorMessage = "El Campo Nombre excede los 50 caracteres permitidos.")]
        public string nombre { get; set; }
        public Cliente ClienteUnico { get; set; }
        public IEnumerable<Cliente> ListaClientes { get; set; }


    }


    public class CreateArea
    {
        public IEnumerable<Area> ListaAreas { get; set; }
        public IEnumerable<Persona> ListaPersonas { get; set; }
        public Area AreaUnica { get; set; }
    }


    public class CreateEquipo
    {
        public int Id { get; set; }
        [StringLength(50, ErrorMessage = "El Campo Nombre excede los 50 caracteres permitidos.")]
        public string Nombre { get; set; }

        public IEnumerable<Equipo> ListaEquipos { get; set; }
        public Equipo EquipoUnico { get; set; }
    }
    public class ListaEquipo
    {

        public IEnumerable<Equipo> ListaEquipos { get; set; }
        public Equipo EquipoUnico { get; set; }

    }
    public class ListaEventoPrueba
    {
        public IEnumerable<Evento> ListaEventos { get; set; }
    }


    
    public class ListaComponente
    {
    
        [StringLength(50, ErrorMessage = "El Campo Nombre excede los 50 caracteres permitidos.")]
        public string Nombre { get; set; }
        [StringLength(50, ErrorMessage = "El Campo NumeroSerie excede los 50 caracteres permitidos.")]
        public string NumeroSerie { get; set; }
        [StringLength(50, ErrorMessage = "El Campo Posicion excede los 50 caracteres permitidos.")]
        public string Posicion { get; set; }
        public int ComponenteParteId { get; set; }
        public int? ParteId { get; set; }
        public int? ComponenteId { get; set; }
        public int? EquipoId { get; set; }
        public int? ModeloComponenteId { get; set; }
        public int? variable { get; set; }
        public IEnumerable<Componente> ListaComponentes { get; set; }
        public Componente ComponenteUnico { get; set; }
        public IEnumerable<Parte> listaPartes { get; set; }
    }

    public class ListaModelo
    {


        public int? ComponenteId { get; set; }
        public int? EquipoId { get; set; }
        public int? ModeloComponenteId { get; set; }
        public IEnumerable<Modelo> ListaModelos { get; set; }
        public Equipo EquipoUnico { get; set; }
        public int? ModeloId { get; set; }
        public IEnumerable<Equipo> ListaEquipos { get; set; }
        public IEnumerable<Componente> ListaComponentes { get; set; }
        public IEnumerable<ModeloComponente> ListaModeloComponentes { get; set; }
    }
    public class CreateModelo
    {
        [StringLength(50, ErrorMessage = "El Campo Nombre excede los 50 caracteres permitidos.")]
        public string Nombre { get; set; }
        public int EquipoId { get; set; }
        public IEnumerable<Equipo> ListaEquipos { get; set; }
        public Modelo ModeloUnico { get; set; }
    }
    public class CreateComponente
    {
        public int? Id { get; set; }
        [StringLength(50, ErrorMessage = "El Campo Nombre excede los 50 caracteres permitidos.")]
        public string Nombre { get; set; }
        [StringLength(50, ErrorMessage = "El Campo NumeroSerie excede los 50 caracteres permitidos.")]
        public string NumeroSerie { get; set; }
        [StringLength(50, ErrorMessage = "El Campo Posicion excede los 50 caracteres permitidos.")]
        public string Posicion { get; set; }

    }

    public class category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int ProductID { get; set; }
        public int SubCategoria { get; set; }
    }
    public class SubCategory
    {
        public int SubCategoriaID { get; set; }
        public int CategoryID { get; set; }
        public string SubCategoryName { get; set; }
    }

    public class MainProduct
    {
        public int ProductoID { get; set; }
        public string ProductoName { get; set; }
        public int SubCategoryID { get; set; }
    }

    public class CreateAreaSubArea
    {
        public IEnumerable<Area> ListaAreas { get; set; }
        public IEnumerable<SubArea> ListaSubAreas { get; set; }
        public SubArea SubAreaUnica { get; set; }
        public Area AreaUnica { get; set; }
    }

    public class CreateParte
    {
        public int ParteId { get; set; }
        [StringLength(50, ErrorMessage = "El Campo Nombre excede los 50 caracteres permitidos.")]
        public string Nombre { get; set; }
        [StringLength(50, ErrorMessage = "El Campo NumeroParte excede los 50 caracteres permitidos.")]
        public string NumeroParte { get; set; }

    }
    public class ListaParte
    {
        public int? variable { get; set; }
        public int? ModeloId { get; set; }
        public int idComponente { get; set; }
        public IEnumerable<Parte> ListaPartes { get; set; }
        public Parte ParteUnico { get; set; }
        public IEnumerable<ComponenteParte> ListaComponenteParte { get; set; }
        public IEnumerable<Componente> ListaComponente { get; set; }
        public IEnumerable<ModeloComponente> ListaModeloComponente { get; set; }
    }
    public class ListaProceso1
    {

        public IEnumerable<Proceso1> ListaProcesos1 { get; set; }
    }

}
