// <copyright file="ChatServer.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

namespace CS3500.Chatting;
using System.Net.Quic;
using System.Text;
using System.Xml;
using CS3500.Networking;
using Microsoft.Extensions.Logging;

/// <summary>
///   A simple ChatServer that handles clients separately and replies with a static message.
/// </summary>
public partial class ChatServer
{
    /// <summary>
    /// Logger object for the ChatServer.
    /// </summary>
    private static readonly ILogger Logger;

    /// <summary>
    /// Dictionary containing all connections in the format key: threadID, value: (connection username, NetworkConnection).
    /// </summary>
    private static readonly Dictionary<int, (string? name, NetworkConnection connection)> Clients;

    /// <summary>
    /// object for locking.
    /// </summary>
    private static readonly object ClientsLock = new ();

    static ChatServer()
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.AddDebug();
            builder.SetMinimumLevel(LogLevel.Trace);
        });
        Logger = loggerFactory.CreateLogger("ChatServer");
        Clients = new Dictionary<int, (string? name, NetworkConnection connection)>();
    }

    /// <summary>
    ///   The main program.
    /// </summary>
    /// <param name="args"> ignored. </param>
    private static void Main(string[] args)
    {
        Server.StartServer(HandleConnect, 11_000);
        Console.Read(); // don't stop the program.
    }

    /// <summary>
    ///   <pre>
    ///     When a new connection is established, enter a loop that receives from and
    ///     replies to a client.
    ///   </pre>
    /// </summary>
    ///
    private static void HandleConnect(NetworkConnection connection)
    {
        // handle all messages until disconnect.
        int id = Thread.CurrentThread.ManagedThreadId;
        lock (ClientsLock)
        {
            Clients.Add(id, (null, connection));
        }

        try
        {
            string name = connection.ReadLine();

            lock (ClientsLock)
            {
                Clients[id] = (name, connection);
            }

            BroadcastMessage("Hello " + name + "!");
            Logger.LogInformation("Assigned client name");
            while (true)
            {
                string? message = connection.ReadLine();

                if (string.IsNullOrEmpty(message))
                {
                    Logger.LogWarning($"Empty message recieved from {name}. Ignoring.");
                    break;
                }

                BroadcastMessage($"{name}: {message}");
                Logger.LogDebug($"Message from {name}: {message}");
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Error with client id {id}");
        }
        finally
        {
            lock (ClientsLock)
            {
                if (Clients.TryGetValue(id, out var clientInfo) && clientInfo.name != null)
                {
                    BroadcastMessage($"{clientInfo.name} disconnected.");
                    Logger.LogInformation($"{clientInfo.name} disconnected.");
                }
            }
        }
    }

    /// <summary>
    /// Helper method to send messages to all clients.
    /// </summary>
    /// <param name="message">The message that should be sent to all clients.</param>
    private static void BroadcastMessage(string message)
    {
        lock (ClientsLock)
        {
            foreach (var (clientname, clientconnection) in Clients.Values)
            {
                try
                {
                    clientconnection.Send(message);
                }
                catch (Exception e)
                {
                    Logger.LogError("message failed to send. " + e);
                }
            }
        }
    }
}