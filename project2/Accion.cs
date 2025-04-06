// Claudia Vidal Otero (claudia.votero@udc.es)
// Aldana Smyna Medina Lostaunau (aldana.medina@udc.es)
// Grupo 2 (Jueves)

/// <summary>
/// Representa una acción con su nombre, precondiciones, efectos positivos y negativos.
/// </summary>
class Accion
{
    public string Nombre { get; private set; }
    public List<string> Precondiciones { get; private set; }
    public List<string> EfectosPositivos { get; private set; }
    public List<string> EfectosNegativos { get; private set; }

    public Accion(string nombre, List<string> precondiciones, List<string> efectosPos, List<string> efectosNeg)
    {
        Nombre = nombre;
        Precondiciones = precondiciones;
        EfectosPositivos = efectosPos;
        EfectosNegativos = efectosNeg;
    }
}