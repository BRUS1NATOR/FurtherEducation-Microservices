using FurtherEducation.Common;
using FurtherEducation.Common.CQRS.Commands;

namespace Education.Application.EduCourses.Commands
{
    public class SignUpUserToCourseCommand : ICommand
    {
        public Guid UserId { get; set; }
        [MongoObjectId]
        public string CourseId { get; set; }

        public SignUpUserToCourseCommand()
        {

        }

        public SignUpUserToCourseCommand(Guid UserId, string CourseId)
        {
            this.UserId = UserId;
            this.CourseId = CourseId;
        }
    }
}
