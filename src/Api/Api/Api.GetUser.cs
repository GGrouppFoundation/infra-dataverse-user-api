using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class DataverseUserApi
{
    public ValueTask<Result<DataverseUserGetOut, Failure<DataverseUserGetFailureCode>>> GetUserAsync(
        DataverseUserGetIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input.ActiveDirectoryUserId, cancellationToken)
        .HandleCancellation()
        .Pipe(
            UserJsonGetOut.CreateEntityGetIn)
        .PipeValue(
            dataverseApi.GetEntityAsync<UserJsonGetOut>)
        .MapFailure(
            static failure => failure.MapFailureCode(MapDataverseFailureCode))
        .MapSuccess(
            static @out => new DataverseUserGetOut(
                systemUserId: @out.Value.SystemUserId,
                firstName: @out.Value.FirstName,
                lastName: @out.Value.LastName,
                fullName: @out.Value.FullName));

    private static DataverseUserGetFailureCode MapDataverseFailureCode(DataverseFailureCode dataverseFailureCode)
        =>
        dataverseFailureCode switch
        {
            DataverseFailureCode.RecordNotFound => DataverseUserGetFailureCode.NotFound,
            _ => DataverseUserGetFailureCode.Unknown
        };
}