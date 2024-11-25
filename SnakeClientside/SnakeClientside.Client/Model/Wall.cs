// <copyright file="Wall.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

namespace CS3500.Client.Model;

using System.Text.Json;
using System.Text.Json.Serialization;
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
public class Wall
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Wall"/> class.
    /// Constructor for Wall class so that Json can deserialize to a Wall.
    /// </summary>
    /// <param name="id">The unique identifier of the Wall.</param>
    /// <param name="p1">The first point in the wall.</param>
    /// <param name="p2">The second point in the wall.</param>
    [JsonConstructor]
    public Wall(int id, Point2D p1, Point2D p2)
    {
        this.Id = id;
        this.P1 = p1;
        this.P2 = p2;
    }

    /// <summary>
    /// Gets or sets the unique identifer for this Wall object.
    /// </summary>
    [JsonPropertyName("wall")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets a corner point on a wall. XXXXXXXXXXXXXXXXXXXXXXXXXXXXX.
    /// </summary>
    [JsonPropertyName("p1")]
    public Point2D P1 { get; set; }

    /// <summary>
    /// Gets or sets the second corner on a wall. XXXXXXXXXXXXXXXXXXXXXXXXX.
    /// </summary>
    [JsonPropertyName("p2")]
    public Point2D P2 { get; set; }

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