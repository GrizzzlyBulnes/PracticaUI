namespace PracticaUI;

public partial class ConversorMoneda : ContentPage
{
    public ConversorMoneda()
    {
        InitializeComponent();
    }

    private void onConvertirClicked(object sender, EventArgs e)
    {
        // Validamos el monto numerico que se haya elegido en ambos Pickers
        // SelectedIndex vale -1 cuando el usuario todavia no ha seleccionado nada 

        if (!double.TryParse(txtMonto.Text, out double monto) || pckOrigen.SelectedIndex == -1
            || pckDestino.SelectedIndex == -1) 
        {
            lblResultado.Text = "Seleccione las monedas e ingrese un monto.";
            return;
        }

        //Primer paso: pasamos todo a dolares(USD) y luego a la moneda destino
        //Asi no necesitamos una tasa para cada moneda
        double montoEnUSD = pckOrigen.SelectedIndex.ToString() switch
        {
            "0" => monto / 24.75, //24.75 lempiras es igual a un dolar
            "2" => monto * 1.08, //1 euro es igual a 1.08 dolares
            "3" => monto / 18.22,
            _ => monto         //"_" es igual a cualquier otro caso
        };

        //Segundo paso: de dolares a la moneda destino
        double resultado = pckDestino.SelectedIndex.ToString() switch
        {
            "0" => montoEnUSD * 24.75, //24.75 lempiras es igual a un dolar
            "2" => montoEnUSD / 1.08, //1 euro es igual a 1.08 dolares
            "3" => montoEnUSD * 18.22, //1 peso equivale 
            _            => montoEnUSD         //"_" es igual a cualquier otro caso
        };

        //{resultado:F2} es igual a mostrar con 2 decimales
        lblResultado.Text = $"Resultado: {resultado:F2}"; string simbolo = pckDestino.SelectedIndex.ToString() switch
        {
            "0" => "L",  // Lempira
            "1" => "$",  // Dolar
            "2" => "€",  // Euro
            "3" => "$",  // Peso mexicano
            _ => ""
        };

        // Mostramos el resultado con su simbolo y dos decimales
        lblResultado.Text = $"Resultado: {simbolo} {resultado:N2}";

    }

}