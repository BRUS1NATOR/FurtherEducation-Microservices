using AutoMapper;
using Education.Domain.EduTests;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using MassTransit;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;

namespace Education.Application.EduTests.Commands
{
    public class SetEduTestQuestionsHandler : IConsumer<SetEduTestQuestionsCommand>
    {
        private readonly IEduTestQuestionRepository _testRepository;
        private readonly IMapper _mapper;

        public SetEduTestQuestionsHandler(IEduTestQuestionRepository testRepository, IMapper mapper)
        {
            _testRepository = testRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<SetEduTestQuestionsCommand> context)
        {
            var mongoId = ObjectId.Parse(context.Message.TestId);
            var result = await _testRepository.SetQuestionsAsync(mongoId, _mapper.Map<List<EduTestQuestion>>(context.Message.Questions));

            if (result is null)
            {
                await context.RespondAsync(new CommandResponse(new EduExceptionMessage("Test not found", StatusCodes.Status404NotFound)));
                return;
            }

            await context.RespondAsync(new CommandResponse());
        }
    }
}
