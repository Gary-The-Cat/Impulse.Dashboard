using Impulse.ErrorReporting;
using Impulse.Repository.Models;

namespace Impulse.Framework.Dashboard.ExtensionMethods
{
    public static class LogRecordExtensions
    {
        public static LogRecordModel ToModel(this LogRecord record) => new LogRecordModel
        {
            Message = record.Message,
            Criticality = (int)record.Criticality,
            TimeStampTicks = record.Timestamp.Ticks,
            StackTrace = record.StackTrace,
        };
    }
}
