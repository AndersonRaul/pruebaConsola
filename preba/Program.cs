using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using preba.Entidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace preba
{
    internal class Program
    {
        static void ObtenerDatosServidorWeb()
        {
            
            var destinationUrl = "https://srienlinea.sri.gob.ec/movil-servicios/api/v1.0/deudas/porIdentificacion/1724338684";
            //var destinationUrl = "http://198.74.55.66:8080/sisjake/api/personas/numdoc/1724338684";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
            request.Host = "srienlinea.sri.gob.ec";
            request.UserAgent = "server";
            //request.Headers.Add("Authorization", "Bearer eyJhbGciOiJIUzUxMiJ9.eyJhdXRob3JpdGllcyI6WyJST0xFX1VTRVIiXSwiaWF0IjoxNjY1MjA5NTcwfQ.NqwBuRNls8XGWUqeqTeHm-Yv_n7V3XwzrP0nePNHM6AHRI4ywiGGpfnZuuVduubktFSD3949aj56JN2cv3tm2Q");
            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            string responseText = new StreamReader(responseStream).ReadToEnd();
        }

        static void descargarPdf()
        {
            //Construct HTTP request to get the logo
            /*HttpWebRequest httpRequest = (HttpWebRequest)
            WebRequest.Create("http://198.74.55.66:8080/eFacturacion/api/comprobantes_electronicos/pdf/1210202201230065441100110010230000001000000002316");
            httpRequest.Method = WebRequestMethods.Http.Post;
            httpRequest.Headers.Set("Authorization", "eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJqbnR1aDIzIiwiYXV0aG9yaXRpZXMiOlsiUk9MRV9VU0VSIl0sInN1Yl9pZCI6MTAsImlhdCI6MTY2NTY4OTQyNSwiZXhwIjoxNjY1NzIzNjAwfQ.wWu09aES5LoqUzHxno2s143N1fRT4x07rdUbAV0CqLdL0iDfX6RUSx3fE5PkqvvAmfaCYXwygK7s4NTzPvawog");

            HttpWebResponse httpResponse = null;
            String fileName = @"d:\\factura.pdf";

            try
            {
                // Get back the HTTP response for web server
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                Stream httpResponseStream = httpResponse.GetResponseStream();

                // Define buffer and buffer size
                int bufferSize = 1024;
                byte[] buffer = new byte[bufferSize];
                int bytesRead = 0;

                
                // Read from response and write to file
                FileStream fileStream = File.Create(fileName);
                while ((bytesRead = httpResponseStream.Read(buffer, 0, bufferSize)) != 0)
                {
                    fileStream.Write(buffer, 0, bytesRead);
                }

                
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    httpResponse = (HttpWebResponse)e.Response;
                    Console.Write("Errorcode: {0}", (int)httpResponse.StatusCode);
                }
                else
                {
                    Console.Write("Error: {0}", e.Status);
                }
            }
            finally
            {
                if (httpResponse != null)
                {
                    httpResponse.Close();
                }
            }

            System.Diagnostics.Process.Start(fileName);
            */
        }

        private static int ColumnRuc;
        private static int ColumnRazonSocial;
        private static int ColumnClaseContribuyente;
        private static int ColumnTipoContribuyente;
        private static int ColumnNombreComercial;

        private static int AsignarColumnas(string header, char separador, string nameColumn)
        {
            string[] columns = header.Split(separador);
            for (int i = 0; i < columns.Length; i++)
            {
                if (columns[i].Equals(nameColumn))
                {
                    return i;
                }
            }
            return -1;
        }


        static string desencriptar(string password)
        {
            /*========== CLAVE DEL EMAIL ===============*/
            string pass = "";
            byte b2;
            byte[] b1 = new byte[1];
            for (int i = 0; i < password.Length; i++)
            {
                string input = Convert.ToString(password[i]);
                Encoding ANSI = Encoding.GetEncoding(1252);
                byte[] ansiBytes = ANSI.GetBytes(input);
                b2 = (byte)(Convert.ToByte(ansiBytes.GetValue(0)) - 17);
                b1[0] = b2;
                string utf8String = Encoding.Default.GetString(b1);
                pass = pass + utf8String;
            }
            /*============== FIN CLAVE ==================*/
            return pass;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hola mundo");

            Console.WriteLine(desencriptar("R_WZSZfd5SR_UZU`CAAH"));
            Console.ReadKey();
            return;

            //String file = @"D:\SRI\CATASTRO RUC\SRI_RUC_Azuay.csv"; // \t
            String file = @"D:\SRI\CATASTRO RUC\SRI_RUC_Santo_Domingo_De_Los_Tsachilas.csv";
            char separador = '|';
            string[] lines = File.ReadAllLines(file, Encoding.Default);

            //Asignando columnas 
            ColumnRuc = AsignarColumnas(lines[0].Replace("\"", ""), '|', "NUMERO_RUC");
            ColumnRazonSocial = AsignarColumnas(lines[0].Replace("\"", ""), separador, "RAZON_SOCIAL");
            ColumnClaseContribuyente = AsignarColumnas(lines[0].Replace("\"", ""), separador, "CLASE_CONTRIBUYENTE");
            ColumnTipoContribuyente = AsignarColumnas(lines[0].Replace("\"", ""), separador, "TIPO_CONTRIBUYENTE");
            ColumnNombreComercial = AsignarColumnas(lines[0].Replace("\"", ""), separador, "NOMBRE_FANTASIA_COMERCIAL");

            if (ColumnRuc < 0 || ColumnRazonSocial < 0 || ColumnClaseContribuyente < 0 || ColumnTipoContribuyente < 0 || ColumnNombreComercial < 0)
            {
                Console.WriteLine("No se encontro el indice de una columna");
                Console.ReadLine();
                return;
            }

            List<Persona> personas = new List<Persona>();
            for (int i = 216749; i < lines.Length; i++)
            {
                lines[i] = lines[i].Replace("\"", "");
                string[] columns = lines[i].Split(separador);

                if(personas.Count >= 500 || i == (lines.Length - 1))
                {
                    Console.WriteLine("Procesando Linea: " + (i - personas.Count)  + " - " + i);
                    string result = (new Persona()).crud_persona(personas);
                    JObject jsonObject = JObject.Parse(result);

                    if (Convert.ToInt32(jsonObject.GetValue("code").ToString()) != 200)
                    {
                        Console.WriteLine("Ocurrio un error" + jsonObject.GetValue("message").ToString());
                    }
                    else
                    {
                        Console.WriteLine(jsonObject.GetValue("message").ToString());
                    }
                    personas.Clear();
                    Console.WriteLine("Fin de procesamiento");
                }

                if (!(columns.Length > 0 && columns[ColumnRuc].Length == 13)) continue;

                Persona persona = new Persona();
                persona.TipoDocumento = "r";
                persona.NumDocumento = columns[ColumnRuc].Substring(0, 10);
                persona.TipoContribuyente = columns[ColumnTipoContribuyente];
                persona.ClaseContribuyente = columns[ColumnClaseContribuyente];

                if (persona.TipoContribuyente.Equals("SOCIEDADES"))
                {
                    if (columns[ColumnRazonSocial].Length < 150)
                    {
                        persona.Apellidos = columns[ColumnRazonSocial];
                        persona.Nombres = "";
                    }
                    else
                    {
                        string[] razonSocial = columns[ColumnRazonSocial].Split(' ');
                        persona.Apellidos = ""; persona.Nombres = "";
                        bool validar = true;
                        for (int j = 0; j < razonSocial.Length; j++)
                        {
                            if (validar)
                            {
                                if ((persona.Apellidos + razonSocial[j] + " ").Length < 150)
                                    persona.Apellidos += razonSocial[j] + " ";
                                else
                                    validar = false;
                            }

                            if (!validar)
                            {
                                persona.Nombres += razonSocial[j] + " ";
                            }
                        }

                        persona.Apellidos = persona.Apellidos.Trim();
                        persona.Nombres = persona.Nombres.Trim();
                    }
                }
                else
                {
                    string[] razonSocial = columns[ColumnRazonSocial].Split(' ');
                    if (razonSocial.Length == 4)
                    {
                        persona.Apellidos = razonSocial[0] + " " + razonSocial[1];
                        persona.Nombres = razonSocial[2] + " " + razonSocial[3];
                    }
                    else
                    {
                        persona.Apellidos = columns[ColumnRazonSocial];
                        persona.Nombres = "";
                    }
                }


                if (!(persona.TipoContribuyente.Equals("PERSONAS NATURALES") || persona.TipoContribuyente.Equals("SOCIEDADES")))
                {
                    Console.WriteLine("Otro contribuyente");
                }

                persona.NombreComercial = columns[ColumnNombreComercial];
                /*persona.Provincia = columns[14];
                persona.Canton = columns[15];
                persona.Parroquia = columns[16];*/
                personas.Add(persona);

            }
            Console.WriteLine("Fin");
            string json = JsonConvert.SerializeObject(personas);
            File.WriteAllText(file.Replace("csv", "json"), json);
            Console.ReadKey();
        }


    }
}
