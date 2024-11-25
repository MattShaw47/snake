// <copyright file="Powerup.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

namespace CS3500.Client.Model;

using System.Text.Json;
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
public class Powerup(int id, Point2D p, bool isDead)
{
    /// <summary>
    /// Gets or sets the unique identifier for this Powerup object.
    /// </summary>
    public int Id { get; set; } = id;

    /// <summary>
    /// Gets or sets the point in space this powerup is at.
    /// </summary>
    public Point2D P { get; set; } = p;

    /// <summary>
    /// Gets or sets a value indicating whether the object is dead or not. (eaten).
    /// </summary>
    public bool IsDead { get; set; } = isDead;

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