using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Application.EduTests.Dto;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using FurtherEducation.Common.Queries;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace Education.Application.EduTests.Queries
{
    public class GetTestHandler : IConsumer<MongoGetQuery<EduTestDto>>
    {
        private readonly IEduTestRepository _testRepository;
        private readonly IMapper _mapper;

        public GetTestHandler(IEduTestRepository testRepository, IMapper mapper)
        {
            _testRepository = testRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<MongoGetQuery<EduTestDto>> context)
        {
            var test = await _testRepository.FindAsync(context.Message.Id);

            if (test is null)
            {
                await context.RespondAsync(new QueryResponse<EduTestDto>(new EduExceptionMessage("Test not found", StatusCodes.Status404NotFound)));
                return;
            }

            await context.RespondAsync(new QueryResponse<EduTestDto>(_mapper.Map<EduTestDto>(test)));
        }
    }
}
