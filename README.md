# Registro_Personas

Resumen de las consultas LINQ solicitadas

Filtrar por nombre (contiene):

var resultado = _context.Personas.Where(p => p.Nombre.Contains(nombre)).ToList();


Eliminar por id:

var persona = _context.Personas.Find(id);
_context.Personas.Remove(persona);
_context.SaveChanges();


Editar / actualizar:

var persona = _context.Personas.Find(id);
persona.Nombre = nuevoNombre;
persona.Edad = nuevaEdad;
_context.SaveChanges();


Mostrar personas ordenadas por nombre:

var ordenadas = _context.Personas.OrderBy(p => p.Nombre).ToList();


Promedio de edad:

double promedio = _context.Personas.Any() ? _context.Personas.Average(p => p.Edad) : 0;


Buscar persona específica (por id o por nombre exacto):

var porId = _context.Personas.Find(id);
var porNombre = _context.Personas.FirstOrDefault(p => p.Nombre == nombreExacto);
