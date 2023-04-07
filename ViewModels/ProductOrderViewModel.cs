﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write_Wash.Models;

namespace Write_Wash.ViewModels
{
    internal class ProductOrderViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        private readonly PointService _pointService;
        private readonly OrderService _orderService;
        public List<Product> Products { get; set; }

        public int MaxProd { get; set; }
        public int CurrentProd { get; set; }

        
        public List<PointContext> Points { get; set; }

        public string _pattern;

        public Visibility NullProduct { get; set; }

        public string FullName { get; set; } = Global.CurrentUser.Name == string.Empty ? "Гость" : $"{Global.CurrentUser.Surname} {Global.CurrentUser.Name} {Global.CurrentUser.Patronymic}";

        public Visibility BasketVisible { get; set; } = Global.CurrentUser.Name == string.Empty ? Visibility.Collapsed : Visibility.Visible;

        public string SumOrder { get; set; }
        public string DiscountOrder { get; set; }

        public ProductOrderViewModel(PageService pageService, ProductService productService, PointService pointService, OrderService orderService)
        {
            _pageService = pageService;
            _productService = productService;
            _pointService = pointService;
            _orderService = orderService;
            Task.Run(async () =>
            {
                Points = await _pointService.GetPoints();
                Products = Global.OrderProductList.ToList();
                
                MaxProd = Products.Count();
                CurrentProd = Products.Count();
                SumOrder = CheckSumOrder().ToString();
                DiscountOrder = DiscountOrderCheck().ToString();
                CheckNullProduct();
                
            }).WaitAsync(TimeSpan.FromMilliseconds(10))
            .ConfigureAwait(false);
            
        }
        float? CheckSumOrder()
        {
            float? sumorder = 0;
            foreach(Product p in Products)
            {
                if(p.Discount != 0)
                {
                    sumorder += p.DisplayedPrice;
                }
                else
                {
                    sumorder += p.Price;
                }
            }
            return sumorder;
        }
        float? DiscountOrderCheck()
        {
            float? disc;
            float pricenondisc = 0;
            float? pricedisc = 0;
            foreach(Product p in Products)
            {
                if(p.Discount > 0)
                {
                    pricedisc += p.DisplayedPrice;
                }
                else
                {
                    pricedisc += p.Price;
                }
                pricenondisc += p.Price;
            }
            disc = (float?)Math.Truncate((decimal)(100 - (pricedisc * 100 / pricenondisc)));
            return disc;
        }
        void CheckNullProduct()
        {
            if (CurrentProd > 0) { NullProduct = Visibility.Hidden; } else { NullProduct = Visibility.Visible; }
        }

        public int SelectedProduct { get; set; }
        
        public DelegateCommand Back => new DelegateCommand(() =>
        {
            _pageService.ChangePage(new BrowseProduct());
        });
        public DelegateCommand DeleteInOrder => new DelegateCommand(() =>
        {
            if(Products.Count() > 0)
            {
                Global.OrderProductList.RemoveAt(SelectedProduct);
                Products = Global.OrderProductList.ToList();
                MaxProd = Products.Count();
                CurrentProd = Products.Count();
                CheckNullProduct();
                SumOrder = CheckSumOrder().ToString();
                if (Global.OrderProductList.Count > 0)
                    DiscountOrder = DiscountOrderCheck().ToString();
                else
                    DiscountOrder = null;
            }
            
        });
        public DelegateCommand RemoveProductInOrder => new DelegateCommand(() =>
        {
            try
            {
                if (Products[SelectedProduct].ProductCount > 1)
                {
                    Global.OrderProductList[SelectedProduct].ProductCount--;
                    Products = Global.OrderProductList.ToList();
                    MaxProd = Products.Count();
                    CurrentProd = Products.Count();
                    CheckNullProduct();
                }
                else if (Products[SelectedProduct].ProductCount == 1)
                {
                    Global.OrderProductList.RemoveAt(SelectedProduct);
                    Products = Global.OrderProductList.ToList();
                    MaxProd = Products.Count();
                    CurrentProd = Products.Count();
                    CheckNullProduct();
                }
            }
            catch { }

        });
        public DelegateCommand AddProductInOrder => new DelegateCommand(() =>
        {
            try
            {
                Global.OrderProductList[SelectedProduct].ProductCount++;
                Products = Global.OrderProductList.ToList();
                MaxProd = Products.Count();
                CurrentProd = Products.Count();
                CheckNullProduct();
            }
            catch { }
            

        });
        public DelegateCommand OrderBut => new DelegateCommand(() =>
        {
            _orderService.NewOrder();
        });
    }
}
