using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Core.Domain.Entities.Base
{
    public class OutPutBase
    {
        
        public bool Result { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }


    }
}
