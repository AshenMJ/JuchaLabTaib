using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.DTO;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [ApiController, Authorize]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService productsService;

        public ProductsController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        [HttpGet]
        public PaginatedData<ProductDto> Get([FromQuery] PaginationDto dto)
            => this.productsService.Get(dto);

        [HttpGet("{productId}")]
        public ProductDto Get(int productId)
           => this.productsService.GetById(productId);

        [HttpPut("{productId}")]
        public ProductDto Put(int productId, PostProductDto dto)
            => this.productsService.Put(productId, dto);

        [HttpPost]
        public ProductDto Post([FromBody] PostProductDto dto)
            => this.productsService.Post(dto);

        [HttpDelete("{productId}")]
        public PaginatedData<ProductDto> Delete(int productId)
            => this.productsService.Delete(productId);
    }
}
