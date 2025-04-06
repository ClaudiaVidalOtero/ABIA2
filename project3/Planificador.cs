
/// <summary>
/// Planificador que genera acciones y encuentra un plan usando búsqueda
/// </summary>
class Planificador
{
    public List<Accion> GenerarAcciones(Estado estado)
    {
        var acciones = new List<Accion>();

        // Buscar la posición vacía
        var vacia = estado.Predicados.First(p => p.Nombre == "Vacia");
        int fila = vacia.Argumentos[0];
        int col = vacia.Argumentos[1];

        // Definir desplazamientos posibles
        var direcciones = new (int df, int dc, string nombre)[]
        {
            (-1, 0, "MoverAbajo"), // la ficha de arriba baja
            (1, 0, "MoverArriba"), // la ficha de abajo sube
            (0, -1, "MoverDerecha"),
            (0, 1, "MoverIzquierda")
        };

        foreach (var (df, dc, nombre) in direcciones)
        {
            int nf = fila + df;
            int nc = col + dc;
            if (nf >= 0 && nf < 3 && nc >= 0 && nc < 3)
            {
                // Encontrar qué ficha está en (nf, nc)
                var fichaEnPos = estado.Predicados.FirstOrDefault(p =>
                    p.Nombre == "En" && p.Argumentos[1] == nf && p.Argumentos[2] == nc);
                if (fichaEnPos != null)
                {
                    int ficha = fichaEnPos.Argumentos[0];

                    var precondiciones = new List<Predicado>
                    {
                        new Predicado("En", ficha, nf, nc),
                        new Predicado("Vacia", fila, col)
                    };

                    var efectosEliminar = new List<Predicado>
                    {
                        new Predicado("En", ficha, nf, nc),
                        new Predicado("Vacia", fila, col)
                    };

                    var efectosAgregar = new List<Predicado>
                    {
                        new Predicado("En", ficha, fila, col),
                        new Predicado("Vacia", nf, nc)
                    };

                    acciones.Add(new Accion($"{nombre}({ficha})", precondiciones, efectosAgregar, efectosEliminar));
                }
            }
        }

        return acciones;
    }

    public List<Accion> EncontrarPlan(Estado inicial, Estado objetivo)
    {
        var frontera = new Queue<Estado>();
        var visitados = new HashSet<Estado>();

        frontera.Enqueue(inicial);

        while (frontera.Count > 0)
        {
            var actual = frontera.Dequeue();

            if (actual.Satisface(objetivo))
                return ReconstruirPlan(actual);

            visitados.Add(actual);

            foreach (var accion in GenerarAcciones(actual))
            {
                var sucesor = actual.Aplicar(accion);
                if (sucesor != null && !visitados.Contains(sucesor))
                    frontera.Enqueue(sucesor);
            }
        }

        return null;
    }

    private List<Accion> ReconstruirPlan(Estado estado)
    {
        var plan = new List<Accion>();
        while (estado.Padre != null)
        {
            plan.Add(estado.AccionAplicada);
            estado = estado.Padre;
        }
        plan.Reverse();
        return plan;
    }
}