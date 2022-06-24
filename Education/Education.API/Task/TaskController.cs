using Education.Application.EduCourses.Dto;
using Education.Application.EduCourses.Queries;
using Education.Application.EduTasks.Commands;
using Education.Application.EduTasks.Dto;
using Education.Domain.EduTasks;
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

namespace Education.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    [Authorize]
    [SwaggerTag("Задания, находятся внутри модуля")]
    public class TaskController : ControllerBase
    {
        private readonly IEduMediator _mediator;
        private readonly IAuthorizationService _authorizationService;

        public TaskController(IEduMediator mediator, IAuthorizationService authorizationService)
        {
            _mediator = mediator;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Создать задание может только учитель")]
        public async Task<IActionResult> Create(CreateEduTaskCommand command)
        {
            var course = await _mediator.GetQueryNotNull<EduCourseDetailedDto>(new CourseByModuleQuery(command.ModuleId));

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, course, "TeacherPolicy");
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var response = await _mediator.SendCommand(command);

            return Created(nameof(Get), response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EduTaskDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        [SwaggerOperation("Получить задание, может воспользоваться авторизованный пользователь, записанный на курс или учитель")]
        public async Task<IActionResult> Get([Required][MongoObjectId] string id)
        {
            var course = await _mediator.GetQueryNotNull<EduCourseDetailedDto>(new CourseByTaskQuery(id));

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, course, "StudentOrTeacherPolicy");
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var response = await _mediator.GetQueryNotNull<EduTaskDto>(new MongoGetQuery<EduTaskDto>(id));

            return Ok(response);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Изменить задание может только учитель")]
        public async Task<IActionResult> Update(UpdateEduTaskCommand command)
        {
            var course = await _mediator.GetQueryNotNull<EduCourseDetailedDto>(new CourseByTaskQuery(command.Id));

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, course, "TeacherPolicy");
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            await _mediator.SendCommand(command);

            return Created(nameof(Get), command);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Удалить задание может только учитель")]
        public async Task<IActionResult> Delete([Required][MongoObjectId] string id)
        {
            var course = await _mediator.GetQueryNotNull<EduCourseDetailedDto>(new CourseByTaskQuery(id));

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, course, "TeacherPolicy");
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var response = await _mediator.SendCommand(new MongoDeleteCommand<EduTask>(id));

            return Ok(response);
        }
    }
}
