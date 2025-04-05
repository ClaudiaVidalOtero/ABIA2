using System;
using System.Collections.Generic;
using System.Linq;

class Estado
{
    public int[,] Tablero { get; private set; }
    public int Costo { get; private set; }
    public int Heuristica { get; private set; }
    public Estado Padre { get; private set; }

    public Estado(int[,] tablero, int costo = 0, Estado padre = null)
    {
        Tablero = (int[,])tablero.Clone();
        Costo = costo;
        Padre = padre;
        Heuristica = CalcularHeuristica();
    }

    private int CalcularHeuristica()
    {
        int heuristica = 0;
        int[,] objetivoTablero = new int[,] {
            { 1, 2, 3 },
            { 4, 5, 6 },
            { 7, 8, 0 }
        };

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int valor = Tablero[i, j];
                if (valor != 0)
                {
                    int objetivoX = (valor - 1) / 3;
                    int objetivoY = (valor - 1) % 3;
                    heuristica += Math.Abs(i - objetivoX) + Math.Abs(j - objetivoY);
                }
            }
        }
        return heuristica;
    }

    public Estado AplicarAccion(Accion accion)
    {
        var (x, y) = ObtenerPosVacia();
        int[,] nuevoTablero = (int[,])Tablero.Clone();

        switch (accion.Nombre)
        {
            case "MoverArriba":
                if (x > 0)
                {
                    nuevoTablero[x, y] = nuevoTablero[x - 1, y];
                    nuevoTablero[x - 1, y] = 0;
                }
                break;
            case "MoverAbajo":
                if (x < 2)
                {
                    nuevoTablero[x, y] = nuevoTablero[x + 1, y];
                    nuevoTablero[x + 1, y] = 0;
                }
                break;
            case "MoverIzquierda":
                if (y > 0)
                {
                    nuevoTablero[x, y] = nuevoTablero[x, y - 1];
                    nuevoTablero[x, y - 1] = 0;
                }
                break;
            case "MoverDerecha":
                if (y < 2)
                {
                    nuevoTablero[x, y] = nuevoTablero[x, y + 1];
                    nuevoTablero[x, y + 1] = 0;
                }
                break;
        }

        return new Estado(nuevoTablero, Costo + 1, this);
    }

    public bool Satisface(Estado objetivo)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (Tablero[i, j] != objetivo.Tablero[i, j]) return false;
            }
        }
        return true;
    }

    public void MostrarTablero()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Console.Write(Tablero[i, j] == 0 ? "  " : Tablero[i, j] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    public (int x, int y) ObtenerPosVacia()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (Tablero[i, j] == 0)
                {
                    return (i, j);
                }
            }
        }
        return (-1, -1);
    }

    public int CalcularCostoTotal()
    {
        return Costo + Heuristica;
    }
}

class Accion
{
    public string Nombre { get; private set; }

    public Accion(string nombre)
    {
        Nombre = nombre;
    }
}

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
        SortedSet<Estado> frontera = new SortedSet<Estado>(Comparer<Estado>.Create((a, b) =>
        {
            return a.CalcularCostoTotal().CompareTo(b.CalcularCostoTotal());
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
            // Aquí corregimos el error: no debemos pasar una acción vacía
            string accion = ObtenerAccionDesdeEstados(estado.Padre, estado);
            plan.Add(accion);
            estado = estado.Padre;
        }
        plan.Reverse();
        return plan;
    }

    // Esta función obtiene la acción realizada entre dos estados
    private string ObtenerAccionDesdeEstados(Estado padre, Estado hijo)
    {
        var (x1, y1) = padre.ObtenerPosVacia();
        var (x2, y2) = hijo.ObtenerPosVacia();

        if (x1 > x2) return "MoverArriba";
        if (x1 < x2) return "MoverAbajo";
        if (y1 > y2) return "MoverIzquierda";
        if (y1 < y2) return "MoverDerecha";
        
        return "";
    }
}

class Program
{
    static void Main()
    {
        int[,] estadoInicialTablero = GenerarTableroAleatorio();
        Estado estadoInicial = new Estado(estadoInicialTablero);

        int[,] estadoObjetivoTablero = new int[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 },
            { 7, 8, 0 }
        };
        Estado estadoObjetivo = new Estado(estadoObjetivoTablero);

        Console.WriteLine("Estado inicial aleatorio:");
        estadoInicial.MostrarTablero();

        Planificador planificador = new Planificador();
        List<string> plan = planificador.EncontrarPlan(estadoInicial, estadoObjetivo);

        if (plan != null)
        {
            Console.WriteLine("Plan encontrado:");
            foreach (string accion in plan)
            {
                Console.WriteLine(accion);
            }

            Console.WriteLine("Ejecución del plan:");
            foreach (string accion in plan)
            {
                Console.WriteLine($"Ejecutando: {accion}");
                estadoInicial.MostrarTablero();
                estadoInicial = estadoInicial.AplicarAccion(new Accion(accion));
            }
        }
        else
        {
            Console.WriteLine("No se encontró solución.");
        }
    }

    static int[,] GenerarTableroAleatorio()
    {
        Random rand = new Random();
        List<int> lista = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 0 };
        lista = lista.OrderBy(x => rand.Next()).ToList();

        int[,] tablero = new int[3, 3];
        int index = 0;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                tablero[i, j] = lista[index++];
            }
        }

        return tablero;
    }
}
