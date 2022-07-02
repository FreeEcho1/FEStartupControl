namespace FreeEcho.FEStartupControl;

/// <summary>
/// スタートアップの有効状態変更に失敗した例外
/// </summary>
public class ChangeEffectiveStateStartupException : System.Exception
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ChangeEffectiveStateStartupException()
        : base("Startup enabled state change failed.")
    {
    }
}
