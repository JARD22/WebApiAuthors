namespace WebApiAutores.Entities
{
    public class Author
    {
        public int id { get; set; }
        public string name{ get; set; }

        public List<Book> Books { get; set; }
    }
}
