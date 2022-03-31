using System.Linq;
using System.Text;

namespace DealerOnSalesTaxAndImport.Models
{
    public class Cart
    {

        // will provide a list of items
        public List<Item> Items { get; set; }
        // total tax from list of items
        public decimal TotalTax { get; set; }
        // total price of list of items
        public decimal TotalSale { get; set; }
        public Cart(List<Item> shoppingCart)
        {
            Items = shoppingCart;
            TotalTax = 0;
            TotalSale = 0;

        }

        

        public decimal CalculateItemSale(Item item)
        {
            var result = item.Price + CalculateItemDuty(item) + CalculateItemTax(item);
            return result;

        }

        public decimal CalculateItemTax(Item item)
        {
            Tax tax = new Tax();
            // checks tax exempt status and sets rate to 0
            var result = tax.Exempt(item);
            return result;
        }

        // calculates items duty amount 
        public decimal CalculateItemDuty(Item item)
        {
            var result = item.Imported.CheckDutyExempt(item);
            return result;
        }

        public void CalculateReceiptTotals()
        {
            // only perform is theres items in cart
            if(Items.Count > 0)
            {
                // works through each item in list of items.
                foreach (var item in Items)
                {
                    // var for storing current items duty
                    var itemDuty = CalculateItemDuty(item);

                    //  checks if tax exempt if not calculates tax
                    var itemTax = CalculateItemTax(item);


                    // adds tax from item to totaltax 
                    TotalTax += itemTax + itemDuty;

                    // adds current items tax and price to the TotalSales 
                    var itemSale = CalculateItemSale(item);

                    TotalSale += itemSale;

                    //Console.WriteLine($"Name: {item.Name} Price: {item.Price} Tax: {itemTax}  Duty: {itemDuty} Sale: {itemSale} ");
                }
            }
        }


        public Dictionary<string, string> PrintCart()
        {
            // creates a dictionary of duplicates 
            // items are grouped and counted by name if greater than 1
            // itemname as the key and the value as count
            var duplicates = Items.GroupBy(x => x.Name)
                .Where(c => c.Count() > 1)
                .ToDictionary(x => x.Key, c => c.Count());

            // creates a new dictionary result for full output
            Dictionary<string, string> Result = new Dictionary<string, string>();

            // itterate through this carts list of items
            foreach (var item in Items)
            {
                if (!Result.ContainsKey(item.Name))
                {
                    if ( duplicates.ContainsKey(item.Name) )
                    {
                        int itemCount = duplicates[item.Name];
                        //Result.Add(item.Name, $"{item.Price * duplicates[item.Name] + (item.Imported.DutyAmount(item) * duplicates[item.Name])} ({duplicates[item.Name]} @ {item.Imported.DutyAmount(item) + item.Price})");
                        Result.Add(item.Name, $"{item.Price * itemCount + (CalculateItemDuty(item) * itemCount)} ({itemCount} @ {CalculateItemDuty(item) + item.Price})");
                    }
                    else
                    {
                        // adds item with calculated item sale price
                        Result.Add(item.Name, CalculateItemSale(item).ToString());
                    }
                }
            }
            if (Items.Count > 0) 
            {
                // adds the Total Tax to the result`    
                Result.Add("Sales Tax", TotalTax.ToString());

                // adds the TotalSales to the result
                Result.Add("Total", TotalSale.ToString());
            }

            return Result;
        }
    }
}
