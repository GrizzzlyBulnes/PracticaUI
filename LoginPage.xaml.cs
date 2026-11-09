namespace PracticaUI
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            //obtiene los datos escritos por el usuario
            string correo = txtCorreo.Text?.Trim() ?? "";
            string clave = txtContrasena.Text ?? "";

            //comprueba que no esten vacíos
            if (string.IsNullOrWhiteSpace(correo) ||
                string.IsNullOrWhiteSpace(clave))
            {
                await DisplayAlert("Aviso", "Ingrese su correo y contraseña.", "Aceptar");
                return;
            }

            try
            {
                await ApiService.LoginAsync(correo, clave);

                await Navigation.PushAsync(new PerfilUsuario());
            }
            catch (ApiException ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
            catch (HttpRequestException)
            {
                await DisplayAlert("Error", "No se pudo conectar con el servidor.", "Aceptar");
            }
            catch (TaskCanceledException)
            {
                await DisplayAlert("Error", "El servidor tardó demasiado en responder.", "Aceptar");
            }
        }
    }
}