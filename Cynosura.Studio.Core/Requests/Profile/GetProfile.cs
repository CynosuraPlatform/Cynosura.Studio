using MediatR;
using Cynosura.Studio.Core.Requests.Profile.Models;

namespace Cynosura.Studio.Core.Requests.Profile
{
    public class GetProfile : IRequest<ProfileModel>
    {
    }
}
