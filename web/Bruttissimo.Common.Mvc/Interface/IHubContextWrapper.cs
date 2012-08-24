using SignalR.Hubs;

namespace Bruttissimo.Common.Mvc
{
    /// <summary>
    /// Provides access to the IHubContext implementation for THub.
    /// </summary>
    public interface IHubContextWrapper<THub> where THub : IHub // THub reference is used for resolving with IoC container.
    {
        /// <summary>
        /// The IHubContext implementation for the hub implementing THub.
        /// </summary>
        IHubContext Context { get; }
    }
}
