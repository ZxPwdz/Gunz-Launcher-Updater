using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace GameUpdater.Helpers;

/// <summary>
/// Helper class for enabling window drag on borderless windows
/// </summary>
public static class WindowDragHelper
{
    [DllImport("user32.dll")]
    private static extern bool ReleaseCapture();

    [DllImport("user32.dll")]
    private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

    private const int WM_NCLBUTTONDOWN = 0xA1;
    private const int HTCAPTION = 0x2;

    /// <summary>
    /// Enable dragging for a window when mouse is pressed on the specified element
    /// </summary>
    public static void EnableDrag(Window window, UIElement dragElement)
    {
        dragElement.MouseLeftButtonDown += (s, e) =>
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragWindow(window);
            }
        };
    }

    /// <summary>
    /// Initiate window drag operation
    /// </summary>
    public static void DragWindow(Window window)
    {
        ReleaseCapture();
        var helper = new WindowInteropHelper(window);
        SendMessage(helper.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
    }
}
