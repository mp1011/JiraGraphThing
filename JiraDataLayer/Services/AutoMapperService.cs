using AutoMapper;
using JiraDataLayer.Models;
using JiraDataLayer.Models.DTO;
using System;
using System.Linq;

namespace JiraDataLayer.Services
{
    class AutoMapperService
    {
        private readonly IMapper _mapper;

        public AutoMapperService()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<JiraIssue, JiraIssueDTO>();
                cfg.CreateMap<JiraIssueDTO, JiraIssue>();
            });

            _mapper = config.CreateMapper();
        }

        public Type GetMappedType<T>()
        {
            return _mapper.ConfigurationProvider
                .GetAllTypeMaps()
                .First(p => p.SourceType == typeof(T))
                .DestinationType;
        }

        public object Map<T>(T input)
        {
            var destination = GetMappedType<T>();
            return _mapper.Map(input, typeof(T), destination);
        }

        public T MapTo<T>(object input)
        {
            return _mapper.Map<T>(input);
        }
    }
}
