// Claudia Vidal Otero (claudia.votero@udc.es)
// Aldana Smyna Medina Lostaunau (aldana.medina@udc.es)
// Grupo 2 (Jueves)

/// <summary>
/// Representa una acción que puede aplicarse sobre un estado del tablero del puzzle.
/// </summary>
class Accion
{
    public string Nombre { get; private set; }

    public Accion(string nombre)
    {
        Nombre = nombre;
    }
}