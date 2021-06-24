using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Core;
using ShopApi.Dto;
using ShopApi.Service;

namespace ShopApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("orders")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        private readonly IMapper _mapper;

        public OrderController(OrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        #region  Orders

        [HttpGet("get-orders")]
        public async Task<CommandResult<List<OrderTable>>> GetOrders()
        {
            var orders = await _orderService.GetOrders();

            return CommandResult<List<OrderTable>>.Success(_mapper.Map<List<OrderTable>>(orders));
        }

        [HttpGet("get-order-by-id/{id}")]
        public async Task<CommandResult<OrderTable>> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderById(id);

            return CommandResult<OrderTable>.Success(_mapper.Map<OrderTable>(order));
        }

        [HttpPost("place-order-without-payment")]
        public async Task<CommandResult<bool>> PlaceOrder()
        {
            await _orderService.PlaceOrderWithOutPayment();
            return CommandResult<bool>.Success(true);
        }

        [HttpPost("pay-order-with-payment")]
        public async Task<CommandResult<bool>> PayOrder([FromBody] PaymentCardNumber paymentCardNumber)
        {
            await _orderService.PlaceOrderWithPayment(paymentCardNumber.CardNumber);
            return CommandResult<bool>.Success(true);
        }

        [HttpPost("pay-order-later-payment")]
        public async Task<CommandResult<bool>> PayOrderLaterPayment([FromBody] PaymentDetails paymentDetails)
        {
            await _orderService.PayOrderLaterPayment(paymentDetails.OrderId, paymentDetails.CardNumber);
            return CommandResult<bool>.Success(true);
        }
        #endregion


    }
}
