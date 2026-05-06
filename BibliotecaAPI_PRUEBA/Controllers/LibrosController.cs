using BibliotecaAPI_PRUEBA.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class LibrosController : ControllerBase
{

    static Queue<string> prestamos = new Queue<string>();
    static Stack<string> historial = new Stack<string>();


    static List<Libro> libros = new List<Libro>()
    {
        new Libro { Id = 1, Titulo = "C# Básico", Autor = "Juan", Categoria = "Programación" },
        new Libro { Id = 2, Titulo = "Estructuras de Datos", Autor = "Ana", Categoria = "Informática" }
    };

    [HttpGet]
    public IActionResult GetLibros()
    {
        return Ok(libros);
    }

    [HttpPost]
    public IActionResult AgregarLibro([FromBody] Libro libro)
    {
        libros.Add(libro);
        return Ok(libro);
    }

    [HttpPut("{id}")]
    public IActionResult ActualizarLibro(int id, [FromBody] Libro libroActualizado)
    {
        var libro = libros.FirstOrDefault(l => l.Id == id);

        if (libro == null)
        {
            return NotFound("Libro no encontrado");
        }

        libro.Titulo = libroActualizado.Titulo;
        libro.Autor = libroActualizado.Autor;
        libro.Categoria = libroActualizado.Categoria;

        return Ok(libro);
    }

    [HttpDelete("{id}")]
    public IActionResult EliminarLibro(int id)
    {
        var libro = libros.FirstOrDefault(l => l.Id == id);

        if (libro == null)
        {
            return NotFound("Libro no encontrado");
        }

        libros.Remove(libro);

        return Ok("Libro eliminado correctamente");
    }

    [HttpPost("prestamo/{id}")]
    public IActionResult PrestarLibro(int id)
    {
        var libro = libros.FirstOrDefault(l => l.Id == id);

        if (libro == null)
        {
            return NotFound("Libro no encontrado");
        }

        prestamos.Enqueue($"Libro prestado: {libro.Titulo}");
        historial.Push($"Se prestó el libro: {libro.Titulo}");

        return Ok("Préstamo registrado");
    }

    [HttpGet("prestamos")]
    public IActionResult VerPrestamos()
    {
        return Ok(prestamos);
    }

    [HttpGet("historial")]
    public IActionResult VerHistorial()
    {
        return Ok(historial);
    }

    [HttpPost("subir-pdf")]
    public async Task<IActionResult> SubirPdf(IFormFile archivo)
    {
        if (archivo == null || archivo.Length == 0)
            return BadRequest("No se envió ningún archivo");

        var rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Libros");

        if (!Directory.Exists(rutaCarpeta))
            Directory.CreateDirectory(rutaCarpeta);

        var nombreArchivo = Path.GetFileName(archivo.FileName);
        var rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);

        //  Verificar si ya existe
        if (System.IO.File.Exists(rutaCompleta))
        {
            return BadRequest(new
            {
                mensaje = "Ya existe un archivo con ese nombre",
                sugerencia = "Cambia el nombre del archivo antes de subirlo"
            });
        }

        using (var stream = new FileStream(rutaCompleta, FileMode.Create))
        {
            await archivo.CopyToAsync(stream);
        }

        return Ok(new
        {
            mensaje = "Archivo subido correctamente",
            archivo = nombreArchivo
        });
    }

    [HttpGet("descargar/{nombreArchivo}")]
    public IActionResult DescargarPdf(string nombreArchivo)
    {
        var ruta = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Libros", nombreArchivo);

        if (!System.IO.File.Exists(ruta))
            return NotFound("Archivo no encontrado");

        var bytes = System.IO.File.ReadAllBytes(ruta);

        return File(bytes, "application/pdf", nombreArchivo);
    }

    [HttpGet("listar-pdfs")]
    public IActionResult ListarPdfs()
    {
        var rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Libros");

        if (!Directory.Exists(rutaCarpeta))
            return Ok(new List<string>());

        var archivos = Directory.GetFiles(rutaCarpeta)
            .Select(Path.GetFileName)
            .ToList();

        return Ok(archivos);
    }

}