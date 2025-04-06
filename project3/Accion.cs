// Claudia Vidal Otero (claudia.votero@udc.es)
// Aldana Smyna Medina Lostaunau (aldana.medina@udc.es)
// Grupo 2 (Jueves)

/// <summary>
/// Representa una acción que puede aplicarse sobre un estado del tablero del puzzle.
/// </summary>
class Accion
{
    /// <summary>
    /// Nombre o descripción de la acción (ej: "MoverArriba", "MoverIzquierda").
    /// </summary>
    public string Nombre { get; private set; }

    /// <summary>
    /// Constructor que inicializa una nueva acción con un nombre determinado.
    /// </summary>
    /// <param name="nombre">Nombre identificador de la acción.</param>
    public Accion(string nombre)
    {
        Nombre = nombre;
    }
}