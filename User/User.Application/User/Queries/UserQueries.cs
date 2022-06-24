using FurtherEducation.Common.CQRS.Queries;

namespace User.Application.User.Queries
{
    public class GetUsersQuery : IQuery
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class GetUserByNameQuery : IQuery
    {
        public string Username;
    }

    public class GetUserByIdQuery : IQuery
    {
        public Guid Id { get; set; }
    }
}
