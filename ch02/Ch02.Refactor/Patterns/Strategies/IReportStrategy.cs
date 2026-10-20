namespace EduHub.Ch02.Patterns.Strategies;

/// <summary>
/// STRATEGY: mỗi chiến lược là một cách định dạng dữ liệu thống kê khác nhau.
/// </summary>
public interface IReportStrategy
{
    string Name { get; }
    string Render(IReadOnlyList<ClubStat> stats);
}
