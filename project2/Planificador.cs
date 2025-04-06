// Claudia Vidal Otero (claudia.votero@udc.es)
// Aldana Smyna Medina Lostaunau (aldana.medina@udc.es)
// Grupo 2 (Jueves)
class Planificador
{
    public List<Accion> Acciones { get; private set; }

    public Planificador()
    {
        Acciones = new List<Accion>();
    }

    public void AgregarAccion(Accion accion)
    {
        Acciones.Add(accion);
    }

    public List<Accion> GenerarPlan(Estado estadoInicial, Estado objetivo)
    {
        Queue<(Estado, List<Accion>)> frontera = new Queue<(Estado, List<Accion>)>();
        frontera.Enqueue((estadoInicial, new List<Accion>()));

        HashSet<string> visitados = new HashSet<string>();
        visitados.Add(string.Join(",", estadoInicial.Predicados));

        while (frontera.Count > 0)
        {
            (Estado estadoActual, List<Accion> plan) = frontera.Dequeue();

            if (estadoActual.Satisface(objetivo.Predicados))
            {
                return plan;
            }

            foreach (Accion accion in Acciones)
            {
                Estado nuevoEstado = estadoActual.AplicarAccion(accion);
                if (nuevoEstado != null)
                {
                    string estadoClave = string.Join(",", nuevoEstado.Predicados);
                    if (!visitados.Contains(estadoClave))
                    {
                        List<Accion> nuevoPlan = new List<Accion>(plan) { accion };
                        frontera.Enqueue((nuevoEstado, nuevoPlan));
                        visitados.Add(estadoClave);
                    }
                }
            }
        }
        return null;
    }
}