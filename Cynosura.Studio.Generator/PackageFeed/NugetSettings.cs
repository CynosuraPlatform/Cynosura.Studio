using System;
using System.Collections.Generic;
using System.Text;

namespace Cynosura.Studio.Generator.PackageFeed
{
    public class NugetSettings
    {
        public string FeedUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public NugetListingApi ListingApi { get; set; } = NugetListingApi.SearchAutocompleteService;
        public string ListingApiVersion { get; set; }
    }
}
