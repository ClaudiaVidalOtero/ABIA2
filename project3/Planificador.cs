// Claudia Vidal Otero (claudia.votero@udc.es)
// Aldana Smyna Medina Lostaunau (aldana.medina@udc.es)
// Grupo 2 (Jueves)

/// <summary>
/// Planificador que genera las acciones posibles desde un estado y encuentra un plan
/// que lleva al estado objetivo aplicando búsqueda en anchura.
/// </summary>
class Planificador
{
    /// <summary>
    /// Genera todas las acciones válidas desde un estado dado,
    /// considerando los movimientos posibles de fichas hacia la casilla vacía.
    /// </summary>
    /// <param name="estado">El estado desde el cual se generan acciones</param>
    /// <returns>Una lista de acciones aplicables en ese estado</returns>
    public List<Accion> GenerarAcciones(Estado estado)
    {
        var acciones = new List<Accion>();

        // Buscar la posición de la casilla vacía en el estado actual
        var vacia = estado.Predicados.First(p => p.Nombre == "Vacia");
        int fila = vacia.Argumentos[0];
        int col = vacia.Argumentos[1];

        // Definir posibles direcciones de movimiento de una ficha hacia la casilla vacía
        var direcciones = new (int df, int dc, string nombre)[]
        {
            (-1, 0, "MoverAbajo"),    // Mover la ficha de arriba hacia abajo
            (1, 0, "MoverArriba"),    // Mover la ficha de abajo hacia arriba
            (0, -1, "MoverDerecha"),  // Mover la ficha de la izquierda hacia la derecha
            (0, 1, "MoverIzquierda")  // Mover la ficha de la derecha hacia la izquierda
        };

        foreach (var (df, dc, nombre) in direcciones)
        {
            int nf = fila + df;
            int nc = col + dc;

            // Verificar que la posición vecina está dentro de los límites del tablero
            if (nf >= 0 && nf < 3 && nc >= 0 && nc < 3)
            {
                // Buscar si hay una ficha en la posición vecina (nf, nc)
                var fichaEnPos = estado.Predicados.FirstOrDefault(p =>
                    p.Nombre == "En" && p.Argumentos[1] == nf && p.Argumentos[2] == nc);

                if (fichaEnPos != null)
                {
                    int ficha = fichaEnPos.Argumentos[0];

                    // Definir las precondiciones para mover la ficha hacia la casilla vacía
                    var precondiciones = new List<Predicado>
                    {
                        new Predicado("En", ficha, nf, nc),
                        new Predicado("Vacia", fila, col)
                    };

                    // Definir los efectos que se eliminan al aplicar la acción
                    var efectosEliminar = new List<Predicado>
                    {
                        new Predicado("En", ficha, nf, nc),
                        new Predicado("Vacia", fila, col)
                    };

                    // Definir los efectos que se agregan al aplicar la acción
                    var efectosAgregar = new List<Predicado>
                    {
                        new Predicado("En", ficha, fila, col),
                        new Predicado("Vacia", nf, nc)
                    };

                    // Crear y agregar la acción a la lista
                    acciones.Add(new Accion($"{nombre}({ficha})", precondiciones, efectosAgregar, efectosEliminar));
                }
            }
        }

        return acciones;
    }

    /// <summary>
    /// Encuentra un plan que transforma el estado inicial en el estado objetivo,
    /// utilizando búsqueda en anchura (BFS).
    /// </summary>
    /// <param name="inicial">Estado desde el que se parte</param>
    /// <param name="objetivo">Estado meta que se desea alcanzar</param>
    /// <returns>Lista de acciones que constituyen el plan o null si no se encuentra</returns>
    public List<Accion> EncontrarPlan(Estado inicial, Estado objetivo)
    {
        var frontera = new Queue<Estado>(); // Cola de estados por explorar
        var visitados = new HashSet<Estado>(); // Conjunto de estados ya visitados

        frontera.Enqueue(inicial);

        while (frontera.Count > 0)
        {
            var actual = frontera.Dequeue();

            // Si el estado actual cumple con el objetivo, se reconstruye el plan
            if (actual.Satisface(objetivo))
                return ReconstruirPlan(actual);

            visitados.Add(actual);

            // Generar y aplicar todas las acciones posibles desde el estado actual
            foreach (var accion in GenerarAcciones(actual))
            {
                var sucesor = actual.Aplicar(accion);
                if (sucesor != null && !visitados.Contains(sucesor))
                    frontera.Enqueue(sucesor);
            }
        }

        // Si no se encuentra un plan, se devuelve null
        return null;
    }

    /// <summary>
    /// Reconstruye el plan desde el estado final al inicial siguiendo la cadena de padres.
    /// </summary>
    /// <param name="estado">Estado final desde el cual se empieza la reconstrucción</param>
    /// <returns>Lista de acciones en orden desde el estado inicial al objetivo</returns>
    private List<Accion> ReconstruirPlan(Estado estado)
    {
        var plan = new List<Accion>();

        // Se recorre hacia atrás la secuencia de estados, acumulando acciones
        while (estado.Padre != null)
        {
            plan.Add(estado.AccionAplicada);
            estado = estado.Padre;
        }

        // Invertir la lista para obtener el orden correcto (inicio -> objetivo)
        plan.Reverse();
        return plan;
    }
}
