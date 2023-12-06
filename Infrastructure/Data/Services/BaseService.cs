using Domain.Interfaces;

namespace Infrastructure.Data.Services
{
    public class BaseService
    {
        protected internal IUnitOfWork UnitOfWork { get; set; }
        public BaseService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

    }
}
