using ShopApi.Database.Entities.ProductManagement;
using ShopApi.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopApi.Service
{
    public class OrderService
    {
        private readonly UserRepo _userRepo;
        private readonly ProductUserShoppingCartRepo _productsUsersShoppingCartRepo;
        private readonly PaymentRepo _paymentRepo;
        private readonly OrderRepo _orderRepo;
        private readonly ProductRepo _productRepo;

        public OrderService(UserRepo userRepo, ProductUserShoppingCartRepo productUserShoppingCartRepo, PaymentRepo paymentRepo, OrderRepo orderRepo, ProductRepo productRepo)
        {
            _userRepo = userRepo;
            _productsUsersShoppingCartRepo = productUserShoppingCartRepo;
            _paymentRepo = paymentRepo;
            _orderRepo = orderRepo;
            _productRepo = productRepo;
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
            var productsUserShoppingCart = await _productsUsersShoppingCartRepo.GetProductsShoppingCartNotOrderedUser(user.Id);
            await _productsUsersShoppingCartRepo.MarkProductsAsOrdered(user.Id);

            double amount = 0;
            foreach (var productsUsersShoppingCart in productsUserShoppingCart)
            {
                amount += productsUsersShoppingCart.Quantity * productsUsersShoppingCart.Product.NewPrice;
                await _productRepo.ReduceProductQuantity(productsUsersShoppingCart.Product.Barcode, productsUsersShoppingCart.Quantity);
            }
            await _orderRepo.PlaceOrder(productsUserShoppingCart, user, amount);
        }

        public async Task PlaceOrderWithPayment(string cardNumber)
        {
            var user = await _userRepo.GetCurrentUser();
            var productsUsersShoppingCarts = await _productsUsersShoppingCartRepo.GetProductsShoppingCartNotOrderedUser(user.Id);
            double amount = 0;

            foreach (var productsUsersShoppingCart in productsUsersShoppingCarts)
            {
                amount += productsUsersShoppingCart.Quantity * productsUsersShoppingCart.Product.NewPrice;
                await _productRepo.ReduceProductQuantity(productsUsersShoppingCart.Product.Barcode, productsUsersShoppingCart.Quantity);

            }
            await _productsUsersShoppingCartRepo.MarkProductsAsOrdered(user.Id);
            var order = await _orderRepo.PlaceOrder(productsUsersShoppingCarts, user, amount);
            await _paymentRepo.AddPayment(amount, cardNumber, order, user);
        }

        public async Task PayOrderLaterPayment(int orderId, string cardNumber)
        {
            var user = await _userRepo.GetCurrentUser();
            var productsUserShoppingCart = await _productsUsersShoppingCartRepo.GetProductsByOrderId(orderId, user.Id);
            double amount = 0;

            foreach (var productsUsersShoppingCart in productsUserShoppingCart)
            {
                amount += productsUsersShoppingCart.Quantity * productsUsersShoppingCart.Product.NewPrice;

            }

            var order = await _orderRepo.GetOrderById(orderId, user.Id);
            await _paymentRepo.AddPayment(amount, cardNumber, order, user);
        }

        public async Task<List<ProductsUsersShoppingCart>> GetProductsByOrderId(int id)
        {
            var user = await _userRepo.GetCurrentUser();
            return await _productsUsersShoppingCartRepo.GetProductsByOrderId(id, user.Id);
        }


    }
}
