using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FINNINGWEB.Models
{
    public class AppUser : IdentityUser
    {
    
        public string rutPersona { get; set; }
        // no additional members are required
        // for basic Identity installation
    }
}
