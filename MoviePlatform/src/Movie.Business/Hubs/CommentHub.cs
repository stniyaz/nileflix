using Microsoft.AspNetCore.SignalR;

namespace Movie.Business.Hubs
{
    public class CommentHub : Hub
    {
        public async Task SendComment(int movieId, string comment)
        {
            await Clients.All.SendAsync("ReceiveMessage", movieId, comment);
        }
    }
}
