using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface ITranslator
    {
        Task<string> TranslateAsync(string text, CancellationToken token);
    }
}
