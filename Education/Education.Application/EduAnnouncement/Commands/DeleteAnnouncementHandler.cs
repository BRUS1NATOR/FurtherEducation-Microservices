using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Domain.Announcement;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace Education.Application.Announcements.Commands
{
    public class DeleteAnnouncementHandler : IConsumer<MongoDeleteCommand<EduAnnouncement>>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IMapper _mapper;

        public DeleteAnnouncementHandler(IAnnouncementRepository announcementRepository, IMapper mapper)
        {
            _announcementRepository = announcementRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<MongoDeleteCommand<EduAnnouncement>> context)
        {
            var success = await _announcementRepository.DeleteAsync(context.Message.Id);

            if (!success)
            {
                await context.RespondAsync(new CommandResponse(new EduExceptionMessage("Announcement not found", StatusCodes.Status404NotFound)));
                return;
            }

            await context.RespondAsync(new CommandResponse());
        }
    }
}
