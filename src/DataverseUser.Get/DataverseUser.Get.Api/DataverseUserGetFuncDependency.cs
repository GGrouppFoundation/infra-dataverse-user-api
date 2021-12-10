using PrimeFuncPack;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GGroupp.Infra.Dataverse.DataverseUser.Get.Tests")]

namespace GGroupp.Infra;

using IDataverseUserGetFunc = IAsyncValueFunc<DataverseUserGetIn, Result<DataverseUserGetOut, Failure<DataverseUserGetFailureCode>>>;

public static class UserGetFuncDependency
{
    public static Dependency<IDataverseUserGetFunc> UseUserGetApi<TDataverseApiClient>(this Dependency<TDataverseApiClient> dependency)
        where TDataverseApiClient : IDataverseEntityGetSupplier
        =>
        dependency.Map<IDataverseUserGetFunc>(apiClient => DataverseUserGetFunc.Create(apiClient));
}