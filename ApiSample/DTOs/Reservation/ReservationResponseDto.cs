using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiSample.DTOs.Reservation
{
    public class ReservationResponseDto
    {
        public int IdReservation { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int RoomNo { get; set; }
    }
}
