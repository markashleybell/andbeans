using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace andbeans.Models.Bing
{
    public class BingSearchResult
    {
        public string MediaUrl { get; set; }
        public string ContentType { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int FileSize { get; set; }
    }
}