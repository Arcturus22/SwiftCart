using Microsoft.AspNetCore.Components;
using Stripe;
using Stripe.Checkout;
using SwiftCart.Data;
using SwiftCart.Repository.IRepository;
using SwiftCart.Utility;

namespace SwiftCart.Services
{
    public class PaymentService
    {
        private readonly NavigationManager _navigationManager;
        private readonly IOrderRepository _orderRepository;

        public PaymentService(NavigationManager navigationManager, IOrderRepository orderRepository)
        {
            _navigationManager = navigationManager;
            _orderRepository = orderRepository;
        }

        public Session CreateStripeCheckoutSession(OrderHeader orderHeader)
        {
            // CREATING A SESSION ON STRIPE

            var lineItems = orderHeader.OrderDetails
                .Select(order => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmountDecimal = (decimal?)(order.Price * 100), // Stripe expects the amount in cents
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = order.ProductName,

                        }

                    },
                    Quantity = order.Count
                }).ToList();

            var options = new Stripe.Checkout.SessionCreateOptions
            {
                SuccessUrl = $"{_navigationManager.BaseUri}order/confirmation/{{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{_navigationManager.BaseUri}cart",
                LineItems = lineItems,
                Mode = "payment",
            };

            

            var service = new SessionService();
            var session = service.Create(options);

            return session;
        }


        public async Task<OrderHeader> CheckPaymentStatusAndUpdateOrder(string sessionId)
        {
            // Was the Session and payment successful?
            OrderHeader orderHeader = await _orderRepository.GetOrderBySessionIdAsync(sessionId);

            // Call an API on Stripe to retrieve the session object
            var service = new SessionService();
            var session = service.Get(sessionId);

            if(session.PaymentStatus.ToLower() == "paid")
            {
                await _orderRepository.UpdateStatusAsync(orderHeader.Id, SD.StatusApproved, session.PaymentIntentId);
            }

            return orderHeader;


        }
    }
}
