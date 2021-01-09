using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;
using TodoApi.Models.UserModels;

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
        // [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            #region Seed Data

            //var todoItem = _context.TodoItems.FirstOrDefault(i => i.Id == 1 || i.Id == 2 || i.Id == 3 || i.Id == 4 || i.Id == 5);
            //if (todoItem == null)
            //{
            //    _context.TodoItems.Add(new TodoItem { Id = 1, Name = "Name 1", IsComplete = false });
            //    _context.TodoItems.Add(new TodoItem { Id = 2, Name = "Name 2", IsComplete = true });
            //    _context.TodoItems.Add(new TodoItem { Id = 3, Name = "Name 3", IsComplete = false });
            //    _context.TodoItems.Add(new TodoItem { Id = 4, Name = "Name 4", IsComplete = true });
            //    _context.TodoItems.Add(new TodoItem { Id = 5, Name = "Name 5", IsComplete = false});
            //}
            //_context.SaveChanges();

            #endregion Seed Data

            return await _context.TodoItems.Where(x => x.UserRefId == GetUserId())
                                    .Select(x => _mapper.Map<TodoItemDTO>(x))
                                    .ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            var todoItem = await TodoItemExists(id);

            if (todoItem == null)
            {
                return NotFound(new { message = "Not Found" });
            }

            return _mapper.Map<TodoItemDTO>(todoItem);
        }

        // PUT: api/TodoItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                return BadRequest(new { message = "Wrong id" });
            }
            var todoItem = await TodoItemExists(id);
            if (todoItem == null)
            {
                return BadRequest(new { message = "Not Found" });
            }
            var todoItemParam = _mapper.Map<TodoItem>(todoItemDTO);
            //todoItemParam.Id = id;
            //Tìm todoItem theo id yêu cầu
            //var todoItem = await _context.TodoItems.Where(x => x.UserRefId == GetUserId() && x.Id == id).FirstAsync();
            
            //Cập nhật data mới
            if (!string.IsNullOrWhiteSpace(todoItemParam.Name))
            {
                todoItem.Name = todoItemParam.Name;
            }
            if (!string.IsNullOrWhiteSpace(todoItemParam.IsComplete.ToString()))
            {
                todoItem.IsComplete = todoItemParam.IsComplete;
            }

            if (!string.IsNullOrWhiteSpace(todoItemParam.DateDue.ToString()))
            {
                todoItem.DateDue = todoItemParam.DateDue;
            }
            if (!string.IsNullOrWhiteSpace(todoItemParam.DateDue.ToString()))
            {
                todoItem.DateCreate = todoItemParam.DateCreate;
            }

            _context.TodoItems.Update(todoItem);
            await _context.SaveChangesAsync();

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
            // Lấy id của User hiện tại cho vào todoItem
            todoItem.UserRefId = GetUserId();
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
            var todoItem = await TodoItemExists(id);
            //var todoItem = await _context.TodoItems.Where(x => x.UserRefId == GetUserId() && x.Id == id).FirstAsync();
            if (todoItem == null)
            {
                return NotFound(new { message = "Not found" });
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Get Item dùng nội bộ, dùng cái này để gọi tới database 1 lần
        // Vừa check và lấy data luôn
        private async Task<TodoItem> TodoItemExists(long idTodoItem)
        {
            // Kiểm tra idTodoItem có chưa 
            if (_context.TodoItems.Any(e => e.Id == idTodoItem))
            {
                // Lấy todo theo Id đã kiểm tra
                var todoExist = await _context.TodoItems.FindAsync(idTodoItem);
                //nêú đúng chủ nhân
                 if(todoExist.UserRefId == GetUserId())
                 {
                    return todoExist;
                 }
                return null;
            }
            else
            {
                //Nếu không có ID trả về Null luôn
                return null;
            }
            
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