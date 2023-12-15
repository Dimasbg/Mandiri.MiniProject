using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mandiri.MiniProject.Utilities.Base
{
    public class ApiBadRequestException : Exception
    {
        public ApiBadRequestException()
        {
        }

        public ApiBadRequestException(string? message) : base(message)
        {
        }

        public ApiBadRequestException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ApiBadRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
    public class ApiCallException : Exception
    {
        public ApiCallException()
        {
        }

        public ApiCallException(string? message) : base(message)
        {
        }

        public ApiCallException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ApiCallException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class DomainLayerException : Exception
    {
        public DomainLayerException()
        {
        }

        public DomainLayerException(string? message) : base(message)
        {
        }

        public DomainLayerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DomainLayerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class DataNotFoundException : Exception
    {
        public DataNotFoundException()
        {
        }

        public DataNotFoundException(string? message) : base(message)
        {
        }

        public DataNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DataNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
