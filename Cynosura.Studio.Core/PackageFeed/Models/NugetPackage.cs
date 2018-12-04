using System.Runtime.Serialization;

namespace Cynosura.Studio.Core.PackageFeed.Models
{
    [DataContract(Name="package", Namespace = "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd")]
    public class NugetPackage:IExtensibleDataObject
    {
        [DataMember(Name = "metadata")]
        public NugetMetadata Metadata { get; set; }
        
        public ExtensionDataObject ExtensionData { get; set; }
    }
}