using FurtherEducation.Common.Extension;
using FurtherEducation.Common.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using User.Application.User.Queries;
using User.Domain.Data.Interfaces;
using User.Domain.Dto;

namespace User.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IEduMediator _mediator;
        public UserController(IEduMediator mediator, IUserRepository repo)
        {
            _mediator = mediator;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        [SwaggerOperation("Данным методом может воспользоваться авторизованный пользователь")]
        public async Task<IActionResult> Get()
        {
            var userId = User.GetLoggedInUserId();

            var response = await _mediator.GetQueryNotNull<UserDto>(new GetUserByIdQuery() { Id = userId });

            return Ok(response);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation("Данным методом может воспользоваться только администратор")]
        public async Task<IActionResult> Get([Required] Guid userId)
        {
            var response = await _mediator.GetQueryNotNull<UserDto>(new GetUserByIdQuery() { Id = userId });

            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AttributeListDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Данным методом может воспользоваться авторизованный пользователь")]
        public async Task<IActionResult> GetAttributes()
        {
            var userId = User.GetLoggedInUserId();

            var response = await _mediator.GetQueryNotNull<AttributeListDto>(new GetUserAttributesByIdQuery() { UserId = userId });

            return Ok(response);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AttributeListDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Данным методом может воспользоваться только администратор")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAttributes([Required] Guid userId)
        {
            var response = await _mediator.GetQueryNotNull<AttributeListDto>(new GetUserAttributesByIdQuery() { UserId = userId });

            return Ok(response);
        }
    }
}
