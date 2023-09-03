using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cynosura.Studio.Generator;

namespace Cynosura.Studio.Core.Requests.Solutions.Models
{
    public class SolutionModel
    {
        public SolutionModel(string name, string path)
        {
            Name = name;
            Path = path;
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

        [DisplayName("Path")]
        public string Path { get; set; }

        public string TemplateName { get; set; }

        public string TemplateVersion { get; set; }

        public void LoadMetadata()
        {
            var accessor = new SolutionAccessor(Path);
            TemplateName = accessor.Metadata.TemplateName;
            TemplateVersion = accessor.Metadata.TemplateVersion;
        }
    }
}
