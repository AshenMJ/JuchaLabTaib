using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.DTO;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [ApiController, Authorize]
    [Route("[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService basketService;

        public BasketController(IBasketService basketService)
        {
            this.basketService = basketService;
        }

        [HttpGet]
        public IEnumerable<BasketItemDto> Get()
            => this.basketService.Get();

        [HttpPost("{productId}")]
        public IEnumerable<BasketItemDto> Post(int productId,[FromBody]int count)
            => this.basketService.Post(productId, count);

        [HttpPut("{basketItemId}")]
        public IEnumerable<BasketItemDto> Put(int basketItemId, [FromBody] int count)
            => this.basketService.Put(basketItemId, count);

        [HttpDelete("{basketItemId}")]
        public IEnumerable<BasketItemDto> Delete(int basketItemId)
            => this.basketService.Delete(basketItemId);

        [HttpDelete]
        public bool Clear()
            => this.basketService.Clear();
    }
}
