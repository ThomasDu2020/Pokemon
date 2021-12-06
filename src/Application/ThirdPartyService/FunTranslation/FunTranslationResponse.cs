using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ThirdPartyService
{
    public class FunTranslationResponse
    {
        public class SuccessItem
        {
            public int Total { get; set; }
        }

        public class ContentItem
        {
            public string Translated { get; set; }

            public string Text { get; set; }

            public string Translation { get; set; }
                 
        }

        public SuccessItem Success { get; set; }

        public ContentItem Contents { get; set; }
    }
}
