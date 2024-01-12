using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

[ApiController]
[Route("api/rooms")]
public class RoomsController : ControllerBase
{
    private readonly MongoDbContext _dbContext;

    public RoomsController(MongoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetRooms()
    {
        var rooms = await _dbContext.Rooms.Find(_ => true).ToListAsync();
        return Ok(rooms);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoom(string id)
    {
        var room = await _dbContext.Rooms.Find(r => r.Id == id).FirstOrDefaultAsync();

        if (room == null)
            return NotFound();

        return Ok(room);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoom([FromBody] Room room)
    {
        await _dbContext.Rooms.InsertOneAsync(room);
        return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoom(string id, [FromBody] Room updatedRoom)
    {
        var filter = Builders<Room>.Filter.Eq(r => r.Id, id);
        var update = Builders<Room>.Update
            .Set(r => r.Type, updatedRoom.Type)
            .Set(r => r.Capacity, updatedRoom.Capacity)
            .Set(r => r.Price, updatedRoom.Price)
            .Set(r => r.Available, updatedRoom.Available); // Update other properties as needed

        var result = await _dbContext.Rooms.UpdateOneAsync(filter, update);

        if (result.ModifiedCount == 0)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom(string id)
    {
        var result = await _dbContext.Rooms.DeleteOneAsync(r => r.Id == id);

        if (result.DeletedCount == 0)
            return NotFound();

        return NoContent();
    }
}
