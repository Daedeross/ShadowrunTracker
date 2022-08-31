namespace ShadowrunTracker
{
    using ShadowrunTracker.Communication;

    public interface IServiceFactory
    {
        ICommunicationService CommunicationService(string userName);

        void Release(object service);
    }
}
