using Microsoft.JSInterop;

namespace CRM.Admin.Extensions;

public static class JSRuntimeExtensions
{
    public static bool IsInvokable(this IJSRuntime jsRuntime)
    {
        return jsRuntime is IJSInProcessRuntime;
    }
}