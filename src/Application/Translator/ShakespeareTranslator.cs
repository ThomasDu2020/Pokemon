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
    public class ShakespeareTranslator : ITranslator
    {
        private readonly IFunTranslation _funtranslation;

        const string type = "shakespeare";

        public async Task<string> TranslateAsync(string text, CancellationToken token)
        {
            return await _funtranslation.TranslateAsync(type, text, token);
        }

        public ShakespeareTranslator(IFunTranslation funtranlation)
        {
            _funtranslation = funtranlation;
        }
    }
}
