using Bruttissimo.Common.Mvc.Interface;
using SignalR;
using SignalR.Hubs;

namespace Bruttissimo.Common.Mvc.SignalR
{
    /// <summary>
    /// Provides access to the IHubContext implementation for THub.
    /// </summary>
    public class HubContextWrapper<THub> : IHubContextWrapper<THub> where THub : IHub
    {
        /// <summary>
        /// The IHubContext implementation for the hub implementing THub.
        /// </summary>
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
