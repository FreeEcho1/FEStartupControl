namespace FEStartupControlSample;

public partial class MainWindow : System.Windows.Window
{
    System.Collections.Generic.List<FreeEcho.FEStartupControl.StartupInformation> StartupInformation;

    public MainWindow()
    {
        InitializeComponent();

        try
        {
            GetStartup();

            StartupListBox.SelectionChanged += StartupListBox_SelectionChanged;
            FileSelectionButton.Click += FileSelectionButton_Click;
            AdditionButton.Click += AdditionButton_Click;
            DeleteButton.Click += DeleteButton_Click;
            ChangeValidityStateButton.Click += ChangeValidityStateButton_Click;
        }
        catch
        {
        }
    }

    private void StartupListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        try
        {
            if (StartupListBox.SelectedItems.Count == 0)
            {
                StartupInformationTextBox.Text = "";
            }
            else
            {
                FreeEcho.FEStartupControl.StartupInformation si = StartupInformation[StartupListBox.SelectedIndex];

                StartupInformationTextBox.Text = "登録名：" + si.RegisterName + "\n";
                StartupInformationTextBox.Text += "有効状態：" + si.RegisterState + "\n";
                StartupInformationTextBox.Text += "パス：" + si.Path + "\n";
                StartupInformationTextBox.Text += "パラメータ：" + si.Parameter + "\n";
                StartupInformationTextBox.Text += "登録場所：" + si.RegisterPlace + "\n";
                StartupInformationTextBox.Text += "実行時の大きさ：" + si.WindowState + "\n";
            }
        }
        catch
        {
        }
    }

    private void FileSelectionButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        try
        {
            Microsoft.Win32.OpenFileDialog ofd = new();

            if (ofd.ShowDialog() == true)
            {
                PathTextBox.Text = ofd.FileName;
            }
        }
        catch
        {
        }
    }

    private void AdditionButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        try
        {
            if ((RegisterNameTextBox.Text.Length != 0) && (System.IO.File.Exists(PathTextBox.Text)))
            {
                FreeEcho.FEStartupControl.StartupRegisterPlace registerPlace = FreeEcho.FEStartupControl.StartupRegisterPlace.FolderLogonUser;
                switch (RegisterPlaceComboBox.SelectedItem)
                {
                    case "フォルダ (ログオンユーザー)":
                        registerPlace = FreeEcho.FEStartupControl.StartupRegisterPlace.FolderLogonUser;
                        break;
                    case "フォルダ (全ユーザー)":
                        registerPlace = FreeEcho.FEStartupControl.StartupRegisterPlace.FolderAllUser;
                        break;
                    case "レジストリ (ログオンユーザー)":
                        registerPlace = FreeEcho.FEStartupControl.StartupRegisterPlace.RegistryLogonUser;
                        break;
                    case "レジストリ (ログオンユーザー、一度)":
                        registerPlace = FreeEcho.FEStartupControl.StartupRegisterPlace.RegistryLogonUserOnce;
                        break;
                    case "レジストリ (全ユーザー)":
                        registerPlace = FreeEcho.FEStartupControl.StartupRegisterPlace.RegistryAllUser;
                        break;
                    case "レジストリ (全ユーザー、一度)":
                        registerPlace = FreeEcho.FEStartupControl.StartupRegisterPlace.RegistryAllUserOnce;
                        break;
                    case "レジストリ (全ユーザー、32bit)":
                        registerPlace = FreeEcho.FEStartupControl.StartupRegisterPlace.RegistryAllUser32Bit;
                        break;
                    case "レジストリ (全ユーザー、一度、32bit)":
                        registerPlace = FreeEcho.FEStartupControl.StartupRegisterPlace.RegistryAllUserOnce32Bit;
                        break;
                }
                FreeEcho.FEStartupControl.WindowState window_state = FreeEcho.FEStartupControl.WindowState.Normal;
                switch (WindowStateComboBox.SelectedItem)
                {
                    case "通常のウィンドウ":
                        window_state = FreeEcho.FEStartupControl.WindowState.Normal;
                        break;
                    case "最大化":
                        window_state = FreeEcho.FEStartupControl.WindowState.Maximized;
                        break;
                    case "最小化":
                        window_state = FreeEcho.FEStartupControl.WindowState.Minimized;
                        break;
                }
                // スタートアップに登録
                FreeEcho.FEStartupControl.StartupControl.RegisterStartup(registerPlace, RegisterNameTextBox.Text, PathTextBox.Text, ParameterTextBox.Text, window_state);
                GetStartup();
                System.Windows.MessageBox.Show("追加しました。");
            }
        }
        catch (FreeEcho.FEStartupControl.RegisterStartupException exception)
        {
            System.Windows.MessageBox.Show(exception.Message);
        }
        catch
        {
            System.Windows.MessageBox.Show("エラーが発生しました。");
        }
    }

    private void DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        try
        {
            if (StartupListBox.SelectedItems.Count == 1)
            {
                FreeEcho.FEStartupControl.StartupInformation si = StartupInformation[StartupListBox.SelectedIndex];

                // スタートアップを削除
                FreeEcho.FEStartupControl.StartupControl.DeleteStartup(si.RegisterPlace, si.RegisterName);
                GetStartup();
                System.Windows.MessageBox.Show("削除しました。");
            }
        }
        catch (FreeEcho.FEStartupControl.DeleteStartupException exception)
        {
            System.Windows.MessageBox.Show(exception.Message);
        }
        catch
        {
            System.Windows.MessageBox.Show("エラーが発生しました。");
        }
    }

    private void ChangeValidityStateButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        try
        {
            FreeEcho.FEStartupControl.StartupControl.ChangeEffectiveState(StartupInformation[StartupListBox.SelectedIndex].RegisterPlace, StartupInformation[StartupListBox.SelectedIndex].RegisterName, !StartupInformation[StartupListBox.SelectedIndex].RegisterState);
            GetStartup();
        }
        catch (FreeEcho.FEStartupControl.ChangeEffectiveStateStartupException exception)
        {
            System.Windows.MessageBox.Show(exception.Message);
        }
        catch
        {
            System.Windows.MessageBox.Show("エラーが発生しました。");
        }
    }

    /// <summary>
    /// スタートアップ取得
    /// </summary>
    private void GetStartup()
    {
        try
        {
            StartupInformation = FreeEcho.FEStartupControl.StartupControl.GetRegisterStartup();
            StartupListBox.Items.Clear();
            foreach (FreeEcho.FEStartupControl.StartupInformation startup in StartupInformation)
            {
                StartupListBox.Items.Add(startup.RegisterName);
            }
        }
        catch
        {
        }
    }
}
