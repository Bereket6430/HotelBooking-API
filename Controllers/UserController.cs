using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;



[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly MongoDbContext _dbContext;

    public UsersController(MongoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _dbContext.Users.Find(_ => true).ToListAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(string id)
    {
        var user = await _dbContext.Users.Find(u => u.Id == id).FirstOrDefaultAsync();

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        await _dbContext.Users.InsertOneAsync(user);
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] User updatedUser)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, id);
        var update = Builders<User>.Update
            .Set(u => u.Username, updatedUser.Username)
            .Set(u => u.Email, updatedUser.Email)
            .Set(u => u.Password, updatedUser.Password); // Update other properties as needed

        var result = await _dbContext.Users.UpdateOneAsync(filter, update);

        if (result.ModifiedCount == 0)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var result = await _dbContext.Users.DeleteOneAsync(u => u.Id == id);

        if (result.DeletedCount == 0)
            return NotFound();

        return NoContent();
    }
}

