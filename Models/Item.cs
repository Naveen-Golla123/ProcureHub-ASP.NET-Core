﻿namespace ProcureHub_ASP.NET_Core.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float baseprice { get; set; }
        public int quatity { get; set; }
    }
}