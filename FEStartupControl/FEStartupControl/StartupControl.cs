namespace FreeEcho.FEStartupControl;

/// <summary>
/// スタートアップ管理
/// </summary>
public static class StartupControl
{
    /// <summary>
    /// 登録されているスタートアップを取得
    /// </summary>
    /// <returns>スタートアップ情報</returns>
    public static System.Collections.Generic.List<StartupInformation> GetRegisterStartup()
    {
        System.Collections.Generic.List<StartupInformation> startupInformation = new();

        GetFolderStartup(ref startupInformation);
        GetRegistryStartup(ref startupInformation);
        GetCancelStartup(ref startupInformation);

        return (startupInformation);
    }

    /// <summary>
    /// 登録されているスタートアップを取得
    /// </summary>
    /// <param name="startupInformation">スタートアップ情報</param>
    public static void GetRegisterStartup(
        out System.Collections.Generic.List<StartupInformation> startupInformation
        )
    {
        startupInformation = new();

        GetFolderStartup(ref startupInformation);
        GetRegistryStartup(ref startupInformation);
        GetCancelStartup(ref startupInformation);
    }

    /// <summary>
    /// フォルダに登録されているスタートアップを取得
    /// </summary>
    /// <param name="startupInformation">スタートアップ情報</param>
    private static void GetFolderStartup(
        ref System.Collections.Generic.List<StartupInformation> startupInformation
        )
    {
        GetDesignationFolder(ref startupInformation, StartupRegisterPlace.FolderLogonUser, System.Environment.GetFolderPath(System.Environment.SpecialFolder.Startup));
        GetDesignationFolder(ref startupInformation, StartupRegisterPlace.FolderAllUser, System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonStartup));
    }

    /// <summary>
    /// 指定したフォルダに登録されているスタートアップを取得
    /// </summary>
    /// <param name="startupInformation">スタートアップ情報</param>
    /// <param name="startupRegisterPlace">登録情報</param>
    /// <param name="startupFolderPath">パス</param>
    private static void GetDesignationFolder(
        ref System.Collections.Generic.List<StartupInformation> startupInformation,
        StartupRegisterPlace startupRegisterPlace,
        string startupFolderPath
        )
    {
        try
        {
            string[] filePath = System.IO.Directory.GetFiles(startupFolderPath, "*.lnk", System.IO.SearchOption.TopDirectoryOnly);      // ファイルパス

            foreach (string nowFilePath in filePath)
            {
                try
                {
                    IWshRuntimeLibrary.IWshShell_Class shell = new();
                    IWshRuntimeLibrary.IWshShortcut_Class shortcut = (IWshRuntimeLibrary.IWshShortcut_Class)shell.CreateShortcut(nowFilePath);
                    StartupInformation newStartupInformation = new()
                    {
                        Path = shortcut.TargetPath,
                        RegisterName = System.IO.Path.GetFileNameWithoutExtension(shortcut.FullName),
                        Parameter = shortcut.Arguments,
                        WorkingDirectory = shortcut.WorkingDirectory,
                        RegisterPlace = startupRegisterPlace
                    };

                    switch (shortcut.WindowStyle)
                    {
                        case 1:
                            newStartupInformation.WindowState = WindowState.Normal;
                            break;
                        case 3:
                            newStartupInformation.WindowState = WindowState.Maximized;
                            break;
                        case 7:
                            newStartupInformation.WindowState = WindowState.Minimized;
                            break;
                    }
                    startupInformation.Add(newStartupInformation);
                }
                catch
                {
                }
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// レジストリに登録されているスタートアップを取得
    /// </summary>
    /// <param name="startupInformation">スタートアップ情報</param>
    private static void GetRegistryStartup(
        ref System.Collections.Generic.List<StartupInformation> startupInformation
        )
    {
        GetDesignationRegistry(ref startupInformation, StartupRegisterPlace.RegistryLogonUser, StartupRegistryHidePath.LogonUser);
        GetDesignationRegistry(ref startupInformation, StartupRegisterPlace.RegistryAllUser, StartupRegistryHidePath.AllUser);
        GetDesignationRegistry(ref startupInformation, StartupRegisterPlace.RegistryLogonUserOnce, StartupRegistryHidePath.LogonUserOnce);
        GetDesignationRegistry(ref startupInformation, StartupRegisterPlace.RegistryAllUserOnce, StartupRegistryHidePath.AllUserOnce);
        if (System.Environment.Is64BitProcess)
        {
            GetDesignationRegistry(ref startupInformation, StartupRegisterPlace.RegistryAllUser32Bit, StartupRegistryHidePath.AllUser32Bit);
            GetDesignationRegistry(ref startupInformation, StartupRegisterPlace.RegistryAllUserOnce32Bit, StartupRegistryHidePath.AllUserOnce32Bit);
        }
    }

    /// <summary>
    /// 指定したレジストリに登録されているスタートアップを取得
    /// </summary>
    /// <param name="startupInformation">スタートアップ情報</param>
    /// <param name="startupRegisterPlace">登録場所</param>
    /// <param name="registryHivePath">RegistryHivePath</param>
    private static void GetDesignationRegistry(
        ref System.Collections.Generic.List<StartupInformation> startupInformation,
        StartupRegisterPlace startupRegisterPlace,
        RegistryHivePath registryHivePath
        )
    {
        try
        {
            using Microsoft.Win32.RegistryKey baseRegistryKey = Microsoft.Win32.RegistryKey.OpenBaseKey(registryHivePath.RegistryHive, Microsoft.Win32.RegistryView.Default);
            using Microsoft.Win32.RegistryKey subRegistryKey = baseRegistryKey.OpenSubKey(registryHivePath.Path);
            string[] valueName = subRegistryKey.GetValueNames();       // 名前

            for (int count = 0; count < valueName.Length; count++)
            {
                try
                {
                    string keyValue = (string)subRegistryKey.GetValue(valueName[count]);      // 値

                    if (keyValue.Length != 0)
                    {
                        string path;        // パス
                        string parameter;       // パラメータ

                        SeparateFullPathParameter(keyValue, out path, out parameter);
                        StartupInformation newStartupInformation = new()
                        {
                            RegisterName = valueName[count],
                            Path = path,
                            Parameter = parameter,
                            RegisterPlace = startupRegisterPlace
                        };
                        startupInformation.Add(newStartupInformation);
                    }
                }
                catch
                {
                }
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// キャンセルされているスタートアップを取得
    /// </summary>
    /// <param name="startupInformation">スタートアップ情報</param>
    private static void GetCancelStartup(
        ref System.Collections.Generic.List<StartupInformation> startupInformation
        )
    {
        GetDesignationCancelStartup(ref startupInformation, StartupRegisterPlace.FolderLogonUser, StartupRegistryHidePath.DisabledFolderLogonUser);
        GetDesignationCancelStartup(ref startupInformation, StartupRegisterPlace.FolderAllUser, StartupRegistryHidePath.DisabledFolderAllUser);
        GetDesignationCancelStartup(ref startupInformation, StartupRegisterPlace.RegistryLogonUser, StartupRegistryHidePath.DisabledRegistryLogonUser);
        if (System.Environment.Is64BitProcess)
        {
            GetDesignationCancelStartup(ref startupInformation, StartupRegisterPlace.RegistryAllUser, StartupRegistryHidePath.DisabledRegistryAllUser);
            GetDesignationCancelStartup(ref startupInformation, StartupRegisterPlace.RegistryAllUser32Bit, StartupRegistryHidePath.DisabledRegistryAllUser32Bit);
        }
    }

    /// <summary>
    /// 指定した登録場所でキャンセルされているスタートアップを取得
    /// </summary>
    /// <param name="startupInformation">スタートアップ情報</param>
    /// <param name="registerPlace">登録場所</param>
    /// <param name="registryHivePath">RegistryHivePath</param>
    private static void GetDesignationCancelStartup(
        ref System.Collections.Generic.List<StartupInformation> startupInformation,
        StartupRegisterPlace registerPlace,
        RegistryHivePath registryHivePath
        )
    {
        try
        {
            using Microsoft.Win32.RegistryKey baseRegistryKey = Microsoft.Win32.RegistryKey.OpenBaseKey(registryHivePath.RegistryHive, Microsoft.Win32.RegistryView.Default);
            using Microsoft.Win32.RegistryKey subRegistryKey = baseRegistryKey.OpenSubKey(registryHivePath.Path);
            foreach (string valueName in subRegistryKey.GetValueNames())
            {
                try
                {
                    byte[] keyValue = (byte[])subRegistryKey.GetValue(valueName);        // 値

                    if (keyValue.Length != 0)
                    {
                        if ((keyValue[0] % 2) != 0)     // 偶数ならキャンセルされている
                        {
                            foreach (StartupInformation nowStartupInformation in startupInformation)
                            {
                                string extension = "";        // 拡張子

                                switch (nowStartupInformation.RegisterPlace)
                                {
                                    case StartupRegisterPlace.FolderLogonUser:
                                    case StartupRegisterPlace.FolderAllUser:
                                        extension = ".lnk";
                                        break;
                                }
                                if ((valueName.ToLower() == (nowStartupInformation.RegisterName + extension).ToLower()) && (registerPlace == nowStartupInformation.RegisterPlace))
                                {
                                    nowStartupInformation.RegisterState = false;
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// スタートアップに登録
    /// </summary>
    /// <param name="startupRegisterPlace">登録場所</param>
    /// <param name="registerName">登録名</param>
    /// <param name="filePath">ファイルパス</param>
    /// <param name="parameter">パラメータ</param>
    /// <param name="windowState">ウィンドウの状態</param>
    /// <param name="workingDirectory">作業ディレクトリ</param>
    /// <exception cref="FreeEcho.FEStartupControl.RegisterStartupException"></exception>
    public static void RegisterStartup(
        StartupRegisterPlace startupRegisterPlace,
        string registerName,
        string filePath,
        string parameter = null,
        WindowState windowState = WindowState.Normal,
        string workingDirectory = null
        )
    {
        try
        {
            switch (startupRegisterPlace)
            {
                case StartupRegisterPlace.FolderLogonUser:
                case StartupRegisterPlace.FolderAllUser:
                    RegisterFolder(startupRegisterPlace, registerName, filePath, parameter, windowState, workingDirectory);
                    break;
                case StartupRegisterPlace.RegistryLogonUser:
                case StartupRegisterPlace.RegistryAllUser:
                case StartupRegisterPlace.RegistryLogonUserOnce:
                case StartupRegisterPlace.RegistryAllUserOnce:
                case StartupRegisterPlace.RegistryAllUser32Bit:
                case StartupRegisterPlace.RegistryAllUserOnce32Bit:
                    RegisterRegistry(startupRegisterPlace, registerName, filePath, parameter);
                    break;
            }
        }
        catch
        {
            throw new RegisterStartupException();
        }
    }

    /// <summary>
    /// スタートアップに登録
    /// </summary>
    /// <param name="startupInformation">スタートアップ情報</param>
    /// <param name="startupRegisterPlace">登録場所</param>
    /// <param name="registerName">登録名</param>
    /// <param name="filePath">ファイルパス</param>
    /// <param name="parameter">パラメータ</param>
    /// <param name="windowState">ウィンドウの状態</param>
    /// <param name="workingDirectory">作業ディレクトリ</param>
    /// <exception cref="FreeEcho.FEStartupControl.RegisterStartupException"></exception>
    public static void RegisterStartup(
        ref System.Collections.Generic.List<StartupInformation> startupInformation,
        StartupRegisterPlace startupRegisterPlace,
        string registerName,
        string filePath,
        string parameter = null,
        WindowState windowState = WindowState.Normal,
        string workingDirectory = null
        )
    {
        RegisterStartup(startupRegisterPlace, registerName, filePath, parameter, windowState, workingDirectory);
        startupInformation.Add(new StartupInformation { RegisterPlace = startupRegisterPlace, RegisterName = registerName, Path = filePath, Parameter = parameter, WindowState = windowState, WorkingDirectory = workingDirectory });
    }

    /// <summary>
    /// フォルダに登録
    /// </summary>
    /// <param name="startupRegisterPlace">スタートアップの登録場所</param>
    /// <param name="registerName">登録名</param>
    /// <param name="filePath">ファイルパス</param>
    /// <param name="parameter">パラメータ</param>
    /// <param name="windowState">ウィンドウの状態</param>
    /// <param name="workingDirectory">作業ディレクトリ</param>
    private static void RegisterFolder(
        StartupRegisterPlace startupRegisterPlace,
        string registerName,
        string filePath,
        string parameter,
        WindowState windowState,
        string workingDirectory
        )
    {
        string startupFolderPath = null;          // スタートアップフォルダのパス

        switch (startupRegisterPlace)
        {
            case StartupRegisterPlace.FolderLogonUser:
                startupFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Startup);
                break;
            case StartupRegisterPlace.FolderAllUser:
                startupFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonStartup);
                break;
        }
        startupFolderPath += "\\" + registerName + ".lnk";

        IWshRuntimeLibrary.IWshShell_Class shell = new();
        IWshRuntimeLibrary.IWshShortcut_Class shortcut = (IWshRuntimeLibrary.IWshShortcut_Class)shell.CreateShortcut(startupFolderPath);

        shortcut.TargetPath = filePath;
        shortcut.Arguments = parameter;
        switch (windowState)
        {
            case WindowState.Normal:
                shortcut.WindowStyle = 1;
                break;
            case WindowState.Maximized:
                shortcut.WindowStyle = 3;
                break;
            case WindowState.Minimized:
                shortcut.WindowStyle = 7;
                break;
        }
        shortcut.WorkingDirectory = workingDirectory;
        shortcut.Save();
    }

    /// <summary>
    /// レジストリに登録
    /// </summary>
    /// <param name="startupRegisterPlace">スタートアップの登録場所</param>
    /// <param name="registerName">登録名</param>
    /// <param name="filePath">ファイルパス</param>
    /// <param name="parameter">パラメータ</param>
    private static void RegisterRegistry(
        StartupRegisterPlace startupRegisterPlace,
        string registerName,
        string filePath,
        string parameter
        )
    {
        RegistryHivePath registryHivePath = null;

        switch (startupRegisterPlace)
        {
            case StartupRegisterPlace.RegistryLogonUser:
                registryHivePath = StartupRegistryHidePath.LogonUser;
                break;
            case StartupRegisterPlace.RegistryAllUser:
                registryHivePath = StartupRegistryHidePath.AllUser;
                break;
            case StartupRegisterPlace.RegistryLogonUserOnce:
                registryHivePath = StartupRegistryHidePath.LogonUserOnce;
                break;
            case StartupRegisterPlace.RegistryAllUserOnce:
                registryHivePath = StartupRegistryHidePath.AllUserOnce;
                break;
            case StartupRegisterPlace.RegistryAllUser32Bit:
                registryHivePath = StartupRegistryHidePath.AllUser32Bit;
                break;
            case StartupRegisterPlace.RegistryAllUserOnce32Bit:
                registryHivePath = StartupRegistryHidePath.AllUserOnce32Bit;
                break;
        }

        string writeString;     // 書き込む文字列
        if (string.IsNullOrEmpty(parameter))
        {
            writeString = filePath;
        }
        else
        {
            writeString = "\"" + filePath + "\" " + parameter;
        }

        using Microsoft.Win32.RegistryKey baseRegistryKey = Microsoft.Win32.RegistryKey.OpenBaseKey(registryHivePath.RegistryHive, Microsoft.Win32.RegistryView.Default);
        using Microsoft.Win32.RegistryKey subRegistryKey = baseRegistryKey.CreateSubKey(registryHivePath.Path);
        subRegistryKey.SetValue(registerName, writeString, Microsoft.Win32.RegistryValueKind.String);
    }

    /// <summary>
    /// スタートアップを削除
    /// </summary>
    /// <param name="startupRegisterPlace">登録場所</param>
    /// <param name="registerName">登録名</param>
    /// <exception cref="DeleteStartupException"></exception>
    public static void DeleteStartup(
        StartupRegisterPlace startupRegisterPlace,
        string registerName
        )
    {
        try
        {
            string startupFolderPath = null;
            RegistryHivePath registryHivePath = null;

            switch (startupRegisterPlace)
            {
                case StartupRegisterPlace.FolderLogonUser:
                    startupFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Startup);
                    break;
                case StartupRegisterPlace.FolderAllUser:
                    startupFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonStartup);
                    break;
                case StartupRegisterPlace.RegistryLogonUser:
                    registryHivePath = StartupRegistryHidePath.LogonUser;
                    break;
                case StartupRegisterPlace.RegistryAllUser:
                    registryHivePath = StartupRegistryHidePath.AllUser;
                    break;
                case StartupRegisterPlace.RegistryLogonUserOnce:
                    registryHivePath = StartupRegistryHidePath.LogonUserOnce;
                    break;
                case StartupRegisterPlace.RegistryAllUserOnce:
                    registryHivePath = StartupRegistryHidePath.AllUserOnce;
                    break;
                case StartupRegisterPlace.RegistryAllUser32Bit:
                    registryHivePath = StartupRegistryHidePath.AllUser32Bit;
                    break;
                case StartupRegisterPlace.RegistryAllUserOnce32Bit:
                    registryHivePath = StartupRegistryHidePath.AllUserOnce32Bit;
                    break;
            }

            if (startupFolderPath != null)
            {
                startupFolderPath += "/" + registerName + ".lnk";
                System.IO.File.Delete(startupFolderPath);
            }
            else if (registryHivePath != null)
            {
                using Microsoft.Win32.RegistryKey baseRegistryKey = Microsoft.Win32.RegistryKey.OpenBaseKey(registryHivePath.RegistryHive, Microsoft.Win32.RegistryView.Default);
                using Microsoft.Win32.RegistryKey sub_registry_key = baseRegistryKey.CreateSubKey(registryHivePath.Path);
                sub_registry_key.DeleteValue(registerName);
            }
        }
        catch
        {
            throw new DeleteStartupException();
        }
    }

    /// <summary>
    /// スタートアップを削除
    /// </summary>
    /// <param name="startupInformation">スタートアップ情報</param>
    /// <param name="startupRegisterPlace">登録場所</param>
    /// <param name="registerName">登録名</param>
    /// <exception cref="DeleteStartupException"></exception>
    public static void DeleteStartup(
        ref System.Collections.Generic.List<StartupInformation> startupInformation,
        StartupRegisterPlace startupRegisterPlace,
        string registerName
        )
    {
        DeleteStartup(startupRegisterPlace, registerName);
        foreach (StartupInformation nowStartupInformation in startupInformation)
        {
            if ((startupRegisterPlace == nowStartupInformation.RegisterPlace) && (registerName == nowStartupInformation.RegisterName))
            {
                startupInformation.Remove(nowStartupInformation);
                break;
            }
        }
    }

    /// <summary>
    /// スタートアップの有効状態を変更
    /// </summary>
    /// <param name="startupRegisterPlace">スタートアップの登録場所</param>
    /// <param name="registerName">登録名</param>
    /// <param name="effectiveState">有効状態</param>
    /// <exception cref="ChangeEffectiveStateStartupException">ChangeEffectiveStateStartupException</exception>
    public static void ChangeEffectiveState(
        StartupRegisterPlace startupRegisterPlace,
        string registerName,
        bool effectiveState
        )
    {
        try
        {
            RegistryHivePath registryHivePath = null;
            string extension = null;        // 拡張子

            switch (startupRegisterPlace)
            {
                case StartupRegisterPlace.FolderLogonUser:
                    registryHivePath = StartupRegistryHidePath.DisabledFolderLogonUser;
                    extension = ".lnk";
                    break;
                case StartupRegisterPlace.FolderAllUser:
                    registryHivePath = StartupRegistryHidePath.DisabledFolderAllUser;
                    extension = ".lnk";
                    break;
                case StartupRegisterPlace.RegistryLogonUser:
                    registryHivePath = StartupRegistryHidePath.DisabledRegistryLogonUser;
                    break;
                case StartupRegisterPlace.RegistryAllUser:
                    registryHivePath = StartupRegistryHidePath.DisabledRegistryAllUser;
                    break;
                case StartupRegisterPlace.RegistryAllUser32Bit:
                    registryHivePath = StartupRegistryHidePath.DisabledRegistryAllUser32Bit;
                    break;
            }

            using Microsoft.Win32.RegistryKey baseRegistryKey = Microsoft.Win32.RegistryKey.OpenBaseKey(registryHivePath.RegistryHive, Microsoft.Win32.RegistryView.Default);
            using Microsoft.Win32.RegistryKey subRegistryKey = baseRegistryKey.CreateSubKey(registryHivePath.Path);
            subRegistryKey.SetValue(registerName + extension, effectiveState ? new byte[] { 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } : new byte[] { 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, Microsoft.Win32.RegistryValueKind.Binary);
        }
        catch
        {
            throw new ChangeEffectiveStateStartupException();
        }
    }

    /// <summary>
    /// ファイルパスとパラメータを分ける
    /// </summary>
    /// <param name="stringData">文字列</param>
    /// <param name="filePath">ファイルパス</param>
    /// <param name="parameter">パラメータ</param>
    private static void SeparateFullPathParameter(
        string stringData,
        out string filePath,
        out string parameter
        )
    {
        parameter = null;

        try
        {
            int startPosition = 0, endPosition = 0;       // 分ける時の位置

            // ファイルパスを取り出す
            if (stringData[0] == '\"')
            {
                startPosition++;
            }
            for (endPosition = startPosition; endPosition < stringData.Length; endPosition++)
            {
                if (stringData[endPosition] == '\"')
                {
                    endPosition--;
                    break;
                }
                if (endPosition < (stringData.Length - 4))
                {
                    if ((stringData[endPosition] == '.') && (stringData[endPosition + 1] == 'e') && (stringData[endPosition + 2] == 'x') && (stringData[endPosition + 3] == 'e'))
                    {
                        endPosition += 4;
                        break;
                    }
                }
            }
            filePath = stringData.Substring(startPosition, endPosition - startPosition);
            // パラメータを取り出す
            if (stringData[endPosition - 1] == '\"')
            {
                endPosition++;
            }
            endPosition++;
            if (endPosition < stringData.Length)
            {
                for (startPosition = endPosition; startPosition < stringData.Length; startPosition++)
                {
                    if (stringData[startPosition] != ' ')
                    {
                        break;
                    }
                }
                if (startPosition < stringData.Length)
                {
                    parameter = stringData.Substring(startPosition);
                }
            }
        }
        catch
        {
            filePath = stringData;
        }
    }
}
