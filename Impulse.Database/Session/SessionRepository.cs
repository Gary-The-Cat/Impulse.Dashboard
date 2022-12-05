using Impulse.Repository.Models;

namespace Impulse.Repository.Session;

public class SessionRepository
{
    public List<LogRecordModel> LogRecords { get; } = new List<LogRecordModel>();
}