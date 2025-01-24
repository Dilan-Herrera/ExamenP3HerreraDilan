using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public Command BuscarAeropuertoCommand => new Command(async () => await BuscarAeropuerto());
    public Command LimpiarCommand => new Command(() =>
    {
        Aeropuertos.Clear();
        SearchText = string.Empty;
    });


    public async Task BuscarAeropuerto()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            await App.Current.MainPage.DisplayAlert("Error", "Por favor, ingrese el nombre del aeropuerto", "OK");
            return;
        }

        try
        {
            var response = await client.GetAsync($"https://freetestapi.com/api/v1/airports?search={Uri.EscapeDataString(SearchText)}");

            var json = await response.Content.ReadAsStringAsync();

            var aeropuertos = JsonConvert.DeserializeObject<List<Aeropuerto>>(json);
            if (aeropuertos != null && aeropuertos.Count > 0)
            {
                Aeropuertos.Clear();
                Aeropuertos.Add(aeropuertos[0]);
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Sin resultados", "No se encontraron aeropuertos", "OK");
            }
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Error", $"Algo salio mal: {ex.Message}", "OK");
        }
    }
}
