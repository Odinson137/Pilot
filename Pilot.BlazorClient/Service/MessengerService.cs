﻿using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.SignalR.Client;
using Pilot.BlazorClient.Data;
using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.Contracts.Services;

namespace Pilot.BlazorClient.Service;

public class MessengerService : IMessengerService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<MessengerService> _logger;
    private HubConnection? _connection;
    private ProtectedSessionStorage _protectedSessionStore;

    public MessengerService(IConfiguration configuration, ILogger<MessengerService> logger,
        ProtectedSessionStorage protectedSessionStore)
    {
        _configuration = configuration;
        _logger = logger;
        _protectedSessionStore = protectedSessionStore;
    }

    public async Task CreateConnectionAsync()
    {
        var tokenResult = await _protectedSessionStore.GetAsync<string>(ClientConstants.Token);
        var token = tokenResult.Success ? tokenResult.Value : null;

        if (string.IsNullOrEmpty(token))
        {
            _logger.LogWarning("Token is null or empty. Connection will not be established.");
            return;
        }

        _connection = new HubConnectionBuilder()
            .WithUrl(_configuration["MessengerServerHub"] ?? throw new Exception("Hub URL not found in configuration"),
                options => { options.AccessTokenProvider = () => Task.FromResult(token)!; })
            .WithAutomaticReconnect()
            .Build();

        _connection.On<string>("ReceiveMessage", message => { OnMessageReceived?.Invoke(message); });
        _connection.On<string>("ReceiveNotification", message =>
        {
            var infoMessage = message.FromJson<InfoMessageViewModel>();
            OnReceiveNotification?.Invoke(infoMessage);
            OnActionNotification?.Invoke(infoMessage);
        });

        try
        {
            await _connection.StartAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("Connecting error: " + ex.Message);
        }
    }

    public async Task SendMessageAsync(int chatId, string messageText)
    {
        if (_connection == null || _connection.State == HubConnectionState.Disconnected)
        {
            _logger.LogError("Connection not established.");
            return;
        }

        await _connection.InvokeAsync("SendMessage", chatId, messageText);
    }

    public event Action<string>? OnMessageReceived;
    public event Action<InfoMessageViewModel>? OnReceiveNotification;
    public event Action<InfoMessageViewModel>? OnActionNotification;
}