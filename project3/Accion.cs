// Claudia Vidal Otero (claudia.votero@udc.es)
// Aldana Smyna Medina Lostaunau (aldana.medina@udc.es)
// Grupo 2 (Jueves)

/// <summary>
/// Representa una acción del sistema de planificación STRIPS.
/// Cada acción tiene un nombre, un conjunto de precondiciones que deben cumplirse
/// para que pueda aplicarse, y dos conjuntos de efectos: los predicados que se agregan
/// y los que se eliminan al aplicar la acción.
/// </summary>
class Accion
{
    public string Nombre { get; private set; }
    public List<Predicado> Precondiciones { get; private set; }
    public List<Predicado> EfectosAgregar { get; private set; }
    public List<Predicado> EfectosEliminar { get; private set; }

    public Accion(string nombre, List<Predicado> precondiciones, List<Predicado> efectosAgregar, List<Predicado> efectosEliminar)
    {
        Nombre = nombre;
        Precondiciones = precondiciones;
        EfectosAgregar = efectosAgregar;
        EfectosEliminar = efectosEliminar;
    }

    public override string ToString() => Nombre;
}
