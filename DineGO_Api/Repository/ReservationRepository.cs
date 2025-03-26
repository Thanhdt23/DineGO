using System;
using System.Collections.Generic;
using System.Linq;
using DineGO_Api.Data;
using DineGO_Api.Model;

namespace DineGO_Api.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ReservationDAO _reservationDAO;
        public ReservationRepository(ReservationDAO reservationDAO)
        {
            _reservationDAO = reservationDAO;
        }
        public List<Reservation> GetReservations() => _reservationDAO.GetReservations();
        public Reservation FindReservationById(int id) => _reservationDAO.FindReservationById(id);
        public void SaveReservation(Reservation r) => _reservationDAO.SaveReservation(r);
        public void UpdateReservation(Reservation r) => _reservationDAO.UpdateReservation(r);
        public void DeleteReservation(int id) => _reservationDAO.DeleteReservation(id);
        public List<Object> GetResByCusId(int id) => _reservationDAO.GetResByCusId(id);
    }
}