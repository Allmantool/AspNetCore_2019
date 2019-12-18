using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Northwind.BusinessLogicServices.Interfaces.Exceptions
{
    public class GetOperationException : Exception
    {
        public GetOperationException()
        {
        }

        protected GetOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public GetOperationException(string message) : base(message)
        {
        }

        public GetOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
