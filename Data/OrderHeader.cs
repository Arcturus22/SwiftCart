﻿using System.ComponentModel.DataAnnotations;

namespace SwiftCart.Data
{
    public class OrderHeader
    {
        public int Id { get; set; }
        [Required] public string UserId { get; set; }

        [Required]
        [Display(Name = "Order Total")]
        public double OrderTotal { get; set; }
        [Required] public DateTime OrderDate { get; set; }
        [Required] public string Status { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required][Display(Name = "Phone Number")] public string PhoneNumber { get; set; }

        [Required][Display(Name = "Email Address")] public string Email { get; set; }

        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
