using System.Text.Json;
using System.Text;
using WM.SysProductos.EN;
using System.Text.Json.Serialization;

namespace WM.SysProductos.AppWeb.Service
{
    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public GeminiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<string> GenerarAnalisisVentasAsync(List<Venta> ventas)
        {
            string apiKey = _config["Gemini:ApiKey"];
            string endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}";
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            string prompt = $@"
                Actúa como un analista de negocios y genera un resumen ejecutivo en formato HTML para mostrar en un reporte web. 
                El resumen debe ser claro, profesional y fácil de leer, con encabezados, listas y énfasis donde corresponda.

                Incluye:
                - Título principal con la fecha del análisis.
                - Secciones con encabezados (h2 o h3) para cada parte: Resumen General, Clientes Más Relevantes, Productos Destacados, Ventas Canceladas, Puntos Clave, Recomendaciones.
                - Usa listas ordenadas o desordenadas para puntos y recomendaciones.
                - Usa negrita para resaltar nombres, cifras o conceptos importantes.
                - Evita etiquetas de estilo inline o CSS, solo HTML semántico básico.

                Datos a analizar (formato JSON):
                {JsonSerializer.Serialize(ventas, options)}
            ";


            var request = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Error al llamar a la API de Gemini");

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<JsonElement>(responseStream);
            return result.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();
        }
    }
}
