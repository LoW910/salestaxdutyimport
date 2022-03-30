namespace DealerOnSalesTaxAndImport.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public IsImported Imported { get; set; }


        public Item(string name, decimal price, bool imported)
        {
            Name = name;
            Price = price;
            Imported = new IsImported (imported );
        }


    }


}
