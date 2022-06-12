using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class BazaDanych : DbContext
    {
        public DbSet<Product> Products {get; set;}
        public DbSet<User> Users { get; set; }
        public DbSet<BasketItem> BasketItem { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder.UseSqlite("Data Source=C: \\Users\\mikiv\\OneDrive\\Pulpit\\MVC_kokot\\Models\\database.db");
        }
    
    }
    [Table("Products")]
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100), Required]
        public string Name { get; set; }
        [MaxLength(100)]
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public List<BasketItem> basketItems { get; set; }

    }
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50), Required]
        public string Login { get; set; }
        [MaxLength(100), MinLength(10), Required]
        public string Password { get; set; }
        [MaxLength(40), Required]
        public string Name { get; set; }
        [MaxLength(60), Required]
        public string Surname { get; set; }
        public List<BasketItem> basketItems { get; set; }

    }
    [Table("BasketItems")]
    public class BasketItem
    {
        [Key]
        public int Id { get; set; }
        public double Count { get; set; }
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User user { get; set; }

        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product product { get; set; }


    }
}
