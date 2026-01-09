using System.Windows;
using System.Windows.Input;
using GameUpdater.Helpers;
using GameUpdater.Services.Interfaces;

namespace GameUpdater.Views.Dialogs;

/// <summary>
/// Dialog for requesting administrator elevation
/// </summary>
public partial class AdminRequiredDialog : Window
{
    private readonly IPermissionService _permissionService;

    public AdminRequiredDialog(IPermissionService permissionService)
    {
        InitializeComponent();
        _permissionService = permissionService;
    }

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        WindowDragHelper.DragWindow(this);
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void RestartAsAdminButton_Click(object sender, RoutedEventArgs e)
    {
        _permissionService.RequestAdminElevation();
        DialogResult = true;
        Close();
    }
}
