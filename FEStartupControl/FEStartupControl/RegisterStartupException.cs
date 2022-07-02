namespace FreeEcho.FEStartupControl;

/// <summary>
/// スタートアップに登録が失敗した例外
/// </summary>
public class RegisterStartupException : System.Exception
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public RegisterStartupException()
        : base("Startup registration failed.")
    {
    }
}
