﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write_Wash.Models
{
    public class Product : OrderProduct
    {
        public string Image { get; set; }
        public string DisplayedImage
        {
            get { return Path.GetFullPath($@"Resources\Image\{Image}"); }
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public float Price { get; set; }
        public int Discount { get; set; }

        public float? DisplayedPrice
        {
            get
            {
                if (Discount != 0)
                {
                    return Price - (Price * Discount / 100);
                }
                else { return null; }
            }
        }

    }
}
