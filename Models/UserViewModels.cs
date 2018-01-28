using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FINNINGWEB.Entities;

namespace FINNINGWEB.Models
{
    public class CreateModel
    {
        public string id { get; set; }
        [StringLength(50, ErrorMessage = "El Campo User Name excede los 50 caracteres permitidos.")]
        public string Name { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "El Campo Email excede los 100 caracteres permitidos.")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string rutPersona { get; set; }
        public string rol { get; set; }
        public IEnumerable<Persona> ListaPersonas { get; set; }
        public IEnumerable<AspNetRoles> ListaRoles { get; set; }
        public AppUser usuarioUnico { get; set; }

    }
    public class LoginModel
    {
        [Required]
        [UIHint("email")]
        public string Email { get; set; }
        [Required]
        [UIHint("password")]
        public string Password { get; set; }
    }
    public class RoleEditModel
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<AppUser> Members { get; set; }
        public IEnumerable<AppUser> NonMembers { get; set; }
    }
    public class RoleModificationModel
    {
        [Required]
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }
    public class usuariosPersonas
    {
        public IEnumerable<Persona> ListaPersonas { get; set; }
        public IEnumerable<AspNetUsers> ListaUsuarios { get; set; }
        public IEnumerable<Area> ListaAreas { get; set; }
        public IEnumerable<SubArea> ListaSubAreas { get; set; }
        public IEnumerable<AspNetRoles> ListaRoles { get; set; }
        public IEnumerable<AspNetUserRoles> ListaUserRoles { get; set; }

    }
    public class CreatePersona
    {
        [Required]
        [StringLength(50, ErrorMessage = "El Campo Rut excede los 50 caracteres permitidos.")]
        public string rut { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "El Campo Nombre excede los 50 caracteres permitidos.")]
        public string Nombre { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "El Campo Apellido Paterno excede los 50 caracteres permitidos.")]
        public string ApellidoPaterno { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "El Campo Apellido Materno excede los 50 caracteres permitidos.")]
        public string ApellidoMaterno { get; set; }
        
        public string NombreCompleto { get; set; }

        public string Sexo { get; set; }
        [StringLength(50, ErrorMessage = "El Campo Cargo excede los 50 caracteres permitidos.")]
        public string Cargo { get; set; }

        public string RutSupervisor { get; set; }

        public int IdArea { get; set; }

        public int IdSubArea { get; set; }

        public IEnumerable<Persona> ListaPersonas { get; set; }
        public IEnumerable<Area> ListaAreas { get; set; }
        public IEnumerable<SubArea> ListaSubAreas { get; set; }
        public SubArea SubAreaUnica { get; set; }
        public Area AreaUnica { get; set; }

    }

    public class DatosPersona
    {

        public string rut { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Sexo { get; set; }
        public string Cargo { get; set; }
        public string RutSupervisor { get; set; }
        public string Area { get; set; }
        public string SubArea { get; set; }

    }
}