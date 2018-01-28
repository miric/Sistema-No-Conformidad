using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FINNINGWEB.Models
{
    public class Form
    {

    }


    public class ApplicationUser : IdentityUser
    {
        public byte[] AvatarImage { get; set; }
    }

    public class RegisterViewModel
    {
        // other properties omitted

        public IFormFile AvatarImage { get; set; }
    }
}
