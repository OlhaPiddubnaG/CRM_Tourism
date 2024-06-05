using Microsoft.JSInterop;

namespace CRM.Admin;

public static class JSRuntimeExtensions
{
    public static bool IsInvokeable(this IJSRuntime jsRuntime)
    {
        return jsRuntime is IJSInProcessRuntime;
    }
}