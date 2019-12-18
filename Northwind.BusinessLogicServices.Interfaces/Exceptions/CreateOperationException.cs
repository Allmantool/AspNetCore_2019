using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Northwind.BusinessLogicServices.Interfaces.Exceptions
{
    public class CreateOperationException : Exception
    {
        public CreateOperationException()
        {
        }

        public CreateOperationException(string message) : base(message)
        {
        }

        public CreateOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CreateOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
