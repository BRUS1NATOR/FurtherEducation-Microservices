using FurtherEducation.Common.CQRS;
using FurtherEducation.Common.CQRS.Commands;
using MongoDB.Bson;

namespace FurtherEducation.Common.Commands
{
    public class MongoDeleteCommand : MongoId, ICommand
    {
        public MongoDeleteCommand() : base()
        {
        }
        public MongoDeleteCommand(ObjectId id) : base(id)
        {
        }

        public MongoDeleteCommand(string id) : base(id)
        {
        }
    }

    public class MongoDeleteCommand<T> : MongoDeleteCommand
    {
        public MongoDeleteCommand() : base()
        {
        }
        public MongoDeleteCommand(ObjectId id) : base(id)
        {
        }

        public MongoDeleteCommand(string id) : base(id)
        {
        }
    }
}
