namespace FreeEcho.FEStartupControl;

/// <summary>
/// スタートアップのレジストリの最上位ノードとパス
/// </summary>
class StartupRegistryHidePath
{
    /// <summary>
    /// logon user
    /// </summary>
    /// <returns>RegistryHivePath</returns>
    public static RegistryHivePath LogonUser => new(Microsoft.Win32.RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\Run");
    /// <summary>
    /// logon user (once)
    /// </summary>
    /// <returns>RegistryHivePath</returns>
    public static RegistryHivePath LogonUserOnce => new(Microsoft.Win32.RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\RunOnce");
    /// <summary>
    /// all user
    /// </summary>
    /// <returns>RegistryHivePath</returns>
    public static RegistryHivePath AllUser => new(Microsoft.Win32.RegistryHive.LocalMachine, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
    /// <summary>
    /// all user (once)
    /// </summary>
    /// <returns>RegistryHivePath</returns>
    public static RegistryHivePath AllUserOnce => new(Microsoft.Win32.RegistryHive.LocalMachine, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce");
    /// <summary>
    /// all user (32 bit)
    /// </summary>
    /// <returns>RegistryHivePath</returns>
    public static RegistryHivePath AllUser32Bit => new(Microsoft.Win32.RegistryHive.LocalMachine, "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Run");
    /// <summary>
    /// all user (once、32 bit)
    /// </summary>
    /// <returns>RegistryHivePath</returns>
    public static RegistryHivePath AllUserOnce32Bit => new(Microsoft.Win32.RegistryHive.LocalMachine, "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\RunOnce");

    /// <summary>
    /// logon user folder (無効化されたスタートアップ)
    /// </summary>
    /// <returns>RegistryHivePath</returns>
    public static RegistryHivePath DisabledFolderLogonUser => new(Microsoft.Win32.RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved\\StartupFolder");
    /// <summary>
    /// all user folder (無効化されたスタートアップ)
    /// </summary>
    /// <returns>RegistryHivePath</returns>
    public static RegistryHivePath DisabledFolderAllUser => new(Microsoft.Win32.RegistryHive.LocalMachine, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved\\StartupFolder");
    /// <summary>
    /// logon user registry (無効化されたスタートアップ)
    /// </summary>
    /// <returns>RegistryHivePath</returns>
    public static RegistryHivePath DisabledRegistryLogonUser => new(Microsoft.Win32.RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved\\Run");
    /// <summary>
    /// all user registry (無効化されたスタートアップ)
    /// </summary>
    /// <returns>RegistryHivePath</returns>
    public static RegistryHivePath DisabledRegistryAllUser => new(Microsoft.Win32.RegistryHive.LocalMachine, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved\\Run");
    /// <summary>
    /// all user registry (32 bit) (無効化されたスタートアップ)
    /// </summary>
    /// <returns>RegistryHivePath</returns>
    public static RegistryHivePath DisabledRegistryAllUser32Bit => new(Microsoft.Win32.RegistryHive.LocalMachine, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved\\Run32");
}
