using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVerenigingLibary.Exceptions
{
    public class IncorrectRightsExeption : Exception
    {
        public override string Message => ("U heeft geen rechten om deze actie uit te voeren");
    }
}