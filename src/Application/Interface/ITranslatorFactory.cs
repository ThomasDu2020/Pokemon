using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Interface;

namespace Application.Interface
{
    public interface ITranslatorFactory
    {
        ITranslator GetITranslator(string type);
    }
}
