using Android.Views.InputMethods;

namespace PracticaUI;

public partial class CalculadoraIMC : ContentPage
{
	public CalculadoraIMC()
	{
		InitializeComponent();
	}

	//"async" porque DisplayAlert espera que el usuario toque OK
    private async void Button_Clicked(object sender, EventArgs e)
    {
		//TryParse intenta convertir el texto a numero
		//Tambien validamos que la altura sea mayor que 0 (evita dividir)

		if(!double.TryParse(txtPeso.Text, out double peso) || !double.TryParse(txtAltura.Text, out double altura)
			|| altura <= 0 || peso <= 0){
			await DisplayAlert("Error", "Por favor ingrese valores numericos validos.", "Ok");
			return;
		}

		//Formula del IMC: peso / (altura al cuadrado)
		double imc = peso / (altura * altura);

		//Formato con 1 decimal (ejemplo: 22.9)
		lblValorIMC.Text = imc.ToString("F1");

		//Clasificamos el IMC y cambiamos texto y colores segun el rango 
		if(imc < 18.5){
			lblDiagnostico.Text = "Bajo peso";
			lblDiagnostico.TextColor = Color.FromArgb("#33230D");

			//Color.FromArgb convierte un codigo Hexadecimal en un color 
			brResultado.BackgroundColor = Color.FromArgb("#E9A23B");
			brResultado.Stroke = Color.FromArgb("#D18B25");

		}else if(imc <= 24.9){
			lblDiagnostico.Text = "Peso normal";
			lblDiagnostico.TextColor = Colors.White;

            brResultado.BackgroundColor = Color.FromArgb("#198754");
            brResultado.Stroke = Color.FromArgb("#146C43");

        }else if(imc <= 29.9){
            lblDiagnostico.Text = "Sobrepeso";
            lblDiagnostico.TextColor = Color.FromArgb("#332B10");

            brResultado.BackgroundColor = Color.FromArgb("#F4C542");
			brResultado.Stroke = Color.FromArgb("#D6A92D");
		}
		else
		{
			lblDiagnostico.Text = "Obseidad";
			lblDiagnostico.TextColor = Colors.White;

			brResultado.BackgroundColor = Color.FromArgb("#D93636");
			brResultado.Stroke = Color.FromArgb("#B52B2B");
		}

		//Por ultimo, muestra la tarjeta que estaba oculta 
		brResultado.IsVisible = true;
		lblRangoIMC.IsVisible = true;
    }
}