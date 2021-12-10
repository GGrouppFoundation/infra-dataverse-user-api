using System;
using System.Runtime.CompilerServices;
using GGroupp.Infra;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GGroupp.Platform.Dataverse.DataverseUser.Get.Tests")]

namespace GGroupp.Platform;

using IDataverseUserGetFunc = IAsyncValueFunc<DataverseUserGetIn, Result<DataverseUserGetOut, Failure<DataverseUserGetFailureCode>>>;

public static class UserGetFuncDependency
{
    public static Dependency<IDataverseUserGetFunc> UseUserGetApi<TDataverseApiClient>(this Dependency<TDataverseApiClient> dependency)
        where TDataverseApiClient : IDataverseEntityGetSupplier
        =>
        dependency.Map<IDataverseUserGetFunc>(apiClient => DataverseUserGetFunc.Create(apiClient));
}