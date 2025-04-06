// Claudia Vidal Otero (claudia.votero@udc.es)
// Aldana Smyna Medina Lostaunau (aldana.medina@udc.es)
// Grupo 2 (Jueves)
class Planificador
{
    public List<Accion> Acciones { get; private set; }

    public Planificador()
    {
        Acciones = new List<Accion>();
        DefinirAcciones();
    }

    private void DefinirAcciones()
    {
        Acciones.Add(new Accion("MoverArriba"));
        Acciones.Add(new Accion("MoverAbajo"));
        Acciones.Add(new Accion("MoverIzquierda"));
        Acciones.Add(new Accion("MoverDerecha"));
    }

    public List<string> EncontrarPlan(Estado inicial, Estado objetivo)
    {
        // Se utiliza SortedSet con comparador extendido para evitar conflictos en costo igual.
        SortedSet<Estado> frontera = new SortedSet<Estado>(Comparer<Estado>.Create((a, b) =>
        {
            int cmp = a.CalcularCostoTotal().CompareTo(b.CalcularCostoTotal());
            if (cmp == 0)
                cmp = a.ToString().CompareTo(b.ToString());
            return cmp;
        }));
        HashSet<Estado> visitados = new HashSet<Estado>();

        frontera.Add(inicial);

        while (frontera.Count > 0)
        {
            Estado estadoActual = frontera.Min;
            frontera.Remove(estadoActual);

            if (estadoActual.Satisface(objetivo))
            {
                return ReconstruirCamino(estadoActual);
            }

            visitados.Add(estadoActual);

            foreach (Accion accion in Acciones)
            {
                Estado nuevoEstado = estadoActual.AplicarAccion(accion);
                if (!visitados.Contains(nuevoEstado) && !frontera.Contains(nuevoEstado))
                {
                    frontera.Add(nuevoEstado);
                }
            }
        }
        return null;
    }

    private List<string> ReconstruirCamino(Estado estado)
    {
        List<string> plan = new List<string>();
        while (estado.Padre != null)
        {
            string accion = ObtenerAccionDesdeEstados(estado.Padre, estado);
            plan.Add(accion);
            estado = estado.Padre;
        }
        plan.Reverse();
        return plan;
    }

    private string ObtenerAccionDesdeEstados(Estado padre, Estado hijo)
    {
        (int x1, int y1) = padre.ObtenerPosVacia();
        (int x2, int y2) = hijo.ObtenerPosVacia();

        if (x1 > x2) return "MoverArriba";
        if (x1 < x2) return "MoverAbajo";
        if (y1 > y2) return "MoverIzquierda";
        if (y1 < y2) return "MoverDerecha";
        return "";
    }
}
