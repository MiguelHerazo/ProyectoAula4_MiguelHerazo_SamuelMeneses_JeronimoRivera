using System;
using System.Collections.Generic;

namespace ProyectoDeAula4_MiguelHerazo_SamuelMeneses_JeronimoRivera.Models
{
    public class IdeaNegocioModel
    {
        public int Id { get; set; } // Este campo mapeará con el ID en la base de datos
        public string Nombre { get; set; }
        public string Impacto { get; set; }
        public double Inversion { get; set; }
        public double Ingresos3Anios { get; set; }
        public string Herramientas4RI { get; set; }

        // Esto reflejará la relación con la tabla de Departamento
        public List<Departamento> Departamentos { get; set; } = new List<Departamento>();

        // Esto reflejará la relación con la tabla de IntegrantesEquipo
        public List<IntegrantesEquipo> IntegranteEquipos { get; set; } = new List<IntegrantesEquipo>();
    }

    public class Departamento
    {
        public int Id { get; set; } // Este campo mapeará con el ID en tu tabla de departamento
        public String Nombre { get; set; }
    }

    public class IntegrantesEquipo
    {
        public int Id { get; set; } // Este campo mapeará con el ID en tu tabla de integrantesequipo
        public String Nombre { get; set; }
        public String Apellidos { get; set; }
        public String Rol { get; set; }
        public String Email { get; set; }
    }
}
