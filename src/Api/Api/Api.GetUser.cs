using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class DataverseUserApi
{
    public ValueTask<Result<DataverseUserGetOut, Failure<DataverseUserGetFailureCode>>> GetUserAsync(
        DataverseUserGetIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .HandleCancellation()
        .Pipe(
            @in => new DataverseEntityGetIn(
                entityPluralName: ApiNames.SystemUserEntityName,
                entityKey: new DataverseAlternateKey(
                    ApiNames.ActiveDirectoryObjectIdFieldName, @in.ActiveDirectoryUserId.ToString("D", CultureInfo.InvariantCulture)),
                selectFields: UserJsonGetOut.SelectedFields))
        .PipeValue(
            entityGetSupplier.GetEntityAsync<UserJsonGetOut>)
        .MapFailure(
            failure => failure.MapFailureCode(MapDataverseFailureCode))
        .MapSuccess(
            entityGetOut => new DataverseUserGetOut(
                systemUserId: entityGetOut.Value.SystemUserId,
                firstName: entityGetOut.Value.FirstName,
                lastName: entityGetOut.Value.LastName,
                fullName: entityGetOut.Value.FullName));

    private static DataverseUserGetFailureCode MapDataverseFailureCode(DataverseFailureCode dataverseFailureCode)
        =>
        dataverseFailureCode switch
        {
            DataverseFailureCode.RecordNotFound => DataverseUserGetFailureCode.NotFound,
            _ => DataverseUserGetFailureCode.Unknown
        };
}