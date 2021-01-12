using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PracticaADOAML.Data;
using PracticaADOAML.Models;

namespace PracticaADOAML.Controllers
{
    public class LibroController : Controller
    {
        LibreriaContext context;

        public LibroController()
        {
            this.context = new LibreriaContext();
        }
        public IActionResult Index()
        {
            List<Libro> libros = this.context.GetLibros();
            return View(libros);
        }

        public IActionResult LibroDetalles(int idLibro)
        {
            Libro libro = this.context.GetLibro(idLibro);
            return View(libro);
        }

        public IActionResult InsertarLibro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InsertarLibro(Libro libro)
        {
            this.context.CrearLibro(libro.Titulo, libro.Autor,libro.Sinopsis, libro.Imagen, libro.IdGenero);
            return RedirectToAction("Index");
        }

        public IActionResult UpdateLibro(int idLibro)
        {
            Libro l = this.context.GetLibro(idLibro);
            return View(l);
        }

        [HttpPost]
        public IActionResult UpdateLibro(Libro libro)
        {
            this.context.UpdateLibro(libro);
            return RedirectToAction("Index");
        }
    }
}
