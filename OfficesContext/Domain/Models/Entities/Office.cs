using System;
using workstation_backend.Shared.Domain.Model;

namespace workstation_backend.OfficesContext.Domain.Models.Entities;

/// <summary>
/// Representa una oficina que puede ser alquilada.
/// </summary>
public class Office : BaseEntity
{
    /// <summary>
    /// Constructor para crear una nueva oficina.
    /// </summary>
    /// <param name="location">Ubicación de la oficina.</param>
    /// <param name="capacity">Capacidad máxima de personas.</param>
    /// <param name="costPerDay">Costo diario de alquiler.</param>
    /// <param name="available">Indica si la oficina está disponible actualmente.</param>
    public Office(string location, int capacity, int costPerDay, bool available)
    {
        Location = location;
        Capacity = capacity;
        CostPerDay = costPerDay;
        IsActive = true;
        Available = available;
        Services = new List<OfficeService>();
    }

    /// <summary>
    /// Constructor vacío requerido por EF.
    /// </summary>
    public Office() {}

    /// <summary>
    /// Ubicación física de la oficina (ej. distrito o dirección).
    /// </summary>
    public string Location { get; set; }

    /// <summary>
    /// Cantidad máxima de personas permitidas.
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// Costo por día para alquilar la oficina.
    /// </summary>
    public int CostPerDay { get; set; }

    /// <summary>
    /// Indica si la oficina está disponible para alquiler.
    /// </summary>
    public bool Available { get; set; }

    /// <summary>
    /// Lista de servicios adicionales disponibles en la oficina.
    /// </summary>
    public List<OfficeService> Services { get; } = new();

    /// <summary>
    /// Calificaciones dadas por los usuarios que alquilaron la oficina.
    /// </summary>
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}

