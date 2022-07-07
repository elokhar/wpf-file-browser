using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Lab2
{
    class ItemsLogic
    {
        static TreeView treeView = ((MainWindow)Application.Current.MainWindow).treeView;
        public static void CreateFileItem(FileInfo file, TreeViewItem parent)
        {
            var item = new TreeViewItem
            {
                Header = file.Name,
                Tag = parent.Tag + "\\" + file.Name
            };

            var cm = new ContextMenu();

            var del = new MenuItem();
            del.Header = "Delete";
            del.Click += DeleteFile_Clicked;
            cm.Items.Add(del);

            var open = new MenuItem();
            open.Header = "Open";
            open.Click += OpenFile_Clicked;
            cm.Items.Add(open);

            item.ContextMenu = cm;
            parent.Items.Add(item);
        }


        public static void CreateDirItem(DirectoryInfo dir, TreeViewItem parent)
        {
            var item = new TreeViewItem
            {
                Header = dir.Name,
                Tag = parent.Tag + "\\" + dir.Name
            };

            var cm = new ContextMenu();

            var del = new MenuItem();
            del.Header = "Delete";
            del.Click += DeleteDir_Clicked;
            cm.Items.Add(del);

            var create = new MenuItem();
            create.Header = "Create";
            create.Click += CreateDir_Clicked;
            cm.Items.Add(create);

            item.ContextMenu = cm;

            try
            {
                foreach (var e in dir.GetDirectories())
                {
                    CreateDirItem(e, item);
                }

                foreach (var e in dir.GetFiles())
                {
                    CreateFileItem(e, item);
                }

                parent.Items.Add(item);
            }
            catch (UnauthorizedAccessException)
            { }
        }


        public static void DeleteFile_Clicked(object sender, RoutedEventArgs e)
        {
            if (treeView.SelectedItem == null)
            {
                return;
            }
            var selectedTVI = (TreeViewItem)treeView.SelectedItem;
            var path = (string)selectedTVI.Tag;
            var attributes = File.GetAttributes(path);
            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                File.SetAttributes(path, attributes & ~FileAttributes.ReadOnly);
            }
            File.Delete((string)selectedTVI.Tag);
            var parentTVI = (TreeViewItem)selectedTVI.Parent;
            parentTVI.Items.Remove(selectedTVI);
        }


        public static void DeleteDir_Clicked(object sender, RoutedEventArgs e)
        {
            if (treeView.SelectedItem == null)
            {
                return;
            }
            var selectedTVI = (TreeViewItem)treeView.SelectedItem;
            var path = (string)selectedTVI.Tag;
            var attributes = File.GetAttributes(path);
            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                File.SetAttributes(path, attributes & ~FileAttributes.ReadOnly);
            }
            Directory.Delete((string)selectedTVI.Tag, true);
            var parentTVI = (TreeViewItem)selectedTVI.Parent;
            parentTVI.Items.Remove(selectedTVI);
        }


        public static void OpenFile_Clicked(object sender, RoutedEventArgs e)
        {
            if (treeView.SelectedItem == null)
            {
                return;
            }
            var openWnd = new OpenFileWindow();
            var selectedTVI = (TreeViewItem)treeView.SelectedItem;
            var path = (string)selectedTVI.Tag;
            using (StreamReader sr = new StreamReader(path))
            {
                String line = sr.ReadToEnd();
                openWnd.text.Text += line;
            }
            openWnd.Title = (string)selectedTVI.Header;
            openWnd.Show();
        }


        public static void CreateDir_Clicked(object sender, RoutedEventArgs e)
        {
            if (treeView.SelectedItem == null)
            {
                return;
            }
            var createWnd = new CreateDirWindow();
            createWnd.Show();
        }
    }
}
