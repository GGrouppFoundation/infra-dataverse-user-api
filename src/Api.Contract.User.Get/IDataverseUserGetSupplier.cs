using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

public interface IDataverseUserGetSupplier
{
    ValueTask<Result<DataverseUserGetOut, Failure<DataverseUserGetFailureCode>>> GetUserAsync(
        DataverseUserGetIn input, CancellationToken cancellationToken);
}