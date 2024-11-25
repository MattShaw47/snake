// <copyright file="Snake.cs" company="UofU-CS3500">
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
public class Snake
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Snake"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of this snake.</param>
    /// <param name="name">The display name of this snake.</param>
    /// <param name="body">The list of Point2Ds representing the body of this snake.</param>
    /// <param name="dir">The direction of this snake as a Point2D.</param>
    /// <param name="score">The int representing this snake's score value.</param>
    /// <param name="died">A bool representing whether the snake died this frame.</param>
    /// <param name="alive">A bool represnting whether this snake is alive this frame.</param>
    /// <param name="dc">A bool representing whether this snake disconnected this frame or not.</param>
    /// <param name="joined">A bool representing whether this snake joined this frame or not.</param>
    [JsonConstructor]
    public Snake(int id, string name, List<Point2D> body, Point2D dir, int score, bool died, bool alive, bool dc, bool joined)
    {
        this.Id = id;
        this.Name = name;
        this.Body = body;
        this.Dir = dir;
        this.Score = score;
        this.Died = died;
        this.Alive = alive;
        this.Dc = dc;
        this.Joined = joined;
    }

    /// <summary>
    /// Gets or sets the unique identifier for this Snake object.
    /// </summary>
    [JsonPropertyName("snake")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the display name of this snake.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the List of points representing this snake's body.
    /// </summary>
    [JsonPropertyName("body")]
    public List<Point2D> Body { get; set; }

    /// <summary>
    /// Gets or sets a point2d representing the direction of snake based upon 0,0. (ex. -1,0 means the snake is heading west.)
    /// </summary>
    [JsonPropertyName("dir")]
    public Point2D Dir { get; set; }

    /// <summary>
    /// Gets or sets the displayed score value of this snake.
    /// </summary>
    [JsonPropertyName("score")]
    public int Score { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this snake is dead or not.
    /// </summary>
    [JsonPropertyName("died")]
    public bool Died { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this snake is alive or not.
    /// </summary>
    [JsonPropertyName("alive")]
    public bool Alive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this snake is disconnected or not.
    /// </summary>
    [JsonPropertyName("dc")]
    public bool Dc { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this snake joined in the last frame or not.
    /// </summary>
    [JsonPropertyName("join")]
    public bool Joined { get; set; }

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