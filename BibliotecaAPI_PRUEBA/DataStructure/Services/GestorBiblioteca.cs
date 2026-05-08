using System.Collections.Generic;
using BibliotecaAPI.DataStructures.Models;

namespace BibliotecaAPI.DataStructures.Services
{
    public class GestorBiblioteca
    {
        // Estructuras principales
        public List<Libro> ListaLibros { get; set; }
        public Dictionary<int, Libro> DiccionarioLibros { get; set; }

        public GestorBiblioteca()
        {
            ListaLibros = new List<Libro>();
            DiccionarioLibros = new Dictionary<int, Libro>();
        }

        public void RegistrarNuevoLibro(Libro libro)
        {
            ListaLibros.Add(libro);
            DiccionarioLibros.Add(libro.Id, libro);
        }

        public Libro BuscarPorId(int id)
        {
            if (DiccionarioLibros.ContainsKey(id))
            {
                return DiccionarioLibros[id];
            }
            return null;
        }
    }
}