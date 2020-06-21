using ApiSample.DTOs.Reservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiSample.DTOs.Guests
{
    public class GuestResponseDto
    {
        public GuestResponseDto()
        {
            Reservations = new List<ReservationResponseDto>();
        }
        public int IdGuest { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? DiscountPercent { get; set; }

        public ICollection<ReservationResponseDto> Reservations { get; set; }
    }
}
