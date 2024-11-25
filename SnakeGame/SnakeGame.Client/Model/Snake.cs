// <copyright file="Snake.cs" company="UofU-CS3500">
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
public class Snake(int id, string name, List<Point2D> body, Point2D dir, int score, bool died, bool alive, bool dc, bool joined)
{
    /// <summary>
    /// Gets or sets the unique identifier for this Snake object.
    /// </summary>
    public int Id { get; set; } = id;

    /// <summary>
    /// Gets or sets the display name of this snake.
    /// </summary>
    public string Name { get; set; } = name;

    /// <summary>
    /// Gets or sets the List of points representing this snake's body.
    /// </summary>
    public List<Point2D> Body { get; set; } = body;

    /// <summary>
    /// Gets or sets a point2d representing the direction of snake based upon 0,0. (ex. -1,0 means the snake is heading west.)
    /// </summary>
    public Point2D Dir { get; set; } = dir;

    /// <summary>
    /// Gets or sets the displayed score value of this snake.
    /// </summary>
    public int Score { get; set; } = score;

    /// <summary>
    /// Gets or sets a value indicating whether this snake is dead or not.
    /// </summary>
    public bool Died { get; set; } = died;

    /// <summary>
    /// Gets or sets a value indicating whether this snake is alive or not.
    /// </summary>
    public bool Alive { get; set; } = alive;

    /// <summary>
    /// Gets or sets a value indicating whether this snake is disconnected or not.
    /// </summary>
    public bool Dc { get; set; } = dc;

    /// <summary>
    /// Gets or sets a value indicating whether this snake joined in the last frame or not.
    /// </summary>
    public bool Joined { get; set; } = joined;

    public static bool operator ==(Snake left, Snake right)
    {
        return left.Id == right.Id;
    }

    public static bool operator !=(Snake left, Snake right)
    {
        return left.Id != right.Id;
    }

    /// <summary>
    /// Checks equality of 2 snakes based upon their unique id's.
    /// </summary>
    /// <param name="obj">The object which will be compared against this wall.</param>
    /// <returns>A bool representing whether obj and this are equal.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is Snake wall && this == wall)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// An override for GetHashCode which returns the unique id for this Snake.
    /// </summary>
    /// <returns>A hashcode created from the snake's id.</returns>
    public override int GetHashCode()
    {
        return this.Id;
    }
}