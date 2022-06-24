using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Domain.Announcement;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace Education.Application.Announcements.Commands
{
    public class UpdateAnnouncementHandler : IConsumer<UpdateAnnouncementCommand>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IMapper _mapper;

        public UpdateAnnouncementHandler(IAnnouncementRepository announcementRepository, IMapper mapper)
        {
            _announcementRepository = announcementRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<UpdateAnnouncementCommand> context)
        {
            var result = await _announcementRepository.UpdateAsync(_mapper.Map<EduAnnouncement>(context.Message));

            if (result is false)
            {
                await context.RespondAsync(new CommandResponse(new EduExceptionMessage("Announcement not found", StatusCodes.Status404NotFound)));
                return;
            }

            await context.RespondAsync(new CommandResponse(context.Message.Id));
        }
    }
}
