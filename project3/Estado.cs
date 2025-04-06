/// <summary>
/// Representa un estado del puzzle como un conjunto de predicados
/// </summary>
class Estado
{
    public HashSet<Predicado> Predicados { get; private set; }
    public Estado Padre { get; private set; }
    public Accion AccionAplicada { get; private set; }
    public int Costo { get; private set; }

    public Estado(HashSet<Predicado> predicados, Estado padre = null, Accion accionAplicada = null, int costo = 0)
    {
        Predicados = new HashSet<Predicado>(predicados);
        Padre = padre;
        AccionAplicada = accionAplicada;
        Costo = costo;
    }

    public bool Satisface(Estado objetivo)
    {
        return objetivo.Predicados.All(p => Predicados.Contains(p));
    }

    public Estado Aplicar(Accion accion)
    {
        if (!accion.Precondiciones.All(p => Predicados.Contains(p)))
            return null;

        var nuevosPredicados = new HashSet<Predicado>(Predicados);

        foreach (var p in accion.EfectosEliminar)
            nuevosPredicados.Remove(p);
        foreach (var p in accion.EfectosAgregar)
            nuevosPredicados.Add(p);

        return new Estado(nuevosPredicados, this, accion, this.Costo + 1);
    }

    public override bool Equals(object obj)
    {
        return obj is Estado otro && Predicados.SetEquals(otro.Predicados);
    }

    public override int GetHashCode()
    {
        int hash = 17;
        foreach (var p in Predicados.OrderBy(p => p.ToString()))
            hash = hash * 23 + p.GetHashCode();
        return hash;
    }

    public void Mostrar()
    {
        var tablero = new int[3, 3];
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
