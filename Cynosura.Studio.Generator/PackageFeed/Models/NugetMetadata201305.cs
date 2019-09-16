using System.Runtime.Serialization;

namespace Cynosura.Studio.Generator.PackageFeed.Models
{
    [DataContract(Name = "metadata", Namespace = "http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd")]
    public class NugetMetadata201305 : IExtensibleDataObject
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "version")]
        public string Version { get; set; }
        
        public ExtensionDataObject ExtensionData { get; set; }
    }
}