using System;

namespace BLL.Exeptions
{
    // Лучше сделать что-нибудь вроде DataNotFound<T> или базовый класс NotFoundException
    internal class ProjectNotFoundException: Exception
    {
        public ProjectNotFoundException()
        {
        }

        public ProjectNotFoundException(string message)
            : base(message)
        {
        }
    }
}
