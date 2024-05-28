using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVerenigingLibrary.Exceptions
{
    public class NameEmptyExeception : Exception

    {
        public override string Message => "Dit naam mag niet leeg zijn!";
    }

}
