  namespace TresEnRaya
{
    /// <summary>
    /// Jugador automático que decide sus movimientos usando el algoritmo Minimax.
    /// </summary>
    public class JugadorAgente : Jugador
    {
        private Random generador;

        /// <summary>
        /// Constructor del agente que inicializa su marca y su generador de números aleatorios.
        /// </summary>
        /// <param name="marca">Marca del jugador (1 para X, -1 para O).</param>
        public JugadorAgente(int marca) : base(marca)
        {
            generador = new Random();
        }

        /// <summary>
        /// Obtiene el movimiento óptimo del agente usando Minimax dependiendo de la marca.
        /// </summary>
        /// <param name="tablero">Estado actual del tablero.</param>
        /// <returns>Movimiento elegido.</returns>
        public override Movimiento ObtenerMovimiento(Tablero tablero)
        {
            // Elige la función Minimax correspondiente según la marca del jugador
            return MarcaJugador == 1 ? MinimaxDecisionMax(tablero) : MinimaxDecisionMin(tablero);
        }

        /// <summary>
        /// Realiza la decisión Minimax cuando el agente es el jugador MAX (X).
        /// </summary>
        /// <param name="estado">Estado actual del tablero.</param>
        /// <returns>Mejor movimiento encontrado.</returns>
        private Movimiento MinimaxDecisionMax(Tablero estado)
        {
            List<Movimiento> mejores = new List<Movimiento>();
            int mejorValor = Int32.MinValue;

            // Evalúa todos los movimientos posibles
            foreach (Movimiento m in estado.ObtenerMovimientosLegales())
            {
                Tablero resultado = estado.AplicarMovimiento(m, MarcaJugador);
                int valor = MinValue(resultado); // Evalúa el peor caso del oponente

                // Si encuentra una mejor opción, actualiza la lista
                if (valor > mejorValor)
                {
                    mejores.Clear();
                    mejores.Add(m);
                    mejorValor = valor;
                }
                // Si el valor es igual al mejor, lo añade como opción válida
                else if (valor == mejorValor)
                {
                    mejores.Add(m);
                }
            }

            // Elige uno aleatorio entre los mejores movimientos
            return mejores[generador.Next(mejores.Count)];
        }

        /// <summary>
        /// Realiza la decisión Minimax cuando el agente es el jugador MIN (O).
        /// </summary>
        /// <param name="estado">Estado actual del tablero.</param>
        /// <returns>Mejor movimiento encontrado.</returns>
        private Movimiento MinimaxDecisionMin(Tablero estado)
        {
            List<Movimiento> mejores = new List<Movimiento>();
            int mejorValor = Int32.MaxValue;

            // Evalúa todos los movimientos posibles
            foreach (Movimiento m in estado.ObtenerMovimientosLegales())
            {
                Tablero resultado = estado.AplicarMovimiento(m, MarcaJugador);
                int valor = MaxValue(resultado); // Evalúa el mejor caso del oponente

                // Si encuentra una opción peor (más baja), la guarda
                if (valor < mejorValor)
                {
                    mejores.Clear();
                    mejores.Add(m);
                    mejorValor = valor;
                }
                // Si es igual al mejor valor actual, la añade como opción
                else if (valor == mejorValor)
                {
                    mejores.Add(m);
                }
            }

            // Elige uno aleatoriamente entre los mejores
            return mejores[generador.Next(mejores.Count)];
        }

        /// <summary>
        /// Función Max del algoritmo Minimax. Representa al jugador X.
        /// </summary>
        /// <param name="estado">Estado del tablero.</param>
        /// <returns>Mejor puntuación alcanzable para Max.</returns>
        private int MaxValue(Tablero estado)
        {
            // Si es terminal, se retorna la utilidad
            if (estado.EsTerminal()) return estado.Utilidad();

            int v = Int32.MinValue;
            foreach (Movimiento m in estado.ObtenerMovimientosLegales())
            {
                Tablero resultado = estado.AplicarMovimiento(m, 1); // 1 = jugador Max
                int valor = MinValue(resultado);
                v = Math.Max(v, valor); // Se queda con el valor más alto
            }
            return v;
        }

        /// <summary>
        /// Función Min del algoritmo Minimax. Representa al jugador O.
        /// </summary>
        /// <param name="estado">Estado del tablero.</param>
        /// <returns>Mejor puntuación alcanzable para Min.</returns>
        private int MinValue(Tablero estado)
        {
            // Si es terminal, se retorna la utilidad
            if (estado.EsTerminal()) return estado.Utilidad();

            int v = Int32.MaxValue;
            foreach (Movimiento m in estado.ObtenerMovimientosLegales())
            {
                Tablero resultado = estado.AplicarMovimiento(m, -1); // -1 = jugador Min
                int valor = MaxValue(resultado);
                v = Math.Min(v, valor); // Se queda con el valor más bajo
            }
            return v;
        }
    }
}
