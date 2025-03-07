﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Исмагилов41
{
    public partial class ProductPage : Page
    {
        // Конструктор для авторизованного пользователя
        public ProductPage(User user)
        {
            InitializeComponent();
            // FIOTB - текстбокс для отображения ФИО
            FIOTB.Text = user.UserSurname + " " + user.UserName + " " + user.UserPatronymic;
            switch (user.UserRole)
            {
                case 1:
                    RoleTB.Text = "Клиент";
                    break;
                case 2:
                    RoleTB.Text = "Менеджер";
                    break;
                case 3:
                    RoleTB.Text = "Администратор";
                    break;
            }
            var currentProducts = Исмагилов41Entities.GetContext().Product.ToList();
            ProductListView.ItemsSource = currentProducts;
        }

        // Конструктор для гостя
        public ProductPage()
        {
            InitializeComponent();
            // При авторизации гостя устанавливаем значения для отображения "гость"
            FIOTB.Text = "гость";
            RoleTB.Text = "гость";
            UpdateProducts();  // Инициализация данных и отображение "30 из 30"
            ComboType.SelectedIndex = 0;
        }

        private void UpdateProducts()
        {
            var currentProducts = Исмагилов41Entities.GetContext().Product.ToList();
            // Поиск
            currentProducts = currentProducts
                .Where(p => p.ProductName.ToLower().Contains(SearchTextBox.Text.ToLower()))
                .ToList();

            // Сортировка
            if (RButtonDown.IsChecked == true)
            {
                currentProducts = currentProducts.OrderByDescending(p => p.ProductCost).ToList();
            }
            else if (RBurronUp.IsChecked == true)
            {
                currentProducts = currentProducts.OrderBy(p => p.ProductCost).ToList();
            }

            // Фильтрация по скидке
            switch (ComboType.SelectedIndex)
            {
                case 1:
                    currentProducts = currentProducts.Where(p => (p.ProductDiscountAmount > 0 && p.ProductDiscountAmount < 10)).ToList();
                    break;
                case 2:
                    currentProducts = currentProducts.Where(p => (p.ProductDiscountAmount >= 10 && p.ProductDiscountAmount < 15)).ToList();
                    break;
                case 3:
                    currentProducts = currentProducts.Where(p => (p.ProductDiscountAmount >= 15)).ToList();
                    break;
            }

            // Обновление источника данных для ListView
            ProductListView.ItemsSource = currentProducts;

            // Обновление текста для отображения количества продуктов
            int totalProducts = Исмагилов41Entities.GetContext().Product.Count();  // Общее количество продуктов
            int filteredProducts = currentProducts.Count();  // Количество отфильтрованных продуктов

            ProductCountTextBlock.Text = $"{filteredProducts} из {totalProducts}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void RButtonDown_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProducts();
        }

        private void RBurronUp_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProducts();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProducts();
        }
    }
}
