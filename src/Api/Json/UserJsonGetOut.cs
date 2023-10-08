using System;
using System.Globalization;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra;

internal readonly record struct UserJsonGetOut
{

    private const string SystemUserEntityName = "systemusers";

    private const string ActiveDirectoryObjectIdFieldName = "azureactivedirectoryobjectid";

    private const string SystemUserIdFieldName = "systemuserid";

    private const string FirstNameFieldName = "firstname";

    private const string LastNameFieldName = "lastname";

    private const string FullNameFieldName = "yomifullname";

    private static readonly FlatArray<string> SelectedFields;

    static UserJsonGetOut()
        =>
        SelectedFields = new(SystemUserIdFieldName, FirstNameFieldName, LastNameFieldName, FullNameFieldName);

    internal static DataverseEntityGetIn CreateEntityGetIn(Guid activeDirectoryUserId)
        =>
        new(
            entityPluralName: SystemUserEntityName,
            entityKey: new DataverseAlternateKey(
                ActiveDirectoryObjectIdFieldName, activeDirectoryUserId.ToString("D", CultureInfo.InvariantCulture)),
            selectFields: SelectedFields);

    [JsonPropertyName(SystemUserIdFieldName)]
    public Guid SystemUserId { get; init; }

    [JsonPropertyName(FirstNameFieldName)]
    public string? FirstName { get; init; }

    [JsonPropertyName(LastNameFieldName)]
    public string? LastName { get; init; }

    [JsonPropertyName(FullNameFieldName)]
    public string? FullName { get; init; }
}