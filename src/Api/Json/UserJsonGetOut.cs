using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra;

internal readonly record struct UserJsonGetOut
{
    public static readonly FlatArray<string> SelectedFields;

    static UserJsonGetOut()
        =>
        SelectedFields = new(ApiNames.SystemUserIdFieldName, ApiNames.FirstNameFieldName, ApiNames.LastNameFieldName, ApiNames.FullNameFieldName);

    [JsonPropertyName(ApiNames.SystemUserIdFieldName)]
    public Guid SystemUserId { get; init; }

    [JsonPropertyName(ApiNames.FirstNameFieldName)]
    public string? FirstName { get; init; }

    [JsonPropertyName(ApiNames.LastNameFieldName)]
    public string? LastName { get; init; }

    [JsonPropertyName(ApiNames.FullNameFieldName)]
    public string? FullName { get; init; }
}