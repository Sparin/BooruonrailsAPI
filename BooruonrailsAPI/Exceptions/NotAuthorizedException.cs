using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BooruonrailsAPI.Exceptions
{
    public class NotAuthorizedException : ApplicationException
    {
        public NotAuthorizedException()
        {

        }

        public NotAuthorizedException(string message): base(message)
        {

        }

        public NotAuthorizedException(string message, Exception inner): base(message, inner)
        {

        }
    }
}
