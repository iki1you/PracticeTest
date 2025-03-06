using System;

namespace BLL.Exeptions
{
    internal class DataNotFoundExeption<T> : Exception where T : class
    {
        public DataNotFoundExeption()
        {
        }

        public DataNotFoundExeption(string message)
            : base(message)
        {
        }
    }
}
