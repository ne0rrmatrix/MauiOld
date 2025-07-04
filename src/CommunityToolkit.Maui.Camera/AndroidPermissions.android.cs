using static Microsoft.Maui.ApplicationModel.Permissions;

namespace CommunityToolkit.Maui.ApplicationModel.Permissions;

sealed class AndroidMediaPermissions : BasePlatformPermission
{
	static readonly Lazy<(string androidPermission, bool isRuntime)[]> permissionsHolder = new(CreatePermissions);

	public override (string androidPermission, bool isRuntime)[] RequiredPermissions => permissionsHolder.Value;

	static (string androidPermission, bool isRuntime)[] CreatePermissions()
	{
		List<(string androidPermission, bool isRuntime)> requiredPermissionsList = [];
		
		requiredPermissionsList.Add((global::Android.Manifest.Permission.RecordAudio, true));
		requiredPermissionsList.Add((global::Android.Manifest.Permission.Camera, true));
		if (!OperatingSystem.IsAndroidVersionAtLeast(34))
		{
			requiredPermissionsList.Add((global::Android.Manifest.Permission.ReadExternalStorage, true));
			requiredPermissionsList.Add((global::Android.Manifest.Permission.ReadExternalStorage, true));
		}
		return [.. requiredPermissionsList];
	}
}