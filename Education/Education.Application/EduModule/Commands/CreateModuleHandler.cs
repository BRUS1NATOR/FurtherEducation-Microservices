using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Domain.EduCourses;
using Education.Domain.EduModules;
using FluentValidation;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace Education.Application.EduModules.Commands
{
    public class CreateModuleHandler : IConsumer<CreateModuleCommand>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IMapper _mapper;
        private readonly AbstractValidator<EduCourse> _validator;
        private readonly ILogger<CreateModuleHandler> _logger;

        public CreateModuleHandler(IModuleRepository moduleRepository, ICourseRepository courseRepository, IMapper mapper, AbstractValidator<EduCourse> validator,
            ILogger<CreateModuleHandler> logger)
        {
            _moduleRepository = moduleRepository;
            _courseRepository = courseRepository;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task Consume(ConsumeContext<CreateModuleCommand> context)
        {
            ObjectId parent = new ObjectId(context.Message.CourseId);

            var course = await _courseRepository.FindAsync(parent);
            if (course != null)
            {
                var module = _mapper.Map<EduModule>(context.Message);
                module.Id = ObjectId.GenerateNewId(DateTime.Now);

                var result = await _moduleRepository.Create(_mapper.Map<EduModule>(module));

                await context.RespondAsync(new CommandResponse(module.Id.ToString()));
                return;
            }

            //var module = await _moduleRepository.FindAsync(new ObjectId(context.Message.CourseOrModuleId));
            //if (module != null)
            //{
            //    var result = await _announcementRepository.CreateAnnouncement(module, _mapper.Map<Announcement>(parent));

            //    await context.RespondAsync(new BaseResponse<AnnouncementPreviewResponse>(_mapper.Map<AnnouncementPreviewResponse>(result)));
            //    return;
            //}

            await context.RespondAsync(new CommandResponse(new EduExceptionMessage("Course with requested id not found", StatusCodes.Status404NotFound)));
            return;
        }
    }
}
