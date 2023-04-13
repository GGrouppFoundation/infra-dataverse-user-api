using System;
using System.Runtime.CompilerServices;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GGroupp.Infra.Dataverse.DataverseUser.Get.Test")]

namespace GGroupp.Infra;

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