using AutoMapper;
using Movie.Business.DTOs.GenreDTOs;
using Movie.Business.DTOs.MovieDTOs;
using Movie.Core.Models;

namespace Movie.Business.Mapper
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Genre, GenreCreateDTO>().ReverseMap();
            CreateMap<Genre, GenreUpdateDTO>().ReverseMap();

            CreateMap<Core.Models.Movie, MovieCreateDTO>().ReverseMap();
        }
    }
}
