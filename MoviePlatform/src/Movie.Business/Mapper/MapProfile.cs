using AutoMapper;
using Movie.Business.DTOs.ComentDTOs;
using Movie.Business.DTOs.GenreDTOs;
using Movie.Business.DTOs.LiveDTOs;
using Movie.Business.DTOs.MovieDTOs;
using Movie.Business.DTOs.SettingDTOs;
using Movie.Business.DTOs.UserDTOs;
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
            CreateMap<Core.Models.Movie, MovieUpdateDTO>().ReverseMap();

            CreateMap<AppUser, UserUpdateDTO>().ReverseMap();
            CreateMap<AppUser, UserEditDTO>().ReverseMap();

            CreateMap<Comment, CommentUpdateDTO>().ReverseMap();

            CreateMap<Live, LiveCreateDTO>().ReverseMap();
            CreateMap<Live, LiveUpdateDTO>().ReverseMap();

            CreateMap<Setting, SettingUpdateDTO>().ReverseMap();
        }
    }
}
