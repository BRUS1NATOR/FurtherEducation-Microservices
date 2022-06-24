using FurtherEducation.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;

namespace FurtherEducation.Common.Helpers
{
    public static class MongoHelper
    {
        public static ObjectId Parse(string id)
        {
            ObjectId mongoId;

            if (!ObjectId.TryParse(id, out mongoId))
            {
                throw new EduException(new EduExceptionMessage(StatusCodes.Status400BadRequest, "ObjectId is incorrect"));
            }
            return mongoId;
        }
    }
}
