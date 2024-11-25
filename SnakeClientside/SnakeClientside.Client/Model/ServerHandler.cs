﻿// <copyright file="ServerHandler.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

using System.Text.Json;
using CS3500.Client.Model;
using CS3500.Networking;

/// <summary>
/// Receives json data from the server and deserializes it for use in the World object.
/// </summary>
public class ServerHandler : IDisposable
{
    private readonly NetworkConnection connection;
    private readonly World world;
    private readonly ILogger<ServerHandler> logger;
    private int worldSize;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerHandler"/> class.
    /// </summary>
    /// <param name="connection">The NetworkConnection object which will handle accepting information from the server.</param>
    /// <param name="world">The world object information will be sent to.</param>
    /// <param name="logger">Logger object.</param>
    public ServerHandler(NetworkConnection connection, World world, ILogger<ServerHandler> logger)
    {
        this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        this.world = world ?? throw new ArgumentNullException(nameof(world));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // Default vals
        this.PlayerId = -1;
        this.worldSize = -1;
    }

    /// <summary>
    /// Gets a value representing the unique id assigned to the player.
    /// </summary>
    public int PlayerId { get; private set; }

    /// <summary>
    /// Sends the player's name to the server.
    /// </summary>
    /// <param name="playerName">The name of the player.</param>
    public void SendPlayerName(string playerName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(playerName))
            {
                this.logger.LogWarning("Player name is null or whitespace.");
                return;
            }

            // Serialize the player's name into a JSON object
            var nameObject = new { name = playerName };
            string jsonMessage = JsonSerializer.Serialize(nameObject);

            // Send the JSON message to the server
            this.connection.Send(jsonMessage);
            this.logger.LogInformation($"Sent player name to server: {playerName}");
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to send player name to the server.");
        }
    }

    /// <summary>
    /// Starts listening for JSON data from the server.
    /// </summary>
    public void StartReceiving()
    {
        if (this.connection.IsConnected)
        {
            this.PlayerId = int.Parse(this.connection.ReadLine());
            this.worldSize = int.Parse(this.connection.ReadLine());
            Task.Run(() =>
            {
                try
                {
                    while (this.connection.IsConnected)
                    {
                        string jsonMessage = this.connection.ReadLine();
                        if (!string.IsNullOrWhiteSpace(jsonMessage))
                        {
                            this.logger.LogDebug($"Received JSON: {jsonMessage}");
                            this.world.UpdateWorld(jsonMessage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error while receiving JSON data.");
                }
            });
        }
        else
        {
            this.logger.LogWarning("Connection is not established. Cannot start receiving JSON.");
        }
    }

    /// <summary>
    /// Attempts to send a given json string to the server.
    /// </summary>
    /// <param name="movementString">A string which can be serialized to json to be sent to the server.</param>
    public void SendMessage(string movementString)
    {
        try
        {
            if (!this.IsValidDirection(movementString))
            {
                this.logger.LogInformation($"Invalid movement string received: {movementString}");
                return;
            }

            // Convert movement key to direction.
            string direction;
            if (movementString == "w" || movementString == "ArrowUp")
            {
                direction = "up";
            }
            else if (movementString == "a" || movementString == "ArrowLeft")
            {
                direction = "left";
            }
            else if (movementString == "s" || movementString == "ArrowDown")
            {
                direction = "down";
            }
            else
            {
                direction = "right";
            }

            // Serialize to JSON
            var movementObject = new { moving = direction };
            string jsonMessage = JsonSerializer.Serialize(movementObject);

            // Send the JSON message to the server
            this.connection.Send(jsonMessage);
            this.logger.LogDebug($"Sent movement message to server: {jsonMessage}");
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to send movement message to the server.");
        }
    }

    /// <summary>
    /// Disposes the network connection to avoid eating up resources.
    /// </summary>
    public void Dispose()
    {
        this.logger.LogInformation("Stopped receiving JSON data.");
        this.connection.Disconnect();
    }

    /// <summary>
    /// Validates if the given direction is one of the allowed inputs.
    /// </summary>
    private bool IsValidDirection(string direction)
    {
        return direction == "w" || direction == "a" || direction == "s" || direction == "d" ||
               direction == "arrowup" || direction == "arrowdown" || direction == "arrowleft" || direction == "arrowright";
    }
}