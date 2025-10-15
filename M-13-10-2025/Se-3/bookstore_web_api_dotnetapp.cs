// Project: dotnetapp (combined files for reference)
// Save each section into its respective file path under the dotnetapp project.

// File: Models/Book.cs
namespace dotnetapp.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }
}

// File: Models/Order.cs
namespace dotnetapp.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
    }
}

// File: Repository/BookRepository.cs
using System.Collections.Generic;
using System.Linq;
using dotnetapp.Models;

namespace dotnetapp.Repository
{
    public class BookRepository
    {
        private readonly List<Book> _books = new();
        private int _nextId = 1;

        public List<Book> GetAll() => _books.ToList();

        public Book Get(int id) => _books.FirstOrDefault(b => b.BookId == id);

        public Book Add(Book book)
        {
            book.BookId = _nextId++;
            _books.Add(book);
            return book;
        }

        public Book Update(int id, Book book)
        {
            var existing = Get(id);
            if (existing == null) return null;
            existing.BookName = book.BookName;
            existing.Category = book.Category;
            existing.Price = book.Price;
            return existing;
        }

        public bool Delete(int id)
        {
            var existing = Get(id);
            if (existing == null) return false;
            return _books.Remove(existing);
        }
    }
}

// File: Repository/OrderRepository.cs
using System.Collections.Generic;
using System.Linq;
using dotnetapp.Models;

namespace dotnetapp.Repository
{
    public class OrderRepository
    {
        private readonly List<Order> _orders = new();
        private int _nextId = 1;

        public List<Order> GetAll() => _orders.ToList();

        public Order Get(int id) => _orders.FirstOrDefault(o => o.OrderId == id);

        public Order Add(Order order)
        {
            order.OrderId = _nextId++;
            _orders.Add(order);
            return order;
        }

        public Order Update(int id, Order order)
        {
            var existing = Get(id);
            if (existing == null) return null;
            existing.CustomerName = order.CustomerName;
            existing.TotalAmount = order.TotalAmount;
            return existing;
        }

        public bool Delete(int id)
        {
            var existing = Get(id);
            if (existing == null) return false;
            return _orders.Remove(existing);
        }
    }
}

// File: Services/IBookService.cs
using System.Collections.Generic;
using dotnetapp.Models;

namespace dotnetapp.Services
{
    public interface IBookService
    {
        List<Book> GetBooks();
        Book GetBook(int id);
        Book SaveBook(Book book);
        Book UpdateBook(int id, Book book);
        bool DeleteBook(int id);
    }
}

// File: Services/IOrderService.cs
using System.Collections.Generic;
using dotnetapp.Models;

namespace dotnetapp.Services
{
    public interface IOrderService
    {
        List<Order> GetOrders();
        Order GetOrder(int id);
        Order SaveOrder(Order order);
        Order UpdateOrder(int id, Order order);
        bool DeleteOrder(int id);
    }
}

// File: Services/BookService.cs
using System.Collections.Generic;
using dotnetapp.Models;
using dotnetapp.Repository;

namespace dotnetapp.Services
{
    public class BookService : IBookService
    {
        private readonly BookRepository _repo;
         public BookService()
        {
          
        }

        public BookService(BookRepository repo)
        {
            _repo = repo;
        }

        public List<Book> GetBooks() => _repo.GetAll();

        public Book GetBook(int id) => _repo.Get(id);

        public Book SaveBook(Book book) => _repo.Add(book);

        public Book UpdateBook(int id, Book book) => _repo.Update(id, book);

        public bool DeleteBook(int id) => _repo.Delete(id);
    }
}

// File: Services/OrderService.cs
using System.Collections.Generic;
using dotnetapp.Models;
using dotnetapp.Repository;

namespace dotnetapp.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderRepository _repo;
         public OrderService()
        {
           
        }

        public OrderService(OrderRepository repo)
        {
            _repo = repo;
        }

        public List<Order> GetOrders() => _repo.GetAll();

        public Order GetOrder(int id) => _repo.Get(id);

        public Order SaveOrder(Order order) => _repo.Add(order);

        public Order UpdateOrder(int id, Order order) => _repo.Update(id, order);

        public bool DeleteOrder(int id) => _repo.Delete(id);
    }
}

// File: Controllers/BookController.cs
using Microsoft.AspNetCore.Mvc;
using dotnetapp.Models;
using dotnetapp.Services;

namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _service;

        public BooksController(IBookService service)
        {
            _service = service;
        }

        // GET: api/books
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = _service.GetBooks();
            return Ok(books);
        }

        // GET: api/books/{id}
        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            var book = _service.GetBook(id);
            if (book == null) return NotFound();
            return Ok(book);
        }

        // POST: api/books
        [HttpPost]
        public IActionResult AddBook([FromBody] Book book)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (book == null) return BadRequest();

            var added = _service.SaveBook(book);
            return Ok(added); // per requirement: 200 OK on creation
        }

        // PUT: api/books/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] Book book)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updated = _service.UpdateBook(id, book);
            if (updated == null) return NotFound();
            return NoContent(); // 204 No Content
        }

        // DELETE: api/books/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var deleted = _service.DeleteBook(id);
            return Ok();
        }
    }
}

// File: Controllers/OrderController.cs
using Microsoft.AspNetCore.Mvc;
using dotnetapp.Models;
using dotnetapp.Services;

namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrdersController(IOrderService service)
        {
            _service = service;
        }

        // GET: api/orders
        [HttpGet]
        public IActionResult GetAllOrders()
        {
            var orders = _service.GetOrders();
            return Ok(orders);
        }

        // GET: api/orders/{id}
        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            var order = _service.GetOrder(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        // POST: api/orders
        [HttpPost]
        public IActionResult AddOrder([FromBody] Order order)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (order == null) return BadRequest();

            var added = _service.SaveOrder(order);
            return Ok(added); // per requirement: 200 OK on creation
        }

        // PUT: api/orders/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, [FromBody] Order order)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updated = _service.UpdateOrder(id, order);
            if (updated == null) return NotFound();
            return NoContent(); // 204 No Content
        }

        // DELETE: api/orders/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var deleted = _service.DeleteOrder(id);
            return Ok();
        }
    }
}

// File: Program.cs
using dotnetapp.Repository;
using dotnetapp.Services;

var builder = WebApplication.CreateBuilder(args);

// Run on port 8080
builder.WebHost.UseUrls("http://localhost:8080");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register repositories and services
builder.Services.AddSingleton<BookRepository>();
builder.Services.AddSingleton<OrderRepository>();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Notes:
// - Place these files in the appropriate folders inside a new ASP.NET Core Web API project named dotnetapp.
// - Use `dotnet restore`, `dotnet build`, and `dotnet run` inside the dotnetapp folder to run the app.
// - Swagger UI will be available at http://localhost:8080/swagger/index.html (or /swagger).
// - The controllers are named BooksController and OrdersController; routes will be /api/books and /api/orders.
// - The code intentionally returns 200 OK for POST (as requested) and 204 No Content for successful PUT updates.
// - Model validation uses ModelState; add [Required] attributes to models if you want stricter validation.
