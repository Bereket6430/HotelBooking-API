using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

[ApiController]
[Route("api/hotels")]
public class HotelsController : ControllerBase
{
    private readonly MongoDbContext _dbContext;

    public HotelsController(MongoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetHotels()
    {
        var hotels = await _dbContext.Hotels.Find(_ => true).ToListAsync();
        return Ok(hotels);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetHotel(string id)
    {
        var hotel = await _dbContext.Hotels.Find(h => h.Id == id).FirstOrDefaultAsync();

        if (hotel == null)
            return NotFound();

        return Ok(hotel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateHotel([FromBody] Hotel hotel)
    {
        await _dbContext.Hotels.InsertOneAsync(hotel);
        return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotel);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateHotel(string id, [FromBody] Hotel updatedHotel)
    {
        var filter = Builders<Hotel>.Filter.Eq(h => h.Id, id);
        var update = Builders<Hotel>.Update
            .Set(h => h.Name, updatedHotel.Name)
            .Set(h => h.Location, updatedHotel.Location)
            .Set(h => h.Price, updatedHotel.Price); // Update other properties as needed

        var result = await _dbContext.Hotels.UpdateOneAsync(filter, update);

        if (result.ModifiedCount == 0)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHotel(string id)
    {
        var result = await _dbContext.Hotels.DeleteOneAsync(h => h.Id == id);

        if (result.DeletedCount == 0)
            return NotFound();

        return NoContent();
    }
}

internal class MongoDbContext
{
}