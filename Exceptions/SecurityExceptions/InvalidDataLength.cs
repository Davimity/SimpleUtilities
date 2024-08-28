using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleUtilities.Exceptions.SecurityExceptions
{
    internal class InvalidDataLength : SecurityException{
        public InvalidDataLength(string message) : base(message){}
    }
}
