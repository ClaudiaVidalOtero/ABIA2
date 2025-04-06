// Claudia Vidal Otero (claudia.votero@udc.es)
// Aldana Smyna Medina Lostaunau (aldana.medina@udc.es)
// Grupo 2 (Jueves)

/// <summary>
/// Representa un estado dentro del espacio de búsqueda del puzzle, definido como un conjunto de predicados.
/// Cada estado puede tener una referencia a su padre, la acción que lo generó y el costo acumulado.
/// Proporciona métodos para verificar si cumple un objetivo, aplicar acciones válidas,
/// comparar estados y mostrar el estado en formato de tablero.
/// </summary>
class Estado
{
    public HashSet<Predicado> Predicados { get; private set; }
    public Estado Padre { get; private set; }
    public Accion AccionAplicada { get; private set; }
    public int Costo { get; private set; }

    public Estado(HashSet<Predicado> predicados, Estado padre = null, Accion accionAplicada = null, int costo = 0)
    {
        // Se crea una copia del conjunto de predicados para evitar referencias compartidas
        Predicados = new HashSet<Predicado>(predicados);
        Padre = padre;
        AccionAplicada = accionAplicada;
        Costo = costo;
    }

    /// <summary>
    /// Verifica si el estado actual satisface todos los predicados del estado objetivo.
    /// </summary>
    public bool Satisface(Estado objetivo)
    {
        return objetivo.Predicados.All(p => Predicados.Contains(p));
    }

    /// <summary>
    /// Intenta aplicar una acción sobre el estado actual.
    /// Si las precondiciones se cumplen, retorna un nuevo estado con los efectos aplicados.
    /// </summary>
    public Estado Aplicar(Accion accion)
    {
        // Verifica que todas las precondiciones estén presentes en el estado actual
        if (!accion.Precondiciones.All(p => Predicados.Contains(p)))
            return null;

        // Clona los predicados actuales para crear el nuevo conjunto
        var nuevosPredicados = new HashSet<Predicado>(Predicados);

        // Aplica los efectos eliminando e insertando predicados según la acción
        foreach (var p in accion.EfectosEliminar)
            nuevosPredicados.Remove(p);
        foreach (var p in accion.EfectosAgregar)
            nuevosPredicados.Add(p);

        // Devuelve un nuevo estado con los predicados modificados
        return new Estado(nuevosPredicados, this, accion, this.Costo + 1);
    }

    /// <summary>
    /// Compara dos estados verificando si tienen el mismo conjunto de predicados.
    /// </summary>
    public override bool Equals(object obj)
    {
        return obj is Estado otro && Predicados.SetEquals(otro.Predicados);
    }

    /// <summary>
    /// Genera un código hash basado en el conjunto ordenado de predicados del estado.
    /// </summary>
    public override int GetHashCode()
    {
        int hash = 17;
        foreach (var p in Predicados.OrderBy(p => p.ToString()))
            hash = hash * 23 + p.GetHashCode();
        return hash;
    }

    /// <summary>
    /// Muestra el estado actual en formato de tablero 3x3.
    /// </summary>
    public void Mostrar()
    {
        var tablero = new int[3, 3];

        // Se llena el tablero según los predicados "En(ficha, fila, columna)"
        foreach (var p in Predicados)
        {
            if (p.Nombre == "En")
            {
                int ficha = p.Argumentos[0];
                int fila = p.Argumentos[1];
                int col = p.Argumentos[2];
                tablero[fila, col] = ficha;
            }
        }

        // Se imprime el tablero en consola
        for (int fila = 0; fila < 3; fila++)
        {
            for (int col = 0; col < 3; col++)
            {
                Console.Write(tablero[fila, col] == 0 ? "_  " : tablero[fila, col] + "  ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}

