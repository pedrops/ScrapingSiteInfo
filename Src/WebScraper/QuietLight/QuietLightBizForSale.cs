using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using AngleSharp.Dom;
using HtmlAgilityPack;

namespace WebScraper
{
    public class QuietLightBizForSale  : IBizForSale, ICosmosEntity
    {
        public string Id => ListingNum.ToString();
        public string PartitionKey => "QuietLightBizForSale";


        public BusinessStatus Status { get; set; }
        public BusinessType BizType { get; set; } = BusinessType.Unknown;
        public int ListingNum { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public int Revenue { get; set; }
        public int Income{ get; set; }
        public decimal Multiple{ get; set; }
        public int AskingPrice{ get; set; }
        public bool? BusinessHasInventory { get; set; }
        public bool? PriceIncludesInventory { get; set; }
        public string Description { get; set; }

    }
}
