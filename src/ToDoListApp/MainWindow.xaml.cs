using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ToDoListApp.Entities;
using ToDoListApp.Utils;

namespace ToDoListApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<ToDoItem> ToDoItemsStorage { get; set; } = [];
        public ObservableCollection<ToDoItem> ToDoItemsPresenter { get; set; } = [];

        public MainWindow()
        {
            InitializeComponent();

            toDoDataGrid.ItemsSource = ToDoItemsPresenter;

            TraceUtil.LogMethodCall();
        }

        private void Grid_PreviewCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            DataGrid grid = (DataGrid)sender;
            if (e.Command == DataGrid.DeleteCommand)
            {
                if (grid.SelectedItem is not ToDoItem item)
                {
                    return;
                }

                if (MessageBox.Show(String.Format("Would you like to delete '{0}'", (item).Text), "Confirm Delete", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                {
                    e.Handled = true;
                }
                else ToDoItemsStorage.Remove(item);
            }
        }

        #region Event Handlers

        private void PriorityChangedInDataGrid(object sender, SelectionChangedEventArgs e)
        {
            ComboBox? comboBox = sender as ComboBox;
            ToDoItem? selectedItem = toDoDataGrid.SelectedItem as ToDoItem;
            if (selectedItem == null || comboBox == null)
            {
                return;
            }

            if (!Enum.TryParse(comboBox.Text, out Priority selectedPriority))
            {
                return;
            }

            selectedItem.Priority = selectedPriority;
            TraceUtil.LogMethodCall(sender, e);
        }

        private void IsCompletedChangedInDataGrid(object sender, RoutedEventArgs e)
        {
            if (toDoDataGrid.SelectedItem is not ToDoItem selectedItem)
            {
                return;
            }

            if (sender is not CheckBox checkBox)
            {
                return;
            }

            if (checkBox.IsChecked == null)
            {
                return;
            }

            selectedItem.IsCompleted = (bool)checkBox.IsChecked;

            TraceUtil.LogMethodCall(sender, e);
        }

        private void ShowCompletedTasks()
        {
            SaveTasksToStorage();

            foreach (var item in ToDoItemsStorage)
            {
                if (!item.IsCompleted)
                {
                    ToDoItemsPresenter.Remove(item);
                }
            }
        }

        private void SaveTasksToStorage()
        {
            ToDoItemsStorage.Clear();
            foreach (ToDoItem item in ToDoItemsPresenter)
            {
                ToDoItemsStorage.Add(item);
            }
        }

        private void LoadTasksFromStorage()
        {
            if (ToDoItemsStorage.Count == 0)
            {
                return;
            }

            ToDoItemsPresenter.Clear();
            foreach (ToDoItem item in ToDoItemsStorage)
            {
                ToDoItemsPresenter.Add(item);
            }
        }

        #endregion

        #region Button Events

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddTitleTextBox.Text.Length == 0 || !Enum.TryParse(AddPriorityComboBox.Text, out Priority selectedPriority))
            {
                return;
            }

            ToDoItem item = new()
            {
                Text = AddTitleTextBox.Text,
                Priority = selectedPriority
            };
            ToDoItemsPresenter.Add(item);

            AddTitleTextBox.Text = "";
            AddPriorityComboBox.SelectedIndex = 0;
            TraceUtil.LogMethodCall(sender, e);
        }
        private void DeleteAllCompletedButton_Click(object sender, RoutedEventArgs e)
        {
            ToDoItem[] presenterData = new ToDoItem[ToDoItemsPresenter.Count];
            ToDoItemsPresenter.CopyTo(presenterData, 0);

            foreach (var item in presenterData)
            {
                if (item.IsCompleted)
                {
                    if (ToDoItemsStorage.Count > 0)
                    {
                        ToDoItemsStorage.Remove(item);
                    }

                    ToDoItemsPresenter.Remove(item);
                }
            }
            TraceUtil.LogMethodCall(sender, e);
        }

        private void LogButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("-----------------------------------------");
            foreach (var item in ToDoItemsPresenter)
            {
                Console.WriteLine($"{item.Text}, {item.Priority}, {item.IsCompleted}, {item.DateLastEdited}, {item.DateCompleted}");
            }

            TraceUtil.LogMethodCall(sender, e);
        }

        #endregion

        private void FilterByCompleted_Click(object sender, RoutedEventArgs e)
        {
            ShowCompletedTasks();
            TraceUtil.LogMethodCall(sender, e);
        }

        private void RemoveFilters_Click(object sender, RoutedEventArgs e)
        {
            LoadTasksFromStorage();
            TraceUtil.LogMethodCall(sender, e);
        }

    }
}