using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra;

public sealed record class DataverseUserGetOut
{
    public DataverseUserGetOut(
        Guid systemUserId,
        [AllowNull] string firstName,
        [AllowNull] string lastName,
        [AllowNull] string fullName)
    {
        SystemUserId = systemUserId;
        FirstName = firstName.OrEmpty();
        LastName = lastName.OrEmpty();
        FullName = fullName.OrEmpty();
    }

    public Guid SystemUserId { get; }

    public string FirstName { get; }

    public string LastName { get; }

    public string FullName { get; }
}