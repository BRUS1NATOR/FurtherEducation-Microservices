using FurtherEducation.Common.CQRS.Queries;
using FurtherEducation.Common.Helpers;
using MongoDB.Bson;

namespace Education.Application.EduQuizes.Queries
{
    public class GetQuizQuery : IQuery
    {
        public ObjectId? Id { get; set; }
        public ObjectId? TestId { get; set; }
        public Guid? UserId { get; set; }

        public GetQuizQuery(string id)
        {
            Id = MongoHelper.Parse(id);
        }

        public GetQuizQuery(ObjectId id)
        {
            Id = id;
        }

        public GetQuizQuery(ObjectId testId, Guid userId)
        {
            TestId = testId;
            UserId = userId;
        }

    }
}
