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
            // create a dictionary of duplicates 
            var duplicates = Items.GroupBy(x => x.Name)
                .Where(c => c.Count() > 1)
                .ToDictionary(k => k.Key, v => v.Count());

            // creates a StringBuilder result for full output
            //StringBuilder result = new StringBuilder();
            Dictionary<string, string> ItemsDict = new Dictionary<string, string>();

            foreach (var item in Items)
            {
                //if (!result.ToString().Contains(item.Name))
                if (!ItemsDict.ContainsKey(item.Name))
                {
                    if ( duplicates.ContainsKey(item.Name) )
                    {
                        // adds a line if 
                        ItemsDict.Add(item.Name, $"{item.Price * duplicates[item.Name] + (item.Imported.DutyAmount(item) * duplicates[item.Name])} ({duplicates[item.Name]} @ {item.Imported.DutyAmount(item) + item.Price})");
                        //result.AppendLine($"{item.Name}: {item.Price * duplicates[item.Name] + (item.Imported.DutyAmount(item) * duplicates[item.Name])} ({duplicates[item.Name]} @ {item.Imported.DutyAmount(item) + item.Price})");
                    }
                    else
                    {
                        ItemsDict.Add(item.Name, CalculateItemSale(item).ToString());
                        //result.AppendLine($"{item.Name}: {CalculateItemSale(item)}");
                    }

                }

            }

            // adds the Total Tax to the StringBuilder result
            //result.AppendLine($"Sales Tax: {TotalTax}");
            ItemsDict.Add("Sales Tax", TotalTax.ToString());

            // adds the TotalSales to the StringBuilder result
            //result.AppendLine($"Total: {TotalSale}");
            ItemsDict.Add("Total", TotalSale.ToString());

            //Console.WriteLine(result);
            //return result.ToString();
            return ItemsDict;
        }




    }


}
