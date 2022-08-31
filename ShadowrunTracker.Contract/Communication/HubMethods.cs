namespace ShadowrunTracker.Communication
{
    /// <summary>
    /// Names of Methods the Server or Clients can call
    /// </summary>
    public static class HubMethods
    {
        #region Server Methods / Client Listeners

        public static string ReceiveUpdate => "ReceiveUpdate";

        #endregion

        #region Client Methods / Hub Listenders

        public static string SendUpdate => "SendUpdate";

        public static string AddToGroup => "AddToGroup";

        public static string RequestState => "RequestState";

        public static string EndSession => "EndSession";

        #endregion
    }
}
