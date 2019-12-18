using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Northwind.BusinessLogicServices.Interfaces.Exceptions
{
    public class IdNotFoundException : Exception
    {
        public IdNotFoundException()
        {
        }

        public IdNotFoundException(string message) : base(message)
        {
        }

        public IdNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IdNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
