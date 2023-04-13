using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

public interface IDataverseUserGetFunc
{
    ValueTask<Result<DataverseUserGetOut, Failure<DataverseUserGetFailureCode>>> InvokeAsync(
        DataverseUserGetIn input, CancellationToken cancellationToken);
}