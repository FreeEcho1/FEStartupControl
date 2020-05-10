namespace FEStartupControlTest
{
    public partial class MainWindow : System.Windows.Window
    {
        System.Collections.Generic.List<FreeEcho.FEStartupControl.StartupInformation> StartupInformation;

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                GetStartup();

                ListBoxStartup.SelectionChanged += ListBoxStartup_SelectionChanged;
                ButtonFileSelection.Click += ButtonFileSelection_Click;
                ButtonAddition.Click += ButtonAddition_Click;
                ButtonDelete.Click += ButtonDelete_Click;
                ButtonChangeValidityState.Click += ButtonChangeValidityState_Click;
            }
            catch
            {
            }
        }

        private void ListBoxStartup_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                if (0 == ListBoxStartup.SelectedItems.Count)
                {
                    TextBoxStartupInformation.Text = "";
                }
                else
                {
                    FreeEcho.FEStartupControl.StartupInformation si = StartupInformation[ListBoxStartup.SelectedIndex];

                    TextBoxStartupInformation.Text = "登録名：" + si.RegisterName + "\n";
                    TextBoxStartupInformation.Text += "有効状態：" + si.RegisterState + "\n";
                    TextBoxStartupInformation.Text += "パス：" + si.Path + "\n";
                    TextBoxStartupInformation.Text += "パラメータ：" + si.Parameter + "\n";
                    TextBoxStartupInformation.Text += "登録場所：" + si.RegisterPlace + "\n";
                    TextBoxStartupInformation.Text += "実行時の大きさ：" + si.WindowState + "\n";
                }
            }
            catch
            {
            }
        }

        private void ButtonFileSelection_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();

                if (ofd.ShowDialog() == true)
                {
                    TextBoxPath.Text = ofd.FileName;
                }
            }
            catch
            {
            }
        }

        private void ButtonAddition_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if ((TextBoxRegisterName.Text.Length != 0) && (System.IO.File.Exists(TextBoxPath.Text)))
                {
                    FreeEcho.FEStartupControl.StartupRegisterPlace register_place = FreeEcho.FEStartupControl.StartupRegisterPlace.FolderLogonUser;
                    switch (ComboBoxRegisterPlace.SelectedItem)
                    {
                        case "フォルダ (ログオンユーザー)":
                            register_place = FreeEcho.FEStartupControl.StartupRegisterPlace.FolderLogonUser;
                            break;
                        case "フォルダ (全ユーザー)":
                            register_place = FreeEcho.FEStartupControl.StartupRegisterPlace.FolderAllUser;
                            break;
                        case "レジストリ (ログオンユーザー)":
                            register_place = FreeEcho.FEStartupControl.StartupRegisterPlace.RegistryLogonUser;
                            break;
                        case "レジストリ (ログオンユーザー、一度)":
                            register_place = FreeEcho.FEStartupControl.StartupRegisterPlace.RegistryLogonUserOnce;
                            break;
                        case "レジストリ (全ユーザー)":
                            register_place = FreeEcho.FEStartupControl.StartupRegisterPlace.RegistryAllUser;
                            break;
                        case "レジストリ (全ユーザー、一度)":
                            register_place = FreeEcho.FEStartupControl.StartupRegisterPlace.RegistryAllUserOnce;
                            break;
                        case "レジストリ (全ユーザー、32bit)":
                            register_place = FreeEcho.FEStartupControl.StartupRegisterPlace.RegistryAllUser32Bit;
                            break;
                        case "レジストリ (全ユーザー、一度、32bit)":
                            register_place = FreeEcho.FEStartupControl.StartupRegisterPlace.RegistryAllUserOnce32Bit;
                            break;
                    }
                    FreeEcho.FEStartupControl.WindowState window_state = FreeEcho.FEStartupControl.WindowState.Normal;
                    switch (ComboBoxWindowState.SelectedItem)
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
                    FreeEcho.FEStartupControl.StartupControl.RegisterStartup(register_place, TextBoxRegisterName.Text, TextBoxPath.Text, TextBoxParameter.Text, window_state);
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

        private void ButtonDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (ListBoxStartup.SelectedItems.Count == 1)
                {
                    FreeEcho.FEStartupControl.StartupInformation si = StartupInformation[ListBoxStartup.SelectedIndex];

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

        private void ButtonChangeValidityState_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                FreeEcho.FEStartupControl.StartupControl.ChangeEffectiveState(StartupInformation[ListBoxStartup.SelectedIndex].RegisterPlace, StartupInformation[ListBoxStartup.SelectedIndex].RegisterName, !StartupInformation[ListBoxStartup.SelectedIndex].RegisterState);
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
                ListBoxStartup.Items.Clear();
                foreach (FreeEcho.FEStartupControl.StartupInformation startup in StartupInformation)
                {
                    ListBoxStartup.Items.Add(startup.RegisterName);
                }
            }
            catch
            {
            }
        }
    }
}
