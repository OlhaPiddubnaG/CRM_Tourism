using System.ComponentModel;

namespace CRM.Domain.Enums;

public enum MealsType
{
    [Description("Room only")]
    RO = 0,
    
    [Description("Bed and breakfast")]
    BB = 1,
    
    [Description("Half board")]
    HB = 2,
    
    [Description("Half board plus")]
    HBplus= 3,
    
    [Description("Full board")]
    FB = 4,
    
    [Description("Full board plus")]
    FBplus = 5,
    
    [Description("All inclusive")]
    AI = 6,
    
    [Description("Ultra all inclusive")]
    UALL = 7
}