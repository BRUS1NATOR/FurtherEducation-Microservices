using Education.Application.EduCourses.Dto;
using Education.Application.EduCourses.Queries;
using Education.Application.EduQuizes.Dto;
using Education.Application.EduTests.Commands;
using Education.Application.EduTests.Dto;
using Education.Domain.EduTests;
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

namespace Education.API.EduTests
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    [Authorize]
    [SwaggerTag("Тесты, находятся внутри модуля")]
    public class TestController : ControllerBase
    {
        private readonly IEduMediator _mediator;
        private readonly IAuthorizationService _authorizationService;

        public TestController(IEduMediator mediator, IAuthorizationService authorizationService)
        {
            _mediator = mediator;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Создавать тест может только учитель")]
        public async Task<IActionResult> Create([Required] CreateEduTestCommand command)
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EduTestDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Получить описание теста, может воспользоваться авторизованный пользователь, записанный на курс или учитель")]
        public async Task<IActionResult> Get([Required][MongoObjectId] string id)
        {
            var course = await _mediator.GetQueryNotNull<EduCourseDetailedDto>(new CourseByTestQuery(id));

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, course, "StudentOrTeacherPolicy");
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var response = await _mediator.GetQueryNotNull<EduTestDto>(new MongoGetQuery<EduTestDto>(id));

            return Ok(response);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Изменить тест может только учитель")]
        public async Task<IActionResult> Update([Required] UpdateEduTestCommand command)
        {
            var course = await _mediator.GetQueryNotNull<EduCourseDetailedDto>(new CourseByTestQuery(command.Id));

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
        [SwaggerOperation("Удалить тест может только учитель")]
        public async Task<IActionResult> Delete([Required][MongoObjectId] string id)
        {
            var course = await _mediator.GetQueryNotNull<EduCourseDetailedDto>(new CourseByTestQuery(id));

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, course, "TeacherPolicy");
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var response = await _mediator.SendCommand(new MongoDeleteCommand<EduTest>(id));

            return Ok(response);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Установить вопросы в тесте, может только учитель")]
        public async Task<IActionResult> SetQuestions([Required] SetEduTestQuestionsCommand command)
        {
            var course = await _mediator.GetQueryNotNull<EduCourseDetailedDto>(new CourseByTestQuery(command.TestId));

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, course, "TeacherPolicy");
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var response = await _mediator.SendCommand(command);

            return Ok(response);
        }

        [HttpGet("{testId}")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EduTestQuestionListDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Просмотреть тестовы вопросы, может только учитель, для выдачи вопросов студенту, используйте методы контроллера Quiz")]
        public async Task<IActionResult> GetQuestions([Required] string testId)
        {
            var course = await _mediator.GetQueryNotNull<EduCourseDetailedDto>(new CourseByTestQuery(testId));

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, course, "TeacherPolicy");
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var response = await _mediator.GetQueryNotNull<EduTestQuestionListDto>(new MongoGetPagedQuery<EduTestQuestionListDto>(testId, 0, 9999));

            return Ok(response);
        }
    }
}
