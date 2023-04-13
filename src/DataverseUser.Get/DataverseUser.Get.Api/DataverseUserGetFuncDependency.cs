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
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map<IDataverseUserGetFunc>(CreateFunc);

        static DataverseUserGetFunc CreateFunc(TDataverseApiClient apiClient)
        {
            ArgumentNullException.ThrowIfNull(apiClient);
            return new(apiClient);
        }
    }
}