using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Domain.EduTests;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace Education.Application.EduTests.Commands
{
    public class DeleteEduTestHandler : IConsumer<MongoDeleteCommand<EduTest>>
    {
        private readonly IEduTestRepository _testRepository;
        private readonly IMapper _mapper;

        public DeleteEduTestHandler(IEduTestRepository testRepository, IMapper mapper)
        {
            _testRepository = testRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<MongoDeleteCommand<EduTest>> context)
        {
            var success = await _testRepository.DeleteAsync(context.Message.Id);

            if (!success)
            {
                await context.RespondAsync(new CommandResponse(new EduExceptionMessage("Test not found", StatusCodes.Status404NotFound)));
                return;
            }

            await context.RespondAsync(new CommandResponse());
        }
    }
}
