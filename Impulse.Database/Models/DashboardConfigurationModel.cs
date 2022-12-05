using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Impulse.Repository.DataStructures;

[DataContract]
public record DashboardConfigurationModel
{
    public int Id { get; init; }

    [Required]
    public int LogLevel { get; init; }
}
