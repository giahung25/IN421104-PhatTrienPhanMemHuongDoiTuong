using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Repositories;

/// <summary>
/// Kho dữ liệu thành viên — interface hẹp (ISP), chỉ quản lý việc liên quan đến thành viên.
/// </summary>
public interface IMemberRepository
{
    IReadOnlyList<Member> GetByClub(Guid clubId);
    Member? GetById(Guid id);
    void Add(Member member);
}
