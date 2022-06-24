using AutoMapper;
using Education.Application.Data.Repositories;
using Education.Application.EduModules.Dto;
using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using FurtherEducation.Common.Queries;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace Education.Application.EduModules.Queries
{
    public class GetModuleHandler : IConsumer<MongoGetQuery<ModuleDto>>
    {
        private readonly IModuleRepository _courseRepository;
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IMapper _mapper;

        public GetModuleHandler(IModuleRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<MongoGetQuery<ModuleDto>> context)
        {
            var module = await _courseRepository.FindDetailedAsync(context.Message.Id);

            if (module is null)
            {
                await context.RespondAsync(new QueryResponse<ModuleDto>(new EduExceptionMessage("Module not found", StatusCodes.Status404NotFound)));
                return;
            }

            var moduleDto = _mapper.Map<ModuleDto>(module);

            await context.RespondAsync(new QueryResponse<ModuleDto>(moduleDto));
        }
    }
}
