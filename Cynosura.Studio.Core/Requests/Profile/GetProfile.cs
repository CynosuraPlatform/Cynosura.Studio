using Cynosura.Studio.Core.Requests.Profile.Models;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Profile
{
    public class GetProfile : IRequest<ProfileModel>
    {
    }
}
