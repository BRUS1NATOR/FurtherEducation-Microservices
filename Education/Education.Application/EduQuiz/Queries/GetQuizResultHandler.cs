using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Application.EduQuizes.Dto;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using FurtherEducation.Common.Queries;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace Education.Application.EduQuizes.Queries
{
    public class GetQuizResultHandler : IConsumer<MongoGetQuery<EduQuizResultDto>>
    {
        private readonly IEduQuizRepository _answerRepository;
        private readonly IMapper _mapper;

        public GetQuizResultHandler(IEduQuizRepository answerRepository, IMapper mapper)
        {
            _answerRepository = answerRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<MongoGetQuery<EduQuizResultDto>> context)
        {
            var result = await _answerRepository.FindAsync(context.Message.Id);

            if (result is null)
            {
                await context.RespondAsync(new QueryResponse<EduQuizResultDto>(new EduExceptionMessage("Test answer not found", StatusCodes.Status404NotFound)));
                return;
            }

            await context.RespondAsync(new QueryResponse<EduQuizResultDto>(_mapper.Map<EduQuizResultDto>(result)));
        }
    }
}
