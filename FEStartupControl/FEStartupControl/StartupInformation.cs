namespace FreeEcho.FEStartupControl;

/// <summary>
/// スタートアップ情報
/// </summary>
public class StartupInformation
{
    /// <summary>
    /// 登録場所
    /// </summary>
    public StartupRegisterPlace RegisterPlace
    {
        get;
        set;
    } = StartupRegisterPlace.FolderLogonUser;
    /// <summary>
    /// 登録名
    /// </summary>
    public string RegisterName
    {
        get;
        set;
    } = "";
    /// <summary>
    /// パス
    /// </summary>
    public string Path
    {
        get;
        set;
    } = "";
    /// <summary>
    /// パラメータ
    /// </summary>
    public string Parameter
    {
        get;
        set;
    } = "";
    /// <summary>
    /// ウィンドウの状態
    /// </summary>
    public WindowState WindowState
    {
        get;
        set;
    } = WindowState.Normal;
    /// <summary>
    /// 作業ディレクトリ
    /// </summary>
    public string WorkingDirectory
    {
        get;
        set;
    } = "";
    /// <summary>
    /// 登録状態 (false=無効/true=有効)
    /// </summary>
    public bool RegisterState
    {
        get;
        set;
    } = true;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public StartupInformation()
    {
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="register_place">スタートアップの登録場所</param>
    /// <param name="register_name">登録名</param>
    /// <param name="path">パス</param>
    /// <param name="parameter">パラメータ</param>
    /// <param name="window_state">ウィンドウの状態</param>
    /// <param name="working_directory">作業ディレクトリ</param>
    /// <param name="register_state">登録状態</param>
    public StartupInformation(
        StartupRegisterPlace register_place,
        string register_name,
        string path,
        string parameter,
        WindowState window_state,
        string working_directory,
        bool register_state
        )
    {
        RegisterPlace = register_place;
        RegisterName = register_name;
        Path = path;
        Parameter = parameter;
        WindowState = window_state;
        WorkingDirectory = working_directory;
        RegisterState = register_state;
    }
}
