// <copyright file="Server.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

namespace CS3500.Networking;

using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;

/// <summary>
///   Represents a server task that waits for connections on a given
///   port and calls the provided delegate when a connection is made.
/// </summary>
public static class Server
{
    /// <summary>
    /// Logger object for the Server.
    /// </summary>
    private static readonly ILogger Logger;

    /// <summary>
    /// A Tcp listener to listen for connections.
    /// </summary>
    private static TcpListener? listener;

    /// <summary>
    /// Dictionary containing all active threads.
    /// </summary>
    private static Dictionary<int, Thread> activeThreads;

    /// <summary>
    /// Initializes static members of the <see cref="Server"/> class.
    ///    Static constructor. Create the logger.
    /// </summary>
    static Server()
    {
        // Always has an error. It wants there to be no whitespace before a bracket, but wants whitespace after an equals.
        activeThreads = new Dictionary<int, Thread>();
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.AddDebug();
            builder.SetMinimumLevel(LogLevel.Trace);
        });
        Logger = loggerFactory.CreateLogger("Server");
    }

    /// <summary>
    ///   Wait on a TcpListener for new connections. Alert the main program
    ///   via a callback (delegate) mechanism.
    /// </summary>
    /// <param name="handleConnect">
    ///   Handler for what the user wants to do when a connection is made.
    ///   This should be run asynchronously via a new thread.
    /// </param>
    /// <param name="port"> The port (e.g., 11000) to listen on. </param>
    public static void StartServer(Action<NetworkConnection> handleConnect, int port)
    {
        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();

        while (true)
        {
            TcpClient tcpClient = listener.AcceptTcpClient();

            Thread connectionThread = new Thread(() =>
            {
                NetworkConnection connection = new (tcpClient, Logger);
                handleConnect(connection);
                Logger.LogDebug($"New connection thread created: {connection}");
            });
            connectionThread.Start();

            activeThreads[connectionThread.ManagedThreadId] = connectionThread;
        }
    }
}
