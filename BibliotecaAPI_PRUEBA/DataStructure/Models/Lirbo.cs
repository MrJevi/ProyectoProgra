namespace BibliotecaAPI.DataStructures.Models
{
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Categoria { get; set; }
        public bool Disponible { get; set; }

        public Libro(int id, string titulo, string autor, string categoria)
        {
            Id = id;
            Titulo = titulo;
            Autor = autor;
            Categoria = categoria;
            Disponible = true;
        }
    }
}