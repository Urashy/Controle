using Api.Models;
using Api.Models.DTO;
using AutoMapper;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace Api.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Animal, AnimalDTO>()
                .ReverseMap();
        }
    }
}
