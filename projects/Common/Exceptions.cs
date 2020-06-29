using System;

namespace Common
{
    public class UserCancelledException : Exception
    {
        public UserCancelledException(string message) : base(message)
        {
        }
    }
}