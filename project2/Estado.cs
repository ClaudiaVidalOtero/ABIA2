// Claudia Vidal Otero (claudia.votero@udc.es)
// Aldana Smyna Medina Lostaunau (aldana.medina@udc.es)
// Grupo 2 (Jueves)

/// <summary>
/// Representa un estado del mundo en el sistema. 
/// Cada estado está definido por un conjunto de predicados que indican qué condiciones son verdaderas.
/// </summary>
class Estado
{
    // Conjunto de predicados que representan el estado actual del mundo (bloques).
    public HashSet<string> Predicados { get; private set; }

    // Constructor: crea un nuevo estado a partir de un conjunto de predicados.
    public Estado(IEnumerable<string> predicados)
    {
        Predicados = new HashSet<string>(predicados);
    }

    /// <summary>
    /// Verifica si todos los predicados del objetivo están presentes en el estado actual.
    /// Es decir, si el estado actual cumple completamente con el estado meta.
    /// </summary>
    public bool Satisface(HashSet<string> objetivos)
    {
        return objetivos.IsSubsetOf(Predicados);
    }

    /// <summary>
    /// Aplica una acción al estado actual, solo si se cumplen todas sus precondiciones.
    /// </summary>
    /// <param name="accion">Acción que se desea aplicar</param>
    /// <returns>Un nuevo estado con los efectos de la acción aplicados, o null si la acción no es aplicable</returns>
    public Estado AplicarAccion(Accion accion)
    {
        // Verifica que el estado actual satisface todas las precondiciones de la acción
        if (!Satisface(new HashSet<string>(accion.Precondiciones))) return null;

        // Copia el estado actual para modificarlo sin alterar el original
        HashSet<string> nuevoEstado = new HashSet<string>(Predicados);

        // Aplica los efectos positivos (añadir predicados)
        foreach (string efecto in accion.EfectosPositivos)
        {
            nuevoEstado.Add(efecto);
        }

        // Aplica los efectos negativos (eliminar predicados)
        foreach (string efecto in accion.EfectosNegativos)
        {
            nuevoEstado.Remove(efecto);
        }

        // Devuelve un nuevo objeto Estado con los predicados resultantes
        return new Estado(nuevoEstado);
    }

    /// <summary>
    /// Guarda los bloques que están directamente sobre la mesa.
    /// Se usa para saber el orden inicial de las pilas, para la representación gráfica.
    /// </summary>
    /// <returns>Lista de bloques base (los que están "Encima(...,Mesa)")</returns>
    public List<string> ObtenerOrdenDesdeEstado()
    {
        return Predicados
            .Where(p => p.StartsWith("Encima(") && p.Contains(",Mesa)")) // Solo los que están sobre la mesa
            .Select(p => p.Substring(7, p.Length - 8).Split(',')[0])     // Extrae el nombre del bloque
            .ToList();
    }

    /// <summary>
    /// Visualiza gráficamente las pilas de bloques, desde la base (mesa) hasta los bloques superiores.
    /// </summary>
    /// <param name="ordenBloques">Orden establecido para mostrar las pilas (para mantener coherencia visual)</param>
    public void MostrarBloques(List<string> ordenBloques)
    {
        Dictionary<string, string> encimaDe = new Dictionary<string, string>(); // Relación: bloque → lo que tiene debajo
        Dictionary<string, string> debajoDe = new Dictionary<string, string>(); // Relación: bloque → lo que tiene encima
        HashSet<string> bloques = new HashSet<string>(); // Todos los bloques involucrados

        // Construcción de relaciones a partir de los predicados "Encima(X,Y)"
        foreach (string pred in Predicados)
        {
            if (pred.StartsWith("Encima("))
            {
                string[] partes = pred.Substring(7, pred.Length - 8).Split(',');
                string arriba = partes[0];
                string abajo = partes[1];
                encimaDe[arriba] = abajo;
                debajoDe[abajo] = arriba;
                bloques.Add(arriba);
                bloques.Add(abajo);
            }
        }

        // Se identifican los bloques base (los que están directamente sobre la mesa)
        List<string> bases = encimaDe
            .Where(par => par.Value == "Mesa")
            .Select(par => par.Key)
            .OrderBy(bloque =>
            {
                // Se ordenan según el orden deseado (para presentación)
                int indice = ordenBloques.IndexOf(bloque);
                return indice >= 0 ? indice : int.MaxValue;
            })
            .ToList();

        // Se construyen las pilas desde cada bloque base hacia arriba
        List<List<string>> pilas = new List<List<string>>();
        foreach (string baseBloque in bases)
        {
            List<string> pila = new List<string> { baseBloque };
            string actual = baseBloque;

            // Se agregan todos los bloques apilados encima del actual
            while (debajoDe.ContainsKey(actual))
            {
                string siguiente = debajoDe[actual];
                pila.Add(siguiente);
                actual = siguiente;
            }

            pilas.Add(pila);
        }

        // Se determina la pila más alta para imprimir correctamente la visualización
        int alturaMaxima = pilas.Any() ? pilas.Max(p => p.Count) : 0;
        Console.WriteLine("Visualización de bloques:");

        // Impresión vertical de las pilas (de arriba hacia abajo)
        for (int i = alturaMaxima - 1; i >= 0; i--)
        {
            foreach (List<string> pila in pilas)
            {
                if (i < pila.Count)
                {
                    Console.Write($"| {pila[i]} | ");
                }
                else
                {
                    Console.Write("      "); // Espacio si no hay bloque en esa altura
                }
            }
            Console.WriteLine();
        }

        // Dibuja la "mesa" debajo de las pilas
        Console.WriteLine(new string('-', 6 * pilas.Count));
        for (int i = 0; i < pilas.Count; i++)
        {
            Console.Write("Mesa  ");
        }
        Console.WriteLine("\n");
    }

    /// <summary>
    /// Muestra todos los predicados del estado actual y su representación visual como pilas de bloques.
    /// </summary>
    /// <param name="ordenBloques">Orden visual establecido para mostrar las torres</param>
    public void ImprimirEstado(List<string> ordenBloques)
    {
        Console.WriteLine("Estado actual:");
        foreach (string predicado in Predicados)
        {
            Console.WriteLine("  " + predicado);
        }
        Console.WriteLine();
        MostrarBloques(ordenBloques);
    }
}
