// <copyright file="Point2D.cs" company="UofU-CS3500">
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
public class Point2D(int x, int y)
{
    /// <summary>
    /// Gets or sets the x coordinate of the Point2D object. Generally horizontal position.
    /// </summary>
    public int X { get; set; } = x;

    /// <summary>
    /// Gets or sets the y coordinate of the Point2D object. Generally vertical position.
    /// </summary>
    public int Y { get; set; } = y;
}