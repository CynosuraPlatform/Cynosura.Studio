using System;
using System.Collections.Generic;

namespace Cynosura.Studio.Core.Requests.Solutions.Models
{
    public class SolutionModel
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public int? CreationUserId { get; set; }
        public Users.Models.UserShortModel CreationUser { get; set; }
        public int? ModificationUserId { get; set; }
        public Users.Models.UserShortModel ModificationUser { get; set; }

        public string Name { get; set; }
        public string Path { get; set; }
    }
}
