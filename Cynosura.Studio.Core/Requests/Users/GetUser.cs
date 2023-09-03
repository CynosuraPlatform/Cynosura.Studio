using System;
using MediatR;
using Cynosura.Studio.Core.Requests.Users.Models;

namespace Cynosura.Studio.Core.Requests.Users
{
    public class GetUser : IRequest<UserModel?>
    {
        public int Id { get; set; }
    }
}
