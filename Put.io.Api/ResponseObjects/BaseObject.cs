using System;
using System.Linq;

namespace Put.io.Api.ResponseObjects
{
    public abstract class BaseObject
    {
        public string status { get; set; }
        public string error_type { get; set; }
        public string error_message { get; set; }

        private readonly string[] _acceptedStatus = { "OK", "IN_QUEUE", "DOWNLOADING", "COMPLETED" };

        public bool SafeToContinue()
        {
            var match = _acceptedStatus.Any(x => x.Equals(status, StringComparison.InvariantCultureIgnoreCase));

            if (match)
                return true;

            return string.IsNullOrEmpty(error_type) && string.IsNullOrEmpty(error_message);
        }
    }
}