using System.Reflection;

namespace CRM.WebApi.Mapping;

public static class AutoMapperProfiles
{
    public static Assembly[] GetAssemblies()
    {
        return new[]
        {
            typeof(CompanyProfile).Assembly,
            typeof(UserProfile).Assembly,
            typeof(ClientProfile).Assembly,
            typeof(ClientPrivateDataProfile).Assembly,
            typeof(ClientStatusHistoryProfile).Assembly,
            typeof(CountryProfile).Assembly,
            typeof(PassportInfoProfile).Assembly,
            typeof(OrderProfile).Assembly,
            typeof(TouroperatorProfile).Assembly,
            typeof(OrderStatusHistoryProfile).Assembly,
            typeof(NumberOfPeopleProfile).Assembly,
            typeof(StaysProfile).Assembly,
            typeof(MealsProfile).Assembly,
            typeof(PaymentProfile).Assembly,
            typeof(RoomTypeProfile).Assembly,
            typeof(HotelProfile).Assembly,
            typeof(UserTasksProfile).Assembly,
        };
    }
}