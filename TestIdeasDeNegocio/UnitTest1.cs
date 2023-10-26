using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProyectoDeAula4_MiguelHerazo_SamuelMeneses_JeronimoRivera.Controllers;
using ProyectoDeAula4_MiguelHerazo_SamuelMeneses_JeronimoRivera.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

[TestClass]
public class HomeControllerTests
{
    [TestMethod]
    public void TestIndexAction()
    {
        // Arrange
        var controller = new HomeController();

        // Act
        var result = controller.Index() as ViewResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Index", result.ViewName);
    }

    [TestMethod]
    public void TestAgregarIdeaGetAction()
    {
        // Arrange
        var controller = new HomeController();

        // Act
        var result = controller.AgregarIdea() as ViewResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result.Model, typeof(IdeaNegocioModel));
    }

    public void TestMostrarIdea()
    {
        // Arrange
        HomeController controller = new HomeController(); // Instancia del controlador

        // Act
        var result = controller.MostrarIdea() as ViewResult; // Llama al método y obtien el resultado

        // Assert
        Assert.IsNotNull(result); // Verifica que el resultado no sea nulo
        Assert.IsInstanceOfType(result.Model, typeof(List<IdeaNegocioModel>)); // Verifica que el modelo sea una lista de IdeaNegocioModel
    }

    public void TestEditarIntegrantes()
    {
        // Arrange
        HomeController controller = new HomeController(); 

        // Act
        var result = controller.EditarIntegrantes(1) as ViewResult; 

        // Assert
        Assert.IsNotNull(result); // Verifica que el resultado no sea nulo
        Assert.IsInstanceOfType(result.Model, typeof(IntegrantesEquipo)); 
    }

    public void TestActualizarIntegrantes()
    {
        // Arrange
        var controller = new HomeController(); // Instancia del controlador
        var integrante = new IntegrantesEquipo
        {
            Id = 1, // ID de un integrante existente en  base de datos
            Nombre = "NuevoNombre",
            Apellidos = "NuevosApellidos",
            Rol = "NuevoRol",
            Email = "nuevoemail@example.com"
        };

        // Act
        var result = controller.ActualizarIntegrantes(integrante) as ActionResult;

        // Assert
        Assert.IsNotNull(result); // Verifica que el resultado no sea nulo

    }

    public void TestEliminarIntegrante()
    {
        // Arrange
        var controller = new HomeController(); // Instancia del controlador
        var integranteId = 1; // ID de un integrante existente en tu base de datos

        // Act
        var result = controller.EliminarIntegrante(integranteId) as ActionResult;

        // Assert
        Assert.IsNotNull(result); // Verifica que el resultado no sea nulo

    }

    public void TestIdeaNegocioImpactaDepartamentos()
    {
        // Arrange
        var controller = new HomeController(); // Instancia del controlador

        // Act
        var result = controller.IdeaNegocioImpactaDepartamentos() as ViewResult;

        // Assert
        Assert.IsNotNull(result); // Verifica que el resultado no sea nulo
        Assert.IsInstanceOfType(result.Model, typeof(Tuple<IdeaNegocioModel, IdeaNegocioModel>)); // Verifica el tipo del modelo

    }

    public void TestNegociosMasRentables()
    {
        // Arrange
        var controller = new HomeController(); // Instancia del controlador

        // Act
        var result = controller.NegociosMasRentables() as ViewResult;

        // Assert
        Assert.IsNotNull(result); // Verifica que el resultado no sea nulo
        Assert.IsInstanceOfType(result.Model, typeof(List<IdeaNegocioModel>)); // Verifica el tipo del modelo

    }

}
