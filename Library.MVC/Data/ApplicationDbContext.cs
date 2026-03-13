using Library.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Library.MVC.Data
{
    
        public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :IdentityDbContext(options)
        {
           
           public DbSet<Books> Books { get; set; }      //books
           public DbSet<Member> Members { get; set; }   //custumer
           public DbSet<Loan> Loans { get; set; }   //loans
        }
        
}
