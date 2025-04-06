/// <summary>
/// Representa una acción STRIPS con nombre, precondiciones y efectos
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

