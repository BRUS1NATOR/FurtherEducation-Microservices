using AutoMapper;
using Education.Application.Announcements.Dto;
using Education.Application.Data.Repositories;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using FurtherEducation.Common.Queries;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace Education.Application.Announcements.Queries
{
    public class GetAnnouncementHandler : IConsumer<MongoGetQuery<AnnouncementDto>>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IMapper _mapper;

        public GetAnnouncementHandler(IAnnouncementRepository announcementRepository, IMapper mapper)
        {
            _announcementRepository = announcementRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<MongoGetQuery<AnnouncementDto>> context)
        {
            var announcement = await _announcementRepository.FindAsync(context.Message.Id);

            if (announcement is null)
            {
                await context.RespondAsync(new QueryResponse<AnnouncementDto>(new EduExceptionMessage("Announcement not found", StatusCodes.Status404NotFound)));
                return;
            }

            await context.RespondAsync(new QueryResponse<AnnouncementDto>(_mapper.Map<AnnouncementDto>(announcement)));
        }
    }
}
