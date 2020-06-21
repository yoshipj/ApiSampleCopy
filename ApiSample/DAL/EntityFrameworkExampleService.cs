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
                var newId = context.Gosc.Max(x => x.IdGosc) + 1;

                var guestToAdd = new Gosc
                {
                    IdGosc = newId,
                    Imie = newGuest.FirstName,
                    Nazwisko = newGuest.LastName,
                    ProcentRabatu = newGuest.DiscountPercent
                };

                context.Gosc.Add(guestToAdd);
                context.SaveChanges();
                return true;
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
                var guestToRemove = context.Gosc.SingleOrDefault(x => x.IdGosc == id);
                if (guestToRemove == null)
                    return false;
                else
                {
                    context.Gosc.Remove(guestToRemove);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteGuestStr(string id)
        {
            throw new NotImplementedException();
        }

        public GuestResponseDto GetGuestById(int idGuest)
        {
            return context.Gosc
                 .Where(x => x.IdGosc == idGuest)
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
                 .SingleOrDefault();
        }

        public ICollection<GuestResponseDto> GetGuestsCollection(string lastName)
        {
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
                    .Where(x => x.Nazwisko == lastName)
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
            return context.Gosc
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
        {
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
