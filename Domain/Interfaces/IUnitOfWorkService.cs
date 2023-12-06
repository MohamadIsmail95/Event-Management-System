using Domain.Interfaces;

namespace ClinicSystem.Services.IServices
{
    public interface IUnitOfWorkService
    {
        IEventService EventService { get; }

    }
}
