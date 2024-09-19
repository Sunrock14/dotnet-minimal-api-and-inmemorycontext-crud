using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// In-memory database için DbContext ayarlarý
builder.Services.AddDbContext<BookContext>(opt => opt.UseInMemoryDatabase("BookList"));

var app = builder.Build();

// CRUD iþlemleri için veri modeli (Book)
app.MapGet("/books", async (BookContext db) => await db.Books.ToListAsync());

app.MapGet("/books/{id}", async (int id, BookContext db) =>
    await db.Books.FindAsync(id) is Book book ? Results.Ok(book) : Results.NotFound());

app.MapPost("/books", async (Book book, BookContext db) =>
{
    db.Books.Add(book);
    await db.SaveChangesAsync();

    return Results.Created($"/books/{book.Id}", book);
});

app.MapPut("/books/{id}", async (int id, Book updatedBook, BookContext db) =>
{
    var book = await db.Books.FindAsync(id);

    if (book is null) return Results.NotFound();

    book.Title = updatedBook.Title;
    book.Author = updatedBook.Author;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/books/{id}", async (int id, BookContext db) =>
{
    var book = await db.Books.FindAsync(id);

    if (book is null) return Results.NotFound();

    db.Books.Remove(book);
    await db.SaveChangesAsync();

    return Results.Ok(book);
});

app.Run();

// Veri modeli
class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
}

// DbContext
class BookContext : DbContext
{
    public BookContext(DbContextOptions<BookContext> options) : base(options) { }

    public DbSet<Book> Books => Set<Book>();
}
