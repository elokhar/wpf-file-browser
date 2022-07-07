using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;


namespace Lab2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void OpenDirectory(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.FolderBrowserDialog { Description = "Select directory to open"};
            var result = dlg.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            var path = dlg.SelectedPath;

            var rootDir = new DirectoryInfo(path);

            var root = new TreeViewItem
            {
                Header = rootDir.Name,
                Tag = path
            };

            try
            {
                foreach (var dir in rootDir.GetDirectories())
                {
                    ItemsLogic.CreateDirItem(dir, root);
                }

                foreach (var file in rootDir.GetFiles())
                {
                    ItemsLogic.CreateFileItem(file, root);
                }

                var cm = new ContextMenu();

                var del = new MenuItem();
                del.Header = "Delete";
                del.Click += ItemsLogic.DeleteDir_Clicked;
                cm.Items.Add(del);

                var create = new MenuItem();
                create.Header = "Create";
                create.Click += ItemsLogic.CreateDir_Clicked;
                cm.Items.Add(create);

                root.ContextMenu = cm;

                treeView.Items.Clear();
                treeView.Items.Add(root);
            }
            catch (UnauthorizedAccessException)
            { }

        }

       
        public void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
       

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treeView.SelectedItem==null)
            {
                attrTextBlock.Text = "";
                return;
            }
            var text = "rahs";
            var path = (string)((TreeViewItem)treeView.SelectedItem).Tag;
            var attr = File.GetAttributes(path);
            if ((attr & FileAttributes.ReadOnly) == 0)
            {
                text=text.Replace('r', '-');
            }
            if ((attr & FileAttributes.Archive) == 0)
            {
                text=text.Replace('a', '-');
            }
            if ((attr & FileAttributes.System) == 0)
            {
                text=text.Replace('s', '-');
            }
            if ((attr & FileAttributes.Hidden) == 0)
            {
                text=text.Replace('h', '-');
            }
            attrTextBlock.Text = text;
        }

    }
}
