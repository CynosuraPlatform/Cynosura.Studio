using System.Runtime.Serialization;

namespace Cynosura.Studio.Generator.PackageFeed.Models
{
    [DataContract(Name = "package", Namespace = "http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd")]
    public class NugetPackage201305 : IExtensibleDataObject
    {
        [DataMember(Name = "metadata")]
        public NugetMetadata201305 Metadata { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}