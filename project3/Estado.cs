// Claudia Vidal Otero (claudia.votero@udc.es)
// Aldana Smyna Medina Lostaunau (aldana.medina@udc.es)
// Grupo 2 (Jueves)

/// <summary>
/// Representa un estado del tablero del 8-puzzle, con su configuración,
/// costo acumulado, heurística y referencia al estado padre.
/// </summary>
class Estado
{
    public int[,] Tablero { get; private set; }
    public int Costo { get; private set; }
    public int Heuristica { get; private set; }
    public Estado Padre { get; private set; }

    /// <summary>
    /// Constructor del estado. Clona el tablero recibido y calcula la heurística.
    /// </summary>
    /// <param name="tablero">Configuración del tablero (3x3)</param>
    /// <param name="costo">Costo acumulado desde el estado inicial</param>
    /// <param name="padre">Estado desde el que se ha llegado a este</param>
    public Estado(int[,] tablero, int costo = 0, Estado padre = null)
    {
        Tablero = (int[,])tablero.Clone();
        Costo = costo;
        Padre = padre;
        Heuristica = CalcularHeuristica();
    }

    /// <summary>
    /// Calcula la heurística como la suma de distancias de Manhattan
    /// entre cada ficha y su posición en el estado objetivo.
    /// </summary>
    private int CalcularHeuristica()
    {
        int heuristicaTotal = 0;

        // Iteramos sobre el tablero para calcular la distancia de Manhattan de cada ficha
        for (int filaActual = 0; filaActual < 3; filaActual++)
        {
            for (int columnaActual = 0; columnaActual < 3; columnaActual++)
            {
                int valorFicha = Tablero[filaActual, columnaActual];

                if (valorFicha != 0) // Ignorar la casilla vacía
                {
                    // Calculamos la fila y columna donde debería estar la ficha en el estado objetivo
                    int filaObjetivo = (valorFicha - 1) / 3;
                    int columnaObjetivo = (valorFicha - 1) % 3;

                    // Distancia de Manhattan entre la posición actual y la posición objetivo
                    int distancia = Math.Abs(filaActual - filaObjetivo) + Math.Abs(columnaActual - columnaObjetivo);

                    // Acumulamos la distancia para todas las fichas
                    heuristicaTotal += distancia;
                }
            }
        }

        return heuristicaTotal;
    }

    /// <summary>
    /// Aplica una acción (movimiento) sobre el estado actual y devuelve un nuevo estado resultante.
    /// </summary>
    /// <param name="accion">Acción a aplicar</param>
    /// <returns>Nuevo estado generado</returns>
    public Estado AplicarAccion(Accion accion)
    {
        // Obtenemos la posición de la casilla vacía (0) en el tablero
        (int filaVacia, int columnaVacia) = ObtenerPosVacia();
        // Creamos una copia del tablero actual para modificarlo sin afectar el estado original
        int[,] nuevoTablero = (int[,])Tablero.Clone();
        switch (accion.Nombre)
        {
            case "MoverArriba":
                // Verificamos si la casilla vacía no está en la primera fila
                if (filaVacia > 0)
                {
                    // Movemos la ficha hacia arriba (intercambiamos las posiciones de la casilla vacía y la ficha que está arriba)
                    nuevoTablero[filaVacia, columnaVacia] = nuevoTablero[filaVacia - 1, columnaVacia];
                    nuevoTablero[filaVacia - 1, columnaVacia] = 0;
                }
                break;

            case "MoverAbajo":
                // Verificamos si la casilla vacía no está en la última fila
                if (filaVacia < 2)
                {
                    // Movemos la ficha hacia abajo (intercambiamos las posiciones de la casilla vacía y la ficha que está abajo)
                    nuevoTablero[filaVacia, columnaVacia] = nuevoTablero[filaVacia + 1, columnaVacia];
                    nuevoTablero[filaVacia + 1, columnaVacia] = 0;
                }
                break;

            case "MoverIzquierda":
                // Verificamos si la casilla vacía no está en la primera columna
                if (columnaVacia > 0)
                {
                    // Movemos la ficha hacia la izquierda (intercambiamos las posiciones de la casilla vacía y la ficha que está a la izquierda)
                    nuevoTablero[filaVacia, columnaVacia] = nuevoTablero[filaVacia, columnaVacia - 1];
                    nuevoTablero[filaVacia, columnaVacia - 1] = 0;
                }
                break;

            case "MoverDerecha":
                // Verificamos si la casilla vacía no está en la última columna
                if (columnaVacia < 2)
                {
                    // Movemos la ficha hacia la derecha (intercambiamos las posiciones de la casilla vacía y la ficha que está a la derecha)
                    nuevoTablero[filaVacia, columnaVacia] = nuevoTablero[filaVacia, columnaVacia + 1];
                    nuevoTablero[filaVacia, columnaVacia + 1] = 0;
                }
                break;
        }
        // Devolvemos el nuevo estado generado, incrementando el costo en 1 (cada movimiento aumenta el costo)
        return new Estado(nuevoTablero, Costo + 1, this);
    }

    /// <summary>
    /// Comprueba si este estado coincide con el estado objetivo.
    /// </summary>
    /// <param name="objetivo">Estado a comparar</param>
    /// <returns>True si los tableros son iguales</returns>
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
    /// Muestra el estado actual del tablero por consola.
    /// </summary>
    public void MostrarTablero()
    {
        // Imprimimos cada fila del tablero
        for (int fila = 0; fila < 3; fila++)
        {
            for (int columna = 0; columna < 3; columna++)
            {
                // Mostramos "_" si la casilla es la vacía (valor 0), sino mostramos el valor de la ficha

                Console.Write(Tablero[fila, columna] == 0 ? "_  " : $"{Tablero[fila, columna]}  ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    /// <summary>
    /// Busca y devuelve la posición de la casilla vacía (valor 0).
    /// </summary>
    /// <returns>Tupla con coordenadas (fila, columna)</returns>
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

    /// <summary>
    /// Devuelve el costo total f(n) = g(n) + h(n) usado para A*.
    /// </summary>
    public int CalcularCostoTotal()
    {
        return Costo + Heuristica;
    }

    /// <summary>
    /// Convierte el estado a un string único para comparación y hashing.
    /// </summary>
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
}
