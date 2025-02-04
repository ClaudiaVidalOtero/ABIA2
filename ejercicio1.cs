using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ejercicio1
{
    class Program
    {
        // Diccionario para almacenar los estudiantes y sus notas
        static Dictionary<string, List<double>> estudiantes = new Dictionary<string, List<double>>();
        
        // Variable para almacenar el archivo actual cargado
        static string archivoActual = "";

        public static void Ejecutar()
        {
            int opcion;
            do
            {
                // Menú de opciones
                Console.WriteLine("\n¿Qué acción desea realizar?");
                Console.WriteLine("1. Cargar los datos de un fichero");
                Console.WriteLine("2. Obtener la nota media");
                Console.WriteLine("3. Obtener la nota máxima");
                Console.WriteLine("4. Obtener la nota mínima");
                Console.WriteLine("5. Añadir una nueva nota");
                Console.WriteLine("6. Guardar");
                Console.WriteLine("7. Cerrar");
                Console.Write("Seleccione una opción: ");

                // Validamos la entrada del usuario (vemos si opcion es int)
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

        // Método para cargar datos desde un archivo CSV
        static void CargarDatos()
        {
            Console.Write("Ingrese la ruta del archivo CSV: ");
            string ruta = Console.ReadLine();

            // Verifica si el archivo existe
            if (File.Exists(ruta))
            {
                estudiantes.Clear();  // Limpia los datos actuales
                archivoActual = ruta;  // Guarda la ruta del archivo actual

                // Lee cada línea del archivo
                foreach (var linea in File.ReadLines(ruta))
                {
                    var partes = linea.Split(';');  // Separa el nombre y la nota
                    if (partes.Length == 2 && double.TryParse(partes[1], out double nota))
                    {
                        string nombre = partes[0].Trim();
                        if (!estudiantes.ContainsKey(nombre))
                        {
                            estudiantes[nombre] = new List<double>();
                        }
                        estudiantes[nombre].Add(nota);  // Agrega la nota al estudiante
                    }
                }
                Console.WriteLine("Datos cargados correctamente.");
            }
            else
            {
                Console.WriteLine("El archivo no existe.");
            }
        }

        // Método para calcular la nota media de todos los estudiantes
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

        // Método para calcular la nota máxima
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

        // Método para calcular la nota mínima
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

        // Método para añadir una nueva nota a un estudiante
        static void AnadirNota()
        {
            if (estudiantes.Count == 0)
            {
                Console.WriteLine("No hay datos cargados.");
                return;
            }
            Console.Write("Ingrese el nombre del estudiante: ");
            string nombre = Console.ReadLine();

            // Verifica si el estudiante existe
            if (estudiantes.ContainsKey(nombre))
            {
                Console.Write("Ingrese la nueva nota: ");
                if (double.TryParse(Console.ReadLine(), out double nota))
                {
                    estudiantes[nombre].Add(nota);  // Agrega la nueva nota
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

        // Método para guardar los datos en el archivo CSV
        static void GuardarDatos()
        {
            if (string.IsNullOrEmpty(archivoActual) || estudiantes.Count == 0)
            {
                Console.WriteLine("No hay datos para guardar.");
                return;
            }

            // Escribe los datos en el archivo
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
}
