
class Estado
{
    public HashSet<string> Predicados { get; private set; }

    public Estado(IEnumerable<string> predicados)
    {
        Predicados = new HashSet<string>(predicados);
    }

    public bool Satisface(HashSet<string> objetivos)
    {
        return objetivos.IsSubsetOf(Predicados);
    }

    public Estado AplicarAccion(Accion accion)
    {
        if (!Satisface(new HashSet<string>(accion.Precondiciones))) return null;

        HashSet<string> nuevoEstado = new HashSet<string>(Predicados);
        foreach (string efecto in accion.EfectosPositivos)
        {
            nuevoEstado.Add(efecto);
        }
        foreach (string efecto in accion.EfectosNegativos)
        {
            nuevoEstado.Remove(efecto);
        }
        return new Estado(nuevoEstado);
    }

    public void ImprimirEstado()
    {
        Console.WriteLine("Estado actual:");
        foreach (string predicado in Predicados.OrderBy(p => p))
        {
            Console.WriteLine("  " + predicado);
        }
        Console.WriteLine();
    }
}
