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
        if (result.HasValue)
        {
            Console.WriteLine("Coordinates: " + string.Join(", ", result.Value.solution.Coords));
            Console.WriteLine("Nodes Evaluated: " + result.Value.nodesEvaluated);
        }
        else
        {
            Console.WriteLine("No solution found.");
        }
    }
}






if __name__ == '__main__':
    solucion_inicial = []
    reinas = 6


    def obtener_vecinos(solucion):
        if len(solucion.coords) == 0:
            row = -1
        else:
            row, _ = solucion.coords[-1]
        vecinos = []
        if row + 1 < reinas:
            for j in range(reinas):
                vecinos.append((row + 1, j))
        return vecinos

    def criterio_parada(solucion):
        if len(solucion.coords) < reinas:
            return False
        for i in range(len(solucion.coords)):
            nodo_i = solucion.coords[i]
            for j  in range(i+1, len(solucion.coords)):
                nodo_j = solucion.coords[j]
                if nodo_j[-1] == nodo_i[-1] or abs(nodo_j[-1] - nodo_i[-1]) == abs(j - i):
                    return False
        return True

    astar = AEstrella()
    solucion, revisados = astar.busqueda(solucion_inicial, criterio_parada, obtener_vecinos, calculo_coste, calculo_heuristica)
    print('Coordenadas:', solucion.coords)
    print('Nodos evaluadas:', revisados)


