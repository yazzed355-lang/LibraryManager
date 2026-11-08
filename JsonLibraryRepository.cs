using System.Text.Json;

namespace BookLibraryManager
{
    class JsonLibraryRepository : ILibraryRepository
    {
        private readonly string _filePath;
        private List<Book> _books = new();
        private bool _isLoaded;

        public JsonLibraryRepository(string filePath)
        {
            _filePath = filePath;
        }

        private async Task EnsureLoadedAsync()
        {
            if (_isLoaded)
                return;

            if (File.Exists(_filePath))
            {
                await using var stream = File.OpenRead(_filePath);
                _books = await JsonSerializer.DeserializeAsync<List<Book>>(stream) ?? new();
            }

            _isLoaded = true;
        }

        private async Task SaveAsync()
        {
            await using var stream = File.Create(_filePath);
            await JsonSerializer.SerializeAsync(
                stream, _books, new JsonSerializerOptions { WriteIndented = true });
        }

        public async Task AddBookAsync(Book book)
        {
            await EnsureLoadedAsync();
            _books.Add(book);
            await SaveAsync();
        }

        public async Task<IReadOnlyList<Book>> GetAllBooksAsync()
        {
            await EnsureLoadedAsync();
            return _books.AsReadOnly();
        }

        public async Task<Book?> FindBookByIdAsync(int id)
        {
            await EnsureLoadedAsync();
            return _books.FirstOrDefault(b => b.Id == id);
        }
        public async Task UpdateBookAsync(Book book)
        {
            await EnsureLoadedAsync();
            await SaveAsync();
        }
    }
}