using AutoMapper;
using Education.Application.Announcements.Commands;
using Education.Application.Data.Repositories;
using Education.Domain.EduTests;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace Education.Application.EduTests.Commands
{
    public class CreateEduTestHandler : IConsumer<CreateEduTestCommand>
    {
        private readonly IEduTestRepository _TestRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateAnnouncementHandler> _logger;

        public CreateEduTestHandler(IModuleRepository moduleRepository, IEduTestRepository TestRepository, IMapper mapper,
            ILogger<CreateAnnouncementHandler> logger)
        {
            _TestRepository = TestRepository;
            _moduleRepository = moduleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CreateEduTestCommand> context)
        {
            var module = await _moduleRepository.FindAsync(new ObjectId(context.Message.ModuleId));
            if (module == null)
            {
                await context.RespondAsync(new CommandResponse(new EduExceptionMessage("Parent module not found", StatusCodes.Status404NotFound)));
                return;
            }

            var entity = _mapper.Map<EduTest>(context.Message);
            entity.CourseId = module.CourseId;
            entity.Id = ObjectId.GenerateNewId(DateTime.UtcNow);
            entity.TestSettings = new EduTestSettings()
            {
                QuestionsAmount = 0,
                ShowScoreOnFinish = true,
                OneTry = false,
                CalculateScoreOnFinish = true,
                TimeToSolve = 600
            };

            var test = await _TestRepository.Create(entity);

            await context.RespondAsync(new CommandResponse(entity.Id.ToString()));
        }
    }
}
