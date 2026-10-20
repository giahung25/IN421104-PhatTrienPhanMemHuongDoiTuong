namespace EduHub.Ch02.Logging;

/// <summary>
/// Abstraction cho việc ghi log (thay cho File.AppendAllText trực tiếp trong mã nguồn cũ).
/// </summary>
public interface IAppLogger
{
    void Info(string message);
}
