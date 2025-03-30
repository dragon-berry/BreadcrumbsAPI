namespace BreadcrumbsAPI.Constants;

public static class CodeValueConstants
{
    //LocationType Constants
    public static Guid AddressLocationType = new Guid("EEA9F692-3A1D-4C70-A22F-D4F2DDBA5D76");
    public static Guid CoordinatesLocationType = new Guid("82485D5F-84FC-4164-9993-6D774B0A2095");
    public static Guid GoogleMapsLocationType = new Guid("A1C843DE-97EA-4B12-9309-145D924D6943");

    //CrumbType Constants
    public static Guid TextCrumbType = new Guid("95AE257D-AD0C-439B-831E-A2CCE06707BD");
    public static Guid ImageCrumbType = new Guid("D09B805B-97B0-4FF7-909E-D6B0DDD05281");
    public static Guid SongCrumbType = new Guid("C7C0D81C-261D-4043-BB40-D7B5D4FC2AD5");
    public static Guid AudioCrumbType = new Guid("980B8BB5-3CFF-4DD2-B1E5-4C73569F3053");

    //LifeSpan Constants
    public static Guid OneHourLifeSpan = new Guid("BA4C51B8-3C27-43D0-95A3-A17796256AC2");
    public static Guid OneDayLifeSpan = new Guid("27E10288-47C2-49D2-AEBB-2B8196FDA84E");
    public static Guid OneWeekLifeSpan = new Guid("C07C9FE3-83B0-41EB-A709-97562C915F64");

    //Status Constants
    public static Guid ActiveStatus = new Guid("8531285F-4855-42F8-8938-FEDEBC9FDABB");
    public static Guid PendingStatus = new Guid("5189315C-0570-42E7-AE0B-A260A162CF4E");
    public static Guid RemovedStatus = new Guid("3AFBFB06-348D-4663-8B4F-7AECB946204F");
}
