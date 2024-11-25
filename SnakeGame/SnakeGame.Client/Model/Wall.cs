// <copyright file="Wall.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

namespace CS3500.Client.Model;

using System.Text.Json;
using System.Threading;
using CS3500.Networking;

/// <summary>
/// An impassable wall object.
/// </summary>
/// <remarks>
/// Constructor for a Point2D object.
/// </remarks>
/// <param name="x">Creates <see cref="X"/>.</param>
/// <param name="y">Creates <see cref="Y"/>.</param>
public class Wall(int id, Point2D p1, Point2D p2)
{
    /// <summary>
    /// Gets or sets the unique identifer for this Wall object.
    /// </summary>
    public int Id { get; set; } = id;

    /// <summary>
    /// Gets or sets a corner point on a wall. XXXXXXXXXXXXXXXXXXXXXXXXXXXXX.
    /// </summary>
    public Point2D P1 { get; set; } = p1;

    /// <summary>
    /// Gets or sets the second corner on a wall. XXXXXXXXXXXXXXXXXXXXXXXXX.
    /// </summary>
    public Point2D P2 { get; set; } = p2;

    public static bool operator ==(Wall left, Wall right)
    {
        return left.Id == right.Id;
    }

    public static bool operator !=(Wall left, Wall right)
    {
        return left.Id != right.Id;
    }

    /// <summary>
    /// Checks equality of 2 walls based upon their unique id's.
    /// </summary>
    /// <param name="obj">The object which will be compared against this wall.</param>
    /// <returns>A bool representing whether obj and this are equal.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is Wall wall && this == wall)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// An override for GetHashCode which returns the unique id for this Wall.
    /// </summary>
    /// <returns>A hashcode created from the wall's id.</returns>
    public override int GetHashCode()
    {
        return this.Id;
    }
}