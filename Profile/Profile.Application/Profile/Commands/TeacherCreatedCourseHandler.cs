using AutoMapper;
using FurtherEducation.Common.Commands;
using FurtherEducation.SharedModels.Commands;
using MassTransit;
using Profile.Domain.Diary;
using Profile.Domain.Diary.Repository;
using Profile.Domain.Profile;
using System.Text.Json;
using User.Domain.Profile.Repository;

namespace Profile.Application.Profile.Commands
{
    public class TeacherCreatedCourseHandler : IConsumer<TeacherCreatedCourse>
    {
        private readonly IProfileRepository _profileRepository;
        private readonly ITeacherDiaryRepository _diaryRepository;
        private readonly IProfileService _profileService;

        public TeacherCreatedCourseHandler(IProfileRepository profileRepository, ITeacherDiaryRepository diaryRepository, IProfileService profileService)
        {
            _profileRepository = profileRepository;
            _diaryRepository = diaryRepository;
            _profileService = profileService;
        }

        public async Task Consume(ConsumeContext<TeacherCreatedCourse> context)
        {
            var jsonMessage = JsonSerializer.Serialize(context.Message);
            Console.WriteLine($"Teacher created course: {jsonMessage}");

            var profile = await _profileService.GetOrCreateProfile(context.Message.UserId);
            if (profile is null)
            {
                await context.RespondAsync(new CommandResponse("Could create profile"));
                return;
            }

            var diary = await _diaryRepository.FindOneAsync(x => x.UserProfile.UserId == context.Message.UserId && x.CourseId == context.Message.CourseId);
            if (diary is not null)
            {
                await context.RespondAsync(new CommandResponse("Diary already exists"));
                return;
            }

            diary = new TeacherDiaryEntity
            {
                UserProfile = profile,
                UserId = context.Message.UserId,
                CourseId = context.Message.CourseId,
                CourseName = context.Message.CourseName,
                CreatedAt = DateTime.UtcNow,
                Status = TeacherDiaryStatus.Active
            };
            await _diaryRepository.CreateAsync(diary);

            await context.RespondAsync(new CommandResponse());
        }
    }
}
