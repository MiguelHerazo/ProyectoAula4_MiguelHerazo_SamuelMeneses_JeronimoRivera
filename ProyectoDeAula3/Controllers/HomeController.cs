using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Web.Mvc;
using ProyectoDeAula4_MiguelHerazo_SamuelMeneses_JeronimoRivera.Models;

namespace ProyectoDeAula4_MiguelHerazo_SamuelMeneses_JeronimoRivera.Controllers
{
    public class HomeController : Controller
    {
        private string connectionString = "Data Source=|DataDirectory|/dproyect4.db;Version=3;";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AgregarIdea()
        {
            return View(new IdeaNegocioModel());
        }

        public ActionResult EditarIntegrantes()
        {
            return View();
        }

        public ActionResult ContarIdeasConInteligenciaArtificial()
        {
            return View();
        }


        public ActionResult MostrarIdea()
        {
            List<IdeaNegocioModel> ideas = new List<IdeaNegocioModel>();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (var cmd = new SQLiteCommand("SELECT * FROM ideanegocio", connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var ideaNegocio = new IdeaNegocioModel
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    Nombre = reader["nombreidea"].ToString(),
                                    Impacto = reader["impactosocial"].ToString(),
                                    Inversion = Convert.ToDouble(reader["inversion"]),
                                    Ingresos3Anios = Convert.ToDouble(reader["ingresos"]),
                                    Herramientas4RI = reader["herramientasri"].ToString(),
                                };

                                ideaNegocio.Departamentos = AgregarDepartamentos(ideaNegocio.Id, connection);
                                ideaNegocio.IntegranteEquipos = AgregarIntegrantes(ideaNegocio.Id, connection);

                                ideas.Add(ideaNegocio);
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {

                string mensajeError = "Error al recuperar datos de la base de datos: " + ex.Message;
                ViewBag.ErrorMessage = mensajeError;
            }
            catch (Exception ex)
            {
                string mensajeError = "Ocurrió un error inesperado: " + ex.Message;
                ViewBag.ErrorMessage = mensajeError;
            }

            return View(ideas);
        }


        private List<IntegrantesEquipo> AgregarIntegrantes(int ideaNegocioId, SQLiteConnection connection)
        {
            List<IntegrantesEquipo> equipos = new List<IntegrantesEquipo>();

            using (var cmd = new SQLiteCommand("SELECT e.* FROM integrantesequipo AS e WHERE e.ideanegocio_id = @ideanegocio", connection))
            {
                cmd.Parameters.AddWithValue("@ideanegocio", ideaNegocioId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var equipo = new IntegrantesEquipo
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Nombre = reader["nombre"].ToString(),
                            Apellidos = reader["apellido"].ToString(),
                            Rol = reader["rol"].ToString(),
                            Email = reader["email"].ToString(),
                        };
                        equipos.Add(equipo);
                    }
                }
            }

            return equipos;
        }

        private List<Departamento> AgregarDepartamentos(int ideaNegocioId, SQLiteConnection connection)
        {
            List<Departamento> departamentos = new List<Departamento>();

            using (var cmd = new SQLiteCommand("SELECT d.* FROM departamento AS d INNER JOIN ideanegocio_departamento AS id ON d.Id = id.departamento_id WHERE id.ideanegocio_id = @ideanegocio", connection))
            {
                cmd.Parameters.AddWithValue("@ideanegocio", ideaNegocioId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var departamento = new Departamento
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Nombre = reader["nombre"].ToString(),
                        };
                        departamentos.Add(departamento);
                    }
                }
            }

            return departamentos;
        }

        [HttpPost]



        public ActionResult EditarIntegrantes(int equipoId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Consulta para cargar los detalles del equipo basado en su ID
                using (var cmd = new SQLiteCommand("SELECT * FROM integrantesequipo WHERE id = @equipoId", connection))
                {
                    cmd.Parameters.AddWithValue("@equipoId", equipoId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var equipo = new IntegrantesEquipo
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Nombre = reader["nombre"].ToString(),
                                Apellidos = reader["apellido"].ToString(),
                                Rol = reader["rol"].ToString(),
                                Email = reader["email"].ToString(),
                                // Puedes cargar más propiedades del equipo según tu modelo
                            };

                            // Pasa el equipo a la vista de edición
                            return View(equipo);
                        }
                    }
                }
            }

            // Si no se encuentra el equipo con el ID dado, puedes manejarlo apropiadamente.
            return HttpNotFound("El equipo no se encontró o no existe.");
        }

        [HttpPost]
        public ActionResult ActualizarIntegrantes(IntegrantesEquipo equipo)
        {
            if (ModelState.IsValid) // Asegúrate de que el modelo sea válido
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (var cmd = new SQLiteCommand("UPDATE integrantesequipo SET nombre = @nombre, apellido = @apellido, rol = @rol, email = @email WHERE id = @equipoId", connection))
                    {
                        cmd.Parameters.AddWithValue("@nombre", equipo.Nombre);
                        cmd.Parameters.AddWithValue("@apellido", equipo.Apellidos);
                        cmd.Parameters.AddWithValue("@rol", equipo.Rol);
                        cmd.Parameters.AddWithValue("@email", equipo.Email);
                        cmd.Parameters.AddWithValue("@equipoId", equipo.Id);

                        cmd.ExecuteNonQuery();
                    }

                    TempData["Mensaje"] = "Equipo actualizado con éxito.";
                    return RedirectToAction("MostrarIdeaNegocio"); // Redirige a la vista que muestra los detalles de la idea de negocio.
                }
            }

            // Si el modelo no es válido, vuelve a la vista de edición con los errores de validación.
            return View(equipo);
        }

        // Acción para eliminar un equipo
        [HttpPost]
        public ActionResult EliminarIntegrante(int equipoId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand("DELETE FROM integrantesequipo WHERE id = @equipoId", connection))
                {
                    cmd.Parameters.AddWithValue("@equipoId", equipoId);
                    cmd.ExecuteNonQuery();
                }
            }

            TempData["Mensaje"] = "Equipo eliminado con éxito.";
            return RedirectToAction("MostrarIdeaNegocio");
        }

        public ActionResult IdeaNegocioImpactaDepartamentos()
        {
            // Obtener la idea de negocio que impacta a más departamentos
            IdeaNegocioModel ideaMasImpacto = ObtenerIdeaMasImpacto();

            // Obtener la idea de negocio con el mayor total de ingresos en los primeros 3 años
            IdeaNegocioModel ideaMayorIngresos = ObtenerIdeaMayorIngresos();

            // Puedes pasar ambos modelos a la vista
            return View(new Tuple<IdeaNegocioModel, IdeaNegocioModel>(ideaMasImpacto, ideaMayorIngresos));
        }

        private IdeaNegocioModel ObtenerIdeaMasImpacto()
        {
            IdeaNegocioModel ideaMasImpacto = null;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand("SELECT * FROM ideanegocio ORDER BY (SELECT COUNT(*) FROM ideanegocio_departamento WHERE ideanegocio_id = ideanegocio.id) DESC LIMIT 1", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ideaMasImpacto = new IdeaNegocioModel
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Nombre = reader["nombreidea"].ToString(),
                                Impacto = reader["impactosocial"].ToString(),
                                Inversion = Convert.ToDouble(reader["inversion"]),
                                Ingresos3Anios = Convert.ToDouble(reader["ingresos"]),
                                Herramientas4RI = reader["herramientasri"].ToString(),
                            };

                            ideaMasImpacto.Departamentos = AgregarDepartamentos(ideaMasImpacto.Id, connection);
                            ideaMasImpacto.IntegranteEquipos = AgregarIntegrantes(ideaMasImpacto.Id, connection);
                        }
                    }
                }
            }

            return ideaMasImpacto;
        }

        private IdeaNegocioModel ObtenerIdeaMayorIngresos()
        {
            IdeaNegocioModel ideaMayorIngresos = null;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand("SELECT * FROM ideanegocio ORDER BY ingresos DESC LIMIT 1", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ideaMayorIngresos = new IdeaNegocioModel
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Nombre = reader["nombreidea"].ToString(),
                                Impacto = reader["impactosocial"].ToString(),
                                Inversion = Convert.ToDouble(reader["inversion"]),
                                Ingresos3Anios = Convert.ToDouble(reader["ingresos"]),
                                Herramientas4RI = reader["herramientasri"].ToString(),
                            };

                            ideaMayorIngresos.Departamentos = AgregarDepartamentos(ideaMayorIngresos.Id, connection);
                            ideaMayorIngresos.IntegranteEquipos = AgregarIntegrantes(ideaMayorIngresos.Id, connection);
                        }
                    }
                }
            }

            return ideaMayorIngresos;
        }

        public ActionResult NegociosMasRentables()
        {
            List<IdeaNegocioModel> ideasMasRentables = ObtenerNegociosMasRentables();
            return View(ideasMasRentables);
        }

        private List<IdeaNegocioModel> ObtenerNegociosMasRentables()
        {
            List<IdeaNegocioModel> ideasMasRentables = new List<IdeaNegocioModel>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand("SELECT * FROM ideanegocio ORDER BY ingresos DESC LIMIT 3", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var ideaNegocio = new IdeaNegocioModel
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Nombre = reader["nombreidea"].ToString(),
                                Ingresos3Anios = Convert.ToDouble(reader["ingresos"]),
                                // Agrega más propiedades según tu modelo
                            };

                            ideasMasRentables.Add(ideaNegocio);
                        }
                    }
                }
            }

            return ideasMasRentables;
        }

        public ActionResult IdeaNegocioImpactaDepartamentosMasTres()
        {
            List<IdeaNegocioModel> ideasConMasDepartamentos = ObtenerIdeasConMasDepartamentos();
            return View(ideasConMasDepartamentos);
        }

        private List<IdeaNegocioModel> ObtenerIdeasConMasDepartamentos()
        {
            List<IdeaNegocioModel> ideasConMasDepartamentos = new List<IdeaNegocioModel>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand("SELECT ideanegocio.*, COUNT(departamento_id) AS numDepartamentos FROM ideanegocio_departamento GROUP BY ideanegocio_id HAVING numDepartamentos > 3", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var ideaNegocio = new IdeaNegocioModel
                            {
                                Id = Convert.ToInt32(reader["ideanegocio_id"]),
                                Nombre = reader["nombreidea"].ToString(),
                            };

                            ideasConMasDepartamentos.Add(ideaNegocio);
                        }
                    }
                }
            }

            return ideasConMasDepartamentos;
        }

        public ActionResult SumaTotalIngresos()
        {
            double sumaIngresos = CalcularSumaTotalIngresos();
            ViewBag.SumaTotalIngresos = sumaIngresos;
            return View();
        }

        private double CalcularSumaTotalIngresos()
        {
            double sumaIngresos = 0;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand("SELECT SUM(ingresos) AS suma FROM ideanegocio", connection))
                {
                    var result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        sumaIngresos = Convert.ToDouble(result);
                    }
                }
            }

            return sumaIngresos;
        }

        public ActionResult SumaTotalInversion()
        {
            double sumaInversion = CalcularSumaTotalInversion();
            ViewBag.SumaTotalInversion = sumaInversion;
            return View();
        }

        private double CalcularSumaTotalInversion()
        {
            double sumaInversion = 0;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand("SELECT SUM(inversion) AS suma FROM ideanegocio", connection))
                {
                    var result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        sumaInversion = Convert.ToDouble(result);
                    }
                }
            }

            return sumaInversion;
        }

        public ActionResult IdeaNegocioMayorHerramientas4RI()
        {
            IdeaNegocioModel ideaNegocio = ObtenerIdeaNegocioMayorHerramientas4RI();
            return View(ideaNegocio);
        }

        private IdeaNegocioModel ObtenerIdeaNegocioMayorHerramientas4RI()
        {
            IdeaNegocioModel ideaNegocio = null;

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (var cmd = new SQLiteCommand(
                        "SELECT in.*, COUNT(eq.id) AS CantidadHerramientas " +
                        "FROM ideanegocio AS in " +
                        "JOIN integrantesequipo AS eq ON in.id = eq.ideanegocio_id " +
                        "GROUP BY in.id " +
                        "ORDER BY CantidadHerramientas DESC " +
                        "LIMIT 1", connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ideaNegocio = new IdeaNegocioModel
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    Nombre = reader["nombreidea"].ToString(),
                                    Impacto = reader["impactosocial"].ToString(),
                                    Inversion = Convert.ToDouble(reader["inversion"]),
                                    Ingresos3Anios = Convert.ToDouble(reader["ingresos"]),
                                    Herramientas4RI = reader["herramientasri"].ToString(),
                                };

                                ideaNegocio.Departamentos = AgregarDepartamentos(ideaNegocio.Id, connection);
                                ideaNegocio.IntegranteEquipos = AgregarIntegrantes(ideaNegocio.Id, connection);
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                // Manejar excepción específica de SQLite, por ejemplo, registrar un mensaje de error.
                Console.WriteLine("Error de SQLite: " + ex.Message);

            }
            catch (Exception ex)
            {
                // Manejar excepciones generales, por ejemplo, registrar un mensaje de error.
                Console.WriteLine("Error inesperado: " + ex.Message);
                // Puedes lanzar la excepción nuevamente o manejarla de acuerdo a tus necesidades.
                throw;
            }

            return ideaNegocio;
        }

        [HttpPost]
        public ActionResult ContarIdeasNegocioConInteligenciaArtificial()
        {
            int cantidadIdeasConIA = ContarIdeasNegocio("inteligencia artificial");
            ViewBag.CantidadIdeasConIA = cantidadIdeasConIA;
            return View();
        }

        private int ContarIdeasNegocio(string keyword)
        {
            int cantidadIdeas = 0;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand(
                    "SELECT COUNT(*) " +
                    "FROM ideanegocio " +
                    "WHERE LOWER(herramientasri) LIKE @keyword", connection))
                {
                    cmd.Parameters.AddWithValue("@keyword", "%" + keyword.ToLower() + "%");
                    cantidadIdeas = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            return cantidadIdeas;
        }

        public ActionResult ContarIdeasDesarrolloSostenible()
        {
            int cantidadIdeas = 0;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new SQLiteCommand("SELECT COUNT(*) FROM ideanegocio", connection))
                {
                    // Obtenemos todos los registros
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string impactoSocial = reader["impactosocial"].ToString().ToLower();
                            if (impactoSocial.Contains("desarrollo sostenible"))
                            {
                                cantidadIdeas++;
                            }
                        }
                    }
                }
            }

            return View(cantidadIdeas);
        }

        public ActionResult IdeasMayorQuePromedio()
        {
            List<IdeaNegocioModel> ideas = new List<IdeaNegocioModel>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Calcular el promedio de ingresos de todas las ideas
                double promedioIngresos = 0;
                using (var cmd = new SQLiteCommand("SELECT AVG(ingresos) FROM ideanegocio", connection))
                {
                    var promedio = cmd.ExecuteScalar();
                    if (promedio != DBNull.Value)
                    {
                        promedioIngresos = Convert.ToDouble(promedio);
                    }
                }

                // Filtrar las ideas de negocio con ingresos mayores al promedio
                using (var cmd = new SQLiteCommand("SELECT * FROM ideanegocio WHERE ingresos > @promedio", connection))
                {
                    cmd.Parameters.AddWithValue("@promedio", promedioIngresos);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var ideaNegocio = new IdeaNegocioModel
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Nombre = reader["nombreidea"].ToString(),
                            };
                            ideas.Add(ideaNegocio);
                        }
                    }
                }
            }

            return View(ideas);
        }



    }
}
