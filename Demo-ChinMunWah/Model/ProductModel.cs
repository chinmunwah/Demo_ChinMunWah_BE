namespace Demo_ChinMunWah.Model
{
    public class GenericProductModel
    {
        public Guid ProductGuid { get; set; }
    }

    public class UpsertProductModel
    {
        public Guid? ProductGuid { get; set; } 

        public string Name { get; set; }

        public string Description { get; set; } 

        public double Price { get; set; }

        public int Stock { get; set; }
    }
}
