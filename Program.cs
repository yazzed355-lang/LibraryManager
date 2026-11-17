using Microsoft.Extensions.DependencyInjection;
using BookLibraryManager;

var services = new ServiceCollection();

string filePath = "library.json";

services.AddSingleton<ILibraryRepository>(new JsonLibraryRepository(filePath));

var provider = services.BuildServiceProvider();
var repository = provider.GetRequiredService<ILibraryRepository>();

bool running = true;

// Add sample books only when the file is empty
if ((await repository.GetAllBooksAsync()).Count == 0)
{
    await repository.AddBookAsync(new Book("The Great Gatsby", "F. Scott Fitzgerald"));
    await repository.AddBookAsync(new Book("To Kill a Mockingbird", "Harper Lee"));
    await repository.AddBookAsync(new Book("1984", "George Orwell"));
    await repository.AddBookAsync(new Book("Pride and Prejudice", "Jane Austen"));
    await repository.AddBookAsync(new Book("The Catcher in the Rye", "J.D. Salinger"));
}

void PrintHeader()
{
    Console.WriteLine($"| {"ID",-5} | {"Title",-30} | {"Author",-20} | {"IsBorrowed",-10} |");
    Console.WriteLine(new string('-', 76));
}

// Add a new book from user input
async Task AddBookFromInput()
{
    Console.Write("Enter title: ");
    var title = Console.ReadLine();
    Console.Write("Enter author: ");
    var author = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author))
        throw new LibraryException("Title and author cannot be empty.");

    await repository.AddBookAsync(new Book(title, author));
    Console.WriteLine("Book added.");
}

// Display all books in the library
async Task DisplayBooks()
{
    var books = await repository.GetAllBooksAsync();

    PrintHeader();
    foreach (var book in books)
        Console.WriteLine(book);
}

// Search for books by title or author
async Task SearchBooks()
{
    Console.Write("Search: ");
    string keyword = Console.ReadLine() ?? "";

    var books = await repository.GetAllBooksAsync();
    var results = books
        .Where(b => b.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                 || b.Author.Contains(keyword, StringComparison.OrdinalIgnoreCase))
        .ToList();

    if (results.Count == 0)
    {
        Console.WriteLine("No books found.");
        return;
    }

    PrintHeader();
    foreach (var book in results)
        Console.WriteLine(book);
}

// Find a book by its ID
async Task<Book> FindById()
{
    Console.Write("Book id: ");
    int id = int.Parse(Console.ReadLine() ?? "");

    var book = await repository.FindBookByIdAsync(id);
    if (book == null)
        throw new LibraryException("Book not found.");

    return book;
}

// Borrow a book by its ID
async Task BorrowBook()
{
    var book = await FindById();
    book.Borrow();
    await repository.UpdateBookAsync(book);
    Console.WriteLine("Book borrowed.");
}

async Task ReturnBook()
{
    var book = await FindById();
    book.Return();
    await repository.UpdateBookAsync(book);
    Console.WriteLine("Book returned.");
}

void DisplayMenu()
{
    Console.WriteLine("\nLibrary Manager");
    Console.WriteLine("1. Add Book");
    Console.WriteLine("2. Display Books");
    Console.WriteLine("3. Search");
    Console.WriteLine("4. Borrow");
    Console.WriteLine("5. Return");
    Console.WriteLine("0. Exit");
    Console.Write("Enter your choice: ");
}

while (running)
{
    DisplayMenu();
    var choice = Console.ReadLine();

    try
    {
        Func<Task> action = choice switch
        {
            "1" => AddBookFromInput,
            "2" => DisplayBooks,
            "3" => SearchBooks,
            "4" => BorrowBook,
            "5" => ReturnBook,
            "0" => () =>
            {
                running = false;
                return Task.CompletedTask;
            }
            ,
            _ => throw new LibraryException("Invalid option.")
        };

        await action();
    }
    catch (LibraryException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    catch (FormatException)
    {
        Console.WriteLine("Please enter a valid number.");
    }
}