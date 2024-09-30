using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PresentationEditor.Server.Models;
using PresentationEditor.Server.Services;

namespace PresentationEditor.Server.Hubs
{
    [Authorize]
    public class PresentationHub : Hub
    {
        readonly PresentationService presentationService;

        public PresentationHub(PresentationService presentationService)
        {
            this.presentationService = presentationService;
        }

        public Task JoinPresentation(Guid presentationId)
        {
            var user = new User(Context.User);

            return presentationService.ConnectUser(Context.ConnectionId, presentationId, user);
           
        }

        public Task UpdatePresentation(byte[] updates)
        {
            return presentationService.ApplyUpdate(Context.ConnectionId, updates);
        }

        public Task LeavePresentation()
        {
            return presentationService.DisconnectUser(Context.ConnectionId);
        }

        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            await LeavePresentation();
            await base.OnDisconnectedAsync(exception);
        }
    }
}
