using System;

namespace Library.Infra.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        bool Commit();
    }
}
