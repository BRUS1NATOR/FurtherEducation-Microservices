using AutoMapper;
using FurtherEducation.Common;
using FurtherEducation.Common.Extension;
using FurtherEducation.Common.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.Application.Diary.Dto;
using Profile.Domain.Diary.Repository;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Profile.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    [SwaggerTag("Дневник по курсу пользователя")]
    public class StudentDiaryController : ControllerBase
    {
        private readonly IEduMediator _mediator;
        private readonly IStudentDiaryRepository _repo;
        private readonly IMapper _mapper;

        public StudentDiaryController(IEduMediator mediator, IStudentDiaryRepository repo, IMapper mapper)
        {
            _mediator = mediator;
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDiaryDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation("Данным методом может воспользоваться только администратор")]
        public async Task<IActionResult> Get([Required] Guid userId, [Required] string courseId)
        {
            var response = await _repo.FindOneAsync(x => x.CourseId == courseId && x.UserId == userId);

            if (response is null)
            {
                return NotFound();
            }
            if (response.UserId != userId)
            {
                return Forbid();
            }
            return Ok(_mapper.Map<StudentDiaryDto>(response));
        }

        [HttpGet("{courseId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDiaryDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        [SwaggerOperation("Данным методом может воспользоваться авторизованный пользователь")]
        public async Task<IActionResult> Get([MongoObjectId] string courseId)
        {
            var userId = User.GetLoggedInUserId();
            var response = await _repo.FindOneAsync(x => x.CourseId == courseId);

            if (response is null)
            {
                return NotFound();
            }
            if (response.UserId != userId)
            {
                return Forbid();
            }
            return Ok(_mapper.Map<StudentDiaryDto>(response));
        }
    }
}
