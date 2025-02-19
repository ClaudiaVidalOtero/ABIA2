partial class Program
{
    static void Main()
    {
        int reinas = 6;

         
        public int calculo_coste(Solucion solucion, Solucion nueva_solucion)
        {
            return 1;
        }
        public int calculo_heuristica(Solucion solucion)
        {
            return 0;
        }

        public List<(int, int)> obtener_vecinos(Solucion solucion, int reinas)
        {
            int row = solucion.Coords.Count == 0 ? -1 : solucion.Coords.Last().Item1;

            List<(int, int)> vecinos = new();

            if (row + 1 < reinas)
            {
                for (int j = 0; j < reinas; j++)
                    vecinos.Add((row + 1, j));
            }
            return vecinos;
        }

        public  bool criterio_parada(Solucion solucion, int reinas)
        {
            if (solucion.Coords.Count < reinas) return false;
            for (int i = 0; i < solucion.Coords.Count; i++)
            {
                (int fila_i, int columna_i) nodo_i = solucion.Coords[i];
                for (int j = i + 1; j < solucion.Coords.Count; j++)
                {
                    (int fila_j, int columna_j) nodo_j = solucion.Coords[j];
                    if (nodo_j.Item2 == nodo_i.Item2 || Math.Abs(nodo_j.Item2 - nodo_i.Item2) == Math.Abs(j - i))
                        return false;
                }
            }
            return true;
        }
    

        AEstrella astar = new AEstrella();
        var result = astar.Search(new List<(int, int)>(), criterio_parada, obtener_vecinos, calculo_coste, calculo_heuristica);

        Console.WriteLine("Coordinates: " + string.Join(", ", result.Value.solution.Coords));
        Console.WriteLine("Nodes Evaluated: " + result.Value.nodesEvaluated);
        
        else
        {
            Console.WriteLine("No solution found.");
        }
    }
}





