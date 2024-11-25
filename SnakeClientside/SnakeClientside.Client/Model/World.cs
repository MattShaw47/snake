// <copyright file="World.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

namespace CS3500.Client.Model;

using System.Text.Json;

/// <summary>
/// A representation of the entire game. Tracks the location of all objects, and allows for them to be updated.
/// </summary>
public class World
{
    /// <summary>
    /// object for locking.
    /// </summary>
    private static readonly object WorldLock = new ();

    /// <summary>
    /// Logger object for the Server.
    /// </summary>
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="World"/> class.
    /// Creates a game world which handles the positions of all objects and players as dicated by the server.
    /// </summary>
    /// <param name="logger">The logger object used to log this world object.</param>
    /// <exception cref="NotImplementedException">we aint made it yet.</exception>
    public World(ILogger logger)
    {
        this.Walls = new Dictionary<int, Wall>();
        this.Snakes = new Dictionary<int, Snake>();
        this.Powerups = new Dictionary<int, Powerup>();

        this.logger = logger;
    }

    /// <summary>
    /// Gets a list of all wall objects in this world.
    /// </summary>
    public Dictionary<int, Wall> Walls { get; private set; }

    /// <summary>
    /// Gets a list of all powerup objects in this world.
    /// </summary>
    public Dictionary<int, Powerup> Powerups { get; private set; }

    /// <summary>
    /// Gets a dictinoary of all snake objects in this world. Organized by id, Snake.
    /// </summary>
    public Dictionary<int, Snake> Snakes { get; private set; }

    /// <summary>
    /// Accepts JSON deserialized to a string object.
    /// </summary>
    /// <param name="worldData">The json string object.</param>
    /// <exception cref="NotImplementedException">we aint made it.</exception>
    public void UpdateWorld(string worldData)
    {
        using JsonDocument doc = JsonDocument.Parse(worldData);
        var root = doc.RootElement;

        var firstItem = root.EnumerateObject().First().Name;
        switch (firstItem)
        {
            case "wall":
                this.HandleWall(doc);
                break;

            case "snake":
                this.HandleSnake(doc);
                break;

            case "power":
                this.HandlePowerup(doc);
                break;

            default:
                this.logger.LogError("Invalid Json recieved; did not fit any known format.");
                break;
        }
    }

    private void HandlePowerup(JsonDocument powerupData)
    {
        Powerup newPowerup = JsonSerializer.Deserialize<Powerup>(powerupData) ?? new Powerup(-1, new Point2D(-1, -1), true);

        this.Powerups[newPowerup.Id] = newPowerup;
        this.logger.LogDebug($"Added or modified snake with id {newPowerup.Id}");
    }

    private void HandleSnake(JsonDocument snakeData)
    {
        Snake newSnake = JsonSerializer.Deserialize<Snake>(snakeData) ?? new Snake(-1, "BAD JSON ENTRY", new List<Point2D>(), new Point2D(-1, -1), -1, true, false, true, false);
        this.Snakes[newSnake.Id] = newSnake;
        this.logger.LogDebug($"Added or modified snake with id {newSnake.Id}");
    }

    private void HandleWall(JsonDocument wallData)
    {
        Wall newWall = JsonSerializer.Deserialize<Wall>(wallData) ?? new Wall(-1, new Point2D(0, 0), new Point2D(0, 0));

        this.Walls[newWall.Id] = newWall;
        this.logger.LogDebug($"Added or modified wall with id {newWall.Id}.");
    }
}