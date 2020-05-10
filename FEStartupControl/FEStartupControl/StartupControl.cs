namespace FreeEcho
{
    namespace FEStartupControl
    {
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

        /// <summary>
        /// ウィンドウの状態
        /// </summary>
        public enum WindowState
        {
            /// <summary>
            /// 通常のサイズ
            /// </summary>
            Normal = 0,
            /// <summary>
            /// 最小化
            /// </summary>
            Minimized,
            /// <summary>
            /// 最大化
            /// </summary>
            Maximized
        }

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
            /// <param name="registry_hive">最上位ノード</param>
            /// <param name="path">パス</param>
            public RegistryHivePath(
                Microsoft.Win32.RegistryHive registry_hive,
                string path
                )
            {
                RegistryHive = registry_hive;
                Path = path;
            }
        }

        /// <summary>
        /// スタートアップのレジストリの最上位ノードとパス
        /// </summary>
        class StartupRegistryHidePath
        {
            /// <summary>
            /// logon user
            /// </summary>
            /// <returns>RegistryHivePath</returns>
            public static RegistryHivePath LogonUser()
            {
                return (new RegistryHivePath(Microsoft.Win32.RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\Run"));
            }
            /// <summary>
            /// logon user (once)
            /// </summary>
            /// <returns>RegistryHivePath</returns>
            public static RegistryHivePath LogonUserOnce()
            {
                return (new RegistryHivePath(Microsoft.Win32.RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\RunOnce"));
            }
            /// <summary>
            /// all user
            /// </summary>
            /// <returns>RegistryHivePath</returns>
            public static RegistryHivePath AllUser()
            {
                return (new RegistryHivePath(Microsoft.Win32.RegistryHive.LocalMachine, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run"));
            }
            /// <summary>
            /// all user (once)
            /// </summary>
            /// <returns>RegistryHivePath</returns>
            public static RegistryHivePath AllUserOnce()
            {
                return (new RegistryHivePath(Microsoft.Win32.RegistryHive.LocalMachine, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce"));
            }
            /// <summary>
            /// all user (32 bit)
            /// </summary>
            /// <returns>RegistryHivePath</returns>
            public static RegistryHivePath AllUser32Bit()
            {
                return (new RegistryHivePath(Microsoft.Win32.RegistryHive.LocalMachine, "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Run"));
            }
            /// <summary>
            /// all user (once、32 bit)
            /// </summary>
            /// <returns>RegistryHivePath</returns>
            public static RegistryHivePath AllUserOnce32Bit()
            {
                return (new RegistryHivePath(Microsoft.Win32.RegistryHive.LocalMachine, "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\RunOnce"));
            }

            /// <summary>
            /// logon user folder (無効化されたスタートアップ)
            /// </summary>
            /// <returns>RegistryHivePath</returns>
            public static RegistryHivePath DisabledFolderLogonUser()
            {
                return (new RegistryHivePath(Microsoft.Win32.RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved\\StartupFolder"));
            }
            /// <summary>
            /// all user folder (無効化されたスタートアップ)
            /// </summary>
            /// <returns>RegistryHivePath</returns>
            public static RegistryHivePath DisabledFolderAllUser()
            {
                return (new RegistryHivePath(Microsoft.Win32.RegistryHive.LocalMachine, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved\\StartupFolder"));
            }
            /// <summary>
            /// logon user registry (無効化されたスタートアップ)
            /// </summary>
            /// <returns>RegistryHivePath</returns>
            public static RegistryHivePath DisabledRegistryLogonUser()
            {
                return (new RegistryHivePath(Microsoft.Win32.RegistryHive.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved\\Run"));
            }
            /// <summary>
            /// all user registry (無効化されたスタートアップ)
            /// </summary>
            /// <returns>RegistryHivePath</returns>
            public static RegistryHivePath DisabledRegistryAllUser()
            {
                return (new RegistryHivePath(Microsoft.Win32.RegistryHive.LocalMachine, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved\\Run"));
            }
            /// <summary>
            /// all user registry (32 bit) (無効化されたスタートアップ)
            /// </summary>
            /// <returns>RegistryHivePath</returns>
            public static RegistryHivePath DisabledRegistryAllUser32Bit()
            {
                return (new RegistryHivePath(Microsoft.Win32.RegistryHive.LocalMachine, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved\\Run32"));
            }
        }

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
                System.Collections.Generic.List<StartupInformation> startup_information = new System.Collections.Generic.List<StartupInformation>();

                GetFolderStartup(ref startup_information);
                GetRegistryStartup(ref startup_information);
                GetCancelStartup(ref startup_information);

                return (startup_information);
            }

            /// <summary>
            /// 登録されているスタートアップを取得
            /// </summary>
            /// <param name="startup_information">スタートアップ情報</param>
            public static void GetRegisterStartup(
                out System.Collections.Generic.List<StartupInformation> startup_information
                )
            {
                startup_information = new System.Collections.Generic.List<StartupInformation>();

                GetFolderStartup(ref startup_information);
                GetRegistryStartup(ref startup_information);
                GetCancelStartup(ref startup_information);
            }

            /// <summary>
            /// フォルダに登録されているスタートアップを取得
            /// </summary>
            /// <param name="startup_information">スタートアップ情報</param>
            private static void GetFolderStartup(
                ref System.Collections.Generic.List<StartupInformation> startup_information
                )
            {
                GetDesignationFolder(ref startup_information, StartupRegisterPlace.FolderLogonUser, System.Environment.GetFolderPath(System.Environment.SpecialFolder.Startup));
                GetDesignationFolder(ref startup_information, StartupRegisterPlace.FolderAllUser, System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonStartup));
            }

            /// <summary>
            /// 指定したフォルダに登録されているスタートアップを取得
            /// </summary>
            /// <param name="startup_information">スタートアップ情報</param>
            /// <param name="startup_register_place">登録情報</param>
            /// <param name="startup_folder_path">パス</param>
            private static void GetDesignationFolder(
                ref System.Collections.Generic.List<StartupInformation> startup_information,
                StartupRegisterPlace startup_register_place,
                string startup_folder_path
                )
            {
                try
                {
                    string[] file_path = System.IO.Directory.GetFiles(startup_folder_path, "*.lnk", System.IO.SearchOption.TopDirectoryOnly);      // ファイルパス

                    foreach (string now_file_path in file_path)
                    {
                        try
                        {
                            IWshRuntimeLibrary.IWshShell_Class shell = new IWshRuntimeLibrary.IWshShell_Class();
                            IWshRuntimeLibrary.IWshShortcut_Class shortcut = (IWshRuntimeLibrary.IWshShortcut_Class)shell.CreateShortcut(now_file_path);
                            StartupInformation new_startup_information = new StartupInformation
                            {
                                Path = shortcut.TargetPath,
                                RegisterName = System.IO.Path.GetFileNameWithoutExtension(shortcut.FullName),
                                Parameter = shortcut.Arguments,
                                WorkingDirectory = shortcut.WorkingDirectory,
                                RegisterPlace = startup_register_place
                            };

                            switch (shortcut.WindowStyle)
                            {
                                case 1:
                                    new_startup_information.WindowState = WindowState.Normal;
                                    break;
                                case 3:
                                    new_startup_information.WindowState = WindowState.Maximized;
                                    break;
                                case 7:
                                    new_startup_information.WindowState = WindowState.Minimized;
                                    break;
                            }
                            startup_information.Add(new_startup_information);
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
            /// <param name="startup_information">スタートアップ情報</param>
            private static void GetRegistryStartup(
                ref System.Collections.Generic.List<StartupInformation> startup_information
                )
            {
                GetDesignationRegistry(ref startup_information, StartupRegisterPlace.RegistryLogonUser, StartupRegistryHidePath.LogonUser());
                GetDesignationRegistry(ref startup_information, StartupRegisterPlace.RegistryAllUser, StartupRegistryHidePath.AllUser());
                GetDesignationRegistry(ref startup_information, StartupRegisterPlace.RegistryLogonUserOnce, StartupRegistryHidePath.LogonUserOnce());
                GetDesignationRegistry(ref startup_information, StartupRegisterPlace.RegistryAllUserOnce, StartupRegistryHidePath.AllUserOnce());
                if (System.Environment.Is64BitProcess)
                {
                    GetDesignationRegistry(ref startup_information, StartupRegisterPlace.RegistryAllUser32Bit, StartupRegistryHidePath.AllUser32Bit());
                    GetDesignationRegistry(ref startup_information, StartupRegisterPlace.RegistryAllUserOnce32Bit, StartupRegistryHidePath.AllUserOnce32Bit());
                }
            }

            /// <summary>
            /// 指定したレジストリに登録されているスタートアップを取得
            /// </summary>
            /// <param name="startup_information">スタートアップ情報</param>
            /// <param name="startup_register_place">登録場所</param>
            /// <param name="registry_hive_path">RegistryHivePath</param>
            private static void GetDesignationRegistry(
                ref System.Collections.Generic.List<StartupInformation> startup_information,
                StartupRegisterPlace startup_register_place,
                RegistryHivePath registry_hive_path
                )
            {
                try
                {
                    using (Microsoft.Win32.RegistryKey base_registry_key = Microsoft.Win32.RegistryKey.OpenBaseKey(registry_hive_path.RegistryHive, Microsoft.Win32.RegistryView.Default))
                    {
                        using (Microsoft.Win32.RegistryKey sub_registry_key = base_registry_key.OpenSubKey(registry_hive_path.Path))
                        {
                            string[] value_name = sub_registry_key.GetValueNames();       // 名前

                            for (int count = 0; count < value_name.Length; count++)
                            {
                                try
                                {
                                    string key_value = (string)sub_registry_key.GetValue(value_name[count]);      // 値

                                    if (key_value.Length != 0)
                                    {
                                        string path;        // パス
                                        string parameter;       // パラメータ

                                        SeparateFullPathParameter(key_value, out path, out parameter);
                                        StartupInformation new_startup_information = new StartupInformation
                                        {
                                            RegisterName = value_name[count],
                                            Path = path,
                                            Parameter = parameter,
                                            RegisterPlace = startup_register_place
                                        };
                                        startup_information.Add(new_startup_information);
                                    }
                                }
                                catch
                                {
                                }
                            }
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
            /// <param name="startup_information">スタートアップ情報</param>
            private static void GetCancelStartup(
                ref System.Collections.Generic.List<StartupInformation> startup_information
                )
            {
                GetDesignationCancelStartup(ref startup_information, StartupRegisterPlace.FolderLogonUser, StartupRegistryHidePath.DisabledFolderLogonUser());
                GetDesignationCancelStartup(ref startup_information, StartupRegisterPlace.FolderAllUser, StartupRegistryHidePath.DisabledFolderAllUser());
                GetDesignationCancelStartup(ref startup_information, StartupRegisterPlace.RegistryLogonUser, StartupRegistryHidePath.DisabledRegistryLogonUser());
                if (System.Environment.Is64BitProcess)
                {
                    GetDesignationCancelStartup(ref startup_information, StartupRegisterPlace.RegistryAllUser, StartupRegistryHidePath.DisabledRegistryAllUser());
                    GetDesignationCancelStartup(ref startup_information, StartupRegisterPlace.RegistryAllUser32Bit, StartupRegistryHidePath.DisabledRegistryAllUser32Bit());
                }
            }

            /// <summary>
            /// 指定した登録場所でキャンセルされているスタートアップを取得
            /// </summary>
            /// <param name="startup_information">スタートアップ情報</param>
            /// <param name="register_place">登録場所</param>
            /// <param name="registry_hive_path">RegistryHivePath</param>
            private static void GetDesignationCancelStartup(
                ref System.Collections.Generic.List<StartupInformation> startup_information,
                StartupRegisterPlace register_place,
                RegistryHivePath registry_hive_path
                )
            {
                try
                {
                    using (Microsoft.Win32.RegistryKey base_registry_key = Microsoft.Win32.RegistryKey.OpenBaseKey(registry_hive_path.RegistryHive, Microsoft.Win32.RegistryView.Default))
                    {
                        using (Microsoft.Win32.RegistryKey sub_registry_key = base_registry_key.OpenSubKey(registry_hive_path.Path))
                        {
                            foreach (string value_name in sub_registry_key.GetValueNames())
                            {
                                try
                                {
                                    byte[] key_value = (byte[])sub_registry_key.GetValue(value_name);        // 値

                                    if (key_value.Length != 0)
                                    {
                                        if ((key_value[0] % 2) != 0)
                                        {
                                            foreach (StartupInformation si in startup_information)
                                            {
                                                string extension = "";        // 拡張子

                                                switch (si.RegisterPlace)
                                                {
                                                    case StartupRegisterPlace.FolderLogonUser:
                                                    case StartupRegisterPlace.FolderAllUser:
                                                        extension = ".lnk";
                                                        break;
                                                }
                                                if ((value_name.ToLower() == (si.RegisterName + extension).ToLower()) && (register_place == si.RegisterPlace))
                                                {
                                                    si.RegisterState = false;
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
                    }
                }
                catch
                {
                }
            }

            /// <summary>
            /// スタートアップに登録
            /// </summary>
            /// <param name="startup_register_place">登録場所</param>
            /// <param name="register_name">登録名</param>
            /// <param name="file_path">ファイルパス</param>
            /// <param name="parameter">パラメータ</param>
            /// <param name="window_state">ウィンドウの状態</param>
            /// <param name="working_directory">作業ディレクトリ</param>
            /// <exception cref="FreeEcho.FEStartupControl.RegisterStartupException"></exception>
            public static void RegisterStartup(
                StartupRegisterPlace startup_register_place,
                string register_name,
                string file_path,
                string parameter = null,
                WindowState window_state = WindowState.Normal,
                string working_directory = null
                )
            {
                try
                {
                    switch (startup_register_place)
                    {
                        case StartupRegisterPlace.FolderLogonUser:
                        case StartupRegisterPlace.FolderAllUser:
                            RegisterFolder(startup_register_place, register_name, file_path, parameter, window_state, working_directory);
                            break;
                        case StartupRegisterPlace.RegistryLogonUser:
                        case StartupRegisterPlace.RegistryAllUser:
                        case StartupRegisterPlace.RegistryLogonUserOnce:
                        case StartupRegisterPlace.RegistryAllUserOnce:
                        case StartupRegisterPlace.RegistryAllUser32Bit:
                        case StartupRegisterPlace.RegistryAllUserOnce32Bit:
                            RegisterRegistry(startup_register_place, register_name, file_path, parameter);
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
            /// <param name="startup_information">スタートアップ情報</param>
            /// <param name="startup_register_place">登録場所</param>
            /// <param name="register_name">登録名</param>
            /// <param name="file_path">ファイルパス</param>
            /// <param name="parameter">パラメータ</param>
            /// <param name="window_state">ウィンドウの状態</param>
            /// <param name="working_directory">作業ディレクトリ</param>
            /// <exception cref="FreeEcho.FEStartupControl.RegisterStartupException"></exception>
            public static void RegisterStartup(
                ref System.Collections.Generic.List<StartupInformation> startup_information,
                StartupRegisterPlace startup_register_place,
                string register_name,
                string file_path,
                string parameter = null,
                WindowState window_state = WindowState.Normal,
                string working_directory = null
                )
            {
                RegisterStartup(startup_register_place, register_name, file_path, parameter, window_state, working_directory);
                startup_information.Add(new StartupInformation { RegisterPlace = startup_register_place, RegisterName = register_name, Path = file_path, Parameter = parameter, WindowState = window_state, WorkingDirectory = working_directory });
            }

            /// <summary>
            /// フォルダに登録
            /// </summary>
            /// <param name="startup_register_place">スタートアップの登録場所</param>
            /// <param name="register_name">登録名</param>
            /// <param name="file_path">ファイルパス</param>
            /// <param name="parameter">パラメータ</param>
            /// <param name="window_state">ウィンドウの状態</param>
            /// <param name="working_directory">作業ディレクトリ</param>
            private static void RegisterFolder(
                StartupRegisterPlace startup_register_place,
                string register_name,
                string file_path,
                string parameter,
                WindowState window_state,
                string working_directory
                )
            {
                string startup_folder_path = null;          // スタートアップフォルダのパス

                switch (startup_register_place)
                {
                    case StartupRegisterPlace.FolderLogonUser:
                        startup_folder_path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Startup);
                        break;
                    case StartupRegisterPlace.FolderAllUser:
                        startup_folder_path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonStartup);
                        break;
                }
                startup_folder_path += "\\" + register_name + ".lnk";

                IWshRuntimeLibrary.IWshShell_Class shell = new IWshRuntimeLibrary.IWshShell_Class();
                IWshRuntimeLibrary.IWshShortcut_Class shortcut = (IWshRuntimeLibrary.IWshShortcut_Class)shell.CreateShortcut(startup_folder_path);

                shortcut.TargetPath = file_path;
                shortcut.Arguments = parameter;
                switch (window_state)
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
                shortcut.WorkingDirectory = working_directory;
                shortcut.Save();
            }

            /// <summary>
            /// レジストリに登録
            /// </summary>
            /// <param name="startup_register_place">スタートアップの登録場所</param>
            /// <param name="register_name">登録名</param>
            /// <param name="file_path">ファイルパス</param>
            /// <param name="parameter">パラメータ</param>
            private static void RegisterRegistry(
                StartupRegisterPlace startup_register_place,
                string register_name,
                string file_path,
                string parameter
                )
            {
                RegistryHivePath registry_hive_path = null;

                switch (startup_register_place)
                {
                    case StartupRegisterPlace.RegistryLogonUser:
                        registry_hive_path = StartupRegistryHidePath.LogonUser();
                        break;
                    case StartupRegisterPlace.RegistryAllUser:
                        registry_hive_path = StartupRegistryHidePath.AllUser();
                        break;
                    case StartupRegisterPlace.RegistryLogonUserOnce:
                        registry_hive_path = StartupRegistryHidePath.LogonUserOnce();
                        break;
                    case StartupRegisterPlace.RegistryAllUserOnce:
                        registry_hive_path = StartupRegistryHidePath.AllUserOnce();
                        break;
                    case StartupRegisterPlace.RegistryAllUser32Bit:
                        registry_hive_path = StartupRegistryHidePath.AllUser32Bit();
                        break;
                    case StartupRegisterPlace.RegistryAllUserOnce32Bit:
                        registry_hive_path = StartupRegistryHidePath.AllUserOnce32Bit();
                        break;
                }

                string write_string;     // 書き込む文字列
                if (string.IsNullOrEmpty(parameter))
                {
                    write_string = file_path;
                }
                else
                {
                    write_string = "\"" + file_path + "\" " + parameter;
                }

                using (Microsoft.Win32.RegistryKey base_registry_key = Microsoft.Win32.RegistryKey.OpenBaseKey(registry_hive_path.RegistryHive, Microsoft.Win32.RegistryView.Default))
                {
                    using (Microsoft.Win32.RegistryKey sub_registry_key = base_registry_key.CreateSubKey(registry_hive_path.Path))
                    {
                        sub_registry_key.SetValue(register_name, write_string, Microsoft.Win32.RegistryValueKind.String);
                    }
                }
            }

            /// <summary>
            /// スタートアップを削除
            /// </summary>
            /// <param name="startup_register_place">登録場所</param>
            /// <param name="register_name">登録名</param>
            /// <exception cref="FreeEcho.FEStartupControl.DeleteStartupException"></exception>
            public static void DeleteStartup(
                StartupRegisterPlace startup_register_place,
                string register_name
                )
            {
                try
                {
                    string startup_folder_path = null;
                    RegistryHivePath registry_hive_path = null;

                    switch (startup_register_place)
                    {
                        case StartupRegisterPlace.FolderLogonUser:
                            startup_folder_path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Startup);
                            break;
                        case StartupRegisterPlace.FolderAllUser:
                            startup_folder_path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonStartup);
                            break;
                        case StartupRegisterPlace.RegistryLogonUser:
                            registry_hive_path = StartupRegistryHidePath.LogonUser();
                            break;
                        case StartupRegisterPlace.RegistryAllUser:
                            registry_hive_path = StartupRegistryHidePath.AllUser();
                            break;
                        case StartupRegisterPlace.RegistryLogonUserOnce:
                            registry_hive_path = StartupRegistryHidePath.LogonUserOnce();
                            break;
                        case StartupRegisterPlace.RegistryAllUserOnce:
                            registry_hive_path = StartupRegistryHidePath.AllUserOnce();
                            break;
                        case StartupRegisterPlace.RegistryAllUser32Bit:
                            registry_hive_path = StartupRegistryHidePath.AllUser32Bit();
                            break;
                        case StartupRegisterPlace.RegistryAllUserOnce32Bit:
                            registry_hive_path = StartupRegistryHidePath.AllUserOnce32Bit();
                            break;
                    }

                    if (startup_folder_path != null)
                    {
                        startup_folder_path += "/" + register_name + ".lnk";
                        System.IO.File.Delete(startup_folder_path);
                    }
                    else if (registry_hive_path != null)
                    {
                        using (Microsoft.Win32.RegistryKey base_registry_key = Microsoft.Win32.RegistryKey.OpenBaseKey(registry_hive_path.RegistryHive, Microsoft.Win32.RegistryView.Default))
                        {
                            using (Microsoft.Win32.RegistryKey sub_registry_key = base_registry_key.CreateSubKey(registry_hive_path.Path))
                            {
                                sub_registry_key.DeleteValue(register_name);
                            }
                        }
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
            /// <param name="startup_information">スタートアップ情報</param>
            /// <param name="startup_register_place">登録場所</param>
            /// <param name="register_name">登録名</param>
            /// <exception cref="FreeEcho.FEStartupControl.DeleteStartupException"></exception>
            public static void DeleteStartup(
                ref System.Collections.Generic.List<StartupInformation> startup_information,
                StartupRegisterPlace startup_register_place,
                string register_name
                )
            {
                DeleteStartup(startup_register_place, register_name);
                foreach (StartupInformation si in startup_information)
                {
                    if ((startup_register_place == si.RegisterPlace) && (register_name == si.RegisterName))
                    {
                        startup_information.Remove(si);
                        break;
                    }
                }
            }

            /// <summary>
            /// スタートアップの有効状態を変更
            /// </summary>
            /// <param name="startup_register_place">スタートアップの登録場所</param>
            /// <param name="register_name">登録名</param>
            /// <param name="effective_state">有効状態</param>
            /// <exception cref="FreeEcho.FEStartupControl.ChangeEffectiveStateStartupException"></exception>
            public static void ChangeEffectiveState(
                StartupRegisterPlace startup_register_place,
                string register_name,
                bool effective_state
                )
            {
                try
                {
                    RegistryHivePath registry_hive_path = null;
                    string extension = null;        // 拡張子

                    switch (startup_register_place)
                    {
                        case StartupRegisterPlace.FolderLogonUser:
                            registry_hive_path = StartupRegistryHidePath.DisabledFolderLogonUser();
                            extension = ".lnk";
                            break;
                        case StartupRegisterPlace.FolderAllUser:
                            registry_hive_path = StartupRegistryHidePath.DisabledFolderAllUser();
                            extension = ".lnk";
                            break;
                        case StartupRegisterPlace.RegistryLogonUser:
                            registry_hive_path = StartupRegistryHidePath.DisabledRegistryLogonUser();
                            break;
                        case StartupRegisterPlace.RegistryAllUser:
                            registry_hive_path = StartupRegistryHidePath.DisabledRegistryAllUser();
                            break;
                        case StartupRegisterPlace.RegistryAllUser32Bit:
                            registry_hive_path = StartupRegistryHidePath.DisabledRegistryAllUser32Bit();
                            break;
                    }

                    using (Microsoft.Win32.RegistryKey base_registry_key = Microsoft.Win32.RegistryKey.OpenBaseKey(registry_hive_path.RegistryHive, Microsoft.Win32.RegistryView.Default))
                    {
                        using (Microsoft.Win32.RegistryKey sub_registry_key = base_registry_key.CreateSubKey(registry_hive_path.Path))
                        {
                            sub_registry_key.SetValue(register_name + extension, effective_state ? new byte[] { 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } : new byte[] { 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, Microsoft.Win32.RegistryValueKind.Binary);
                        }
                    }
                }
                catch
                {
                    throw new ChangeEffectiveStateStartupException();
                }
            }

            /// <summary>
            /// ファイルパスとパラメータを分ける
            /// </summary>
            /// <param name="string_data">文字列</param>
            /// <param name="file_path">ファイルパス</param>
            /// <param name="parameter">パラメータ</param>
            private static void SeparateFullPathParameter(
                string string_data,
                out string file_path,
                out string parameter
                )
            {
                file_path = null;
                parameter = null;

                try
                {
                    int start_position = 0, end_position = 0;       // 分ける時の位置

                    // ファイルパスを取り出す
                    if (string_data[0] == '\"')
                    {
                        start_position++;
                    }
                    for (end_position = start_position; end_position < string_data.Length; end_position++)
                    {
                        if (string_data[end_position] == '\"')
                        {
                            end_position--;
                            break;
                        }
                        if (end_position < (string_data.Length - 4))
                        {
                            if ((string_data[end_position] == '.') && (string_data[end_position + 1] == 'e') && (string_data[end_position + 2] == 'x') && (string_data[end_position + 3] == 'e'))
                            {
                                end_position += 4;
                                break;
                            }
                        }
                    }
                    file_path = string_data.Substring(start_position, end_position - start_position);
                    // パラメータを取り出す
                    if (string_data[end_position - 1] == '\"')
                    {
                        end_position++;
                    }
                    end_position++;
                    if (end_position < string_data.Length)
                    {
                        for (start_position = end_position; start_position < string_data.Length; start_position++)
                        {
                            if (string_data[start_position] != ' ')
                            {
                                break;
                            }
                        }
                        if (start_position < string_data.Length)
                        {
                            parameter = string_data.Substring(start_position);
                        }
                    }
                }
                catch
                {
                    file_path = string_data;
                }
            }
        }
    }
}
