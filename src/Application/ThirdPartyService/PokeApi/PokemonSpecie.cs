using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Application.ThirdPartyService
{
    public class PokemonSpecie
    {
        public class NameUrlItem
        {
            public string Name { get; set; }

            public string Url { get; set; }
        }


        public class NameItem
        {
            public string Name { get; set; }

            public NameUrlItem Language { get; set; }
        }

        public class FlavorTextEntry
        {
            [JsonProperty("flavor_text")]
            public string FlavorText { get; set; }

            public NameUrlItem Language { get; set; }

            public NameUrlItem Version { get; set; }
        }

        public int Id { get; set; }

        public string Name { get; set; }

        [JsonProperty("is_legendary")]
        public bool IsLegendary { get; set; }


        public NameUrlItem Habitat { get; set; }


        public NameUrlItem Color { get; set; }

        public NameUrlItem Shape { get; set; }


        public NameUrlItem[] Names { get; set; }

        [JsonProperty("flavor_text_entries")]
        public FlavorTextEntry[] FlavorTextEntries { get; set; }

    }
}
