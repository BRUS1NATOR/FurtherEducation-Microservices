using AutoMapper;
using FurtherEducation.Common.Commands;
using FurtherEducation.SharedModels.Commands;
using MassTransit;
using Profile.Domain.Diary;
using Profile.Domain.Diary.Repository;
using System.Text.Json;

namespace Profile.Application.Profile.Commands
{
    public class StudentGaveAnswerHandler : IConsumer<StudentGaveAnswer>
    {
        private readonly IStudentDiaryRepository _diaryRepository;
        private readonly IDiaryRecordRepository _diaryRecordRepository;
        private readonly IMapper _mapper;

        public StudentGaveAnswerHandler(IStudentDiaryRepository diaryRepository, IDiaryRecordRepository diaryRecordRepository, IMapper mapper)
        {
            _diaryRepository = diaryRepository;
            _diaryRecordRepository = diaryRecordRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<StudentGaveAnswer> context)
        {
            var jsonMessage = JsonSerializer.Serialize(context.Message);
            Console.WriteLine($"Student gave answer to task: {jsonMessage}");

            var diary = await _diaryRepository.FindOneAsync(x => x.UserProfile.UserId == context.Message.UserId && x.CourseId == context.Message.CourseId);
            if (diary is null)
            {
                await context.RespondAsync(new CommandResponse("Diary doesnt exists", CommandStatus.Failed));
                return;
            }

            var diaryRecordEntity = await _diaryRecordRepository.FindOneAsync(x => x.UserId == context.Message.UserId && x.TaskId == context.Message.TaskId);
            if (diaryRecordEntity is null)
            {
                diaryRecordEntity = new DiaryRecordEntity
                {
                    Diary = diary,
                    UserId = context.Message.UserId,
                    CourseId = context.Message.CourseId,
                    ModuleId = context.Message.ModuleId,
                    TaskId = context.Message.TaskId
                };
            }

            diaryRecordEntity.CourseId = context.Message.CourseId;
            diaryRecordEntity.AnswerGivenAt = context.Message.AnswerGivenAt;
            diaryRecordEntity.Score = context.Message.Score;

            await _diaryRecordRepository.CreateAsync(diaryRecordEntity);

            await context.RespondAsync(new CommandResponse());
        }
    }
}
