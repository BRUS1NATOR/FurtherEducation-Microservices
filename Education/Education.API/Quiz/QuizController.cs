using Education.Application.EduCourses.Dto;
using Education.Application.EduCourses.Queries;
using Education.Application.EduQuizes.Commands;
using Education.Application.EduQuizes.Dto;
using Education.Application.EduQuizes.Queries;
using FurtherEducation.Common;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Extension;
using FurtherEducation.Common.Helpers;
using FurtherEducation.Common.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Education.API.EduQuiz
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    [SwaggerTag("Опрос")]
    [Authorize]
    public class QuizController : ControllerBase
    {
        private readonly IEduMediator _mediator;
        private readonly IAuthorizationService _authorizationService;

        public QuizController(IEduMediator mediator, IAuthorizationService authorizationService)
        {
            _mediator = mediator;
            _authorizationService = authorizationService;
        }

        [HttpPost("{testId}")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Начать тест, может воспользоваться авторизованный пользователь, записанный на курс")]
        public async Task<IActionResult> Start([Required][MongoObjectId] string testId)
        {
            var course = await _mediator.GetQueryNotNull<EduCourseDetailedDto>(new CourseByTestQuery(testId));

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, course, "StudentPolicy");
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var userId = User.GetLoggedInUserId();
            var response = await _mediator.SendCommand(new StartEduQuizCommand() { Id = ObjectId.GenerateNewId(DateTime.Now), TestId = testId, UserId = userId });

            return Created(nameof(Get), response);
        }

        [HttpGet("{quizId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EduQuizInProgressDto))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Просмотреть начатый тест, может воспользоваться авторизованный пользователь, записанный на курс")]
        public async Task<IActionResult> Get([Required][MongoObjectId] string quizId)
        {
            var response = await _mediator.GetQueryNotNull<EduQuizInProgressDto>(new GetQuizQuery(quizId));

            if (response.UserId != response.UserId)
            {
                return Forbid();
            }

            return Ok(response);
        }

        [HttpGet("{testId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EduQuizInProgressDto))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Просмотреть начатый тест, может воспользоваться авторизованный пользователь, записанный на курс")]
        public async Task<IActionResult> GetByTestId([Required][MongoObjectId] string testId)
        {
            var userId = User.GetLoggedInUserId();
            var response = await _mediator.GetQueryNotNull<EduQuizInProgressDto>(new GetQuizQuery(MongoHelper.Parse(testId), userId));

            if (response.UserId != response.UserId)
            {
                return Forbid();
            }

            return Ok(response);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Закончить тест, может воспользоваться авторизованный пользователь, записанный на курс")]
        public async Task<IActionResult> Finish([Required] FinishEduQuizCommand finishTestCommand)
        {
            var userId = User.GetLoggedInUserId();
            finishTestCommand.UserId = userId;

            var response = await _mediator.SendCommand(finishTestCommand);

            return Ok(response);
        }
    }
}
