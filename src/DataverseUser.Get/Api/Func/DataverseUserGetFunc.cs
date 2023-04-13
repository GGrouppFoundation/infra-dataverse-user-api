using System;

namespace GGroupp.Infra;

internal sealed partial class DataverseUserGetFunc : IDataverseUserGetFunc
{
    private static readonly FlatArray<string> selectedFields;

    static DataverseUserGetFunc()
        =>
        selectedFields = new(ApiNames.SystemUserIdFieldName, ApiNames.FirstNameFieldName, ApiNames.LastNameFieldName, ApiNames.FullNameFieldName);

    private readonly IDataverseEntityGetSupplier entityGetSupplier;

    internal DataverseUserGetFunc(IDataverseEntityGetSupplier entityGetSupplier)
        =>
        this.entityGetSupplier = entityGetSupplier;
}