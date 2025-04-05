using System;
using System.Collections.Generic;

class Estado
{
    public int[,] Tablero { get; private set; } // Usamos una matriz para representar el tablero

    public Estado(int[,] tablero)
    {
        Tablero = (int[,])tablero.Clone(); // Clonamos el tablero para evitar efectos secundarios
    }

    // Aplica una acción y devuelve el nuevo estado
    public Estado AplicarAccion(Accion accion)
    {
        // Encontrar la posición de la casilla vacía
        var (x, y) = ObtenerPosVacia();

        // Realizar el movimiento según la acción
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

        return new Estado(nuevoTablero);
    }

    // Verifica si el estado actual cumple con el objetivo (compara con la matriz objetivo)
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

    // Mostrar el tablero de manera legible
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

    // Función que encuentra la posición de la casilla vacía (0)
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
        return (-1, -1); // Si no se encuentra la casilla vacía (no debería pasar)
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
        // Definir las acciones, pero ahora trabajaremos directamente con las coordenadas del tablero.
        Acciones.Add(new Accion("MoverArriba"));
        Acciones.Add(new Accion("MoverAbajo"));
        Acciones.Add(new Accion("MoverIzquierda"));
        Acciones.Add(new Accion("MoverDerecha"));
    }

    public List<string> EncontrarPlan(Estado inicial, Estado objetivo)
    {
        Queue<(Estado, List<string>)> frontera = new Queue<(Estado, List<string>)>();
        HashSet<Estado> visitados = new HashSet<Estado>();
        
        frontera.Enqueue((inicial, new List<string>()));
        visitados.Add(inicial);

        while (frontera.Count > 0)
        {
            (Estado estadoActual, List<string> planActual) = frontera.Dequeue();
            
            if (estadoActual.Satisface(objetivo))
                return planActual;

            foreach (Accion accion in Acciones)
            {
                // Aplica la acción y obtiene el nuevo estado
                Estado nuevoEstado = estadoActual.AplicarAccion(accion);
                if (nuevoEstado != null && !visitados.Contains(nuevoEstado))
                {
                    List<string> nuevoPlan = new List<string>(planActual) { accion.Nombre };
                    frontera.Enqueue((nuevoEstado, nuevoPlan));
                    visitados.Add(nuevoEstado);
                }
            }
        }
        return null; // No hay solución
    }
}

class Program
{
    static void Main()
    {
        // Estado inicial del 8-Puzle (con el 0 representando la casilla vacía)
        int[,] estadoInicialTablero = new int[,]
        {
            { 1, 8, 7 },
            { 4, 0, 2 },
            { 3, 6, 5 }
        };
        Estado estadoInicial = new Estado(estadoInicialTablero);

        // Estado objetivo del 8-Puzle
        int[,] estadoObjetivoTablero = new int[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 },
            { 7, 8, 0 }
        };
        Estado estadoObjetivo = new Estado(estadoObjetivoTablero);

        Planificador planificador = new Planificador();
        List<string> plan = planificador.EncontrarPlan(estadoInicial, estadoObjetivo);

        if (plan != null)
        {
            Console.WriteLine("Tablero inicial aleatorio:");
            estadoInicial.MostrarTablero();

            Console.WriteLine("Plan encontrado:");
            foreach (string accion in plan)
            {
                Console.WriteLine(accion);
            }

            Console.WriteLine("Ejecución del plan:");
            foreach (string accion in plan)
            {
                Console.WriteLine($"Ejecutando: {accion}");
                // Aplicar la acción al estado
                estadoInicial = estadoInicial.AplicarAccion(new Accion(accion));
                estadoInicial.MostrarTablero();

            }

        }
        else
        {
            Console.WriteLine("No se encontró solución.");
        }
    }
}
