
if __name__ == '__main__':
    solucion_inicial = []
    reinas = 6

    def calculo_coste(solucion, nueva_solucion):
        return 1

    def calculo_heuristica(solucion):
        return 0

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


