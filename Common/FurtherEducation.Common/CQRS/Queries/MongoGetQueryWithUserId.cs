using MongoDB.Bson;
using System;

namespace FurtherEducation.Common.CQRS.Queries
{
    public class MongoGetQueryWithUserId<T> : MongoId, IQuery
    {
        public Guid UserId { get; set; }

        public MongoGetQueryWithUserId() : base()
        {
        }
        public MongoGetQueryWithUserId(ObjectId id) : base(id)
        {
        }

        public MongoGetQueryWithUserId(string id) : base(id)
        {
        }

        public MongoGetQueryWithUserId(ObjectId id, Guid userId) : base(id)
        {
            UserId = userId;
        }

        public MongoGetQueryWithUserId(string id, Guid userId) : base(id)
        {
            UserId = userId;
        }
    }
}
