using ShopApi.Database.Entities.ProductManagement;
using ShopApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Service
{
    public class OrderService
    {
        private readonly UserRepo _userRepo;
        private readonly ShoppingCartRepo _shoppingCartRepo;
        private readonly PaymentRepo _paymentRepo;
        private readonly OrderRepo _orderRepo;
        private readonly ProductRepo _productRepo;
        private readonly OrderedProductRepo _orderedProductRepo;

        public OrderService(UserRepo userRepo, ShoppingCartRepo shoppingCartRepo, PaymentRepo paymentRepo, OrderRepo orderRepo, ProductRepo productRepo, OrderedProductRepo orderedProductRepo)
        {
            _userRepo = userRepo;
            _shoppingCartRepo = shoppingCartRepo;
            _paymentRepo = paymentRepo;
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _orderedProductRepo = orderedProductRepo;
        }

        public async Task<List<Order>> GetOrders()
        {
            var user = await _userRepo.GetCurrentUser();
            return await _orderRepo.GetOrdersCurrentUser(user.Id);
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            var user = await _userRepo.GetCurrentUser();
            return await _orderRepo.GetOrderById(orderId, user.Id);
        }

        public async Task PlaceOrderWithOutPayment()
        {
            var user = await _userRepo.GetCurrentUser();
            var productsShoppingCart = await _shoppingCartRepo.GetProductsShoppingCart(user.Id);
            await _shoppingCartRepo.RemoveUserProducts(user.Id);

            var orderedProducts = productsShoppingCart.Select(x => new OrderedProduct
            {
                PricePerProduct = x.Product.BasePrice - (x.Product.BasePrice * x.Product.Discount / 100),
                Product = x.Product,
                Quantity = x.Quantity,
            }).ToList();
            double amount = 0;
            foreach (var orderedProduct in orderedProducts)
            {
                amount += orderedProduct.Quantity * orderedProduct.PricePerProduct;

                await _productRepo.ReduceProductQuantity(orderedProduct.Product.Barcode, orderedProduct.Quantity);
            }
            var order = await _orderRepo.PlaceOrder(new Order
            {
                User = user,
                OrderDate = DateTime.Now,
                LimitDate = DateTime.Now.AddDays(7),
                InvoiceAmount = amount
            });

            foreach (var orderedProduct in orderedProducts)
            {
                orderedProduct.Order = order;
            }
            await _orderedProductRepo.OrderProducts(orderedProducts);
        }

        public async Task PlaceOrderWithPayment(string cardNumber)
        {
            var user = await _userRepo.GetCurrentUser();
            var productsShoppingCart = await _shoppingCartRepo.GetProductsShoppingCart(user.Id);
            await _shoppingCartRepo.RemoveUserProducts(user.Id);

            var orderedProducts = productsShoppingCart.Select(x => new OrderedProduct
            {
                PricePerProduct = x.Product.BasePrice - (x.Product.BasePrice * x.Product.Discount / 100),
                Product = x.Product,
                Quantity = x.Quantity,
            }).ToList();

            double amount = 0;
            foreach (var orderedProduct in orderedProducts)
            {
                amount += orderedProduct.Quantity * orderedProduct.PricePerProduct;
                await _productRepo.ReduceProductQuantity(orderedProduct.Product.Barcode, orderedProduct.Quantity);
            }
            var order = await _orderRepo.PlaceOrder(new Order
            {
                User = user,
                OrderDate = DateTime.Now,
                LimitDate = DateTime.Now.AddDays(7)
            });
            foreach (var orderedProduct in orderedProducts)
            {
                orderedProduct.Order = order;
            }

            await _orderedProductRepo.OrderProducts(orderedProducts);
            await _paymentRepo.AddPayment(new Payment
            {
                Amount = amount,
                CardNumber = cardNumber,
                Order = order,
                User = user
            });
        }

        public async Task PayOrderLaterPayment(int orderId, string cardNumber)
        {
            var user = await _userRepo.GetCurrentUser();
            var order = await _orderRepo.GetOrderById(orderId, user.Id);
            await _paymentRepo.AddPayment(new Payment
            {
                Amount = order.InvoiceAmount,
                CardNumber = cardNumber,
                Order = order,
                User = user
            });
        }

        public async Task<List<OrderedProduct>> GetProductsByOrderId(int id)
        {
            return await _orderedProductRepo.GetProductsByOrderId(id);
        }
    }
}
