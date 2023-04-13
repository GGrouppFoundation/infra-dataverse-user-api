using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using GGroupp.Infra;
using Moq;
using Xunit;

namespace GGroupp.Platform.Dataverse.DataverseUser.Get.Tests;

partial class DataverseUserGetFuncTest
{
    [Fact]
    public static void InvokeAsync_CancellationTokenIsCanceled_ExpectValueTaskIsCanceled()
    {
        var dataverseOut = new DataverseEntityGetOut<UserJsonGetOut>(default);
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);

        var func = CreateFunc(mockDataverseApiClient.Object);

        var input = new DataverseUserGetIn(SomeActiveDirectoryGuid);
        var token = new CancellationToken(canceled: true);

        var actual = func.InvokeAsync(input, token);
        Assert.True(actual.IsCanceled);
    }

    [Fact]
    public static async Task InvokeAsync_CancellationTokenIsNotCanceled_ExpectCallDataVerseApiClientOnce()
    {
        var dataverseOut = new DataverseEntityGetOut<UserJsonGetOut>(default);
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);

        var func = CreateFunc(mockDataverseApiClient.Object);

        var input = new DataverseUserGetIn(SomeActiveDirectoryGuid);
        var token = new CancellationToken(false);

        _ = await func.InvokeAsync(input, token);

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
    [InlineData(DataverseFailureCode.UserNotEnabled, DataverseUserGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange, DataverseUserGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound, DataverseUserGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unknown, DataverseUserGetFailureCode.Unknown)]
    public static async Task InvokeAsync_DataverseResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode, DataverseUserGetFailureCode expectedFailureCode)
    {
        var dataverseFailure = Failure.Create(sourceFailureCode, "Some failure message");
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseFailure);

        var func = CreateFunc(mockDataverseApiClient.Object);

        var input = new DataverseUserGetIn(SomeActiveDirectoryGuid);
        var actual = await func.InvokeAsync(input, CancellationToken.None);

        var expected = Failure.Create(expectedFailureCode, dataverseFailure.FailureMessage);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public static async Task InvokeAsync_DataverseResultIsSuccess_ExpectSuccess()
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

        var func = CreateFunc(mockDataverseApiClient.Object);

        var input = new DataverseUserGetIn(SomeActiveDirectoryGuid);
        var actual = await func.InvokeAsync(input, CancellationToken.None);

        var expected = new DataverseUserGetOut(
            systemUserId: dataverseUser.SystemUserId,
            firstName: firstName,
            lastName: lastName,
            fullName: fullName);

        Assert.Equal(expected, actual);
    }
}