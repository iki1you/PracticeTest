using System;

namespace BLL.Exeptions
{
    public class DataDuplicateExeption<T> : Exception where T : class
    {
        public DataDuplicateExeption()
        {
        }

        public DataDuplicateExeption(string message)
            : base(message)
        {
        }
    }
}