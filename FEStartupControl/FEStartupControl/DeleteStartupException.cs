namespace FreeEcho.FEStartupControl;

/// <summary>
/// スタートアップの削除に失敗した例外
/// </summary>
public class DeleteStartupException : System.Exception
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public DeleteStartupException()
        : base("Startup delete failed.")
    {
    }
}
