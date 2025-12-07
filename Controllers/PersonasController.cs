using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Registro_Personas.Data;
using Registro_Personas.Models;

namespace Registro_Personas.Controllers
{
    public class PersonasController : Controller
    {
        private readonly PersonasRepo _context;

        public PersonasController(PersonasRepo context)
        {
            _context = context;
        }

        // INDEX: muestra todas las personas ordenadas por nombre y el promedio de edad
        public IActionResult Index()
        {
            // a) Mostrar personas ordenadas por nombre (LINQ)
            var personasOrdenadas = _context.Personas
                                            .OrderBy(p => p.Nombre)
                                            .ToList();

            // b) Mostrar el promedio de edad (si hay registros)
            double promedioEdad = 0;
            if (_context.Personas.Any())
            {
                promedioEdad = _context.Personas.Average(p => p.Edad);
            }

            ViewBag.PromedioEdad = promedioEdad;
            return View(personasOrdenadas);
        }

        // DETALLES (opcional)
        public IActionResult Details(int id)
        {
            var persona = _context.Personas.Find(id);
            if (persona == null) return NotFound();
            return View(persona);
        }

        // BUSCAR: GET muestra el formulario
        public IActionResult Buscar()
        {
            return View();
        }

        // BUSCAR: POST recibe filtros (nombre o edad) y devuelve resultados
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Buscar(string nombre, int? edad)
        {
            // Si ambos filtros están vacíos, devolver todos
            var query = _context.Personas.AsQueryable();

            if (!string.IsNullOrEmpty(nombre))
            {
                // usar Contains para buscar por nombre (case-sensitive depende de la BD)
                query = query.Where(p => p.Nombre.Contains(nombre));
            }

            if (edad.HasValue)
            {
                query = query.Where(p => p.Edad == edad.Value);
            }

            var resultado = query.ToList();
            ViewBag.FiltroNombre = nombre;
            ViewBag.FiltroEdad = edad;
            return View("BuscarResultados", resultado); // usar vista separada o la misma
        }

        // BUSCAR UNA PERSONA ESPECÍFICA (por ejemplo por Id o por nombre exacto)
        public IActionResult BuscarPorId(int id)
        {
            // c) Buscar una persona en específico.
            var persona = _context.Personas.Find(id);
            if (persona == null) return NotFound();
            return View("Details", persona);
        }

        public IActionResult BuscarPorNombreExacto(string nombre)
        {
            var persona = _context.Personas.FirstOrDefault(p => p.Nombre == nombre);
            if (persona == null) return NotFound();
            return View("Details", persona);
        }

        // EDIT: GET - mostrar formulario con datos actuales
        public IActionResult Edit(int id)
        {
            var persona = _context.Personas.Find(id);
            if (persona == null) return NotFound();
            return View(persona);
        }

        // EDIT: POST - guardar cambios
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Personas model)
        {
            if (id != model.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                var personaExistente = _context.Personas.Find(id);
                if (personaExistente == null) return NotFound();

                // actualizar campos
                personaExistente.Nombre = model.Nombre;
                personaExistente.Edad = model.Edad;

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // DELETE: GET - confirmación
        public IActionResult Delete(int id)
        {
            var persona = _context.Personas.Find(id);
            if (persona == null) return NotFound();
            return View(persona);
        }

        // DELETE: POST - realizar eliminación
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var persona = _context.Personas.Find(id);
            if (persona == null) return NotFound();

            _context.Personas.Remove(persona);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // CREATE: (opcional) para añadir personas durante pruebas
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Personas model)
        {
            if (ModelState.IsValid)
            {
                _context.Personas.Add(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}

