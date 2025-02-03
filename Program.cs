using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// --------------------------------------
//             EJERCICIO 1
//---------------------------------------
/*
class Program
{
    static Dictionary<string, List<double>> estudiantes = new Dictionary<string, List<double>>();
    static string archivoActual = "";

    static void Main()
    {
        int opcion;
        do
        {
            Console.WriteLine("\n¿Qué acción desea realizar?");
            Console.WriteLine("1. Cargar los datos de un fichero");
            Console.WriteLine("2. Obtener la nota media");
            Console.WriteLine("3. Obtener la nota máxima");
            Console.WriteLine("4. Obtener la nota mínima");
            Console.WriteLine("5. Añadir una nueva nota");
            Console.WriteLine("6. Guardar");
            Console.WriteLine("7. Cerrar");
            Console.Write("Seleccione una opción: ");

            if (int.TryParse(Console.ReadLine(), out opcion))
            {
                switch (opcion)
                {
                    case 1:
                        CargarDatos();
                        break;
                    case 2:
                        CalcularMedia();
                        break;
                    case 3:
                        CalcularMaximo();
                        break;
                    case 4:
                        CalcularMinimo();
                        break;
                    case 5:
                        AnadirNota();
                        break;
                    case 6:
                        GuardarDatos();
                        break;
                    case 7:
                        Console.WriteLine("Cerrando el programa...");
                        break;
                    default:
                        Console.WriteLine("Opción inválida. Intente de nuevo.");
                        break;
                }
            }
        } while (opcion != 7);
    }

    static void CargarDatos()
    {
        Console.Write("Ingrese la ruta del archivo CSV: ");
        string ruta = Console.ReadLine();
        if (File.Exists(ruta))
        {
            estudiantes.Clear();
            archivoActual = ruta;
            foreach (var linea in File.ReadLines(ruta))
            {
                var partes = linea.Split(';');
                if (partes.Length == 2 && double.TryParse(partes[1], out double nota))
                {
                    string nombre = partes[0].Trim();
                    if (!estudiantes.ContainsKey(nombre))
                    {
                        estudiantes[nombre] = new List<double>();
                    }
                    estudiantes[nombre].Add(nota);
                }
            }
            Console.WriteLine("Datos cargados correctamente.");
        }
        else
        {
            Console.WriteLine("El archivo no existe.");
        }
    }

    static void CalcularMedia()
    {
        if (estudiantes.Count == 0)
        {
            Console.WriteLine("No hay datos cargados.");
            return;
        }
        double media = estudiantes.SelectMany(e => e.Value).Average();
        Console.WriteLine($"Nota media: {media:F2}");
    }

    static void CalcularMaximo()
    {
        if (estudiantes.Count == 0)
        {
            Console.WriteLine("No hay datos cargados.");
            return;
        }
        double maximo = estudiantes.SelectMany(e => e.Value).Max();
        Console.WriteLine($"Nota máxima: {maximo:F2}");
    }

    static void CalcularMinimo()
    {
        if (estudiantes.Count == 0)
        {
            Console.WriteLine("No hay datos cargados.");
            return;
        }
        double minimo = estudiantes.SelectMany(e => e.Value).Min();
        Console.WriteLine($"Nota mínima: {minimo:F2}");
    }

    static void AnadirNota()
    {
        if (estudiantes.Count == 0)
        {
            Console.WriteLine("No hay datos cargados.");
            return;
        }
        Console.Write("Ingrese el nombre del estudiante: ");
        string nombre = Console.ReadLine();
        if (estudiantes.ContainsKey(nombre))
        {
            Console.Write("Ingrese la nueva nota: ");
            if (double.TryParse(Console.ReadLine(), out double nota))
            {
                estudiantes[nombre].Add(nota);
                Console.WriteLine("Nota añadida correctamente.");
            }
            else
            {
                Console.WriteLine("Nota inválida.");
            }
        }
        else
        {
            Console.WriteLine("Estudiante no encontrado.");
        }
    }

    static void GuardarDatos()
    {
        if (string.IsNullOrEmpty(archivoActual) || estudiantes.Count == 0)
        {
            Console.WriteLine("No hay datos para guardar.");
            return;
        }
        using (StreamWriter sw = new StreamWriter(archivoActual))
        {
            foreach (var estudiante in estudiantes)
            {
                sw.WriteLine($"{estudiante.Key};{string.Join(",", estudiante.Value)}");
            }
        }
        Console.WriteLine("Datos guardados correctamente.");
    }
}

*/

// --------------------------------------
//             EJERCICIO 2
//---------------------------------------
using System;
using System.Collections.Generic;

class Bolsa
{
    private List<string> bolas;
    private Random random;

    public Bolsa()
    {
        bolas = new List<string> { "azul", "roja", "verde" };
        random = new Random();
    }

    public string RetirarBola()
    {
        if (bolas.Count == 0)
            return "No quedan bolas";

        int index = random.Next(bolas.Count);
        string bolaSeleccionada = bolas[index];
        bolas.RemoveAt(index);
        return bolaSeleccionada;
    }

    public List<string> ObtenerBolas()
    {
        return new List<string>(bolas);
    }
}

class Perro
{
    public string ColorPelo { get; private set; }
    public double Altura { get; private set; }
    public double Peso { get; private set; }
    public Bolsa BolsaDeBolas { get; private set; }

    public Perro(string colorPelo, double altura, double peso, Bolsa bolsa)
    {
        ColorPelo = colorPelo;
        Altura = altura;
        Peso = peso;
        BolsaDeBolas = bolsa;
    }

    public string ComerBola()
    {
        return BolsaDeBolas.RetirarBola();
    }
}

class Program
{
    static void Main()
    {
        Bolsa bolsaCompartida = new Bolsa();

        Console.Write("Ingrese el color del perro: ");
        string color = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(color)) color = "rojo";
        
        Console.Write("Ingrese la altura del perro: ");
        string alturaInput = Console.ReadLine();
        double altura = string.IsNullOrWhiteSpace(alturaInput) ? 20 : double.Parse(alturaInput);
        
        Console.Write("Ingrese el peso del perro: ");
        string pesoInput = Console.ReadLine();
        double peso = string.IsNullOrWhiteSpace(pesoInput) ? 1.8 : double.Parse(pesoInput);
        
        Perro perroOriginal = new Perro(color, altura, peso, bolsaCompartida);
        
        Console.Write("¿Cuántas veces quieres clonar el perro? ");
        if (int.TryParse(Console.ReadLine(), out int cantidadClones) && cantidadClones > 0)
        {
            Console.WriteLine($"Color: {perroOriginal.ColorPelo} Altura: {perroOriginal.Altura} Peso: {perroOriginal.Peso} Bolsa: {string.Join(" ", bolsaCompartida.ObtenerBolas())}");
            
            for (int i = 1; i <= cantidadClones; i++)
            {
                Perro clon = new Perro(perroOriginal.ColorPelo, perroOriginal.Altura, perroOriginal.Peso, bolsaCompartida);
                string bolaComida = clon.ComerBola();
                Console.WriteLine($"Color: {clon.ColorPelo} Altura: {clon.Altura} Peso: {clon.Peso} Bolsa: {string.Join(" ", bolsaCompartida.ObtenerBolas())}");
            }
        }
        else
        {
            Console.WriteLine("Entrada inválida. Debe ingresar un número entero positivo.");
        }
    }
}
