﻿using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Hoster.Controls
{
    public class MultiSelectDataGrid : DataGrid
    {
        public MultiSelectDataGrid()
        {
            SelectionChanged += DataGridMultiItemSelect_SelectionChanged;
        }

        private void DataGridMultiItemSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItemsList = SelectedItems;
        }

        public IList SelectedItemsList
        {
            get { return (IList)GetValue(SelectedItemsListProperty); }
            set { SetValue(SelectedItemsListProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsListProperty = DependencyProperty.Register("SelectedItemsList", typeof(IList), typeof(MultiSelectDataGrid), new PropertyMetadata(null));
    }
}
