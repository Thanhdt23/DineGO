using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DineGO_Api.Data;
using DineGO_Api.Model;

public class ReservationDAO
{
    private readonly ApplicationDbContext _context;

    public ReservationDAO(ApplicationDbContext context)
    {
        _context = context;
    }

    // Get all reservations
    public List<Reservation> GetReservations()
    {
        try
        {
            return _context.reservations.ToList();
        }
        catch (Exception e)
        {
            throw new Exception($"Error fetching reservations: {e.Message}");
        }
    }

    // Get reservation by ID
    public Reservation FindReservationById(int id)
    {
        try
        {
            return _context.reservations.SingleOrDefault(x => x.reser_id == id);
        }
        catch (Exception e)
        {
            throw new Exception($"Error finding reservation: {e.Message}");
        }
    }

    // Save a new reservation
    public void SaveReservation(Reservation reservation)
    {
        try
        {
            _context.reservations.Add(reservation);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            throw new Exception($"Error saving reservation: {e.Message}");
        }
    }

    // Update reservation details
    public void UpdateReservation(Reservation reservation)
    {
        try
        {
            _context.Entry(reservation).State = EntityState.Modified;
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            throw new Exception($"Error updating reservation: {e.Message}");
        }
    }

    // Delete reservation by ID
    public void DeleteReservation(int id)
    {
        try
        {
            var reservation = _context.reservations.SingleOrDefault(x => x.reser_id == id);
            if (reservation != null)
            {
                _context.reservations.Remove(reservation);
                _context.SaveChanges();
            }
        }
        catch (Exception e)
        {
            throw new Exception($"Error deleting reservation: {e.Message}");
        }
    }
}
