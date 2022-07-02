namespace FreeEcho.FEStartupControl;

/// <summary>
/// スタートアップの登録場所
/// </summary>
public enum StartupRegisterPlace
{
    /// <summary>
    /// startup folder (logon user) (C:\Users\(user name)\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup)
    /// </summary>
    FolderLogonUser = 0,
    /// <summary>
    /// startup folder (all user) (C:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp)
    /// </summary>
    FolderAllUser,
    /// <summary>
    /// registry (logon user) (HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run)
    /// </summary>
    RegistryLogonUser,
    /// <summary>
    /// registry (logon user, once) (HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce)
    /// </summary>
    RegistryLogonUserOnce,
    /// <summary>
    /// registry (all user) (HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run)
    /// </summary>
    RegistryAllUser,
    /// <summary>
    /// registry (all user, once) (HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce)
    /// </summary>
    RegistryAllUserOnce,
    /// <summary>
    /// registry (all user, 32 bit) (64 bit OS only) (HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Run)
    /// </summary>
    RegistryAllUser32Bit,
    /// <summary>
    /// registry (all user, once, 32 bit) (64 bit OS only) (HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\RunOnce)
    /// </summary>
    RegistryAllUserOnce32Bit
}
