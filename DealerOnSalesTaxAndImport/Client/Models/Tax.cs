namespace DealerOnSalesTaxAndImport.Models
{
    // create a tax class
    // will allow creating of county/state/federal tax rates

    public class Tax
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Rate { get; set; } 

        public Tax(string name = "", decimal rate = .10m )
        {
            Name = name;
            Rate = rate;
        }

        // checks if the item is tax exempt,
        // changes rate to zero,
        // creates and returns result of tax.amount 
        public decimal Exempt(Item item)
        {
            decimal result = 0;
            if (item.Name.ToLower().Contains("choc") || item.Name.ToLower().Contains("book") || item.Name.ToLower().Contains("pill"))
            {
                Rate = 0.0m;
                result = Amount(item);
                return result;
            }
            else
            {
                Rate = .10m;
                result = Amount(item);
                return result;
                
            }

        }

        // calculates the item tax amount
        // 11.5 * .05 = .575 / 0.05 = 11.5 = math.ceil(11.5) = 12 * .05 = .6
        // 7.12 * 0.05 = .356 / .05 = 7.12 = Math.ceil(7.12) = 8 * .05 = .4
        public decimal Amount (Item item)
        {
            return Math.Ceiling((item.Price * Rate) / 0.05m) * 0.05m;
        }

    }
}
