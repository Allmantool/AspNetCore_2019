using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Northwind.BusinessLogicServices.Interfaces.Exceptions
{
    public class UpdateOperationException : Exception
    {
        public UpdateOperationException()
        {
        }

        public UpdateOperationException(string message) : base(message)
        {
        }

        public UpdateOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UpdateOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
