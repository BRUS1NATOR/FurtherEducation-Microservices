using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Domain.EduTests;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Education.Application.EduTests.Commands
{
    public class UpdateEduTestHandler : IConsumer<UpdateEduTestCommand>
    {
        private readonly IEduTestRepository _testRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateEduTestHandler> _logger;

        public UpdateEduTestHandler(IEduTestRepository testRepository, IMapper mapper,
            ILogger<UpdateEduTestHandler> logger)
        {
            _testRepository = testRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<UpdateEduTestCommand> context)
        {
            var result = await _testRepository.UpdateAsync(_mapper.Map<EduTest>(context.Message));

            if (result is false)
            {
                await context.RespondAsync(new CommandResponse(new EduExceptionMessage("Test not found", StatusCodes.Status404NotFound)));
                return;
            }

            await context.RespondAsync(new CommandResponse(context.Message.Id));
        }
    }
}
