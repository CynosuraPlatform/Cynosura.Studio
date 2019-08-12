using System.Runtime.Serialization;

namespace Cynosura.Studio.Generator.PackageFeed.Models
{
    [DataContract(Name = "package", Namespace = "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd")]
    public class NugetPackage201007 : IExtensibleDataObject
    {
        [DataMember(Name = "metadata")]
        public NugetMetadata201007 Metadata { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}
