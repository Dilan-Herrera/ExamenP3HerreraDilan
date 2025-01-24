using System.Collections.ObjectModel;
using System.ComponentModel;
using Newtonsoft.Json;
using SQLite;
using ExamenP3HerreraDilan.Models;

namespace ExamenP3HerreraDilan.ViewModels
{
    public class BusquedaViewModelDHerrera : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ObservableCollection<Aeropuerto> Aeropuertos { get; } = new ObservableCollection<Aeropuerto>();
        private HttpClient client = new HttpClient();

        private SQLiteAsyncConnection database;
        private static readonly string DbPath = Path.Combine(FileSystem.AppDataDirectory, "DHerrera.db");

        private string searchText;
        public string SearchText
        {
            get => searchText;
            set
            {
                if (searchText != value)
                {
                    searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                }
            }
        }

        public BusquedaViewModelDHerrera()
        {
            database = new SQLiteAsyncConnection(DbPath);
            InitializeDatabase();
        }

        private async void InitializeDatabase()
        {
            await database.CreateTableAsync<AeropuertoDbModel>();
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
                await App.Current.MainPage.DisplayAlert("Error", "Por favor, ingresa el nombre del aeropuerto", "OK");
                return;
            }

            var response = await client.GetAsync($"https://freetestapi.com/api/v1/airports?search={Uri.EscapeDataString(SearchText)}");

            var json = await response.Content.ReadAsStringAsync();
            var aeropuertos = JsonConvert.DeserializeObject<List<Aeropuerto>>(json);

            if (aeropuertos != null && aeropuertos.Count > 0)
            {
                Aeropuertos.Clear();
                var aeropuerto = aeropuertos[0];
                Aeropuertos.Add(aeropuerto);

                var aeropuertoDb = new AeropuertoDbModel
                {
                    Nombre = aeropuerto.Name,
                    Pais = aeropuerto.Country,
                    Latitud = aeropuerto.Location?.Latitude ?? 0,
                    Longitud = aeropuerto.Location?.Longitude ?? 0,
                    Email = aeropuerto.ContactInfo?.Email ?? "No disponible",
                    RegistradoPor = "DHerrera"
                };

                var existing = await database.Table<AeropuertoDbModel>()
                                             .Where(a => a.Nombre == aeropuertoDb.Nombre)
                                             .FirstOrDefaultAsync();

                if (existing == null)
                {
                    await database.InsertAsync(aeropuertoDb);
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Sin resultados", "No se encontraron aeropuertos", "OK");
            }
        }
    }
}
