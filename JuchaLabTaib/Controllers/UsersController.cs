using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.DTO;

namespace WebAPI.Controllers
{
    [ApiController, Authorize(Roles="admin")]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpGet]
        public PaginatedData<UserDto> Get([FromQuery] PaginationDto dto)
            => this.usersService.Get(dto);

        [HttpGet("{productId}")]
        public UserDto Get(int productId)
            => this.usersService.Get(productId);

        [HttpPost]
        public UserDto Post(PostUserDto dto)
            => this.usersService.Post(dto);

        [HttpPut("{userId}")]
        public UserDto Put(int userId, PostUserDtoPasswd dto)
            => this.usersService.Put(userId, dto);

        [HttpDelete("{userId}")]
        public PaginatedData<UserDto> Delete(int userId)
           => this.usersService.Delete(userId);
    }
}
