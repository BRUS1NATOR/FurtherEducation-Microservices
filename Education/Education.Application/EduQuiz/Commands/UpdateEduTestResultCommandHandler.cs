using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Application.EduQuizes.Dto;
using Education.Domain.EduQuizes;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace Education.Application.EduQuizes.Commands
{
    public class UpdateEduTestResultCommandHandler : IConsumer<UpdateEduQuizResultCommand>
    {
        private readonly IEduQuizRepository _answerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateEduTestResultCommandHandler> _logger;

        public UpdateEduTestResultCommandHandler(IEduQuizRepository answerRepository, IMapper mapper,
            ILogger<UpdateEduTestResultCommandHandler> logger)
        {
            _answerRepository = answerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<UpdateEduQuizResultCommand> context)
        {
            var answer = await _answerRepository.FindAsync(new ObjectId(context.Message.Id));

            if (answer is null)
            {
                await context.RespondAsync(new QueryResponse<EduQuizResultDto>(new EduExceptionMessage("Answer not found", StatusCodes.Status404NotFound)));
                return;
            }

            var result = await _answerRepository.UpdateAsync(_mapper.Map<EduQuiz>(context.Message));
            await context.RespondAsync(new QueryResponse<EduQuizResultDto>(_mapper.Map<EduQuizResultDto>(result)));
        }
    }
}
