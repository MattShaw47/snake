// <copyright file="Powerup.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

namespace CS3500.Client.Model;

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using CS3500.Networking;

/// <summary>
/// A point in 2d space. Contains an x and y coordinate.
/// </summary>
/// <remarks>
/// Constructor for a Point2D object.
/// </remarks>
/// <param name="x">Creates <see cref="X"/>.</param>
/// <param name="y">Creates <see cref="Y"/>.</param>
public class Powerup
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Powerup"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of this powerup.</param>
    /// <param name="p">The Point2D representing the point at which this powerup is.</param>
    /// <param name="isDead">A bool representing whether this powerup died this frame or not.</param>
    [JsonConstructor]
    public Powerup(int id, Point2D p, bool isDead)
    {
        this.Id = id;
        this.P = p;
        this.IsDead = isDead;
    }

    /// <summary>
    /// Gets or sets the unique identifier for this Powerup object.
    /// </summary>
    [JsonPropertyName("power")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the point in space this powerup is at.
    /// </summary>
    [JsonPropertyName("loc")]
    public Point2D P { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the object is dead or not. (eaten).
    /// </summary>
    [JsonPropertyName("died")]
    public bool IsDead { get; set; }

    public static bool operator ==(Powerup left, Powerup right)
    {
        return left.Id == right.Id;
    }

    public static bool operator !=(Powerup left, Powerup right)
    {
        return left.Id != right.Id;
    }

    /// <summary>
    /// Checks equality of 2 powerups based upon their unique id's.
    /// </summary>
    /// <param name="obj">The object which will be compared against this powerup.</param>
    /// <returns>A bool representing whether obj and this are equal.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is Powerup wall && this == wall)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// An override for GetHashCode which returns the unique id for this Powerup.
    /// </summary>
    /// <returns>A hashcode created from the wall's id.</returns>
    public override int GetHashCode()
    {
        return this.Id;
    }
}