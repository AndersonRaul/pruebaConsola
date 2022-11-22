using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace preba.Entidades
{
    public class Persona
    {
        ConexionDB con = new ConexionDB();
        public Persona() { }
        
        [JsonProperty("tipo_documento")]
        public string TipoDocumento { get; set; }
        [JsonProperty("num_documento")]
        public string NumDocumento { get; set; }
        [JsonProperty("apellidos")]
        public string Apellidos { get; set; }
        [JsonProperty("nombres")]
        public string Nombres { get; set; }
        [JsonProperty("nombre_comercial")]
        public string NombreComercial { get;set; }
        [JsonIgnore]
        public string Provincia { get; set; }
        [JsonIgnore]
        public string Canton { get;set;}
        [JsonIgnore]
        public string Parroquia { get; set; }
        [JsonProperty("clase_contribuyente")]
        public string ClaseContribuyente { get; set; }
        [JsonProperty("tipo_contribuyente")]
        public string TipoContribuyente { get; set; }

        public string crud_persona()
        {
            string rpta;
            try
            {
                string json = JsonConvert.SerializeObject(this);
                con.Conectar();
                NpgsqlCommand command = new NpgsqlCommand("select crud_persona (@option,@_json::json);", con.Connection);
                command.Parameters.AddWithValue("option", NpgsqlTypes.NpgsqlDbType.Varchar, "IU");
                command.Parameters.AddWithValue("@_json", NpgsqlTypes.NpgsqlDbType.Varchar, json);
                rpta = command.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                rpta = @"{""status"": ""ok"",""code"": 400,""message"": """ + ex.Message + @""",""table"": ""Codigo retencion""}";
            }
            finally
            {
                con.Desconectar();
            }
            return rpta;
        }

        public string crud_persona(List<Persona> personas)
        {
            string rpta;
            try
            {
                string json = JsonConvert.SerializeObject(personas);
                con.Conectar();
                NpgsqlCommand command = new NpgsqlCommand("select crud_persona (@option,@_json::json);", con.Connection);
                command.Parameters.AddWithValue("option", NpgsqlTypes.NpgsqlDbType.Varchar, "IU");
                command.Parameters.AddWithValue("@_json", NpgsqlTypes.NpgsqlDbType.Varchar, json);
                rpta = command.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                rpta = @"{""status"": ""ok"",""code"": 400,""message"": """ + ex.Message + @""",""table"": ""Codigo retencion""}";
            }
            finally
            {
                con.Desconectar();
            }
            return rpta;
        }

    }
}
