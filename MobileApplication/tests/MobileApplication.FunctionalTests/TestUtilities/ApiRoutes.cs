namespace MobileApplication.FunctionalTests.TestUtilities
{
    public class ApiRoutes
    {
        public const string Base = "api";
        public const string Health = Base + "/health";        

public static class RideRequests
        {
            public const string RideRequestId = "{rideRequestId}";
            public const string GetList = Base + "/rideRequests";
            public const string GetRecord = Base + "/rideRequests/" + RideRequestId;
            public const string Create = Base + "/rideRequests";
            public const string Delete = Base + "/rideRequests/" + RideRequestId;
            public const string Put = Base + "/rideRequests/" + RideRequestId;
            public const string Patch = Base + "/rideRequests/" + RideRequestId;
        }

        // new api route marker - do not delete
    }
}