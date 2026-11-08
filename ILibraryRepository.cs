namespace BookLibraryManager
{
    interface ILibraryRepository

    {
        Task AddBookAsync(Book book);
        Task<IReadOnlyList<Book>> GetAllBooksAsync();
        Task<Book?> FindBookByIdAsync(int id);
        Task UpdateBookAsync(Book book);
    }

}