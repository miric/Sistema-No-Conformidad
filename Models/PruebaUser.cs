using FINNINGWEB.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FINNINGWEB.Models
{
    public class PruebaUser
    {
        public AspNetUsers Usuario { get; set; }
        public Persona Persona { get; set; }
        public string RolName { get; set; }
        public IEnumerable<Area> ListaArea { get; set; }
        public IEnumerable<SubArea> ListaSubArea { get; set; }
        public IEnumerable<AspNetUsers> ListaUsuarios { get; set; }
        public IEnumerable<Persona> ListaPersonas { get; set; }

        [Required]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }


}
