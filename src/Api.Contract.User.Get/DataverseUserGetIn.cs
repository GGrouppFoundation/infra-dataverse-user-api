using System;

namespace GarageGroup.Infra;

public readonly record struct DataverseUserGetIn
{
    public DataverseUserGetIn(Guid activeDirectoryUserId)
        =>
        ActiveDirectoryUserId = activeDirectoryUserId;

    public Guid ActiveDirectoryUserId { get; }
}