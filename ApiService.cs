using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Microsoft.Maui.Devices;


namespace PracticaUI
{
    //Error propio: guarda el mensaje que mandó la API y el codigo http 
    //(401, 409)
    public class ApiException : Exception
    {
        public HttpStatusCode Codigo { get; }
        public ApiException(string mensaje, HttpStatusCode codigo) : base(mensaje) => Codigo = codigo;
    }

    //static: crea una sola copia compartida por todas las paginas de la app
    public static class ApiService
    {
        //El puerto 5080 es el mismo que colocamos en launchSettings.json
        private static readonly string BaseUrl = DeviceInfo.Platform == DevicePlatform.Android
            ? "http://10.0.2.2:5080/"
            : "http://localhost:5080";

        //HttpClient: el "cartero" que lleva y trae mensajes de la API
        private static readonly HttpClient Http = new()
        {
            BaseAddress = new Uri(BaseUrl),
            Timeout = TimeSpan.FromSeconds(10) //si la API está apagada, no esperamos siempre
        };

        //El token (pase) que entrega la API al iniciar sesion. null = nadie ha entrado 
        private static string? token;
        public static bool HaySesion => token is not null;

        //Llama a POST /api/auth/login. Si los datos son correctos, guarda el token.
        public static async Task LoginAsync(string correo, string clave)
        {
            var resp = await Http.PostAsJsonAsync("/api/auth/login", new { correo, clave });
            await RevisarAsync(resp); // si hubo error(401...) aqui se lanza ApiException

            var datos = await resp.Content.ReadFromJsonAsync<TokenRespuesta>();
            token = datos!.Token;

            //Desde ahora, TODA peticion lleva pase en el encabezado  Authorization
            Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        //Consulta los datos del perfil en la API
        public static async Task<PerfilRespuesta> ObtenerPerfilAsync()
        {
            var resp = await Http.GetAsync("/api/perfil");

            await RevisarAsync(resp);

            var perfil = await resp.Content.ReadFromJsonAsync<PerfilRespuesta>();

            return perfil!;
        }

        //Borra el token: es "cerrar sesion"
        public static void CerrarSesion()
        {
            token = null;
            Http.DefaultRequestHeaders.Authorization = null;
        }

        private static async Task RevisarAsync(HttpResponseMessage resp)
        {
            if (resp.IsSuccessStatusCode) return; //200, 204...: todo está bien 

            string mensaje = resp.StatusCode == HttpStatusCode.Unauthorized
                ? "Sesión no válida o vencida."
                : "La API respondió con un error.";

            try{
                //La API manda mensaje {"mensaje: "..."}: si viene, lo usamos
                var texto = (await resp.Content.ReadFromJsonAsync<ErrorRespuesta>())?.Mensaje;
                if (!string.IsNullOrWhiteSpace(texto)) mensaje = texto;

            }
            catch
            {
                //la respuesta no viene del JSON: dejamos el mensaje ppr defecto  
            }

            throw new ApiException(mensaje, resp.StatusCode);
        }

        //Moldes para leer JSON de las API 
        private record TokenRespuesta(string Token);
        private record ErrorRespuesta(String? Mensaje);


    }
}

public record PerfilRespuesta(
    string Nombre,
    bool Disponible,
    int AniosExperiencia
);
