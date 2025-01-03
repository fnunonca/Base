using System;
using System.Collections.Generic;
using System.Text;

namespace Transversal.Common
{
    public interface IAppLogger
    {
        void LogInfo(string message, params object[] args);

        void LogWarn(string message, params object[] args);

        void LogError(string message, params object[] args);

        void LogError(Exception exception, string message, params object[] args);
    }
}
