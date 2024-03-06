
using System.Collections.Generic;

namespace MyDataClass
{
    public class ActualResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }

    public class Supply
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Uom { get; set; }
        public int PriceInCents { get; set; }
        public string ProviderId { get; set; }
        public string MaterialType { get; set; }
    }

    public class Partner
    {
        public string Name { get; set; }
        public string PartnerType { get; set; }
        public string PartnerAddress { get; set; }
        public List<Supply> Supplies { get; set; }
    }

    public class WrapperClass
    {
        public List<Partner> Partners { get; set; }
    }
}
