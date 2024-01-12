using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

[ApiController]
[Route("api/reservations")]
public class ReservationsController : ControllerBase
{
    private readonly MongoDbContext _dbContext;

    public ReservationsController(MongoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetReservations()
    {
        var reservations = await _dbContext.Reservations.Find(_ => true).ToListAsync();
        return Ok(reservations);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReservation(string id)
    {
        var reservation = await _dbContext.Reservations.Find(r => r.Id == id).FirstOrDefaultAsync();

        if (reservation == null)
            return NotFound();

        return Ok(reservation);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReservation([FromBody] Reservation reservation)
    {
        await _dbContext.Reservations.InsertOneAsync(reservation);
        return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReservation(string id, [FromBody] Reservation updatedReservation)
    {
        var filter = Builders<Reservation>.Filter.Eq(r => r.Id, id);
        var update = Builders<Reservation>.Update
            .Set(r => r.CheckInDate, updatedReservation.CheckInDate)
            .Set(r => r.CheckOutDate, updatedReservation.CheckOutDate)
            .Set(r => r.TotalCost, updatedReservation.TotalCost)
            .Set(r => r.Status, updatedReservation.Status); // Update other properties as needed

        var result = await _dbContext.Reservations.UpdateOneAsync(filter, update);

        if (result.ModifiedCount == 0)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReservation(string id)
    {
        var result = await _dbContext.Reservations.DeleteOneAsync(r => r.Id == id);

        if (result.DeletedCount == 0)
            return NotFound();

        return NoContent();
    }
}
