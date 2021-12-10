using System;
using System.Text.Json.Serialization;

namespace GGroupp.Platform;

internal readonly record struct UserJsonGetOut
{
    [JsonPropertyName(ApiNames.SystemUserIdFieldName)]
    public Guid SystemUserId { get; init; }

    [JsonPropertyName(ApiNames.FirstNameFieldName)]
    public string? FirstName { get; init; }

    [JsonPropertyName(ApiNames.LastNameFieldName)]
    public string? LastName { get; init; }

    [JsonPropertyName(ApiNames.FullNameFieldName)]
    public string? FullName { get; init; }
}