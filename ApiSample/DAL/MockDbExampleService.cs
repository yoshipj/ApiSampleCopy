using ApiSample.DTOs.Guests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiSample.DAL
{
    public class MockDbExampleService : IExampleService
    {
        private readonly List<Gosc> Guests;

        public MockDbExampleService()
        {
            Guests = new List<Gosc>
            {
                new Gosc()
                {
                    IdGosc = 1,
                    Imie = "Marcin",
                    Nazwisko = "Niewiński",
                    ProcentRabatu = 30
                },
                new Gosc()
                {
                    IdGosc = 2,
                    Imie= "Jan",
                    Nazwisko = "Kowalski"
                }
            };
        }

        public bool AddGuest(GuestRequestDto newGuest)
        {
            try
            {
                var newId = Guests.Max(x => x.IdGosc) + 1;
                var guestToAdd = new Gosc
                {
                    IdGosc = newId,
                    Imie = newGuest.FirstName,
                    Nazwisko = newGuest.LastName,
                    ProcentRabatu = newGuest.DiscountPercent
                };
                Guests.Add(guestToAdd);
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }

        public bool UpdateGuest(int id, GuestRequestDto updateGuest)
        {
            var guestToUpdate = Guests.SingleOrDefault(x => x.IdGosc == id);
            if (guestToUpdate == null)
                return false;
            else
            {
                guestToUpdate.Imie = updateGuest.FirstName;
                guestToUpdate.Nazwisko = updateGuest.LastName;
                guestToUpdate.ProcentRabatu = updateGuest.DiscountPercent;
                return true;
            }
        }

        public bool DeleteGuest(int id)
        {
            var guestToDelete = Guests.SingleOrDefault(x => x.IdGosc == id);
            if (guestToDelete == null)
                return false;
            else
            {
                Guests.Remove(guestToDelete);
                return true;
            }
        }
        public GuestResponseDto GetGuestById(int idGuest)
        {
           return Guests.Where(x => x.IdGosc == idGuest)
                        .Select(x => new GuestResponseDto
                        {
                            IdGuest = x.IdGosc,
                            FirstName = x.Imie,
                            LastName = x.Nazwisko,
                            DiscountPercent = x.ProcentRabatu
                        }).SingleOrDefault();
        }

        public ICollection<GuestResponseDto> GetGuestsCollection(string lastName)
        {
            var listToReturn = Guests.Select(x => new GuestResponseDto
            {
                IdGuest = x.IdGosc,
                FirstName = x.Imie,
                LastName = x.Nazwisko,
                DiscountPercent = x.ProcentRabatu
            }).ToList();

            if (!string.IsNullOrEmpty(lastName)) // jeśli filtrujemy po nazwisku
                return listToReturn.Where(x => x.LastName == lastName).ToList();
            else
                return listToReturn;
        }

        public string Test()
        {
            return "Działa!";
        }

        public bool DeleteGuestStr(string id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Models.Gosc> GetGuestsCollectionWithReservations(string lastName)
        {
            throw new NotImplementedException();
        }

        private class Gosc
        {
            public int IdGosc { get; set; }
            public string Imie { get; set; }
            public string Nazwisko { get; set; }
            public int? ProcentRabatu { get; set; }
        }
    }
}
