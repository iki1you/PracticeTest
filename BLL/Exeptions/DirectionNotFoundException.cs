using System;

namespace BLL.Exeptions
{
    internal class DirectionNotFoundException : Exception
    {
        public DirectionNotFoundException()
        {
        }

        public DirectionNotFoundException(string message)
            : base(message)
        {
        }
    }
}
