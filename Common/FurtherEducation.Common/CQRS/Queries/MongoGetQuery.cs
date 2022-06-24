using FurtherEducation.Common.CQRS;
using FurtherEducation.Common.CQRS.Queries;
using MongoDB.Bson;

namespace FurtherEducation.Common.Queries
{
    public class MongoGetQuery : MongoId, IQuery
    {
        public MongoGetQuery() : base()
        {
        }
        public MongoGetQuery(ObjectId id) : base(id)
        {
        }

        public MongoGetQuery(string id) : base(id)
        {
        }
    }

    public class MongoGetQuery<T> : MongoGetQuery
    {
        public MongoGetQuery() : base()
        {
        }
        public MongoGetQuery(ObjectId id) : base(id)
        {
        }

        public MongoGetQuery(string id) : base(id)
        {
        }
    }
}
