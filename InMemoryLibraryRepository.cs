namespace BookLibraryManager
{
    class InMemoryLibraryRepository : ILibraryRepository
    {
        private readonly List<Book> _books = new();

        public Task AddBookAsync(Book book)
        {
            _books.Add(book);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<Book>> GetAllBooksAsync()
        {
            return Task.FromResult<IReadOnlyList<Book>>(_books.AsReadOnly());
        }

        public Task<Book?> FindBookByIdAsync(int id)
        {
            return Task.FromResult(_books.FirstOrDefault(b => b.Id == id));
        }
        public Task UpdateBookAsync(Book book)
        {
            return Task.CompletedTask;
        }
    }
}