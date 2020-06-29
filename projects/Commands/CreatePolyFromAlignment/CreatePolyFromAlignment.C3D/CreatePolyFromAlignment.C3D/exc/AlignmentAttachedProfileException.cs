using System;

namespace CreatePolyFromAlignment.C3D.exc
{
    public class AlignmentAttachedProfileException : ArgumentException
    {
        public AlignmentAttachedProfileException(string message) : 
            base(message)
        { }
    }

}
