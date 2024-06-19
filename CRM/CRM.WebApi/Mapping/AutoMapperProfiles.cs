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
            typeof(CountryProfile).Assembly,
            typeof(ClientPrivateDataProfile).Assembly,
            typeof(PassportInfoProfile).Assembly,
        };
    }
}