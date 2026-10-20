using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Repositories;

/// <summary>
/// Kho dữ liệu sự kiện — interface hẹp (ISP), chỉ quản lý việc liên quan đến sự kiện.
/// </summary>
public interface IEventRepository
{
    IReadOnlyList<Event> GetByClub(Guid clubId);
    Event? GetById(Guid id);
    void Add(Event ev);
}
