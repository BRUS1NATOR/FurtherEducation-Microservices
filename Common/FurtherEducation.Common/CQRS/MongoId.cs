using FurtherEducation.Common.Helpers;
using MongoDB.Bson;

namespace FurtherEducation.Common.CQRS
{
    public class MongoId
    {
        public ObjectId Id;
        public MongoId()
        {
        }
        public MongoId(string id)
        {
            Id = MongoHelper.Parse(id);
        }
        public MongoId(ObjectId id)
        {
            Id = id;
        }
    }
}
