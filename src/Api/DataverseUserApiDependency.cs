using System;
using System.Runtime.CompilerServices;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Infra.Dataverse.DataverseUser.Api.Test")]

namespace GarageGroup.Infra;

public static class DataverseUserApiDependency
{
    public static Dependency<IDataverseUserApi> UseUserApi<TDataverseApiClient>(this Dependency<TDataverseApiClient> dependency)
        where TDataverseApiClient : IDataverseEntityGetSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map<IDataverseUserApi>(CreateFunc);

        static DataverseUserApi CreateFunc(TDataverseApiClient dataverseApi)
        {
            ArgumentNullException.ThrowIfNull(dataverseApi);
            return new(dataverseApi);
        }
    }
}