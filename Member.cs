namespace BookLibraryManager
{
    class Member
    {
        private static int _nextId = 1;
        public int Id { get; private set; }
        public string Name { get; set; }
        private readonly List<Book> _borrowedBooks = new();

        public Member(string name)
        {
            Id = _nextId++;
            Name = name;
        }
        public void BorrowBook(Book book)
        {
            book.Borrow();
            _borrowedBooks.Add(book);
        }
        public void ReturnBook(Book book)
        {
            if (!_borrowedBooks.Contains(book))
                throw new LibraryException("This member did not borrow this book.");
            book.Return();
            _borrowedBooks.Remove(book);
        }

        public IReadOnlyList<Book> BorrowedBooks => _borrowedBooks;


    }



}