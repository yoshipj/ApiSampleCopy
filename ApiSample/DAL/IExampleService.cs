
using ApiSample.DTOs.Guests;
using ApiSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiSample.DAL
{
    public interface IExampleService
    {
        string Test();
        ICollection<GuestResponseDto> GetGuestsCollection(string lastName);
        GuestResponseDto GetGuestById(int idGuest);
        bool AddGuest(GuestRequestDto newGuest);
        bool UpdateGuest(int id, GuestRequestDto updateGuest);
        bool DeleteGuest(int id);


        /// <summary>
        /// Przykład metody wrażliwej na SqlI
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteGuestStr(string id);
        /// <summary>
        /// Tego nie powinniśmy używać. Nie wykorzystujemy tu GuestResponseDto tylko model oparty bezpośrednio o tabelę w bazie danych.
        /// </summary>
        /// <param name="lastName"></param>
        /// <returns></returns>
        ICollection<Gosc> GetGuestsCollectionWithReservations(string lastName);
    }
}
