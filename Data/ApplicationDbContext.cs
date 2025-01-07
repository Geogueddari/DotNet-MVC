using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Text;
using System;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace WebApplication2.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Produit> Produits { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Categorie>().HasData(
            new Categorie { Id = 1, Name = "Mobile", OrdreAffichage = "1" },
            new Categorie { Id = 2, Name = "Ordinateur", OrdreAffichage = "2" },
            new Categorie { Id = 3, Name = "Périphériques", OrdreAffichage = "3"}
            );
            modelBuilder.Entity<Produit>().HasData(
                new Produit{Id = 1,
                Name = "Iphone 14",
                Description = "iPhone 14 Pro. Avec un appareil photo principal de 48 MP pour capturer des détails époustouflants.DynamicIsland et l'écran toujours activé, qui offrent une toute nouvelle expérience sur iPhone",
                PrixProduit=10000, 
                Code=123456789,
                categorieId = 1,
                imageURL = " "
                },
                new Produit{
                Id = 2,
                Name = "Imprimante hp deskjet 2710",
                Description = "Toutes les fonctions de base,désormais faciles à utiliser.Imprimez,numérisez et copiezles documents quotidiens,et profitez d’une connexion simple et sans fil", 
                PrixProduit = 5000,
                Code = 546456789,
                categorieId = 2,
                imageURL =" "
                }
            );
        }
    }
}