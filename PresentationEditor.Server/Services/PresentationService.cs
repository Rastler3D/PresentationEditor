global using UserID = System.Guid;
global using PresentationID = System.Guid;
global using ConnectionId = string;

using PresentationEditor.Server.Models;
using System.Collections.Concurrent;
using YDotNet.Document;
using Microsoft.AspNetCore.SignalR;
using PresentationEditor.Server.Hubs;
using System.Text.RegularExpressions;


namespace PresentationEditor.Server.Services
{
    public class PresentationService
    {
        IHubContext<PresentationHub> presentationHub { get; set; }
        ConcurrentDictionary<PresentationID, PresentationRoom> presentationRooms { get; set; } = new();
        ConcurrentDictionary<ConnectionId, (UserID userId, PresentationID presentationId)> connections { get; set; } = new();

        public PresentationService(IHubContext<PresentationHub> presentationHub)
        {
            this.presentationHub = presentationHub;
        }

        public async Task ApplyUpdate(string connectionId, byte[] update)
        {
            if (connections.TryGetValue(connectionId, out var connection)){
                var (userId, presentationId) = connection;
                var presentation = presentationRooms[presentationId].Presentation;
                if (presentation.Administrator == userId || presentation.Editors.Contains(userId))
                {
                    presentation
                    .Data
                    .WriteTransaction()
                    .ApplyV2(update);

                    await presentationHub.Clients
                        .GroupExcept(presentationId.ToString(), connectionId)
                        .SendAsync("PresentationUpdate", update);
                }
            }
        }

        public async Task ConnectUser(string connectionId, PresentationID presentationId, User user)
        {
            if (presentationRooms.TryGetValue(presentationId, out var presentationRoom))
            {
                connections[connectionId] = (user.Id, presentationId);
                presentationRoom.ConnectedParticipants.Add(user);

                await presentationHub.Clients.Client(connectionId).SendAsync("PresentationSnapshot", presentationRoom);
                await presentationHub.Clients.Group(presentationId.ToString()).SendAsync("UserJoin", user);
                await presentationHub.Groups.AddToGroupAsync(connectionId, presentationId.ToString());
            }
        }

        public async Task DisconnectUser(string connectionId)
        {
            if (connections.Remove(connectionId, out var connection))
            {
                var (userId, presentationId) = connection;
                presentationRooms[presentationId].ConnectedParticipants.RemoveWhere(x => x.Id == connection.userId);

                await presentationHub.Groups.RemoveFromGroupAsync(connectionId, presentationId.ToString());
                await presentationHub.Clients
                    .Group(presentationId.ToString())
                    .SendAsync("UserLeave", userId);
            }

        }

        public IEnumerable<PresentationRoom> GetPresentations()
        {
            return presentationRooms.Values;
        }

        public PresentationRoom? GetPresentation(Guid presentationId)
        {
            if (presentationRooms.TryGetValue(presentationId, out var presentation))
            {
                return presentation;
            }
            return null;
        }

        public PresentationRoom CreatePresentation(UserID creatorId, string name)
        {
            var presentationRoom = new PresentationRoom(creatorId, name);
            var presentationId = presentationRoom.Presentation.Id;
            presentationRooms[presentationId] = presentationRoom;

            return presentationRoom;
        }

        public async Task RemovePresentation(UserID userId, PresentationID presentationId)
        {
            if (presentationRooms.TryGetValue(presentationId, out var presentationRoom))
            {
                if (presentationRoom.Presentation.Administrator == userId)
                {
                    var connectionsToRemove = connections
                        .Where(connection => connection.Value.presentationId == presentationId)
                        .Select(connection => connection.Key)
                        .ToArray();

                    foreach (var connectionId in connectionsToRemove)
                    {
                        connections.Remove(connectionId, out _);

                    }
                    presentationRooms.Remove(presentationId, out _);


                    await presentationHub.Clients
                        .Clients(connectionsToRemove)
                        .SendAsync("PresentationRemove", presentationId);
                }
            }
        }

        public async Task AddEditor(UserID userId, PresentationID presentationId, UserID editorId)
        {
            if (presentationRooms.TryGetValue(presentationId, out var presentationRoom))
            {
                if (presentationRoom.Presentation.Administrator == userId)
                {
                    presentationRoom.Presentation.Editors.Add(userId);

                    await presentationHub.Clients
                        .Group(presentationId.ToString())
                        .SendAsync("EditorAdd", userId);
                }
            }
        }

        public async Task RemoveEditor(UserID userId, PresentationID presentationId, UserID editorId)
        {
            if (presentationRooms.TryGetValue(presentationId, out var presentationRoom))
            {
                if (presentationRoom.Presentation.Administrator == userId || editorId == userId)
                {
                    presentationRoom.Presentation.Editors.Remove(userId);

                    await presentationHub.Clients
                        .Group(presentationId.ToString())
                        .SendAsync("EditorRemove", userId);
                }

            }
        }

        public (UserID userId, PresentationID presentationId)? GetConnectionInfo(ConnectionId connectionId)
        {
            if (connections.TryGetValue(connectionId, out var connectionInfo))
            {
                return connectionInfo;
            }
            return null;
        }
    }
}
