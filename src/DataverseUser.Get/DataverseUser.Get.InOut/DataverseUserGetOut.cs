using System;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Platform;

public readonly record struct DataverseUserGetOut
{
    private readonly string? firstName, lastName, fullName;

    public DataverseUserGetOut(
        Guid systemUserId,
        [AllowNull] string firstName,
        [AllowNull] string lastName,
        [AllowNull] string fullName)
    {
        SystemUserId = systemUserId;
        this.firstName = string.IsNullOrEmpty(firstName) ? default : firstName;
        this.lastName = string.IsNullOrEmpty(lastName) ? default : lastName;
        this.fullName = string.IsNullOrEmpty(fullName) ? default : fullName;
    }

    public Guid SystemUserId { get; }

    public string FirstName => firstName ?? string.Empty;

    public string LastName => lastName ?? string.Empty;

    public string FullName => fullName ?? string.Empty;
}