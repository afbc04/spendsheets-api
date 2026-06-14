using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Controllers;

[ApiController]
[Route("v1.0/users")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/todo
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetAll()
    {
        var items = await _context.TodoItems
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        return Ok(items);
    }

    // GET: api/todo/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TodoItem>> GetById(int id)
    {
        var item = await _context.TodoItems.FindAsync(id);
        if (item is null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    // POST: api/todo
    [HttpPost]
    public async Task<ActionResult<TodoItem>> Create(UsersTemplateCreate user)
    {
        /*
        user.Id = 0;
        user.CreatedAt = DateTime.UtcNow;

        _context.TodoItems.Add(item);
        await _context.SaveChangesAsync();
*/
        return CreatedAtAction(nameof(GetById), user);
    }

    // PUT: api/todo/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, TodoItem item)
    {
        if (id != item.Id)
        {
            return BadRequest("O id da rota não corresponde ao id do corpo do pedido.");
        }

        var exists = await _context.TodoItems.AnyAsync(t => t.Id == id);
        if (!exists)
        {
            return NotFound();
        }

        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/todo/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.TodoItems.FindAsync(id);
        if (item is null)
        {
            return NotFound();
        }

        _context.TodoItems.Remove(item);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
