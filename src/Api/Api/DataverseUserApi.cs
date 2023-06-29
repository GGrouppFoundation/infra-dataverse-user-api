namespace GarageGroup.Infra;

internal sealed partial class DataverseUserApi : IDataverseUserApi
{
    private readonly IDataverseEntityGetSupplier entityGetSupplier;

    internal DataverseUserApi(IDataverseEntityGetSupplier entityGetSupplier)
        =>
        this.entityGetSupplier = entityGetSupplier;
}