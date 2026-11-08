using System.Text.Json.Serialization;


namespace BookLibraryManager
{
    class Book


    {
        private static int _nextId = 1;
        public int Id { get; private set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public bool IsBorrowed { get; private set; }
        // Constructor to initialize a new book with default values
        public Book()
        {
            Id = _nextId++;
            Title = "Unknown";
            Author = "Unknown";
            IsBorrowed = false;

        }

        // Constructor to initialize a new book with specified title and author
        public Book(string title, string author)
        {
            Id = _nextId++;
            Title = title;
            Author = author;
            IsBorrowed = false;

        }

        // Used by JSON to rebuild a saved book with its original values
        [JsonConstructor]
        public Book(int id, string title, string author, bool isBorrowed)
        {
            Id = id;
            Title = title;
            Author = author;
            IsBorrowed = isBorrowed;

            if (id >= _nextId)
                _nextId = id + 1;
        }

        public void Borrow()
        {
            if (IsBorrowed)
                throw new LibraryException("This book is already borrowed.");

            IsBorrowed = true;
        }
        public void Return()
        {
            if (!IsBorrowed)
                throw new LibraryException("This book was not borrowed.");

            IsBorrowed = false;
        }

        //  ToString method 
        public override string ToString()
        {
            return $"| {Id,-5} | {Title,-30} | {Author,-20} | {IsBorrowed,-10} |";
        }



    }

}