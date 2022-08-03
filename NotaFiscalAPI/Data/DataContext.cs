
using Microsoft.EntityFrameworkCore;

namespace NotaFiscalAPI.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {

        }

        //public DbSet<NotaFiscal> NotasFiscais { get; set; }
        public DbSet<Desenvolvedor> Desenvolvedores { get; set; }

    }
}
 