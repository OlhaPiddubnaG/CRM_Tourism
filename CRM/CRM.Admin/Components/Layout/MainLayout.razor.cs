using MudBlazor;

namespace CRM.Admin.Components.Layout;

public partial class MainLayout
{
    bool open = false;
    bool dense = false;
    bool preserveOpenState = false;
    Breakpoint breakpoint = Breakpoint.Lg;
    DrawerClipMode clipMode = DrawerClipMode.Never;

    void ToggleDrawer()
    {
        open = !open;
    }
}