﻿using System.Runtime.Serialization;

namespace Cynosura.Studio.Core.PackageFeed.Models
{
    [DataContract(Name = "metadata", Namespace = "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd")]
    public class NugetMetadata:IExtensibleDataObject
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "version")]
        public string Version { get; set; }
        
        public ExtensionDataObject ExtensionData { get; set; }
    }
}