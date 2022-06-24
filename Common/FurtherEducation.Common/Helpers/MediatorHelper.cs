using FurtherEducation.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using System;

namespace FurtherEducation.Common.Helpers
{
    public class MediatorHelper
    {
        public static void IsMessageConsumptionFailedOrNotFound(EduMessage? message)
        {
            if (message is EduExceptionMessage exceptionMessage)
            {
                throw new EduException(exceptionMessage);
            }
        }

        public static void IsMessageConsumptionFailed(EduMessage? message)
        {
            if (message is EduExceptionMessage exceptionMessage)
            {
                if (exceptionMessage.Code != StatusCodes.Status404NotFound)
                {
                    throw new EduException((EduExceptionMessage)message);
                }
            }
        }

        public static Guid ParseGuid(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                throw new EduException(new EduExceptionMessage(StatusCodes.Status400BadRequest, "Guid is incorrect"));
            }
            return guid;
        }
    }
}
