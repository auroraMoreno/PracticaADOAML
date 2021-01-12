using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PracticaADOAML.Data;
using PracticaADOAML.Models;

namespace PracticaADOAML.Controllers
{
    public class GeneroController : Controller
    {
        LibreriaContext context;

        public GeneroController()
        {
            this.context = new LibreriaContext();
        }

        public IActionResult Index()
        {
            List<Genero> generos = this.context.GetGeneros();
            return View(generos);
        }

        public IActionResult BuscarLibros()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BuscarLibros(int idGenero)
        {
            List<Libro> libros = this.context.BuscarLibrosGenero(idGenero);
            if(libros == null)
            {
                ViewBag.Mensaje = "No hay libros en ese género";
                return View();
            }
            else
            {
                return View(libros);
            }
        }

        public IActionResult InsertarGenero()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InsertarGenero(Genero genero)
        {
            this.context.CrearGenero(genero.GeneroNombre);
            return RedirectToAction("Index");
        }

        public IActionResult GetGenero(int idGenero)
        {
            Genero g = this.context.GetGenero(idGenero);
            return View(g);
        }

        [HttpPost]
        public IActionResult DeleteGenero(int idGenero)
        {
            this.context.EliminarGenero(idGenero);
            return RedirectToAction("Index");
        }
    }
}
