// Claudia Vidal Otero (claudia.votero@udc.es)
// Aldana Smyna Medina Lostaunau (aldana.medina@udc.es)
// Grupo 2 (Jueves)

/// <summary>
/// Clase encargada de generar planes utilizando el modelo STRIPS.
/// Emplea búsqueda en anchura (BFS) para encontrar una secuencia de acciones
/// que transforme el estado inicial en el estado objetivo.
/// </summary>
class Planificador
{
    /// <summary>
    /// Lista de acciones disponibles en el dominio.
    /// Cada acción tiene precondiciones y efectos positivos y negativos.
    /// </summary>
    public List<Accion> Acciones { get; private set; }

    public Planificador()
    {
        Acciones = new List<Accion>();
    }

    /// <summary>
    /// Agrega una acción al conjunto de acciones posibles que el planificador puede usar.
    /// </summary>
    public void AgregarAccion(Accion accion)
    {
        Acciones.Add(accion);
    }

    /// <summary>
    /// Genera un plan para alcanzar el estado objetivo desde un estado inicial dado.
    /// Aplica búsqueda en anchura (BFS) sobre el espacio de estados posibles generados
    /// por la aplicación de acciones válidas.
    /// </summary>
    /// <param name="estadoInicial">Estado desde el cual se parte</param>
    /// <param name="objetivo">Estado objetivo que se quiere alcanzar</param>
    /// <returns>Una lista de acciones que llevan al objetivo, o null si no hay plan</returns>
    public List<Accion> GenerarPlan(Estado estadoInicial, Estado objetivo)
    {
        // Frontera: cola de estados por explorar junto con el plan
        Queue<(Estado, List<Accion>)> frontera = new Queue<(Estado, List<Accion>)>();
        frontera.Enqueue((estadoInicial, new List<Accion>()));

        // Conjunto de estados ya visitados para evitar ciclos
        HashSet<string> visitados = new HashSet<string>();
        visitados.Add(string.Join(",", estadoInicial.Predicados));

        while (frontera.Count > 0)
        {
            // Extraemos el siguiente estado a explorar y su plan asociado
            (Estado estadoActual, List<Accion> plan) = frontera.Dequeue();

            // Si el estado actual cumple con el objetivo, devolvemos el plan construido
            if (estadoActual.Satisface(objetivo.Predicados))
            {
                return plan;
            }

            // Probar todas las acciones aplicables en el estado actual
            foreach (Accion accion in Acciones)
            {
                // Se genera el nuevo estado resultante si la acción se puede aplicar
                Estado nuevoEstado = estadoActual.AplicarAccion(accion);

                if (nuevoEstado != null)
                {
                    // Crear una clave única del estado
                    string estadoClave = string.Join(",", nuevoEstado.Predicados);

                    // Solo se continúa si este estado no se ha explorado antes
                    if (!visitados.Contains(estadoClave))
                    {
                        // Se crea un nuevo plan extendido con la acción actual
                        List<Accion> nuevoPlan = new List<Accion>(plan) { accion };

                        // Añadir el nuevo estado y su plan a la frontera
                        frontera.Enqueue((nuevoEstado, nuevoPlan));

                        // Marcar el nuevo estado como visitado
                        visitados.Add(estadoClave);
                    }
                }
            }
        }

        // Si no se encuentra ningún plan que llegue al objetivo, se devuelve null
        return null;
    }
}
