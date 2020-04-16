using AutoMapper;
using System.Reflection;

namespace DeliveryClub.Infrastructure.Mapping
{
    public class Mapper
    {
        private readonly IMapper _mapper;

        public Mapper(Assembly assembly)
        {
            _mapper = new MapperConfiguration(cfg => cfg.AddMaps(assembly)).CreateMapper();
        }

        public T2 Map<T1, T2>(T1 obj)
        {
            return _mapper.Map<T1, T2>(obj);
        }
    }
}
