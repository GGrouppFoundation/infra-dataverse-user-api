using System;
using System.Threading;
using Moq;

namespace GarageGroup.Infra.Dataverse.DataverseUser.Api.Test;

public static partial class DataverseUserApiTest
{
    private static readonly Guid SomeActiveDirectoryGuid = Guid.Parse("1203c0e2-3648-4596-80dd-127fdd2610b7");

    private static Mock<IDataverseEntityGetSupplier> CreateMockDataverseApiClient(
        Result<DataverseEntityGetOut<UserJsonGetOut>, Failure<DataverseFailureCode>> result)
    {
        var mock = new Mock<IDataverseEntityGetSupplier>();

        _ = mock
            .Setup(s => s.GetEntityAsync<UserJsonGetOut>(It.IsAny<DataverseEntityGetIn>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        return mock;
    }
}