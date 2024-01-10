using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace bibliotecaApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrudLibros : ContentPage
    {
        string apiUrl = "https://biblioteca-api-vx1x.onrender.com/api/v1/libros/";
        public CrudLibros()
        {
            InitializeComponent();
        }

        private async void cmdInsert_Clicked(object sender, EventArgs e)
        {
            try
            {
                using (var webClient = new HttpClient())
                {
                    var libro = new Libro
                    {
                        Titulo = txtTitulo.Text,
                        Autor = txtAutor.Text,
                        Genero = txtGenero.Text
                    };

                    var queryParams = $"?titulo={Uri.EscapeDataString(libro.Titulo)}&autor=" +
                        $"{Uri.EscapeDataString(libro.Autor)}&genero={Uri.EscapeDataString(libro.Genero)}";

                    var response = await webClient.PostAsync(apiUrl + queryParams, null);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

                        responseObject.Libro.Genero = responseObject.Libro.Genero?.Replace("\n", "");

                        lblMensaje.Text = $"{responseObject.Mensaje}, " +
                                          $"ID: {responseObject.Libro.Id}, " +
                                          $"Título: {responseObject.Libro.Titulo}, " +
                                          $"Autor: {responseObject.Libro.Autor}, " +
                                          $"Género: {responseObject.Libro.Genero}";
                    }
                    else
                    {
                        lblMensaje.Text = $"Error al crear el libro. Código de estado: {response.StatusCode}";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = $"Error: {ex.Message}";
            }
        }

         private async void cmdUpdate_Clicked(object sender, EventArgs e)
        {
            try
            {
                using (var webClient = new HttpClient())
                {
                    int libroId = int.Parse(txtId.Text);

                    var libro = new Libro
                    {
                        Titulo = txtTitulo.Text,
                        Autor = txtAutor.Text,
                        Genero = txtGenero.Text
                    };

                    var requestBody = new
                    {
                        titulo = libro.Titulo,
                        autor = libro.Autor,
                        genero = libro.Genero
                    };

                    var jsonBody = JsonConvert.SerializeObject(requestBody);
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    var response = await webClient.PutAsync(apiUrl + libroId, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

                        responseObject.Libro.Genero = responseObject.Libro.Genero?.Replace("\n", "");

                        lblMensaje.Text = $"{responseObject.Mensaje}, " +
                                          $"ID: {responseObject.Libro.Id}, " +
                                          $"Título: {responseObject.Libro.Titulo}, " +
                                          $"Autor: {responseObject.Libro.Autor}, " +
                                          $"Género: {responseObject.Libro.Genero}";
                    }
                    else
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        lblMensaje.Text = $"Error al actualizar el libro. Código de estado: {response.StatusCode}, " +
                                          $"Detalles: {errorResponse}";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = $"Error: {ex.Message}";
            }
         }

        private async void cmdReadOne_Clicked(object sender, EventArgs e)
        {
            try
            {
                using (var webClient = new HttpClient())
                {
                    int libroId = int.Parse(txtId.Text);

                    var response = await webClient.GetAsync(apiUrl + libroId);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

                        responseObject.Libro.Genero = responseObject.Libro.Genero?.Replace("\n", "");

                        lblMensaje.Text = responseObject.Mensaje;       

                        txtTitulo.Text = responseObject.Libro.Titulo;
                        txtAutor.Text = responseObject.Libro.Autor;
                        txtGenero.Text = responseObject.Libro.Genero;
                    }
                    else
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        lblMensaje.Text = $"Error al obtener el libro. Código de estado: {response.StatusCode}, " +
                                          $"Detalles: {errorResponse}";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = $"Error: {ex.Message}";
            }
        }

        private async void cmdDelete_Clicked(object sender, EventArgs e)
        {
            try
            {
                using (var webClient = new HttpClient())
                {
                    int libroId = int.Parse(txtId.Text);

                    var response = await webClient.DeleteAsync(apiUrl + "eliminar/" + libroId);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

                        lblMensaje.Text = $"{responseObject.Mensaje}, " +
                                          $"ID: {responseObject.Libro.Id}, " +
                                          $"Título: {responseObject.Libro.Titulo}, " +
                                          $"Autor: {responseObject.Libro.Autor}, " +
                                          $"Género: {responseObject.Libro.Genero}";

                        txtTitulo.Text = "";
                        txtAutor.Text = "";
                        txtGenero.Text = "";
                        txtGenero.Text = "";
                    }
                    else
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        lblMensaje.Text = $"Error al eliminar el libro. Código de estado: {response.StatusCode}, " +
                                          $"Detalles: {errorResponse}";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = $"Error: {ex.Message}";
            }
        }

        private async void cmdCerrarSesion_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void cmdIrALibros_Clicked(object sender, EventArgs e)
        {
            try
            {
                var librosPage = new LibrosPage();

                await Navigation.PushAsync(librosPage);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error: {ex.Message}", "Aceptar");
            }
        }

    }
}

