using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using TodoApi.Models;
using TodoApi.Models.Helpers;
using TodoApi.Models.UserModels;
using AutoMapper;


namespace TodoApi.Controllers
{
    //Comment chỗ này để test client
    [Authorize(Roles = Role.User + "," + Role.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;
        //Nhầm ở chỗ này, nhớ để interface chỗ này và chỗ para 
        //của Contructor
        private readonly IMapper _mapper;
        
        public TodoItemsController(TodoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            
        }
        //GetUserId
        // Trong ControllerBase có thuộc tính User chính là User đang
         // được xác thực. Lấy Id của User này bằng Claims.First
         // chính là Claim.Type.Name được cấu hình ở UserController.cs
        private int GetUserId()
        {
            return Int16.Parse(User.Claims.First().Value);
        }
        
        // GET: api/TodoItems
        //Thêm [AllowAnonymous] mục đích để test Client
        //[AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            #region Seed Data

            var todoItem = _context.TodoItems.FirstOrDefault(i => i.Id == 1 || i.Id == 2 || i.Id == 3 || i.Id == 4 || i.Id == 5);
            if (todoItem == null)
            {
                _context.TodoItems.Add(new TodoItem { Id = 1, Name = "Name 1", IsComplete = false });
                _context.TodoItems.Add(new TodoItem { Id = 2, Name = "Name 2", IsComplete = true });
                _context.TodoItems.Add(new TodoItem { Id = 3, Name = "Name 3", IsComplete = false });
                _context.TodoItems.Add(new TodoItem { Id = 4, Name = "Name 4", IsComplete = true });
                _context.TodoItems.Add(new TodoItem { Id = 5, Name = "Name 5", IsComplete = false});
            }
            _context.SaveChanges();

            #endregion Seed Data
            return await _context.TodoItems.Where(x=>x.UserRefId==GetUserId())
                                    .Select(x=> _mapper.Map<TodoItemDTO>(x))
                                    .ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return _mapper.Map<TodoItemDTO>(todoItem);
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                return BadRequest();
            }
            //Tìm todoItem theo id yêu cầu
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            //Cập nhật data mới
            todoItem.Name = todoItemDTO.Name;
            todoItem.IsComplete = todoItemDTO.IsComplete;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/TodoItems
        
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItemDTO todoItemDTO)
        {
            //Tạo todoItem thủ công để lưu trữ (từ todoItemDTO)
            // var todoItem = new TodoItem
            // {
            //     IsComplete = todoItemDTO.IsComplete,
            //     Name = todoItemDTO.Name
            // };
            var todoItem = _mapper.Map<TodoItem>(todoItemDTO);
            // Lấy id của User hiện tại:

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTodoItem),
                new { id = todoItem.Id },
                _mapper.Map<TodoItemDTO>(todoItem));
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }

        // //Chuyển TodoItem thành ItemToDTO thủ công, không cần dùng nữa
        // private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
        //     new TodoItemDTO
        //     {
        //         Id = todoItem.Id,
        //         Name = todoItem.Name,
        //         IsComplete = todoItem.IsComplete
        //     };
    }
}
