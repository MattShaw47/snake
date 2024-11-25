// <copyright file="Point2D.cs" company="UofU-CS3500">
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
public class Point2D
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Point2D"/> class.
    /// </summary>
    /// <param name="x">An int representing where this point is on the x plane.</param>
    /// <param name="y">An int representing where this point is on the y plane.</param>
    [JsonConstructor]
    public Point2D(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    /// <summary>
    /// Gets or sets the x coordinate of the Point2D object. Generally horizontal position.
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// Gets or sets the y coordinate of the Point2D object. Generally vertical position.
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// Creates a string representing this Point2D.
    /// </summary>
    /// <returns>A string representation of this Point2D.</returns>
    public override string ToString()
    {
        return $"{this.X}, {this.Y}";
    }
}