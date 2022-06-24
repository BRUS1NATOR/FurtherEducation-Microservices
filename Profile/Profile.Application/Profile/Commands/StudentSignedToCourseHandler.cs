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
    public class StudentSignedToCourseHandler : IConsumer<StudentSignedToCourse>
    {
        private readonly IStudentDiaryRepository _diaryRepository;
        private readonly IProfileService _profileService;

        public StudentSignedToCourseHandler(IStudentDiaryRepository diaryRepository, IProfileService profileService)
        {
            _diaryRepository = diaryRepository;
            _profileService = profileService;
        }

        public async Task Consume(ConsumeContext<StudentSignedToCourse> context)
        {
            var jsonMessage = JsonSerializer.Serialize(context.Message);
            Console.WriteLine($"Student signed to course: {jsonMessage}");

            var profile = await _profileService.GetOrCreateProfile(context.Message.UserId);
            if (profile is null)
            {
                await context.RespondAsync(new CommandResponse("Could not create profile"));
                return;
            }

            var diary = await _diaryRepository.FindOneAsync(x => x.UserProfile.UserId == context.Message.UserId && x.CourseId == context.Message.CourseId);
            if (diary is not null)
            {
                await context.RespondAsync(new CommandResponse("Diary already exists"));
                return;
            }

            diary = new StudentDiaryEntity
            {
                UserProfile = profile,
                UserId = context.Message.UserId,
                CourseId = context.Message.CourseId,
                CourseName = context.Message.CourseName,
                StartedAt = DateTime.UtcNow,
                Status = StudentDiaryStatus.Active
            };
            await _diaryRepository.CreateAsync(diary);

            await context.RespondAsync(new CommandResponse());
        }
    }
}
