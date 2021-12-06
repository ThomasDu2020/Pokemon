using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Application.Interface;
using Application.ThirdPartyService.Interface;

namespace Application.Translator
{
    public class YodaTranslator : ITranslator
    {
        private readonly IFunTranslation _funtranslation;

        const string type = "yoda";

        public async Task<string> TranslateAsync(string text, CancellationToken token)
        {
            return await _funtranslation.TranslateAsync(type, text, token);
        }

        public YodaTranslator(IFunTranslation funtranlation)
        {
            _funtranslation = funtranlation;
        }
    }
}
