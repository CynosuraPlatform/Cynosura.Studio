using System;
using System.Collections.Generic;

namespace Cynosura.Studio.Core.Requests.Users.Models
{
    public class UserShortModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
