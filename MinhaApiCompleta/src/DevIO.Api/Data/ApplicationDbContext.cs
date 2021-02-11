using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        // Quando tem mais de um contexto na aplicação, é preciso passar a especificação <T> de qual contexto que é
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
