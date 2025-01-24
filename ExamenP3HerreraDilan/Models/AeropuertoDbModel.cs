using SQLite;

namespace ExamenP3HerreraDilan.Models
{
    public class AeropuertoDbModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Nombre { get; set; }
        public string Pais { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Email { get; set; }
        public string RegistradoPor { get; set; } = "DHerrera";
    }
}
