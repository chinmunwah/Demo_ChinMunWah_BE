using AutoMapper;
using Demo_ChinMunWah.Domain;
using Demo_ChinMunWah.Model;

namespace Demo_ChinMunWah.AutoMapper
{
    public class AutoMapperRegistrar : Profile
    {
        public AutoMapperRegistrar() 
        {
            CreateMap<UpsertProductModel, Product>();
        }
    }
}
