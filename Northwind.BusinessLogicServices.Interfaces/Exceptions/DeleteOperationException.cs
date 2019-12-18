using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Northwind.BusinessLogicServices.Interfaces.Exceptions
{
    public class DeleteOperationException : Exception
    {
        public DeleteOperationException()
        {
        }

        protected DeleteOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DeleteOperationException(string message) : base(message)
        {
        }

        public DeleteOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
