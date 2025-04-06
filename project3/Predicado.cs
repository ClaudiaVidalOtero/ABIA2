
/// <summary>
/// Representa un predicado lógico en STRIPS, como En(5, 0, 1) o Vacia(2, 2)
/// </summary>
class Predicado
{
    public string Nombre { get; private set; }
    public List<int> Argumentos { get; private set; }

    public Predicado(string nombre, params int[] argumentos)
    {
        Nombre = nombre;
        Argumentos = argumentos.ToList();
    }

    public override bool Equals(object obj)
    {
        if (obj is Predicado otro)
        {
            return Nombre == otro.Nombre && Argumentos.SequenceEqual(otro.Argumentos);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Nombre, string.Join(",", Argumentos));
    }

    public override string ToString()
    {
        return $"{Nombre}({string.Join(",", Argumentos)})";
    }
}
