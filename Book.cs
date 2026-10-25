namespace BookLibraryManager
{
    class Book


    {
        private static int _nextId = 1;
        public int Id { get; private set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public bool IsBorrowed { get; set; }
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
        //  ToString method 
        public override string ToString()
        {
            return $"| {Id,-5} | {Title,-30} | {Author,-20} | {IsBorrowed,-10} |";
        }


    }
}