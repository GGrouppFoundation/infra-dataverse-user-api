using System;
using System.Runtime.CompilerServices;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Infra.Dataverse.DataverseUser.Get.Api.Test")]

namespace GarageGroup.Infra;

public static class DataverseUserApiDependency
{
    public static Dependency<IDataverseUserApi> UseUserGetApi<TDataverseApiClient>(this Dependency<TDataverseApiClient> dependency)
        where TDataverseApiClient : IDataverseEntityGetSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map<IDataverseUserApi>(CreateFunc);

        static DataverseUserApi CreateFunc(TDataverseApiClient apiClient)
        {
            ArgumentNullException.ThrowIfNull(apiClient);
            return new(apiClient);
        }
    }
}