using System;
using System.Data;

namespace Transversal.Common
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection { get; }

    }
}
