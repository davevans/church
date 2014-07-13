using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;

namespace Church.Common.Mapping
{
    public static class Mapper
    {
        public static MappingExpressionProxy<TSource, TDestination> CreateMap<TSource, TDestination>()
        {
            return new MappingExpressionProxy<TSource, TDestination>(AutoMapper.Mapper.CreateMap<TSource, TDestination>());
        }

        public static IEnumerable<TDestination> MapList<TSource, TDestination>(IEnumerable<TSource> source)
        {
            return source.Select(Map<TSource, TDestination>);
        }

        public static TDestination Map<TSource, TDestination>(TSource source)
        {
            return AutoMapper.Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return AutoMapper.Mapper.Map(source, destination);
        }

        public static object Map(object source, Type sourceType, Type destinationType)
        {
            return AutoMapper.Mapper.Map(sourceType, sourceType, destinationType);
        }

        public static object Map(object source, object destination, Type sourceType, Type destinationType)
        {
            return AutoMapper.Mapper.Map(source, destination, sourceType, destinationType);
        }
    }

    public class MappingExpressionProxy<TSource, TDestination>
    {
        private readonly IMappingExpression<TSource, TDestination> _inner;

        public MappingExpressionProxy(IMappingExpression<TSource, TDestination> inner)
        {
            _inner = inner;
        }

        public IMappingExpression<TSource, TDestination> Include<TOtherSource, TOtherDestination>()
            where TOtherSource : TSource
            where TOtherDestination : TDestination
        {
            return _inner.Include<TOtherSource, TOtherDestination>();
        }

        public IMappingExpression<TSource, TDestination> ForMember(Expression<Func<TDestination, object>> destinationMember,
                                                                   Action<IMemberConfigurationExpression<TSource>> memberOptions)
        {
            return _inner.ForMember(destinationMember, memberOptions);
        }

        public void ConvertUsing(Func<TSource, TDestination> mapper)
        {
            _inner.ConvertUsing(mapper);
        }

        public void AfterMap(Action<TSource, TDestination> afterFunction)
        {
            _inner.AfterMap(afterFunction);
        }
    }
}
