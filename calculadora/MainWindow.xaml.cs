using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace calculadora
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }



        private void NumeroButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string numero = button.Content.ToString();
            if (numero == "x")
            {
                numero = "*";
            }
            if(numero == "x²")
            {
                numero = "^";
            }


            string textoActual = TextBoxResultado.Text;


            TextBoxResultado.Text = textoActual + numero; // de esta forma cada vez que presionamos un numero se borra el anterior

        }

        private void IgualButton_Click(object sender, RoutedEventArgs e)
        {
            string expresion = TextBoxResultado.Text;

            List<double> numerosOperacion = MainWindow.DevolverNumeroExpresion(expresion);

            List<string> operadores = MainWindow.DevolverOperadoresExpresion(expresion);

            double resultado = 0;

            if (operadores[0]== "^" || operadores[0]== "√") 
            {
                if (operadores[0] == "^")
                {
                    resultado = Math.Pow(numerosOperacion[0], 2);
                }
                if (operadores[0] == "√")
                {
                    resultado = Math.Sqrt(numerosOperacion[0]);
                }
            }
            else
            {
                resultado = numerosOperacion[0]; 

                for (int i = 0; i < operadores.Count; i++)
                {


                    double siguienteNumero = numerosOperacion[i + 1];

                    switch (operadores[i])
                    {
                        case "+":
                            resultado += siguienteNumero;
                            break;
                        case "-":
                            resultado -= siguienteNumero;
                            break;
                        case "*":
                            resultado *= siguienteNumero;
                            break;
                        case "÷":
                            if (siguienteNumero != 0)
                            {
                                resultado /= siguienteNumero;
                            }
                            else
                            {
                                TextBoxResultado.Text = "Error: División por cero.";
                                return;
                            }
                            break;
                        default:

                            TextBoxResultado.Text = "Error: Operador no reconocido.";
                            return;
                    }
                }
            }


            TextBoxResultado.Text = resultado.ToString();

        }

        private static List<double> DevolverNumeroExpresion(string expresion)
        {
            Regex regex = new Regex(@"\b\d+(\,\d+)?\b");
            MatchCollection matches = regex.Matches(expresion);

            List<double> numerosOperacion = new List<double>();

            foreach(Match match in matches)
            {
                if(double.TryParse(match.Value,out double numero))
                {
                    numerosOperacion.Add(numero);
                }
            }

            return numerosOperacion;
        }
        
        private static List<string> DevolverOperadoresExpresion(string expresion)
        {
            Regex regex = new Regex(@"[\+\-\*/÷/^/√]");

            MatchCollection matches = regex.Matches(expresion);

            List<string> operadores = new List<string>();

            foreach(Match m in matches)
            {
                operadores.Add(m.Value);
            }

            return operadores;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            TextBoxResultado.Text = "";
        }
    }
}
