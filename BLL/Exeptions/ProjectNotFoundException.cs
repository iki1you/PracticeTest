using System;

namespace BLL.Exeptions
{
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
