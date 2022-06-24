using AutoMapper;
using Education.Application.Data.Repositories;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using Education.Domain.Announcement;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace Education.Application.Announcements.Commands
{
    public class CreateAnnouncementHandler : IConsumer<CreateAnnouncementCommand>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateAnnouncementHandler> _logger;

        public CreateAnnouncementHandler(IAnnouncementRepository announcementRepository, ICourseRepository courseRepository,
            IModuleRepository moduleRepository, IMapper mapper,
            ILogger<CreateAnnouncementHandler> logger)
        {
            _announcementRepository = announcementRepository;

            _courseRepository = courseRepository;
            _moduleRepository = moduleRepository;

            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CreateAnnouncementCommand> context)
        {
            var courseId = new ObjectId(context.Message.CourseId);


            var announcement = _mapper.Map<EduAnnouncement>(context.Message);
            announcement.Id = ObjectId.GenerateNewId(DateTime.Now);

            var course = await _courseRepository.FindAsync(courseId);
            if (course != null)
            {
                announcement.CourseId = courseId;
            }
            else
            {
                await context.RespondAsync(new CommandResponse(new EduExceptionMessage("Course with requested id not found", StatusCodes.Status404NotFound)));
                return;
            }

            if (!string.IsNullOrEmpty(context.Message.ModuleId))
            {
                var moduleId = new ObjectId(context.Message.ModuleId);

                var module = await _moduleRepository.FindAsync(moduleId);
                if (module != null && module.CourseId == module.CourseId)
                {
                    announcement.ModuleId = courseId;
                }
                else
                {
                    await context.RespondAsync(new CommandResponse(new EduExceptionMessage("Module with requested id not found", StatusCodes.Status404NotFound)));
                    return;
                }
            }


            var result = await _announcementRepository.Create(announcement);
            await context.RespondAsync(new CommandResponse(announcement.Id.ToString()));
            return;

        }
    }
}
