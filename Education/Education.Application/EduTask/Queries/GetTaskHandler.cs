using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Application.EduTasks.Dto;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using FurtherEducation.Common.Queries;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace Education.Application.EduTasks.Queries
{
    public class GetTaskHandler : IConsumer<MongoGetQuery<EduTaskDto>>
    {
        private readonly IEduTaskRepository _courseRepository;
        private readonly IMapper _mapper;

        public GetTaskHandler(IEduTaskRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<MongoGetQuery<EduTaskDto>> context)
        {
            var task = await _courseRepository.FindAsync(context.Message.Id);

            if (task is null)
            {
                await context.RespondAsync(new QueryResponse<EduTaskDto>(new EduExceptionMessage("Task not found", StatusCodes.Status404NotFound)));
                return;
            }

            await context.RespondAsync(new QueryResponse<EduTaskDto>(_mapper.Map<EduTaskDto>(task)));
        }
    }
}
