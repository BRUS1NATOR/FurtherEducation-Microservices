using AutoMapper;
using FurtherEducation.Common.Extension;
using FurtherEducation.Common.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.Application.Profile;
using Profile.Application.Profile.Dto;
using Profile.Domain.Profile;
using Swashbuckle.AspNetCore.Annotations;
using User.Domain.Profile.Repository;

namespace Profile.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    [SwaggerTag("Профиль пользователя")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IEduMediator _mediator;
        private readonly IProfileRepository _repo;
        private readonly IProfileService _service;
        private readonly IMapper _mapper;
        public ProfileController(IEduMediator mediator, IProfileRepository repo, IProfileService service, IMapper mapper)
        {
            _mediator = mediator;
            _repo = repo;
            _service = service;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Инициализировать профиль текущего пользователя, если такого нет")]
        public async Task<IActionResult> Create()
        {
            var userId = User.GetLoggedInUserId();
            var profile = await _repo.FindByIdAsync(userId);

            if (profile is null)
            {
                profile = await _service.CreateProfile(userId);

                return Created(nameof(Get), profile);
            }

            return Conflict(new { message = "User profile already exists" });
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProfileDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Получить профиль текущего пользователя, данным методом может воспользоваться авторизованный пользователь")]
        public async Task<IActionResult> Get()
        {
            var userId = User.GetLoggedInUserId();
            var profile = await _repo.FindByIdAsync(userId);

            if (profile is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ProfileDto>(profile));
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProfileDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Получить профиль пользователя, данным методом может воспользоваться авторизованный пользователь")]
        public async Task<IActionResult> Get(Guid userId)
        {
            var profile = await _repo.FindByIdAsync(userId);

            return Ok(_mapper.Map<ProfileDto>(profile));
        }
    }
}
