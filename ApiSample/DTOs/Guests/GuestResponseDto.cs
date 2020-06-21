using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiSample.DTOs.Guests
{
    public class GuestResponseDto
    {
        public int IdGuest { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? DiscountPercent { get; set; }
    }
}
