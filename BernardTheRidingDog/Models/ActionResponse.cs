using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BernardTheRidingDog.Models
{
    public class ActionResponse
    {
        public ActionResponse() { }
        public ActionResponse(string message, bool isSuccessful)
        {
            Message = message;
            IsSuccessful = isSuccessful;
        }

        public string Message { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
