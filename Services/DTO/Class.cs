using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class AuthorizationDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
    public class ProductDto
    {
        public ProductDto() { }
        public ProductDto(Product p)
        {
            Name = p.Name;
            Id = p.Id;
            Description = p.Description;
            Price = p.Price;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
    public class PostProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
    public class BasketItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public double Count { get; set; }
    }
    public class PaginationDto
    {
        public string SortColumn { get; set; } = "name";
        public int Page { get; set; } = 1;
        public int RowsPerPage { get; set; } = 10;
        public bool SortAscending { get; set; } = false;
    }
    public class PostUserDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surrname { get; set; }
    }
    public class PostUserDtoPasswd
    {
        public string Login { get; set; }
        public string PasswordOld { get; set; }
        public string PasswordNew { get; set; }
        public string Name { get; set; }
        public string Surrname { get; set; }
    }
    public class UserDto
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Surrname { get; set; }

        public UserDto() { }
        public UserDto(User user)
        {
            Id = user.Id;
            Login = user.Login;
            Name = user.Name;
            Surrname = user.Surname;
        }
    }
    public class PaginatedData<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int Count { get; set; }
    }
}
