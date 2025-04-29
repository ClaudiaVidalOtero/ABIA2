namespace TresEnRaya
{
    /// <summary>
    /// Representa un movimiento en el tablero mediante coordenadas de fila y columna.
    /// </summary>
    public class Movimiento
    {
        /// <summary>
        /// Fila del movimiento.
        /// </summary>
        public int Fila { get; private set; }

        /// <summary>
        /// Columna del movimiento.
        /// </summary>
        public int Columna { get; private set; }

        /// <summary>
        /// Constructor que inicializa un movimiento con fila y columna.
        /// </summary>
        /// <param name="fila">Índice de la fila.</param>
        /// <param name="columna">Índice de la columna.</param>
        public Movimiento(int fila, int columna)
        {
            Fila = fila;
            Columna = columna;
        }

        /// <summary>
        /// Devuelve una representación en texto del movimiento.
        /// </summary>
        /// <returns>Cadena con el formato "(fila, columna)".</returns>
        public override string ToString()
        {
            return string.Format("({0}, {1})", Fila, Columna);
        }
    }
}
