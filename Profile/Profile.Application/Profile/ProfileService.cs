using FurtherEducation.Common.Commands;
using FurtherEducation.Common.Exceptions;
using FurtherEducation.SharedModels.Commands;
using MassTransit;
using Profile.Domain.Profile;
using User.Domain.Profile.Repository;

namespace Profile.Application.Profile
{
    public interface IProfileService
    {
        public Task<ProfileEntity?> GetOrCreateProfile(Guid UserId);
        public Task<ProfileEntity?> CreateProfile(Guid UserId);
    }

    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IBus _bus;

        public ProfileService(IProfileRepository profileRepository, IBus bus)
        {
            _profileRepository = profileRepository;
            _bus = bus;
        }

        public async Task<ProfileEntity?> GetOrCreateProfile(Guid UserId)
        {
            var profile = await _profileRepository.FindByIdAsync(UserId);

            if(profile is null)
            {
                profile = await CreateProfile(UserId);
            }

            return profile;
        }

        public async Task<ProfileEntity?> CreateProfile(Guid UserId)
        {
            var response = await _bus.Request<GetUserFullNameRequest, QueryResponse<UserFullNameDto>>(new(UserId));

            if (response.Message.Message is EduExceptionMessage)
            {
                return null;
            }

            var profile = new ProfileEntity();
            profile.UserId = UserId;
            profile.FirstName = response.Message.Data.FirstName;
            profile.LastName = response.Message.Data.LastName;

            return await _profileRepository.CreateAsync(profile);
        }

    }
}
