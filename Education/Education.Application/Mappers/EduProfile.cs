using AutoMapper;
using FurtherEducation.Common.Models;
using MongoDB.Bson;

namespace Education.Application.Mappers
{
    public class EduProfile : Profile
    {
        public EduProfile()
        {
            CreateMap(typeof(PagedList<>), typeof(PagedList<>)).ConvertUsing(typeof(PagedListConverter<,>));

            CreateMap<List<ObjectId>, List<string>>().ConvertUsing(o => o.Select(os => os.ToString()).ToList());
            CreateMap<List<string>, List<ObjectId>>().ConvertUsing(o => o.Select(os => ObjectId.Parse(os)).ToList());

            Func<string, ObjectId> stringToObjectId = s =>
            {
                ObjectId result = ObjectId.Empty;
                ObjectId.TryParse(s, out result);
                return result;
            };

            CreateMap<ObjectId, string>().ConvertUsing(o => o.ToString());
            CreateMap<string, ObjectId>().ConvertUsing(o => stringToObjectId(o));

            CreateMap<ObjectId, DateTime>().ConvertUsing(o => o.CreationTime);
        }
    }
}
