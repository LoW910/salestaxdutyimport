namespace DealerOnSalesTaxAndImport.Models
{
    public class IsImported
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public bool DutyExempt { get; set; }
        public decimal DutyRate { get; set; } = .05m;

        public IsImported(bool dutyExempt, string name = "")
        {
            Name = name;
            DutyExempt = dutyExempt;
        }

        public decimal DutyAmount(Item item)
        {
            return Math.Ceiling((item.Price * item.Imported.DutyRate) / 0.05m) * 0.05m;
        }


        public decimal CheckDutyExempt(Item item)
        {
            if (item.Imported.DutyExempt == true)
            {
                item.Imported.DutyRate = 0;
            }
            var result = DutyAmount(item);

            return result;
        }

    }



}
