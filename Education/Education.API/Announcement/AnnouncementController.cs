using Education.Application.Announcements.Commands;
using Education.Application.Announcements.Dto;
using Education.Application.EduCourses.Dto;
using Education.Application.EduCourses.Queries;
using Education.Domain.Announcement;
using FurtherEducation.Common;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Mediator;
using FurtherEducation.Common.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Education.API.EduAnnouncements
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    [SwaggerTag("Анонсы, могут находится как внутри курса, так и внутри модуля")]
    public class AnnouncementController : ControllerBase
    {
        private readonly IEduMediator _mediator;
        private readonly IAuthorizationService _authorizationService;

        public AnnouncementController(IAuthorizationService authorizationService, IEduMediator mediator)
        {
            _authorizationService = authorizationService;
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Создавать анонс может только учитель")]
        public async Task<IActionResult> Create([Required] CreateAnnouncementCommand command)
        {
            var course = await _mediator.GetQueryNotNull<EduCourseDetailedDto>(new MongoGetQuery<EduCourseDetailedDto>(command.CourseId));

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, course, "TeacherPolicy");
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var response = await _mediator.SendCommand(command);

            return Created(nameof(Get), response);
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AnnouncementDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([Required][MongoObjectId] string id)
        {
            var course = await _mediator.GetQueryNotNull<EduCourseDetailedDto>(new CourseByAnnouncementQuery(id));

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, course, "StudentOrTeacherPolicy");
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var response = await _mediator.GetQueryNotNull<AnnouncementDto>(new MongoGetQuery<AnnouncementDto>(id));

            return Ok(response);
        }

        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Изменить анонс может только учитель")]
        public async Task<IActionResult> Update([Required] UpdateAnnouncementCommand command)
        {
            var course = await _mediator.GetQueryNotNull<EduCourseDetailedDto>(new CourseByAnnouncementQuery(command.Id));

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, course, "TeacherPolicy");
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var response = await _mediator.SendCommand(command);

            return Ok(response);
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Удалить анонс может только учитель")]
        public async Task<IActionResult> Delete([Required][MongoObjectId] string id)
        {
            var course = await _mediator.GetQueryNotNull<EduCourseDetailedDto>(new CourseByAnnouncementQuery(id));

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, course, "TeacherPolicy");
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var response = await _mediator.SendCommand(new MongoDeleteCommand<EduAnnouncement>(id));

            return Ok(response);
        }
    }
}
