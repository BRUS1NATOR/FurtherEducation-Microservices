using FurtherEducation.Common.Exceptions;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace FurtherEducation.Common.Commands
{
    public class QueryResponse<T> where T : class
    {
        public T? Data { get; set; }

        public EduMessage? Message { get; set; }

        public QueryResponse(EduMessage message)
        {
            this.Message = message;
        }

        public QueryResponse(T Response)
        {
            this.Data = Response;
        }

        public QueryResponse()
        {

        }
    }

    public enum CommandStatus { [EnumMember(Value = "Success")] Success, [EnumMember(Value = "Failed")] Failed }
    public class CommandResponse
    {
        [JsonInclude]
        public CommandStatus Status { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Id { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public EduMessage? Message { get; set; }

        public CommandResponse(CommandStatus Status = CommandStatus.Success)
        {
            this.Status = Status;
        }

        public CommandResponse(string Id, CommandStatus Status = CommandStatus.Success)
        {
            this.Status = Status;
            this.Id = Id;
        }

        public CommandResponse(EduMessage Message, CommandStatus Status = CommandStatus.Success)
        {
            this.Status = Status;
            this.Message = Message;

            if (Message is EduExceptionMessage)
            {
                this.Status = CommandStatus.Failed;
            }
        }

        public CommandResponse(EduMessage Message, string Id, CommandStatus Status = CommandStatus.Success)
        {
            this.Status = Status;
            this.Id = Id;
            this.Message = Message;

            if (Message is EduExceptionMessage)
            {
                Status = CommandStatus.Failed;
            }
        }
    }
}
