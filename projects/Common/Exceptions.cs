using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common {
    public class UserCancelledException : Exception {
        public UserCancelledException(string message) : base(message) { }
    }
}
