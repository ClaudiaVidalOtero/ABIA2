    
namespace TresEnRaya
{  
    /// <summary>
    /// Clase base abstracta para jugadores.
    /// </summary>
    public abstract class Jugador
    {
        public int MarcaJugador { get; private set; }

        protected Jugador(int marca)
        {
            MarcaJugador = marca;
        }

        public abstract Movimiento ObtenerMovimiento(Tablero tablero);
    }

    public class JugadorHumano : Jugador
    {
        public JugadorHumano(int marca) : base(marca) { }

        public override Movimiento ObtenerMovimiento(Tablero tablero)
        {
            int numeroSeleccionado;
            Movimiento movimiento = null;

            do
            {
                Console.WriteLine("Introduce un número del 1 al 9 correspondiente a la casilla:");
                string linea = Console.ReadLine();
                Int32.TryParse(linea, out numeroSeleccionado);

                int fila = (numeroSeleccionado - 1) / 3;
                int columna = (numeroSeleccionado - 1) % 3;
                movimiento = new Movimiento(fila, columna);
            }
            while (numeroSeleccionado < 1 || numeroSeleccionado > 9 ||
                !tablero.ObtenerMovimientosLegales().Exists(m => m.Fila == movimiento.Fila && m.Columna == movimiento.Columna));

            return movimiento;
        }
    }
}