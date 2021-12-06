using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Application.ThirdPartyService
{
    [JsonObject]
    public class FunTranslationRequest
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
