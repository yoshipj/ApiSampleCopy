using ApiSample.DTOs.Guests;
using ApiSample.DTOs.Reservation;
using ApiSample.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace ApiSample.DAL
{
    public class EntityFrameworkExampleService : IExampleService
    {
        private readonly yoshiContext context;
        public EntityFrameworkExampleService(yoshiContext _context)
        {
            context = _context;
        }

        public bool AddGuest(GuestRequestDto newGuest)
        {
            try
            {
                var newId = context.Gosc.Max(x => x.IdGosc) + 1; //znajdujemy wartość nowego id

                var guestToAdd = new Gosc //tworzymy nowy obiekt typu Gosc (czyli naszego odpowiednika tabeli z bazy danych)
                {
                    IdGosc = newId,
                    Imie = newGuest.FirstName,
                    Nazwisko = newGuest.LastName,
                    ProcentRabatu = newGuest.DiscountPercent
                };

                context.Gosc.Add(guestToAdd); //dodajemy go do kolekcji gości (odpowiednik przygotowania polecenia INSERT)
                context.SaveChanges(); //zapisujemy zmiany w bazie (odpowednik wykonania napisanego wcześniej polecenia)
                return true; //zwracamy true - nasza metoda AddGuest musi zwrócić rezultat operacji (true/false).
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteGuest(int id)
        {
            try
            {
                var guestToRemove = context.Gosc.SingleOrDefault(x => x.IdGosc == id); //znajdujemy gościa którego chcemy usunąć
                if (guestToRemove == null)
                    return false; //jeśli gość o podanym id nie istnieje w bazie to zwracamy do kontrolera odpowiedź false 
                else
                {
                    context.Gosc.Remove(guestToRemove); //usuwamy gościa z kolekcji
                    context.SaveChanges(); //zatwierdzamy operację usuwania (w tym miejscu faktycznie wykonujemy DELETE na bazie)
                    return true; //zwracamy true, oznaczające że usunęliśmy gościa
                }
            }
            catch (Exception)
            {//w przypadku jakiegokolwiek błędu zwracamy false
                return false;
            }
        }

        public bool DeleteGuestStr(string id)
        {//nieużywane, metoda pojawiła się tutaj ponieważ jest wymieniona w interfejscie IExampleService.
            throw new NotImplementedException();
        }

        public GuestResponseDto GetGuestById(int idGuest)
        {
            return context.Gosc
                 .Where(x => x.IdGosc == idGuest)
                 .Include(x => x.Rezerwacja) // dołączamy informacje o rezerwacjach powiązanych w bazie z naszym gościem
                 .Select(x => new GuestResponseDto // "tłumaczymy" wczytany przez EF obiekt typu Gosc na zwracany docelowo obiekt typu GuestResponseDto
                 {
                     IdGuest = x.IdGosc,
                     FirstName = x.Imie,
                     LastName = x.Nazwisko,
                     DiscountPercent = x.ProcentRabatu,
                     Reservations = x.Rezerwacja.Select(x => new ReservationResponseDto //podobnie jak Gosciem - tłumaczymy listę rezerwacji na listę ReservationResponseDto
                     {
                         IdReservation = x.IdRezerwacja,
                         DateFrom = x.DataOd,
                         DateTo = x.DataDo,
                         RoomNo = x.NrPokoju
                     }).ToList()
                 })
                 .SingleOrDefault();
        }

        public ICollection<GuestResponseDto> GetGuestsCollection(string lastName)
        {//metoda działa analogicznie do GetGuestById, zamiast jednego elementu zwracamy jednak listę. 
            //W zależności od tego czy przekazaliśmy nazwisko do filtrowania wywołujemy zapytanie z warunkiem WHERE lub bez
            if (string.IsNullOrEmpty(lastName))
                return context.Gosc
                     .Include(x => x.Rezerwacja)
                     .Select(x => new GuestResponseDto
                     {
                         IdGuest = x.IdGosc,
                         FirstName = x.Imie,
                         LastName = x.Nazwisko,
                         DiscountPercent = x.ProcentRabatu,
                         Reservations = x.Rezerwacja.Select(x => new ReservationResponseDto
                         {
                             IdReservation = x.IdRezerwacja,
                             DateFrom = x.DataOd,
                             DateTo = x.DataDo,
                             RoomNo = x.NrPokoju
                         }).ToList()
                     })
                     .ToList();
            else
                return context.Gosc
                    .Where(x => x.Nazwisko == lastName) //to jest jedyna różnica pomiędzy tymi dwoma zapytaniami
                    .Include(x => x.Rezerwacja)
                    .Select(x => new GuestResponseDto
                    {
                        IdGuest = x.IdGosc,
                        FirstName = x.Imie,
                        LastName = x.Nazwisko,
                        DiscountPercent = x.ProcentRabatu,
                        Reservations = x.Rezerwacja.Select(x => new ReservationResponseDto
                        {
                            IdReservation = x.IdRezerwacja,
                            DateFrom = x.DataOd,
                            DateTo = x.DataDo,
                            RoomNo = x.NrPokoju
                        }).ToList()
                    })
                    .ToList();
        }

        public ICollection<Gosc> GetGuestsCollectionWithReservations(string lastName)
        {
            return context.Gosc//metoda działająca na klasach modelowych opartych bezpośrednio o tabele w bazie. 
                    .Where(x => x.Nazwisko == lastName)
                    .Include(x => x.Rezerwacja)
                    .ThenInclude(x => x.NrPokojuNavigation)
                    .ThenInclude(x => x.IdKategoriaNavigation)
                    .ToList();
        }

        public string Test()
        {
            return "Działa! Ta wersja API korzysta z bazy danych db-mssql.pjwstk.edu.pl przy użyciu Entity Framework";
        }

        public bool UpdateGuest(int id, GuestRequestDto updateGuest)
        {//podobnie jak w przypadku DELETE, znajdujemy gościa którego chcemy edytować i przypisujemy mu nowe wartości do wszystkich pól
            var guestToEdit = context.Gosc.SingleOrDefault(x => x.IdGosc == id); 
            if (guestToEdit == null)
                return false;
            else
            {
                guestToEdit.Imie = updateGuest.FirstName;
                guestToEdit.Nazwisko = updateGuest.LastName;
                guestToEdit.ProcentRabatu = updateGuest.DiscountPercent;
                context.SaveChanges();
                return true;
            }
        }
    }
}
