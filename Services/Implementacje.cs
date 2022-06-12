using Models;
using Services;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class AuthorizationService: IAuthorizationService
    {
        private readonly BazaDanych database;
        public AuthorizationService(BazaDanych baza)
        {
            this.database = baza;
           
        }

        public int Login(AuthorizationDto dto)
        {
            var autoryzacja = database.Users.Where(p => p.Login == dto.Login && p.Password == dto.Password).FirstOrDefault();
            if (autoryzacja == null)
                return -1;
            else
                return autoryzacja.Id;
        }
    }
    public class ProductsService : IProductsService
    {
        private readonly BazaDanych database;
        public ProductsService(BazaDanych baza)
        {
            this.database = baza;
            if (!baza.Products.Any())
            {
                baza.AddRange(new List<Product>
                {
                    new Product{Description="Opis1", Name="Produkt 1", Price=10},
                    new Product{Description="Opis2", Name="Produkt 2", Price=20},
                    new Product{Description="Opis3", Name="Produkt 3", Price=30},
                    new Product{Description="Opis4", Name="Produkt 4", Price=40},
                    new Product{Description="Opis5", Name="Produkt 5", Price=50}
                });
                baza.SaveChanges();
            }
        }

        public PaginatedData<ProductDto> Get(PaginationDto dto)
        {
            PaginatedData <ProductDto> data= new PaginatedData<ProductDto>();
            data.Data = this.database.Products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price
            });
          
            data.Count = data.Data.Count();

            switch (dto.SortColumn.ToLower())
            {
                case "name":
                default:
                    {
                        if (dto.SortAscending) data.Data = data.Data.OrderBy(p => p.Name);
                        else data.Data = data.Data.OrderByDescending(p => p.Name);
                        break;
                    }
                case "description":
                    {
                        if (dto.SortAscending) data.Data = data.Data.OrderBy(p => p.Description);
                        else data.Data = data.Data.OrderByDescending(p => p.Description);
                        break;
                    }
                case "price":
                    {
                        if (dto.SortAscending) data.Data = data.Data.OrderBy(p => p.Price);
                        else data.Data = data.Data.OrderByDescending(p => p.Price);
                        break;
                    }

            }

            data.Data = data.Data.Skip((dto.Page - 1) * dto.RowsPerPage).Take(dto.RowsPerPage);
            return data;
        }
        public ProductDto Put(int productId, PostProductDto dto)
        {
            var obiekt = database.Products.Where(p => p.Id == productId).FirstOrDefault();

            if (obiekt == null)
                return null;
            
            obiekt.Name = dto.Name;
            obiekt.Price = dto.Price;
            obiekt.Description = dto.Description;

            this.database.SaveChanges();
            return new ProductDto(obiekt);
        }
        public ProductDto Post(PostProductDto dto)
        {
 
            var procuctDto = database.Products.Add(new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price
            });
            this.database.SaveChanges();
            return new ProductDto(procuctDto.Entity);
        }
        public PaginatedData<ProductDto> Delete(int productId)
        {
            var obiekt = database.Products.SingleOrDefault(p => p.Id == productId);
            if (obiekt != null)
            {
                database.Products.Remove(obiekt);
                this.database.SaveChanges();
            }
            return this.Get(new PaginationDto());
        }

        public ProductDto GetById(int id)
        {
            Product product = database.Products.SingleOrDefault(x => x.Id == id);
            if (product == null)
                return null;
            return new ProductDto(product);
        }
    }
    public class BasketService:IBasketService
    {
        private readonly BazaDanych database;
        public BasketService(BazaDanych baza)
        {
            this.database = baza;
        }
        public bool Clear()
        {
            IEnumerable<BasketItem> basketItems = this.database.BasketItem
                .Where(bi => bi.UserId == this.database.Users.First().Id);

            if (basketItems.Count() > 0)
            {
                this.database.RemoveRange(basketItems);
                this.database.SaveChanges();
                return true;
            }

            return false;
        }

        public IEnumerable<BasketItemDto> Delete(int basketItemId)
        {
            BasketItem basketItem = this.database.BasketItem
                .Where(bi => bi.Id == basketItemId).FirstOrDefault();

            if (basketItem == null)
                return null;

            this.database.BasketItem.Remove(basketItem);
            this.database.SaveChanges();

            return this.Get();
        }

        public IEnumerable<BasketItemDto> Get()
        {
            return this.database.BasketItem
                .Where(bi => bi.UserId == this.database.Users.First().Id)
                .Join(
                this.database.Products,
                bi => bi.ProductId,
                p => p.Id,
                (bi, p) => new BasketItemDto
                {
                    Id = bi.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Count = bi.Count
                });
        }

        public IEnumerable<BasketItemDto> Post(int productId, int count)
        {
            BasketItem basket = this.database.BasketItem.Where(z => z.ProductId == productId).FirstOrDefault();
            if (basket!=null)
            {
                Put(basket.Id, Convert.ToInt32(basket.Count + 1));
                return this.Get();
            }

            Product product = this.database.Products
                .Where(p => p.Id == productId).FirstOrDefault();

            if (product == null)
                return this.Get();

            if (count < 1)
                return this.Get();

            BasketItem basketItem = new BasketItem()
            {
                Count = count,
                ProductId = productId,
                UserId = this.database.Users.First().Id
            };
            this.database.BasketItem.Add(basketItem);
            this.database.SaveChanges();
            return this.Get();
        }

        public IEnumerable<BasketItemDto> Put(int basketItemId, int count)
        {
            if (count < 0)
                return this.Get();

                
            BasketItem basketItem = this.database.BasketItem
                .Where(bi => bi.Id == basketItemId).FirstOrDefault();

            if (basketItem != null)
            {
                basketItem.Count = count;
                this.database.SaveChanges();
            }
            return this.Get();
        }
    }
}
    public class UsersService:IUsersService
    {
        private readonly BazaDanych database;
        public UsersService(BazaDanych baza)
        {
            this.database = baza;
        if (!baza.Users.Any())
        {
            baza.AddRange(new List<User>
                {
                    new User { Login = "abc", Name = "abc", Password = "abc", Surname = "abc" },
                    new User { Login = "xxx", Name = "xxx", Password = "xxx", Surname = "xxx" }
                });
            baza.SaveChanges();
        }

    }
    public PaginatedData<UserDto> Get(PaginationDto dto)
    {
        if (dto.Page == null)
            dto.Page = 1;
        if (dto.RowsPerPage == null)
            dto.RowsPerPage = 5;
        if (dto.SortColumn == null)
            dto.SortColumn = "nothing";
            
        PaginatedData<UserDto> paginatedData = new PaginatedData<UserDto>();
        paginatedData.Data = this.database.Users
            .Select(u => new UserDto(u));
        paginatedData.Count = paginatedData.Data.Count();
        
        switch (dto.SortColumn.ToLower())
        {
            case "login":
                {
                    if (dto.SortAscending) paginatedData.Data = paginatedData.Data.OrderBy(u => u.Login);
                    else paginatedData.Data = paginatedData.Data.OrderByDescending(u => u.Login);
                    break;
                }
            case "name":
                {
                    if (dto.SortAscending) paginatedData.Data = paginatedData.Data.OrderBy(u => u.Name);
                    else paginatedData.Data = paginatedData.Data.OrderByDescending(u => u.Name);
                    break;
                }
            case "surname":
                {
                    if (dto.SortAscending) paginatedData.Data = paginatedData.Data.OrderBy(u => u.Surrname);
                    else paginatedData.Data = paginatedData.Data.OrderByDescending(u => u.Surrname);
                    break;
                }
            default:
                {
                    if (dto.SortAscending) paginatedData.Data = paginatedData.Data.OrderBy(u => u.Login);
                    else paginatedData.Data = paginatedData.Data.OrderByDescending(u => u.Login);
                    break;
                }
        }

        paginatedData.Data = paginatedData.Data.Skip((dto.Page - 1) * dto.RowsPerPage).Take(dto.RowsPerPage);
        return paginatedData;
    }
    public PaginatedData<UserDto> Delete(int userId)
    {
        var obiekt = database.Users.SingleOrDefault(p => p.Id == userId);
        if (obiekt != null)
        {
            database.Users.Remove(obiekt);
            this.database.SaveChanges();
        }
        return this.Get(new PaginationDto());
    }

    public UserDto Get(int id)
    {
        User users = database.Users.SingleOrDefault(x => x.Id == id);
        if (users == null)
            return null;
        return new UserDto(users);
    }
    public UserDto Post(PostUserDto dto)
    {
        User user = new User
        {
            Login = dto.Login,
            Password = dto.Password,
            Name = dto.Name,
            Surname = dto.Surrname
        };

        this.database.Users.Add(user);
        this.database.SaveChanges();

        return new UserDto
        {
            Id = user.Id,
            Login = user.Login,
            Name = user.Name,
            Surrname = user.Surname
        };
    }

    public UserDto Put(int usertId, PostUserDtoPasswd dto)
    {

            var obiekt = database.Users.Where(p => p.Id == usertId).FirstOrDefault();
        
            if (obiekt == null)
                return null;
        if (obiekt.Password != dto.PasswordOld)
                return null;

            obiekt.Name = dto.Name;
            obiekt.Login = dto.Login;
        if (dto.PasswordNew.Length > 1)
            obiekt.Password = dto.PasswordNew;
            obiekt.Name = dto.Name;
            obiekt.Surname = dto.Surrname;

            this.database.SaveChanges();
           return new UserDto(obiekt);
        
    }
}
