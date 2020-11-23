using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TerraformMinds.Models.Exceptions
{
    public class ValidationException : Exception
    {
        public List<Exception> ValidationExceptions { get; set; }

        //Constructor
        public ValidationException() : base("Please view ValidationExceptions for details.")
        {
            ValidationExceptions = new List<Exception>();
        }

        // Constructor
        public ValidationException(string message) : base("Please view ValidationExceptions for details.")
        {
            ValidationExceptions = new List<Exception>()
            {
                new Exception(message)
            };
        }
    }
}
