// <copyright file="NetworkConnection.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

namespace CS3500.Networking;

using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Logging;

/// <summary>
///   Wraps the StreamReader/Writer/TcpClient together so we
///   don't have to keep creating all three for network actions.
/// </summary>
public sealed class NetworkConnection : IDisposable
{
    /// <summary>
    ///     Logger object for NetworkConnections.
    /// </summary>
    private readonly ILogger logger;

    /// <summary>
    ///   The connection/socket abstraction.
    /// </summary>
    private TcpClient tcpClient = new ();

    /// <summary>
    ///   Reading end of the connection.
    /// </summary>
    private StreamReader? reader = null;

    /// <summary>
    ///   Writing end of the connection.
    /// </summary>
    private StreamWriter? writer = null;

    /// <summary>
    ///   Initializes a new instance of the <see cref="NetworkConnection"/> class.
    ///   <para>
    ///     Create a network connection object.
    ///   </para>
    /// </summary>
    /// <param name="tcpClient">
    ///   An already existing TcpClient.
    /// </param>
    /// <param name="logger"> The logging element. </param>
    public NetworkConnection(TcpClient tcpClient, ILogger logger)
    {
        this.logger = logger;
        this.tcpClient = tcpClient;
        if (this.IsConnected)
        {
            // Only establish the reader/writer if the provided TcpClient is already connected.
            this.reader = new StreamReader(this.tcpClient.GetStream(), Encoding.UTF8);
            this.writer = new StreamWriter(this.tcpClient.GetStream(), Encoding.UTF8) { AutoFlush = true }; // AutoFlush ensures data is sent immediately
            this.logger.LogInformation("Reader and writer initialized.");
        }
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="NetworkConnection"/> class.
    ///   <para>
    ///     Create a network connection object.  The tcpClient will be unconnected at the start.
    ///   </para>
    /// </summary>
    /// <param name="logger">The logger object to use for the connection.</param>
    public NetworkConnection(ILogger logger)
        : this(new TcpClient(), logger)
    {
    }

    /// <summary>
    /// Gets a value indicating whether the socket is connected.
    /// </summary>
    public bool IsConnected
    {
        get
        {
            try
            {
                if (this.tcpClient.Client != null && this.tcpClient.Client.Connected)
                {
                    if (this.tcpClient.Client.Poll(0, SelectMode.SelectRead) && this.tcpClient.Client.Available == 0)
                    {
                        this.logger.LogWarning("Lost Connection");
                        return false;
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "No connection");
            }

            return false;
        }
    }

    /// <summary>
    ///   Try to connect to the given host:port.
    /// </summary>
    /// <param name="host"> The URL or IP address, e.g., www.cs.utah.edu, or  127.0.0.1. </param>
    /// <param name="port"> The port, e.g., 11000. </param>
    public void Connect(string host, int port)
    {
        try
        {
            this.tcpClient.Connect(host, port);
            this.reader = new StreamReader(this.tcpClient.GetStream(), Encoding.UTF8);
            this.writer = new StreamWriter(this.tcpClient.GetStream(), Encoding.UTF8) { AutoFlush = true };
            this.logger.LogInformation($"Connected to {host}:{port}");
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, $"Failed to connect to {host}:{port}");
            throw;
        }
    }

    /// <summary>
    ///   Send a message to the remote server.  If the <paramref name="message"/> contains
    ///   new lines, these will be treated on the receiving side as multiple messages.
    ///   This method should attach a newline to the end of the <paramref name="message"/>
    ///   (by using WriteLine).
    ///   If this operation can not be completed (e.g. because this NetworkConnection is not
    ///   connected), throw an InvalidOperationException.
    /// </summary>
    /// <param name="message"> The string of characters to send. </param>
    public void Send(string message)
    {
        if (!this.IsConnected || this.writer == null)
        {
            this.logger.LogError("Not connected");
            throw new InvalidOperationException("Not connected.");
        }

        try
        {
            this.writer.WriteLine(message);
            this.logger.LogInformation($"Sent message: {message}");
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to send message.");
            throw;
        }
    }

    /// <summary>
    ///   Read a message from the remote side of the connection.  The message will contain
    ///   all characters up to the first new line. See <see cref="Send"/>.
    ///   If this operation can not be completed (e.g. because this NetworkConnection is not
    ///   connected), throw an InvalidOperationException.
    /// </summary>
    /// <returns> The contents of the message. </returns>
    public string ReadLine()
    {
        if (!this.IsConnected || this.reader == null)
        {
            this.logger.LogError("Not connected.");
            throw new InvalidOperationException("Not connected");
        }

        try
        {
            string? message = this.reader.ReadLine();
            this.logger.LogInformation($"Recieved message: {message}");
            return message ?? string.Empty;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to send message.");
            throw;
        }
    }

    /// <summary>
    ///   If connected, disconnect the connection and clean
    ///   up (dispose) any streams.
    /// </summary>
    public void Disconnect()
    {
        if (this.tcpClient.Connected)
        {
            this.reader?.Dispose();
            this.writer?.Dispose();
            this.tcpClient.Close();
            this.logger.LogInformation("Disconnected");
        }
        else
        {
            this.logger.LogWarning("Attempted to disconnect, but already disconnected");
        }
    }

    /// <summary>
    ///   Automatically called with a using statement (see IDisposable).
    /// </summary>
    public void Dispose()
    {
        this.Disconnect();
        this.tcpClient.Dispose();
    }
}
