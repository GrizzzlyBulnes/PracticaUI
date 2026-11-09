namespace PracticaUI;

public partial class PerfilUsuario : ContentPage
{
    public PerfilUsuario()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            // solicita los datos a la API
            var perfil = await ApiService.ObtenerPerfilAsync();

            // muestra los datos en pantalla
            lblNombre.Text = perfil.Nombre;

            lblDisponible.Text = perfil.Disponible
                ? "Disponible"
                : "No disponible";

            lblExperiencia.Text = perfil.AniosExperiencia.ToString();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }
    }

    private async void CerrarSesion_Clicked(object sender, EventArgs e)
    {
        ApiService.CerrarSesion();

        await Navigation.PopToRootAsync();
    }
}