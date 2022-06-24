using FurtherEducation.Common.CQRS.Queries;

namespace User.Application.User.Queries
{
    public class GetUserAttributesByIdQuery : IQuery
    {
        public Guid UserId { get; set; }
    }
}