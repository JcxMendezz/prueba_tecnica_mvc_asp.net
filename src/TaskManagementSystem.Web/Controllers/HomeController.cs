using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Controllers;

/// <summary>
/// Controller for home and general pages.
/// </summary>
public class HomeController : Controller
{
    /// <summary>
    /// Redirects to the tasks list.
    /// </summary>
    /// <returns>Redirect to Tasks/Index.</returns>
    public IActionResult Index()
    {
        // Redirigir directamente a la lista de tareas
        return RedirectToAction("Index", "Tasks");
    }

    /// <summary>
    /// Shows the privacy policy page.
    /// </summary>
    /// <returns>The privacy view.</returns>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Shows the error page.
    /// </summary>
    /// <returns>The error view.</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    /// <summary>
    /// Shows the 404 error page.
    /// </summary>
    /// <returns>The 404 error view.</returns>
    public IActionResult Error404()
    {
        Response.StatusCode = 404;
        return View("Error404");
    }
}
