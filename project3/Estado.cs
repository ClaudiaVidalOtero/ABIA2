// Claudia Vidal Otero (claudia.votero@udc.es)
// Aldana Smyna Medina Lostaunau (aldana.medina@udc.es)
// Grupo 2 (Jueves)

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

    /// <summary>
    /// Calcula la heurística utilizando la suma de distancias de Manhattan
    /// entre cada ficha y su posición en el tablero objetivo.
    /// </summary>
    private int CalcularHeuristica()
    {
        int heuristicaTotal = 0;

        int[,] tableroObjetivo = new int[,] {
            { 1, 2, 3 },
            { 4, 5, 6 },
            { 7, 8, 0 }
        };

        for (int filaActual = 0; filaActual < 3; filaActual++)
        {
            for (int columnaActual = 0; columnaActual < 3; columnaActual++)
            {
                int valorFicha = Tablero[filaActual, columnaActual];

                if (valorFicha != 0) // No cuenta la casilla vacía
                {
                    int filaObjetivo = (valorFicha - 1) / 3;
                    int columnaObjetivo = (valorFicha - 1) % 3;

                    int distanciaManhattan = Math.Abs(filaActual - filaObjetivo) + Math.Abs(columnaActual - columnaObjetivo);
                    heuristicaTotal += distanciaManhattan;
                }
            }
        }

        return heuristicaTotal;
    }

    /// <summary>
    /// Devuelve un nuevo estado tras aplicar la acción de mover una ficha.
    /// </summary>
    public Estado AplicarAccion(Accion accion)
    {
        (int filaVacia, int columnaVacia) = ObtenerPosVacia();
        int[,] nuevoTablero = (int[,])Tablero.Clone();

        switch (accion.Nombre)
        {
            case "MoverArriba":
                if (filaVacia > 0)
                {
                    nuevoTablero[filaVacia, columnaVacia] = nuevoTablero[filaVacia - 1, columnaVacia];
                    nuevoTablero[filaVacia - 1, columnaVacia] = 0;
                }
                break;

            case "MoverAbajo":
                if (filaVacia < 2)
                {
                    nuevoTablero[filaVacia, columnaVacia] = nuevoTablero[filaVacia + 1, columnaVacia];
                    nuevoTablero[filaVacia + 1, columnaVacia] = 0;
                }
                break;

            case "MoverIzquierda":
                if (columnaVacia > 0)
                {
                    nuevoTablero[filaVacia, columnaVacia] = nuevoTablero[filaVacia, columnaVacia - 1];
                    nuevoTablero[filaVacia, columnaVacia - 1] = 0;
                }
                break;

            case "MoverDerecha":
                if (columnaVacia < 2)
                {
                    nuevoTablero[filaVacia, columnaVacia] = nuevoTablero[filaVacia, columnaVacia + 1];
                    nuevoTablero[filaVacia, columnaVacia + 1] = 0;
                }
                break;
        }

        return new Estado(nuevoTablero, Costo + 1, this);
    }

    /// <summary>
    /// Verifica si el estado actual es igual al estado objetivo.
    /// </summary>
    public bool Satisface(Estado objetivo)
    {
        for (int fila = 0; fila < 3; fila++)
        {
            for (int columna = 0; columna < 3; columna++)
            {
                if (Tablero[fila, columna] != objetivo.Tablero[fila, columna])
                    return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Muestra el tablero en consola.
    /// </summary>
    public void MostrarTablero()
    {
        for (int fila = 0; fila < 3; fila++)
        {
            for (int columna = 0; columna < 3; columna++)
            {
                Console.Write(Tablero[fila, columna] == 0 ? "   " : $"{Tablero[fila, columna]}  ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    /// <summary>
    /// Encuentra la posición de la casilla vacía (valor 0).
    /// </summary>
    public (int fila, int columna) ObtenerPosVacia()
    {
        for (int fila = 0; fila < 3; fila++)
        {
            for (int columna = 0; columna < 3; columna++)
            {
                if (Tablero[fila, columna] == 0)
                    return (fila, columna);
            }
        }

        return (-1, -1); // No encontrada
    }

    public int CalcularCostoTotal()
    {
        return Costo + Heuristica;
    }

    public override string ToString()
    {
        string resultado = "";
        for (int fila = 0; fila < 3; fila++)
        {
            for (int columna = 0; columna < 3; columna++)
            {
                resultado += Tablero[fila, columna] + ",";
            }
        }
        return resultado;
    }

    public override bool Equals(object obj)
    {
        if (obj is Estado otroEstado)
            return this.ToString() == otroEstado.ToString();
        return false;
    }
}
