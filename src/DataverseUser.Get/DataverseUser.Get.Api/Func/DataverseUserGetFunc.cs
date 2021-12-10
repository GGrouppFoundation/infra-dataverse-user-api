using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

using IDataverseUserGetFunc = IAsyncValueFunc<DataverseUserGetIn, Result<DataverseUserGetOut, Failure<DataverseUserGetFailureCode>>>;

internal sealed partial class DataverseUserGetFunc : IDataverseUserGetFunc
{
    private static readonly ReadOnlyCollection<string> selectedFields;

    static DataverseUserGetFunc()
        =>
        selectedFields = new(new[]
        {
            ApiNames.SystemUserIdFieldName,
            ApiNames.FirstNameFieldName,
            ApiNames.LastNameFieldName,
            ApiNames.FullNameFieldName
        });

    private readonly IDataverseEntityGetSupplier entityGetSupplier;

    private DataverseUserGetFunc(IDataverseEntityGetSupplier entityGetSupplier)
        =>
        this.entityGetSupplier = entityGetSupplier;

    public static DataverseUserGetFunc Create(IDataverseEntityGetSupplier entityGetSupplier)
        =>
        new(entityGetSupplier ?? throw new ArgumentNullException(nameof(entityGetSupplier)));

    public partial ValueTask<Result<DataverseUserGetOut, Failure<DataverseUserGetFailureCode>>> InvokeAsync(
        DataverseUserGetIn input, CancellationToken cancellationToken = default);
}