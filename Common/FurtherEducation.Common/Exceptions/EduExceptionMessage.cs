using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace FurtherEducation.Common.Exceptions
{
    public class EduException : Exception
    {
        public EduExceptionMessage EduExceptionMessage { get; set; }

        public EduException() : base()
        {
            EduExceptionMessage = new EduExceptionMessage();
        }

        public EduException(EduExceptionMessage eduExceptionMessage)
        {
            EduExceptionMessage = eduExceptionMessage;
        }
    }

    public class EduMessage
    {
        public string Message { get; set; }
        public EduMessage()
        {
            Message = string.Empty;
        }
        public EduMessage(string message)
        {
            this.Message = message;
        }
    }

    public class EduExceptionMessage : EduMessage
    {
        public int Code { get; set; }

        public EduExceptionMessage()
        {

        }

        public EduExceptionMessage(string message, int code = StatusCodes.Status500InternalServerError)
        {
            Message = message;
            Code = code;
        }

        public EduExceptionMessage(int code = StatusCodes.Status500InternalServerError, string message = "")
        {
            Message = message;
            Code = code;
        }

        public virtual string GetJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
        }
    }
}
