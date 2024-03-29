﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cynosura.Studio.Core.Enums;

namespace Cynosura.Studio.Core.Requests.FileGroups.Models
{
    public class FileGroupModel
    {
        public FileGroupModel(string name, FileGroupType type)
        {
            Name = name;
            Type = type;
        }

        [DisplayName("Id")]
        public int Id { get; set; }

        [DisplayName("Creation Date")]
        public DateTime CreationDate { get; set; }

        [DisplayName("Modification Date")]
        public DateTime ModificationDate { get; set; }

        [DisplayName("Creation User")]
        public int? CreationUserId { get; set; }
        public Users.Models.UserShortModel? CreationUser { get; set; }

        [DisplayName("Modification User")]
        public int? ModificationUserId { get; set; }
        public Users.Models.UserShortModel? ModificationUser { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Type")]
        public Core.Enums.FileGroupType Type { get; set; }

        [DisplayName("Location")]
        public string? Location { get; set; }

        [DisplayName("Accept")]
        public string? Accept { get; set; }
    }
}
