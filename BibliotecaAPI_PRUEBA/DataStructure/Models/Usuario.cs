namespace BibliotecaAPI.DataStructures.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }

        public Usuario(int id, string nombre, string correo)
        {
            Id = id;
            Nombre = nombre;
            Correo = correo;
        }
    }
}