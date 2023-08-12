using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace GarageGroup.Infra.Dataverse.DataverseUser.Api.Test;

partial class DataverseUserApiTest
{
    [Fact]
    public static void GetUserAsync_CancellationTokenIsCanceled_ExpectValueTaskIsCanceled()
    {
        var dataverseOut = new DataverseEntityGetOut<UserJsonGetOut>(default);
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);

        var func = new DataverseUserApi(mockDataverseApiClient.Object);

        var input = new DataverseUserGetIn(SomeActiveDirectoryGuid);
        var token = new CancellationToken(canceled: true);

        var actual = func.GetUserAsync(input, token);
        Assert.True(actual.IsCanceled);
    }

    [Fact]
    public static async Task GetUserAsync_CancellationTokenIsNotCanceled_ExpectCallDataVerseApiClientOnce()
    {
        var dataverseOut = new DataverseEntityGetOut<UserJsonGetOut>(default);
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);

        var func = new DataverseUserApi(mockDataverseApiClient.Object);

        var input = new DataverseUserGetIn(SomeActiveDirectoryGuid);
        var token = new CancellationToken(false);

        _ = await func.GetUserAsync(input, token);

        var expectedRequest = new DataverseEntityGetIn(
            entityPluralName: "systemusers",
            entityKey: new DataverseAlternateKey(
                new KeyValuePair<string, string>[]
                {
                    new("azureactivedirectoryobjectid", SomeActiveDirectoryGuid.ToString("D", CultureInfo.InvariantCulture))
                }),
            selectFields: new("systemuserid", "firstname", "lastname", "yomifullname"));

        mockDataverseApiClient.Verify(c => c.GetEntityAsync<UserJsonGetOut>(expectedRequest, token), Times.Once);
    }

    [Theory]
    [InlineData(DataverseFailureCode.RecordNotFound, DataverseUserGetFailureCode.NotFound)]
    [InlineData(DataverseFailureCode.Unauthorized, DataverseUserGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange, DataverseUserGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.UserNotEnabled, DataverseUserGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.PrivilegeDenied, DataverseUserGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Throttling, DataverseUserGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound, DataverseUserGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.DuplicateRecord, DataverseUserGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unknown, DataverseUserGetFailureCode.Unknown)]
    public static async Task GetUserAsync_DataverseResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode, DataverseUserGetFailureCode expectedFailureCode)
    {
        var sourceException = new Exception("Some Error Message");
        var dataverseFailure = Failure.Create(sourceFailureCode, "Some failure message", sourceException);

        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseFailure);
        var func = new DataverseUserApi(mockDataverseApiClient.Object);

        var input = new DataverseUserGetIn(SomeActiveDirectoryGuid);

        var actual = await func.GetUserAsync(input, CancellationToken.None);
        var expected = Failure.Create(expectedFailureCode, dataverseFailure.FailureMessage, sourceException);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public static async Task GetUserAsync_DataverseResultIsSuccess_ExpectSuccess()
    {
        const string firstName = "Ivan";
        const string lastName = "Petrov";
        const string fullName = "Sergey Denisov";

        var dataverseUser = new UserJsonGetOut
        {
            SystemUserId = Guid.Parse("bd8b8e33-554e-e611-80dc-c4346bad0190"),
            FirstName = firstName,
            LastName = lastName,
            FullName = fullName
        };

        var dataverseOut = new DataverseEntityGetOut<UserJsonGetOut>(dataverseUser);
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);

        var func = new DataverseUserApi(mockDataverseApiClient.Object);

        var input = new DataverseUserGetIn(SomeActiveDirectoryGuid);
        var actual = await func.GetUserAsync(input, CancellationToken.None);

        var expected = new DataverseUserGetOut(
            systemUserId: dataverseUser.SystemUserId,
            firstName: firstName,
            lastName: lastName,
            fullName: fullName);

        Assert.Equal(expected, actual);
    }
}