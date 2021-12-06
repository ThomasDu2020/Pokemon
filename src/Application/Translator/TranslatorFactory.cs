using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Application.Interface;
using Application.ThirdPartyService.Interface;

namespace Application.Translator
{
    public class TranslatorFactory : ITranslatorFactory
    {
        private readonly IFunTranslation _funTranslation;

        public ITranslator GetITranslator(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentNullException("translator type shouldn't be null or empty.");
            }

            switch (type.Trim().ToLower())
            {
                case "shakespeare":
                    return new ShakespeareTranslator(_funTranslation);

                case "yoda":
                    return new YodaTranslator(_funTranslation);

                default:
                    throw new NotSupportedException($"Not support translator {type}.");
            }
        }

        public TranslatorFactory(IFunTranslation funtranlation)
        {
            _funTranslation = funtranlation;
        }
    }
}
