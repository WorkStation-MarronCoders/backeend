using System;

namespace workstation_backend.OfficesContext.Domain.Models.Exceptions;

public class NotServicesFoundException : Exception
{
        public NotServicesFoundException() : base("Not chapters found")
    {
    }

    public NotServicesFoundException(string message)
        : base(message)
    {
    }

    public NotServicesFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
