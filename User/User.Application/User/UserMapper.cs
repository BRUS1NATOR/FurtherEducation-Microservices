using AutoMapper;
using User.Domain.Dto;
using User.Domain.User;

namespace User.Application.User
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            // CreateUserRequest -> UserEntity
            //CreateMap<CreateUserCommand, KeycloakUserEntity>();

            // UserEntity -> UserResponse
            CreateMap<UserEntity, UserDto>();

            CreateMap<UserAttribute, AttributeDto>().ReverseMap();
        }
    }
}