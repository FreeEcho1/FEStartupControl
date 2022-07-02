namespace FreeEcho.FEStartupControl;

/// <summary>
/// レジストリの最上位ノードとパス
/// </summary>
class RegistryHivePath
{
    /// <summary>
    /// 最上位ノード
    /// </summary>
    public Microsoft.Win32.RegistryHive RegistryHive
    {
        get;
        set;
    } = Microsoft.Win32.RegistryHive.CurrentUser;
    /// <summary>
    /// パス
    /// </summary>
    public string Path
    {
        get;
        set;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public RegistryHivePath()
    {
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="registryHive">最上位ノード</param>
    /// <param name="path">パス</param>
    public RegistryHivePath(
        Microsoft.Win32.RegistryHive registryHive,
        string path
        )
    {
        RegistryHive = registryHive;
        Path = path;
    }
}
