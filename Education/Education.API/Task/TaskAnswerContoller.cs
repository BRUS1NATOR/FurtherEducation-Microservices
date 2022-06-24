using Education.Application.EduCourses.Dto;
using Education.Application.EduCourses.Queries;
using Education.Application.EduTaskAnswers.Commands;
using Education.Application.EduTasks.Dto;
using FurtherEducation.Common;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Extension;
using FurtherEducation.Common.Mediator;
using FurtherEducation.Common.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Education.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    [SwaggerTag("Ответы на задания")]
    public class TaskAnswerContoller : ControllerBase
    {
        private readonly IEduMediator _mediator;
        private readonly IAuthorizationService _authorizationService;

        public TaskAnswerContoller(IEduMediator mediator, IAuthorizationService authorizationService)
        {
            _mediator = mediator;
            _authorizationService = authorizationService;
        }

        //CREATE
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Добавить ответ на задание,может воспользоваться авторизованный пользователь, записанный на курс")]
        public async Task<IActionResult> Create([Required] CreateEduTaskAnswerCommand command)
        {
            var course = await _mediator.GetQueryNotNull<EduCourseDetailedDto>(new CourseByTaskQuery(command.TaskId));

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, course, "StudentPolicy");
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            command.UserId = User.GetLoggedInUserId();
            var response = await _mediator.SendCommand(command);

            return Created(nameof(Get), response);
        }

        //READ
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EduTaskAnswerDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Получить ответ на задание, может воспользоваться авторизованный пользователь, записанный на курс")]
        public async Task<IActionResult> Get([Required][MongoObjectId] string id)
        {
            var course = await _mediator.GetQueryNotNull<EduCourseDetailedDto>(new CourseByTaskAnswerQuery(id));

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, course, "StudentPolicy");
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var response = await _mediator.GetQueryNotNull<EduTaskAnswerDto>(new MongoGetQuery<EduTaskAnswerDto>(id));

            return Ok(response);
        }

        [HttpGet("{taskId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EduTaskAnswerListDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        [SwaggerOperation("Получить все ответы на задание, может воспользоваться только преподаватель курса в который входит задание")]
        public async Task<IActionResult> GetAnswers([Required][MongoObjectId] string taskId)
        {
            var course = await _mediator.GetQueryNotNull<EduCourseDetailedDto>(new CourseByTaskQuery(taskId));

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, course, "TeacherPolicy");
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var response = await _mediator.GetQueryNotNull<EduTaskAnswerListDto>(new MongoGetPagedQuery<EduTaskAnswerListDto>(taskId, 0, 99));

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EduTaskAnswerListDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        [SwaggerOperation("Оценить ответ на задание, может воспользоваться только преподаватель курса в который входит задание")]
        public async Task<IActionResult> RateAnswer([Required] RateEduTaskAnswerCommand command)
        {
            var course = await _mediator.GetQueryNotNull<EduCourseDetailedDto>(new CourseByTaskAnswerQuery(command.Id));

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, course, "TeacherPolicy");
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var response = await _mediator.SendCommand(command);

            return Ok(response);
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [SwaggerOperation("Изменить ответ на задание, может воспользоваться авторизованный пользователь, записанный на курс")]
        public async Task<IActionResult> Update([Required] UpdateEduTaskAnswerCommand command)
        {
            var course = await _mediator.GetQueryNotNull<EduCourseDetailedDto>(new CourseByTaskAnswerQuery(command.TaskId));

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, course, "StudentPolicy");
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var userId = User.GetLoggedInUserId();
            command.UserId = userId;

            var response = await _mediator.SendCommand(command);

            return Created(nameof(Get), response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Удалить ответ на задание, может воспользоваться авторизованный пользователь, записанный на курс")]
        public async Task<IActionResult> Delete([Required][MongoObjectId] string id)
        {
            var course = await _mediator.GetQueryNotNull<EduCourseDetailedDto>(new CourseByTaskAnswerQuery(id));

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, course, "TeacherPolicy");
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var response = await _mediator.SendCommand(new MongoDeleteCommand<EduTaskAnswerDto>(id));

            return Ok(response);
        }
    }
}
