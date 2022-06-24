using Education.Application.Data.Repositories;
using Education.Application.EduCourses.Commands;
using Education.Application.EduCourses.Dto;
using Education.Domain.EduCourses;
using FurtherEducation.Common;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Extension;
using FurtherEducation.Common.Mediator;
using FurtherEducation.Common.Queries;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Education.API.Courses
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    [SwaggerTag("Курсы")]
    public class CourseController : ControllerBase
    {
        private readonly IEduMediator _mediator;
        private readonly IAuthorizationService _authorizationService;

        public CourseController(IAuthorizationService authorizationService, IEduMediator mediator)
        {
            _authorizationService = authorizationService;
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost("{courseId}")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Записаться на курс, может воспользоваться любой авторизованный пользователь")]
        public async Task<IActionResult> SignUser([MongoObjectId][Required] string courseId)
        {
            var userId = User.GetLoggedInUserId();

            var response = await _mediator.SendCommand(new SignUpUserToCourseCommand()
            {
                UserId = userId,
                CourseId = courseId,
            });

            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Создать курс, учителем курса будет авторизованный пользователь")]
        public async Task<IActionResult> Create(CreateCourseCommand command)
        {
            command.Teacher = User.GetLoggedInUserId();
            var response = await _mediator.SendCommand(command);

            return Created(nameof(Get), response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EduCourseDetailedDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([Required][MongoObjectId] string id)
        {
            var response = await _mediator.GetQueryNotNull<EduCourseDetailedDto>(new MongoGetQuery<EduCourseDetailedDto>(id));

            return Ok(response);
        }

        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Изменять курс может только учитель")]
        public async Task<IActionResult> Update([Required] UpdateCourseCommand command)
        {
            var course = await _mediator.GetQueryNotNull<EduCourseDetailedDto>(new MongoGetQuery<EduCourseDetailedDto>(command.Id));

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, course, "TeacherPolicy");

            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            command.Teacher = User.GetLoggedInUserId();
            var response = await _mediator.SendCommand(command);

            return Ok(response);
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Удалять курс может только учитель")]
        public async Task<IActionResult> Delete([Required][MongoObjectId] string id)
        {
            var course = await _mediator.GetQueryNotNull<EduCourseDetailedDto>(new MongoGetQuery<EduCourseDetailedDto>(id));

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, course, "TeacherPolicy");

            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var response = await _mediator.SendCommand(new MongoDeleteCommand<EduCourse>(id));

            return Ok(response);
        }
    }
}