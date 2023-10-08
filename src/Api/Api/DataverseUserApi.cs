namespace GarageGroup.Infra;

internal sealed partial class DataverseUserApi : IDataverseUserApi
{
    private readonly IDataverseEntityGetSupplier dataverseApi;

    internal DataverseUserApi(IDataverseEntityGetSupplier dataverseApi)
        =>
        this.dataverseApi = dataverseApi;
}