using System.Collections.ObjectModel;
using System.ComponentModel;
using SQLite;
using ExamenP3HerreraDilan.Models;

namespace ExamenP3HerreraDilan.ViewModels
{
    public class HistorialViewModelDHerrera : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ObservableCollection<string> HistorialAeropuertos { get; } = new ObservableCollection<string>();
        private SQLiteAsyncConnection database;
        private static readonly string DbPath = Path.Combine(FileSystem.AppDataDirectory, "DHerrera.db");

        public HistorialViewModelDHerrera()
        {
            database = new SQLiteAsyncConnection(DbPath);
            CargarHistorial();
        }

        private async void CargarHistorial()
        {
            var aeropuertos = await database.Table<AeropuertoDbModel>().ToListAsync();
            HistorialAeropuertos.Clear();

            foreach (var aeropuerto in aeropuertos)
            {
                HistorialAeropuertos.Add($"Nombre: {aeropuerto.Nombre} - Pais: {aeropuerto.Pais} - Latitud: {aeropuerto.Latitud} - Longitud: {aeropuerto.Longitud} - Correo: {aeropuerto.Email} - DHerrera: {aeropuerto.RegistradoPor})");
            }
        }
    }
}
