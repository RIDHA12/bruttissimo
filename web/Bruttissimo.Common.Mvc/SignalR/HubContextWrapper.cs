using SignalR;
using SignalR.Hubs;

namespace Bruttissimo.Common.Mvc
{
    public class HubContextWrapper<THub> : IHubContextWrapper<THub> where THub : IHub
    {
        public IHubContext Context
        {
            get
            {
                IHubContext context = GlobalHost.ConnectionManager.GetHubContext<THub>();
                return context;
            }
        }
    }
}