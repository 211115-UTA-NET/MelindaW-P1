namespace PlainOldStoreApp.DataStorage
{
    public interface ICustomerRepository
    {
        Task<bool> GetCustomerEmail(string? email);

        Task<List<Customer>> GetAllCustomer(string? firstName, string? lasName);

        Task<bool> AddNewCustomer(
            string? firstName,
            string? lastName,
            string? address1,
            string? city,
            string? state,
            string? zip,
            string email);

        Task<Guid> SqlGetCustomerId(string email);
    }
}
