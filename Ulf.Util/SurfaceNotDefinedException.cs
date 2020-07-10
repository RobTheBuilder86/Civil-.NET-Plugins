using System;

namespace Ulf.Util
{
    public class SurfaceNotDefinedException : Exception
    {
        public SurfaceNotDefinedException(string message) : base(message) { }
    }
}
