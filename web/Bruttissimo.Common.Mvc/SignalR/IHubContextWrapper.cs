using SignalR.Hubs;

namespace Bruttissimo.Common.Mvc
{
    public interface IHubContextWrapper<THub> where THub : IHub // THub reference is used for resolving with IoC container.
    {
        IHubContext Context { get; }
    }
}