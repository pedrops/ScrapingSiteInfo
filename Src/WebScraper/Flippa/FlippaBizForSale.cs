using System;
using System.Collections.Generic;
using System.Text;

namespace WebScraper
{
    public class FlippaBizForSale: IBizForSale, ICosmosEntity
    {
        public string Id => ListingNum.ToString();
        public string PartitionKey => "FlippaBizForSale";


        public int ListingNum { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public int Revenue { get; set; }
        public int Income { get; set; }
        public decimal Multiple { get; set; }
        public int AskingPrice { get; set; }
        public bool? BusinessHasInventory { get; set; }
        public bool? PriceIncludesInventory { get; set; }
        public string Description { get; set; }
        public BusinessStatus Status { get; set; }
        public BusinessType BizType { get; set; } = BusinessType.Unknown;
    }
}
