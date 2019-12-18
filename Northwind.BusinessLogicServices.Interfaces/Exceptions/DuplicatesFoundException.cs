using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Northwind.BusinessLogicServices.Interfaces.Exceptions
{
    public class DuplicatesFoundException : Exception
    {
        public DuplicatesFoundException()
        {
        }

        public DuplicatesFoundException(string message) : base(message)
        {
        }

        public DuplicatesFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DuplicatesFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
