using System;
using System.Collections.Generic;
using System.Linq;

class Estado
{
    public HashSet<string> Predicados { get; private set; }

    public Estado(IEnumerable<string> predicados)
    {
        Predicados = new HashSet<string>(predicados);
    }

    public bool Satisface(HashSet<string> objetivos)
    {
        return objetivos.IsSubsetOf(Predicados);
    }

    public Estado AplicarAccion(Accion accion)
    {
        if (!Satisface(new HashSet<string>(accion.Precondiciones))) return null;

        HashSet<string> nuevoEstado = new HashSet<string>(Predicados);
        foreach (string efecto in accion.EfectosPositivos)
        {
            nuevoEstado.Add(efecto);
        }
        foreach (string efecto in accion.EfectosNegativos)
        {
            nuevoEstado.Remove(efecto);
        }
        return new Estado(nuevoEstado);
    }

    public List<string> ObtenerOrdenDesdeEstado()
    {
        // Devuelve los bloques que están en la mesa según el orden en que aparecen
        return Predicados
            .Where(p => p.StartsWith("Encima(") && p.Contains(",Mesa)"))
            .Select(p => p.Substring(7, p.Length - 8).Split(',')[0])
            .ToList();
    }

    public void MostrarBloques(List<string> ordenDeseado)
    {
        Dictionary<string, string> encimaDe = new Dictionary<string, string>();
        Dictionary<string, string> debajoDe = new Dictionary<string, string>();
        HashSet<string> bloques = new HashSet<string>();

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

        // Obtener todas las bases actuales del estado (bloques sobre la mesa)
        List<string> bases = encimaDe
            .Where(kv => kv.Value == "Mesa")
            .Select(kv => kv.Key)
            .OrderBy(b =>
            {
                int index = ordenDeseado.IndexOf(b);
                return index >= 0 ? index : int.MaxValue;
            })
            .ToList();

        // Construir las pilas desde la base hacia arriba
        List<List<string>> pilas = new List<List<string>>();
        foreach (string baseBloque in bases)
        {
            List<string> pila = new List<string> { baseBloque };
            string actual = baseBloque;

            while (debajoDe.ContainsKey(actual))
            {
                string siguiente = debajoDe[actual];
                pila.Add(siguiente);
                actual = siguiente;
            }

            pilas.Add(pila);
        }

        int alturaMaxima = pilas.Any() ? pilas.Max(p => p.Count) : 0;
        Console.WriteLine("Visualización de bloques:");
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
                    Console.Write("      ");
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine(new string('-', 6 * pilas.Count));
        for (int i = 0; i < pilas.Count; i++)
        {
            Console.Write("Mesa  ");
        }
        Console.WriteLine("\n");
    }

    public void ImprimirEstado(List<string> ordenDeseado)
    {
        Console.WriteLine("Estado actual:");
        foreach (string predicado in Predicados)
        {
            Console.WriteLine("  " + predicado);
        }
        Console.WriteLine();
        MostrarBloques(ordenDeseado);
    }
}
