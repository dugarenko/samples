using System;
using System.Collections.Generic;

#nullable disable

namespace WebAuthorizationKey.Areas.Api.Models
{
    public partial class Kraj
    {
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public string ISO2 { get; set; }
        public string ISO3 { get; set; }
    }
}
