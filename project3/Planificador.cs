// Claudia Vidal Otero (claudia.votero@udc.es)
// Aldana Smyna Medina Lostaunau (aldana.medina@udc.es)
// Grupo 2 (Jueves)

/// <summary>
/// Clase encargada de planificar una secuencia de acciones para resolver el 8-puzzle
/// utilizando búsqueda informada (A*).
/// </summary>
class Planificador
{
    /// <summary>
    /// Lista de acciones disponibles (mover en las 4 direcciones).
    /// </summary>
    public List<Accion> Acciones { get; private set; }

    /// <summary>
    /// Constructor que inicializa las acciones posibles.
    /// </summary>
    public Planificador()
    {
        Acciones = new List<Accion>();
        DefinirAcciones();
    }

    /// <summary>
    /// Define las cuatro acciones básicas del 8-puzzle.
    /// </summary>
    private void DefinirAcciones()
    {
        Acciones.Add(new Accion("MoverArriba"));
        Acciones.Add(new Accion("MoverAbajo"));
        Acciones.Add(new Accion("MoverIzquierda"));
        Acciones.Add(new Accion("MoverDerecha"));
    }

    /// <summary>
    /// Encuentra un plan (lista de movimientos) desde un estado inicial hasta uno objetivo,
    /// utilizando el algoritmo de búsqueda A*.
    /// </summary>
    /// <param name="inicial">Estado inicial del tablero</param>
    /// <param name="objetivo">Estado objetivo deseado</param>
    /// <returns>Lista de movimientos (como strings), o null si no hay solución</returns>
    public List<string> EncontrarPlan(Estado inicial, Estado objetivo)
    {
        // Conjunto ordenado según costo f(n) = g(n) + h(n). Si hay empate, se usa ToString para evitar conflictos.
        SortedSet<Estado> frontera = new SortedSet<Estado>(Comparer<Estado>.Create((estadoA, estadoB) =>
        {
            int comparacionCosto = estadoA.CalcularCostoTotal().CompareTo(estadoB.CalcularCostoTotal());
            if (comparacionCosto == 0)
                comparacionCosto = estadoA.ToString().CompareTo(estadoB.ToString());
            return comparacionCosto;
        }));

        // Conjunto para guardar los estados ya visitados
        HashSet<Estado> visitados = new HashSet<Estado>();

        frontera.Add(inicial);

        // Bucle principal de búsqueda
        while (frontera.Count > 0)
        {
            Estado estadoActual = frontera.Min;
            frontera.Remove(estadoActual);

            // Verificar si se ha alcanzado el estado objetivo
            if (estadoActual.Satisface(objetivo))
            {
                return ReconstruirCamino(estadoActual);
            }

            visitados.Add(estadoActual);

            // Explorar vecinos del estado actual
            foreach (Accion accion in Acciones)
            {
                Estado nuevoEstado = estadoActual.AplicarAccion(accion);

                // Solo considerar estados no visitados y no presentes en la frontera
                if (!visitados.Contains(nuevoEstado) && !frontera.Contains(nuevoEstado))
                {
                    frontera.Add(nuevoEstado);
                }
            }
        }

        // Si no se encuentra solución, se retorna null
        return null;
    }

    /// <summary>
    /// Reconstruye el camino desde el estado inicial hasta el actual, siguiendo los punteros al padre.
    /// </summary>
    /// <param name="estado">Estado final alcanzado</param>
    /// <returns>Lista de acciones (como strings) desde el inicio hasta el objetivo</returns>
    private List<string> ReconstruirCamino(Estado estado)
    {
        List<string> plan = new List<string>();

        while (estado.Padre != null)
        {
            string accion = ObtenerAccionDesdeEstados(estado.Padre, estado);
            plan.Add(accion);
            estado = estado.Padre;
        }

        plan.Reverse(); // Invertir para obtener el orden correcto
        return plan;
    }
    /// <summary>
    /// Determina qué acción llevó desde un estado padre a su hijo.
    /// </summary>
    /// <param name="padre">Estado anterior</param>
    /// <param name="hijo">Estado actual</param>
    /// <returns>Nombre de la acción aplicada</returns>
    private string ObtenerAccionDesdeEstados(Estado padre, Estado hijo)
    {
        (int filaPadre, int columnaPadre) = padre.ObtenerPosVacia();
        (int filaHijo, int columnaHijo) = hijo.ObtenerPosVacia();

        if (filaPadre > filaHijo) return "MoverArriba";
        if (filaPadre < filaHijo) return "MoverAbajo";
        if (columnaPadre > columnaHijo) return "MoverIzquierda";
        if (columnaPadre < columnaHijo) return "MoverDerecha";

        return ""; // Acción desconocida (no debería pasar)
    }
}
