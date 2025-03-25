using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DineGO_Api.Model;

public interface IReservationRepository
{
    List<Reservation> GetReservations();
    Reservation FindReservationById(int ID);
    void SaveReservation(Reservation reservation);
    void UpdateReservation(Reservation reservation);
    void DeleteReservation(int reservationId);
}
