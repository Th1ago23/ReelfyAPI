namespace Application.Extensions
{
    public static class MapperExtensions
    {
        public static IEnumerable<TDestination> MapTo<TSource, TDestination>(
            this IEnumerable<TSource> source,
            Func<TSource, TDestination> mapFunction)
        {
            if (source == null)
            {
                return Enumerable.Empty<TDestination>();
            }
            return source.Select(mapFunction);
        }
    }
}