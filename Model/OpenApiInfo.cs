using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myMicroservice.Model
{
    public class OpenApiInfo : Swashbuckle.AspNetCore.Swagger.Info
    {
        public string Title { set; get; }
        public string Version { set; get; }
    }
}
