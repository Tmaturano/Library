using Library.Infra.Data.Interfaces;

namespace Library.Application.Services
{
    public abstract class ApplicationService
    {
        protected readonly IUnitOfWork unitOfWork;

        public ApplicationService(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }

        public bool Commit()
        {
            return unitOfWork.Commit();
        }
    }
}
