using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace bibliotecaApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaPrincipal : ContentPage
    {
        string apiUrl = "https://biblioteca-api-vx1x.onrender.com/api/v1/login/";
        public PaginaPrincipal()
        {
            InitializeComponent();
        }

        private async void cmdCrudLibros_Clicked(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                try
                {
                    var usuario = new Usuario
                    {
                        Username = txtUsername.Text,
                        Contrasena = txtPassword.Text
                    };

                    string loginUrl = $"{apiUrl}?username={Uri.EscapeDataString(usuario.Username)}&password={Uri.EscapeDataString(usuario.Contrasena)}";

                    using (var webClient = new HttpClient())
                    {
                        var response = await webClient.PostAsync(loginUrl, null);

                        if (response.IsSuccessStatusCode)
                        {
                            var jsonResponse = await response.Content.ReadAsStringAsync();
                            var usuarioBD = JsonConvert.DeserializeObject<Usuario>(jsonResponse);

                            if (usuarioBD.Es_admin)
                            {
                                var crudLibros = new CrudLibros();
                                await Navigation.PushAsync(crudLibros);
                            }
                            else
                            {
                                await DisplayAlert("Inicio de sesión", "Credenciales Incorrectas", "Aceptar");
                            }                       
                        }
                        else
                        {
                            var errorResponse = await response.Content.ReadAsStringAsync();
                            await DisplayAlert("Error", $"Error al iniciar sesión. Detalles: {errorResponse}", "Aceptar");
                        }
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Error: {ex.Message}", "Aceptar");
                }
            }
            else
            {
                await DisplayAlert("Campos vacíos", "Por favor, ingrese nombre de usuario y contraseña.", "Aceptar");
            }
        }

    }
}