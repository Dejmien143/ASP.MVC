using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Warehouse_Management.Models;

namespace Warehouse_Management.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Warehouse_Management.Models.ProductModel>? ProductModel { get; set; }
        public DbSet<Warehouse_Management.Models.CategoryModel>? CategoryModel { get; set; }
    }
}