using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamenP3HerreraDilan.Models
{
    public class Aeropuerto
    {
        public string Pais { get; set; }
        public Ubicacion Ubicacion { get; set; }
        public Contacto Contacto { get; set; }
    }

    public class Ubicacion
    {
        public double Latitud { get; set; }
        public double Longitud { get; set; }
    }

    public class Contacto
    {
        public string Correo { get; set; }
    }
}
