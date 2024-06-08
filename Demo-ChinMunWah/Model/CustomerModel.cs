namespace Demo_ChinMunWah.Model
{
    public class GenericCustomerModel
    {
        public Guid CustomerGuid { get; set; }
    }

    public class UpsertCustomerModel
    {
        public Guid? CustomerGuid { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }
}
