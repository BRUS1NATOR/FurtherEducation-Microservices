using FurtherEducation.Common.CQRS.Queries;
using FurtherEducation.Common.Helpers;
using MongoDB.Bson;


namespace FurtherEducation.Common.Queries
{
    public class MongoGetPagedQuery<T> : IQuery
    {
        public ObjectId ParentId;
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }

        public MongoGetPagedQuery()
        {
         
        }


        public MongoGetPagedQuery(int page, int pageSize, string search = "")
        {
            Page = page;
            PageSize = pageSize;
            Search = search;
        }

        public MongoGetPagedQuery(ObjectId parentId, int page, int pageSize, string search="")
        {
            ParentId = parentId;
            Page = page;
            PageSize = pageSize;
            Search = search;
        }

        public MongoGetPagedQuery(string parentId, int page, int pageSize, string search = "")
        {
            ParentId = MongoHelper.Parse(parentId);
            Page = page;
            PageSize = pageSize;
            Search = search;
        }
    }
}
