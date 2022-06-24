using Education.Application.EduCourses.Dto;
using Education.Application.EduCourses.Queries;
using Education.Application.EduModules.Commands;
using Education.Application.EduModules.Dto;
using FurtherEducation.Common;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Mediator;
using FurtherEducation.Common.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Threading.Tasks;

namespace Education.API.EduModule
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    [SwaggerTag("Модули, находятся внутри курса")]
    public class ModuleController : ControllerBase
    {
        private readonly IEduMediator _mediator;
        private readonly IAuthorizationService _authorizationService;
        public ModuleController(IAuthorizationService authorizationService, IEduMediator mediator)
        {
            _authorizationService = authorizationService;
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Создавать модуль может только учитель")]
        public async Task<IActionResult> Create([Required] CreateModuleCommand command)
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ModuleDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Получить модуль, может воспользоваться авторизованный пользователь, записанный на курс или учитель")]
        public async Task<IActionResult> Get([Required][MongoObjectId] string id)
        {
            var course = await _mediator.GetQueryNotNull<EduCourseDetailedDto>(new CourseByModuleQuery(id));

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, course, "StudentOrTeacherPolicy");
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var response = await _mediator.GetQueryNotNull<ModuleDto>(new MongoGetQuery<ModuleDto>(id));

            return Ok(response);
        }

        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Изменять модуль может только учитель")]
        public async Task<IActionResult> Update([Required] UpdateModuleCommand command)
        {
            var course = await _mediator.GetQueryNotNull<EduCourseDetailedDto>(new CourseByModuleQuery(command.Id));

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, course, "TeacherPolicy");
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            await _mediator.SendCommand(command);

            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Удалять модуль может только учитель")]
        public async Task<IActionResult> Delete([Required][MongoObjectId] string id)
        {
            var course = await _mediator.GetQueryNotNull<EduCourseDetailedDto>(new CourseByModuleQuery(id));

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, course, "TeacherPolicy");
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var response = await _mediator.SendCommand(new MongoDeleteCommand<Module>(id));

            return Ok(response);
        }
    }
}
