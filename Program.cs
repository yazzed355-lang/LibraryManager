using BookLibraryManager;

var bookList = new List<Book>();
bool running = true;

void PrintHeader()
{
    Console.WriteLine($"| {"ID",-5} | {"Title",-30} | {"Author",-20} | {"IsBorrowed",-10} |");
    Console.WriteLine(new string('-', 76));
}
// Add a new book from user input
void AddBookFromInput()
{
    Console.Write("Enter title: ");
    var title = Console.ReadLine();
    Console.Write("Enter author: ");
    var author = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author))
        throw new LibraryException("Title and author cannot be empty.");

    bookList.Add(new Book(title, author));
    Console.WriteLine("Book added.");
}
// Display all books in the library
void DisplayBooks()
{
    PrintHeader();
    foreach (var book in bookList)
        Console.WriteLine(book);
}
// Search for books by title or author
void SearchBooks()
{
    Console.Write("Search: ");
    string keyword = Console.ReadLine() ?? "";

    var results = bookList
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
Book FindById()
{
    Console.Write("Book id: ");
    int id = int.Parse(Console.ReadLine() ?? "");

    var book = bookList.FirstOrDefault(b => b.Id == id);
    if (book == null)
        throw new LibraryException("Book not found.");

    return book;
}
// Borrow a book by its ID
void BorrowBook()
{
    var book = FindById();
    if (book.IsBorrowed)
        throw new LibraryException("This book is already borrowed.");

    book.IsBorrowed = true;
    Console.WriteLine("Book borrowed.");
}

void ReturnBook()
{
    var book = FindById();
    if (!book.IsBorrowed)
        throw new LibraryException("This book was not borrowed.");

    book.IsBorrowed = false;
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
        Action action = choice switch
        {
            "1" => AddBookFromInput,
            "2" => DisplayBooks,
            "3" => SearchBooks,
            "4" => BorrowBook,
            "5" => ReturnBook,
            "0" => () => running = false,
            _ => throw new LibraryException("Invalid option.")
        };

        action();
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