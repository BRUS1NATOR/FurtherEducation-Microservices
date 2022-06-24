using Education.Application.EduCourses.Dto;
using Education.Application.EduQuizes.Commands;
using Education.Application.EduQuizes.Dto;
using FurtherEducation.Common;
using FurtherEducation.Common.Extension;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Mediator;
using FurtherEducation.Common.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Education.Application.EduCourses.Queries;
using Microsoft.AspNetCore.Authorization;

namespace Education.API.EduQuiz
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    [SwaggerTag("Результаты опросов")]
    public class QuizResultContoller : ControllerBase
    {
        private readonly IEduMediator _mediator;
        private readonly IAuthorizationService _authorizationService;

        public QuizResultContoller(IEduMediator mediator, IAuthorizationService authorizationService)
        {
            _mediator = mediator;
            _authorizationService = authorizationService;
        }

        [HttpGet("{quizId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EduQuizResultDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Просмотреть результаты теста")]
        public async Task<IActionResult> Get([Required][MongoObjectId] string quizId)
        {
            var response = await _mediator.GetQueryNotNull<EduQuizResultDto>(new MongoGetQuery<EduQuizResultDto>(quizId));

            return Ok(response);
        }

        [HttpGet("{testId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EduQuizResultListDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Просмотреть результаты теста")]
        public async Task<IActionResult> GetByTestId([Required] string testId)
        {
            var userId = User.GetLoggedInUserId();

            var course = await _mediator.GetQueryNotNull<EduCourseDetailedDto>(new CourseByTestQuery(testId));
            if (course.Teacher != userId)
            {
                return Forbid();
            }

            var response = await _mediator.GetQueryNotNull<EduQuizResultListDto>(new MongoGetPagedQuery<EduQuizResultListDto>(testId, 0, 50));

            return Ok(response);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Изменить результаты теста, не использовать!")]
        public async Task<IActionResult> Update([Required] UpdateEduQuizResultCommand command)
        {
            var result = await _mediator.SendCommand(command);

            return Created(nameof(Get), result);
        }
    }
}
