using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace FurtherEducation.Common.Models
{
    [Serializable]
    public class PagedList<T> //: IEnumerable<T> JSON serialization bug?
    {
        [JsonInclude]
        public int Page { get; set; }
        [JsonInclude]
        public int Size { get; set; }
        [JsonInclude]
        public long Total { get; set; }

        public IEnumerable<T> Value { get; set; }

        public PagedList()
        {

        }
        public PagedList(IEnumerable<T> data, int pageNow, int pageSize)
        {
            Value = data;
            Page = pageNow;
            Size = pageSize;
            Total = data.Count();
        }
        public PagedList(IEnumerable<T> data, int pageNow, int pageSize, long total)
        {
            Value = data;
            Page = pageNow;
            Size = pageSize;
            Total = total;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Value.GetEnumerator();
        }
    }

    public class PagedListConverter<TSource, TDestination> : ITypeConverter<PagedList<TSource>, PagedList<TDestination>> where TSource : class where TDestination : class
    {
        public PagedList<TDestination> Convert(PagedList<TSource> source, PagedList<TDestination> destination, ResolutionContext context)
        {
            var collection = context.Mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(source.Value);

            return new PagedList<TDestination>(collection, source.Page, source.Size, source.Total);
        }
    }
}
