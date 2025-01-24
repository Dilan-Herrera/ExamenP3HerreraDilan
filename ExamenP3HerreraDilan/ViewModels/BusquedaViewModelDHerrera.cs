using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using ExamenP3HerreraDilan.Models;
using Newtonsoft.Json;

namespace ExamenP3HerreraDilan.ViewModels;

public class BusquedaViewModelDHerrera : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public ObservableCollection<Aeropuerto> Aeropuertos { get; } = new ObservableCollection<Aeropuerto>();
    private HttpClient client = new HttpClient();

    private string searchText;
    public string SearchText
    {
        get => searchText;
        set
        {
            if (searchText != value)
            {
                searchText = value;
                Console.WriteLine($"SearchText actualizado: {searchText}");
                OnPropertyChanged(nameof(SearchText));
            }
        }
    }


    public Command<string> BuscarAeropuertoCommand => new Command<string>(async (name) => await BuscarAeropuerto(name));
    public Command LimpiarCommand => new Command(() => Aeropuertos.Clear());

    public async Task BuscarAeropuerto(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            await App.Current.MainPage.DisplayAlert("Error", "Por favor, ingresa el nombre del aeropuerto.", "OK");
            return;
        }

        try
        {
            Console.WriteLine($"Buscando aeropuerto: {name}"); 

            var response = await client.GetAsync($"https://freetestapi.com/api/v1/airports?search={Uri.EscapeDataString(name)}");

            Console.WriteLine($"Respuesta de API: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Respuesta JSON: {json}");

                if (!string.IsNullOrEmpty(json))
                {
                    var aeropuertos = JsonConvert.DeserializeObject<List<Aeropuerto>>(json);
                    if (aeropuertos != null && aeropuertos.Count > 0)
                    {
                        Aeropuertos.Clear();
                        foreach (var aeropuerto in aeropuertos)
                        {
                            Aeropuertos.Add(aeropuerto);
                        }
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Sin resultados", "No se encontraron aeropuertos.", "OK");
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Respuesta vacía de la API.", "OK");
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", $"No se pudo conectar con la API. Código: {response.StatusCode}", "OK");
            }
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Error", $"Algo salió mal: {ex.Message}", "OK");
        }
    }


}
