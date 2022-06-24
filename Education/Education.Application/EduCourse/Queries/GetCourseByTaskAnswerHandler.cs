using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Application.EduCourses.Dto;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using FurtherEducation.Common.Helpers;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace Education.Application.EduCourses.Queries
{
    public class GetCourseByTaskAnswerHandler : IConsumer<CourseByTaskAnswerQuery>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IEduTaskAnswerRepository _taskAnswerRepository;
        private readonly IMapper _mapper;

        public GetCourseByTaskAnswerHandler(ICourseRepository courseRepository, IEduTaskAnswerRepository taskAnswerRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _taskAnswerRepository = taskAnswerRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<CourseByTaskAnswerQuery> context)
        {
            var answer = await _taskAnswerRepository.FindAsync(MongoHelper.Parse(context.Message.AnswerId));
            if (answer is null)
            {
                await context.RespondAsync(new QueryResponse<EduCourseDetailedDto>(new EduExceptionMessage("Answer not found", StatusCodes.Status404NotFound)));
                return;
            }
            var course = await _courseRepository.FindByTaskAsync(answer.TaskId);

            if (course is null)
            {
                await context.RespondAsync(new QueryResponse<EduCourseDetailedDto>(new EduExceptionMessage("Course not found", StatusCodes.Status404NotFound)));
                return;
            }

            await context.RespondAsync(new QueryResponse<EduCourseDetailedDto>(_mapper.Map<EduCourseDetailedDto>(course)));
        }
    }
}
