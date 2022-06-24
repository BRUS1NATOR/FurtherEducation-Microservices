using Education.Application.EduCourses.Dto;
using FurtherEducation.Common.Mediator;
using FurtherEducation.Common.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace Education.API.Courses
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    [SwaggerTag("Каталог курсов")]
    public class CatalogController : ControllerBase
    {
        private readonly IEduMediator _mediator;

        public CatalogController(IEduMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseCatalogDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int page, int size, string search = "")
        {
            if (size == 0)
            {
                size = 20;
            }

            var response = await _mediator.GetQueryNotNull<CourseCatalogDto>
                (new MongoGetPagedQuery<CourseCatalogDto> { Page = page, PageSize = size, Search = search });

            return Ok(response);
        }
    }
}
