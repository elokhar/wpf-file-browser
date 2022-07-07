using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Lab2
{
    public partial class CreateDirWindow : Window
    {
        public CreateDirWindow()
        {
            InitializeComponent();
        }

        private void Cancel_Clicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            var name = nameBox.Text;
            var selectedTVI = (TreeViewItem)((MainWindow)Application.Current.MainWindow).treeView.SelectedItem;
            var parentPath = (string)selectedTVI.Tag;
            var path = parentPath + "\\" + name;
            if ((bool)isDir.IsChecked)
            {
                Directory.CreateDirectory(path);
                SetRahs(path);
                ItemsLogic.CreateDirItem(new DirectoryInfo(path), selectedTVI);
                Close();
            }
            else
            {
                var pattern = new Regex(@"[\w|\d|_~-]{1,8}\.(txt|php|html)");
                var validName = pattern.IsMatch(name);
                if (!validName)
                {
                    var text = "Name may contain letters, digits and/or ~_- characters and should end with an extension (.txt, .php, .html)";
                    var caption = "Invalid filename";
                    MessageBoxButton button = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Warning;
                    MessageBox.Show(text, caption, button, icon);
                }
                else
                {
                    File.Create(path);
                    SetRahs(path);
                    ItemsLogic.CreateFileItem(new FileInfo(path), selectedTVI);
                    Close();
                }
            }                     
        }


        private void SetRahs(string path)
        {
            var attr = FileAttributes.Normal;
            if((bool)readOnly.IsChecked)
            {
                attr = attr | FileAttributes.ReadOnly;
            }
            if ((bool)archive.IsChecked)
            {
                attr = attr | FileAttributes.Archive;
            }
            if ((bool)hidden.IsChecked)
            {
                attr = attr | FileAttributes.Hidden;
            }
            if ((bool)system.IsChecked)
            {
                attr = attr | FileAttributes.System;
            }
            File.SetAttributes(path, attr);
        }

    }
}
