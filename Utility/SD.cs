using SwiftCart.Data;
using System.Runtime.CompilerServices;

namespace SwiftCart.Utility
{
    public static class SD 
    {
        // SD = Static Details or Static Data
        public static string Role_Admin = "Admin";
        public static string Role_Customer = "Customer";

        //public static readonly IEnumerable<string> Roles = new[] { Role_Admin, Role_Customer };

        public static string StatusPending = "Pending";
        public static string StatusReadyForPickUp = "ReadyForPickUp ";
        public static string StatusCompleted= "Completed";
        public static string StatusCancelled = "Cancelled";

        // Conversion method that will convert list of shopping cart to list of order details
        public static List<OrderDetail> ConvertShoppingCartToOrderDetails(List<ShoppingCart> shoppingCarts)
        {
            List<OrderDetail> orderDetails = new List<OrderDetail>();

            foreach(var cart in shoppingCarts)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = cart.ProductId,
                    Count = cart.Count,
                    Price = Convert.ToDouble(cart.Product.Price),
                    ProductName = cart.Product.Name

                };
                orderDetails.Add(orderDetail);
            }

            return orderDetails;
        }
    }
}
