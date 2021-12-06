using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using Application.ThirdPartyService.Interface;

namespace Application.ThirdPartyService
{
    public class FunTranslations : IFunTranslation
    {
        const string Url = @"https://api.funtranslations.com/translate/";

        private readonly HttpClient _httpClient;

        private readonly ILogger<PokeApi> _logger;

        public async Task<string> TranslateAsync(string type, string text, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException("type is null or empty.");
            }

            type = type.Trim().ToLower();

            if (type != "yoda" && type != "shakespeare")
            {
                throw new ArgumentException("Unsupported translation type, only support shakespeare and yoda.");
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("words is null or empty.");
            }

            text = text.Trim();

            var url = Url + type;

            string result = null;
            try
            {
                var request = new FunTranslationRequest()
                {
                    Text = text
                };

                var jsonContent = JsonConvert.SerializeObject(request);

                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, stringContent, token);

                var strResult = await response.Content.ReadAsStringAsync(token);

                var objResult = JsonConvert.DeserializeObject<FunTranslationResponse>(strResult);

                if (objResult != null && objResult.Success != null && objResult.Success.Total == 1)
                {
                    result = objResult.Contents.Translated;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Translate error.");
            }

            return result;
        }

        public FunTranslations(ILogger<PokeApi> logger, HttpClient client)
        {
            _httpClient = client;
            _logger = logger;
        }
    }
}

