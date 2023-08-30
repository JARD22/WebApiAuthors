namespace WebApiAutores.Entities
{
    public class Book
    {
        public int id { get; set; }
        public string title{ get; set; }
        public int authorId { get; set; }
        public  Author Author { get; set; }

    }
}
