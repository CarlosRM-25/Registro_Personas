using Microsoft.EntityFrameworkCore;
using Registro_Personas.Models;

namespace Registro_Personas.Data
{
    // Solución: Heredar de DbContext para que PersonasRepo sea un contexto válido
    public class PersonasRepo : DbContext
    {
        public PersonasRepo(DbContextOptions<PersonasRepo> options)
            : base(options) { }

        public DbSet<Personas> Personas { get; set; } = null!;
    }
}
