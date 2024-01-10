using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace bibliotecaApp
{
    public partial class LibrosPage : ContentPage
    {
        string apiUrl = "https://biblioteca-api-vx1x.onrender.com/api/v1/libros/";

        public ObservableCollection<Libro> Libros { get; set; }

        public LibrosPage()
        {
            InitializeComponent();
            Libros = new ObservableCollection<Libro>();
            CargarLibros();
        }

        private async void CargarLibros()
        {
            try
            {
                using (var webClient = new HttpClient())
                {
                    var response = await webClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

                        if (responseObject != null)
                        {
                            if (!string.IsNullOrEmpty(responseObject.Mensaje))
                            {
                                await DisplayAlert("Mensaje", responseObject.Mensaje, "Aceptar");
                            }

                            if (responseObject.Libros != null && responseObject.Libros.Any())
                            {
                                var tableSection = new TableSection("Lista de Libros");

                                foreach (var libro in responseObject.Libros)
                                {
                                    var viewCell = new ViewCell
                                    {
                                        View = new StackLayout
                                        {
                                            Orientation = StackOrientation.Horizontal,
                                            Padding = new Thickness(10, 0, 10, 0),
                                            Children =
                                    {
                                        new Label { Text = libro.Titulo },
                                        new Label { Text = libro.Autor },
                                        new Label { Text = libro.Genero }
                                    }
                                        }
                                    };

                                    tableSection.Add(viewCell);
                                }

                                librosTableView.Root.Add(tableSection);
                            }
                            else if (responseObject.Libro != null)
                            {
                                var tableSection = new TableSection();
                                var viewCell = new ViewCell
                                {
                                    View = new StackLayout
                                    {
                                        Orientation = StackOrientation.Horizontal,
                                        Padding = new Thickness(10, 0, 10, 0),
                                        Children =
                                {
                                    new Label { Text = responseObject.Libro.Titulo },
                                    new Label { Text = responseObject.Libro.Autor },
                                    new Label { Text = responseObject.Libro.Genero }
                                }
                                    }
                                };

                                tableSection.Add(viewCell);
                                librosTableView.Root.Add(tableSection);
                            }
                            else
                            {
                                await DisplayAlert("Error", "No se encontraron libros en la respuesta JSON.", "Aceptar");
                            }
                        }
                        else
                        {
                            await DisplayAlert("Error", "Respuesta JSON no válida.", "Aceptar");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", $"Error al cargar la lista de libros. Código de estado: {response.StatusCode}", "Aceptar");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error: {ex.Message}", "Aceptar");
            }
        }



        private async void cmdRegresar_Clicked(object sender, EventArgs e)
        {
            try
            {
                var crudLibros = new CrudLibros();
                await Navigation.PushAsync(crudLibros);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error: {ex.Message}", "Aceptar");
            }
        }
    }
}