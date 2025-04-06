// Claudia Vidal Otero (claudia.votero@gudc.es)
// Aldana Smyna Medina Lostaunau (aldana.medina@udc.es)
// Grupo 2 (Jueves)

class Estado
{
    public int[,] Tablero { get; private set; }
    public int Costo { get; private set; }
    public int Heuristica { get; private set; }
    public Estado Padre { get; private set; }

    public Estado(int[,] tablero, int costo = 0, Estado padre = null)
    {
        Tablero = (int[,])tablero.Clone();
        Costo = costo;
        Padre = padre;
        Heuristica = CalcularHeuristica();
    }

    private int CalcularHeuristica()
    {
        int heuristica = 0;
        // Tablero objetivo
        int[,] objetivoTablero = new int[,] {
            { 1, 2, 3 },
            { 4, 5, 6 },
            { 7, 8, 0 }
        };

        // Suma de distancias de Manhattan
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int valor = Tablero[i, j];
                if (valor != 0)
                {
                    int objetivoX = (valor - 1) / 3;
                    int objetivoY = (valor - 1) % 3;
                    heuristica += Math.Abs(i - objetivoX) + Math.Abs(j - objetivoY);
                }
            }
        }
        return heuristica;
    }

    public Estado AplicarAccion(Accion accion)
    {
        (int x, int y) = ObtenerPosVacia();
        int[,] nuevoTablero = (int[,])Tablero.Clone();

        switch (accion.Nombre)
        {
            case "MoverArriba":
                if (x > 0)
                {
                    nuevoTablero[x, y] = nuevoTablero[x - 1, y];
                    nuevoTablero[x - 1, y] = 0;
                }
                break;
            case "MoverAbajo":
                if (x < 2)
                {
                    nuevoTablero[x, y] = nuevoTablero[x + 1, y];
                    nuevoTablero[x + 1, y] = 0;
                }
                break;
            case "MoverIzquierda":
                if (y > 0)
                {
                    nuevoTablero[x, y] = nuevoTablero[x, y - 1];
                    nuevoTablero[x, y - 1] = 0;
                }
                break;
            case "MoverDerecha":
                if (y < 2)
                {
                    nuevoTablero[x, y] = nuevoTablero[x, y + 1];
                    nuevoTablero[x, y + 1] = 0;
                }
                break;
        }

        return new Estado(nuevoTablero, Costo + 1, this);
    }

    public bool Satisface(Estado objetivo)
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (Tablero[i, j] != objetivo.Tablero[i, j])
                    return false;
        return true;
    }

    public void MostrarTablero()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Console.Write(Tablero[i, j] == 0 ? "  " : Tablero[i, j] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    public (int x, int y) ObtenerPosVacia()
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (Tablero[i, j] == 0)
                    return (i, j);
        return (-1, -1);
    }

    public int CalcularCostoTotal()
    {
        return Costo + Heuristica;
    }

    // Convertir el tablero a string para comparar estados
    public override string ToString()
    {
        string s = "";
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                s += Tablero[i, j] + ",";
        return s;
    }

    public override bool Equals(object obj)
    {
        if (obj is Estado otro)
            return this.ToString() == otro.ToString();
        return false;
    }

}