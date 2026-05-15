using System;
using System.Collections.Generic;
using BibliotecaAPI.DataStructures.Models;

namespace BibliotecaAPI.DataStructures.Services
{
    public class GestorBiblioteca
    {
       
        public List<Libro> ListaLibros { get; set; }
        public Dictionary<int, Libro> DiccionarioLibros { get; set; }

       //Cola de préstamos
        public Queue<Prestamo> ColaPrestamos { get; set; }

        //Pila de historial
        public Stack<string> HistorialAcciones { get; set; }

        public GestorBiblioteca()
        {
            ListaLibros = new List<Libro>();
            DiccionarioLibros = new Dictionary<int, Libro>();
            
            ColaPrestamos = new Queue<Prestamo>();
            HistorialAcciones = new Stack<string>();
        }

        public void RegistrarNuevoLibro(Libro libro)
        {
            ListaLibros.Add(libro);
            DiccionarioLibros.Add(libro.Id, libro);
           
            HistorialAcciones.Push($"Libro registrado: {libro.Titulo}");
        }

        
        public void SolicitarPrestamo(Prestamo prestamo)
        {
            ColaPrestamos.Enqueue(prestamo);
            HistorialAcciones.Push($"Usuario {prestamo.Usuario.Nombre} entró en la fila de espera.");
        }

        
        public string ObtenerUltimaActividad()
        {
            return HistorialAcciones.Count > 0 ? HistorialAcciones.Peek() : "No hay historial.";
        }
    }
}