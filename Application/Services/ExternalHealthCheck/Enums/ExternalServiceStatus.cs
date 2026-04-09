using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Application.Services.ExternalHealthCheck.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ExternalServiceStatus
{
    [EnumMember(Value = "Healthy")]
    Healthy,

    [EnumMember(Value = "Unhealthy")]
    Unhealthy
}
