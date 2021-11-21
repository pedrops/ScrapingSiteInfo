using System;
using System.Collections.Generic;
using System.Text;

namespace WebScraper
{
    public interface IBizForSale
    {
        int ListingNum { get; set; }
        string FileName { get; set; }
        string Url { get; set; }
        string Title { get; set; }
        int Revenue { get; set; }
        int Income { get; set; }
        decimal Multiple { get; set; }
        int AskingPrice { get; set; }
        bool? BusinessHasInventory { get; set; }
        bool? PriceIncludesInventory { get; set; }
        string Description { get; set; }
        BusinessStatus Status { get; set; }
        BusinessType BizType { get; set; }
    }

    public enum BusinessType
    {
        Unknown,
        Saas
    }

    public enum BusinessStatus
    {
        Unknown,
        ForSale,
        Sold,
        LetterOfIntent,
        UnderOffer
    }

}