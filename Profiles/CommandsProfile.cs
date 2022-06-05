namespace SixMinApi.Profiles
{
    using AutoMapper;
    using SixMinApi.Dtos;
    using SixMinApi.Models;

    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            // Source -> Target
            CreateMap<Command, CommandReadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<CommandUpdateDto, Command>();
            CreateMap<CommandReadDto, Command>();
        }
    }
}
