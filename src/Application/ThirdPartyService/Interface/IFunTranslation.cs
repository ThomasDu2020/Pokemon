using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

namespace Application.ThirdPartyService.Interface
{
    public interface IFunTranslation
    {
        Task<string> TranslateAsync(string type, string text, CancellationToken token);
    }
}
