    using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace myMicroservice.ApiErrors
{
    public class ForbidError : ApiError
    {
        public ForbidError()
            : base(404, HttpStatusCode.Forbidden.ToString())
        {
        }

        public ForbidError(string message)
            : base(404, HttpStatusCode.Forbidden.ToString(), message)
        {
        }
    }
}
