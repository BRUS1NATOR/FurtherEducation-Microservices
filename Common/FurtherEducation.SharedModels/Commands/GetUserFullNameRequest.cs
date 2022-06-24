namespace FurtherEducation.SharedModels.Commands
{
    public class GetUserFullNameRequest
    {
        public Guid UserId { get; set; }

        public GetUserFullNameRequest()
        {

        }

        public GetUserFullNameRequest(Guid UserId)
        {
            this.UserId = UserId;
        }
    }

    public class UserFullNameDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

}
