using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Hv2020.Felanmalan.Login.WCF
{
    public partial class LoginModel : DbContext
    {
        public LoginModel()
            : base("name=LoginModel")
        {
        }

        public virtual DbSet<Login> Login { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Login>()
                .Property(e => e.Anvandarnamn)
                .IsUnicode(false);

            modelBuilder.Entity<Login>()
                .Property(e => e.Salt)
                .IsUnicode(false);

            modelBuilder.Entity<Login>()
                .Property(e => e.Losenord)
                .IsUnicode(false);
        }
    }
}
