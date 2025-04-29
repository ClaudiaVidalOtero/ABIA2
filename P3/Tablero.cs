namespace TresEnRaya
{
    /// <summary>
    /// Clase que representa el tablero 3x3 del juego Tres en Raya.
    /// Maneja la lógica del estado del juego, verificación de ganadores y movimientos.
    /// </summary>
    public class Tablero
    {
        private int[,] casillas;

        /// <summary>
        /// Constructor que inicializa el tablero vacío (0).
        /// </summary>
        public Tablero()
        {
            casillas = new int[3, 3]; // 0 = vacío, 1 = X, -1 = O
        }

        /// <summary>
        /// Obtiene una lista con todos los movimientos legales disponibles (casillas vacías).
        /// </summary>
        /// <returns>Lista de movimientos válidos.</returns>
        public List<Movimiento> ObtenerMovimientosLegales()
        {
            List<Movimiento> movimientos = new List<Movimiento>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (casillas[i, j] == 0)
                    {
                        movimientos.Add(new Movimiento(i, j)); // Añade solo las casillas vacías
                    }
                }
            }
            return movimientos;
        }

        /// <summary>
        /// Aplica un movimiento al tablero y devuelve un nuevo estado del tablero.
        /// </summary>
        /// <param name="mov">Movimiento a aplicar.</param>
        /// <param name="jugador">Jugador que realiza el movimiento (1 o -1).</param>
        /// <returns>Nuevo tablero con el movimiento aplicado.</returns>
        public Tablero AplicarMovimiento(Movimiento mov, int jugador)
        {
            Tablero nuevo = new Tablero();
            Array.Copy(casillas, nuevo.casillas, casillas.Length);
            nuevo.casillas[mov.Fila, mov.Columna] = jugador; // Inserta la marca del jugador
            return nuevo;
        }

        /// <summary>
        /// Indica si el estado actual del tablero es terminal (victoria o empate).
        /// </summary>
        /// <returns>True si hay ganador o empate, False en caso contrario.</returns>
        public bool EsTerminal()
        {
            return ObtenerGanador() != 0 || ObtenerMovimientosLegales().Count == 0;
        }

        /// <summary>
        /// Devuelve la utilidad del estado: 1 (X gana), -1 (O gana), 0 (empate o en curso).
        /// </summary>
        /// <returns>Valor entero representando el resultado.</returns>
        public int Utilidad()
        {
            return ObtenerGanador();
        }

        /// <summary>
        /// Determina si hay un ganador en el tablero.
        /// </summary>
        /// <returns>1 si gana X, -1 si gana O, 0 si no hay ganador aún.</returns>
        public int ObtenerGanador()
        {
            for (int i = 0; i < 3; i++)
            {
                // Verifica filas
                if (casillas[i, 0] != 0 && casillas[i, 0] == casillas[i, 1] && casillas[i, 1] == casillas[i, 2])
                    return casillas[i, 0];

                // Verifica columnas
                if (casillas[0, i] != 0 && casillas[0, i] == casillas[1, i] && casillas[1, i] == casillas[2, i])
                    return casillas[0, i];
            }

            // Verifica diagonal principal
            if (casillas[0, 0] != 0 && casillas[0, 0] == casillas[1, 1] && casillas[1, 1] == casillas[2, 2])
                return casillas[0, 0];

            // Verifica diagonal secundaria
            if (casillas[0, 2] != 0 && casillas[0, 2] == casillas[1, 1] && casillas[1, 1] == casillas[2, 0])
                return casillas[0, 2];

            return 0; // No hay ganador
        }

        /// <summary>
        /// Muestra el estado actual del tablero en la consola.
        /// </summary>
        public void Mostrar()
        {
            Console.WriteLine("-------------");
            for (int i = 0; i < 3; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < 3; j++)
                {
                    int valor = casillas[i, j];
                    int posicion = i * 3 + j + 1;
                    // Muestra la posición si está vacío, o la ficha del jugador
                    string simbolo = valor == 0 ? posicion.ToString() : (valor == 1 ? "X" : "O");
                    Console.Write(simbolo + " | ");
                }
                Console.WriteLine();
                Console.WriteLine("-------------");
            }
        }
    }
}
