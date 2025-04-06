// Claudia Vidal Otero (claudia.votero@udc.es)
// Aldana Smyna Medina Lostaunau (aldana.medina@udc.es)
// Grupo 2 (Jueves)

/// <summary>
/// Representa un predicado lógico en el estilo STRIPS, como En(5, 0, 1) o Vacia(2, 2).
/// Cada predicado tiene un nombre y una lista de argumentos enteros que definen su instancia.
/// </summary>
class Predicado
{
    public string Nombre { get; private set; }
    public List<int> Argumentos { get; private set; }

    /// <summary>
    /// Constructor del predicado que recibe el nombre y una lista variable de argumentos.
    /// </summary>
    /// <param name="nombre">Nombre del predicado </param>
    /// <param name="argumentos">Argumentos enteros del predicado</param>
    public Predicado(string nombre, params int[] argumentos)
    {
        Nombre = nombre;
        Argumentos = argumentos.ToList(); // Se convierte el array a lista para facilitar su uso
    }

    /// <summary>
    /// Determina si dos predicados son equivalentes, es decir,
    /// si tienen el mismo nombre y los mismos argumentos en el mismo orden.
    /// </summary>
    public override bool Equals(object obj)
    {
        if (obj is Predicado otro)
        {
            // Comparación: nombre y secuencia de argumentos
            return Nombre == otro.Nombre && Argumentos.SequenceEqual(otro.Argumentos);
        }
        return false;
    }

    /// <summary>
    /// Calcula el hash del predicado para permitir su uso en estructuras como HashSet o diccionarios.
    /// </summary>
    public override int GetHashCode()
    {
        // Combina el nombre y los argumentos como string para generar un hash único
        return HashCode.Combine(Nombre, string.Join(",", Argumentos));
    }

    /// <summary>
    /// Devuelve una representación en texto del predicado.
    /// </summary>
    public override string ToString()
    {
        return $"{Nombre}({string.Join(",", Argumentos)})";
    }
}

