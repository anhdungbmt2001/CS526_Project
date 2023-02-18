﻿using CS526_Project.Model;
using Microsoft.Maui.Graphics.Text;
using System.Collections.Generic;
using System.Text.Json;

namespace CS526_Project;

public partial class AddTaskPage : ContentPage
{
    List<DatePicker> listNotiDatePickers = new List<DatePicker>();
    List<TimePicker> listNotiTimePickers = new List<TimePicker>();
    List<object> listBtnRemoveNoti = new List<object>();

    List<HorizontalStackLayout> listCategoryEntryWrapper = new List<HorizontalStackLayout>();
    List<string> listCategoriesName = new List<string>();
    List<object> listBtnRemoveCategory = new List<object>();

    public AddTaskPage()
    {
        foreach (var category in App.Database.GetAllCategories())
        {
            listCategoriesName.Add(category.Name);
        }
        listCategoriesName.Add(App.Setting.IsVietnamese ? "Thêm nhãn" : "Add tag");

        InitializeComponent();

        create_btnAddNotiEntry();
        create_btnAddCategory();
        if (!App.Setting.IsVietnamese)
        {
            labelNewTask.Text = "NEW TASK";
            labelTaskName.Text = "TASK NAME";
            txtName.Placeholder = "Task Name";
            labelDeadline.Text = "DEADLINE";
            labelNoDeadLine.Text = "NO DEADLINE";
            labelDay.Text = "DAY";
            labelHour.Text = "HOUR";
            labelRemind.Text = "REMIND";
            labelLabel.Text = "LABEL";
            labelDescription.Text = "DESCRIPTION";
            txtDescription.Placeholder = "Description";
            btnAddTask.Text = "COMPLETED";
        }
    }

    private void create_NotiControls()
    {
        create_NotiTime_entry();
        create_btnAddNotiEntry();
    }
    private void create_NotiTime_entry()
    {
        var labelDate = new Label()
        {
            Style = (Style)this.Resources["DateTimeLabelStyle"],
            Text = App.Setting.IsVietnamese ? "NGÀY" : "DAY"
        };
        var dateNoti = new DatePicker()
        {
            Format = "d",
            Margin = new Thickness(0, 0, 30, 0)
        };
        var labelTime = new Label()
        {
            Style = (Style)this.Resources["DateTimeLabelStyle"],
            Text = App.Setting.IsVietnamese ? "GIỜ" : "HOUR"
        };
        var timeNoti = new TimePicker()
        {
            Format = "t",
            Margin = new Thickness(0, 0, 30, 0)
        };
        var btnRemoveNotiTime = new ImageButton() { Style = (Style)this.Resources["RemoveButtonStyle"] };
        btnRemoveNotiTime.Clicked += btnRemoveNotiTime_Clicked;

        var NotiTimeEntryWrapper = new HorizontalStackLayout();
        NotiTimeEntryWrapper.Children.Add(labelDate);
        NotiTimeEntryWrapper.Children.Add(dateNoti);
        NotiTimeEntryWrapper.Children.Add(labelTime);
        NotiTimeEntryWrapper.Children.Add(timeNoti);
        NotiTimeEntryWrapper.Children.Add(btnRemoveNotiTime);

        NotiTimeWrapper.Children.Add(NotiTimeEntryWrapper);

        listNotiDatePickers.Add(dateNoti);
        listNotiTimePickers.Add(timeNoti);
        listBtnRemoveNoti.Add(btnRemoveNotiTime);
    }
    private void create_btnAddNotiEntry()
    {
        var btnAddNotiTime = new ImageButton() { Style = (Style)this.Resources["AddButtonStyle"] };
        btnAddNotiTime.Clicked += btnAddNotiTime_Clicked;
        NotiTimeWrapper.Children.Add(btnAddNotiTime);
    }
    private void remove_NotiTimeEntry(int i)
    {
        listNotiDatePickers.RemoveAt(i);
        listNotiTimePickers.RemoveAt(i);
        listBtnRemoveNoti.RemoveAt(i);

        NotiTimeWrapper.Children.RemoveAt(i);
    }

    private void create_CategoryControls()
    {
        create_Category_entry();
        create_btnAddCategory();
    }
    private void create_Category_entry()
    {
        var CategoryPicker = new Picker
        {
            ItemsSource = listCategoriesName,
            Title = "Select a category",
            Margin = new Thickness(0, 0, 30, 0)
        };
        CategoryPicker.SelectedIndexChanged += OnSelectedIndexChanged;
        var btnRemoveCategory = new ImageButton() { Style = (Style)this.Resources["RemoveButtonStyle"] };
        btnRemoveCategory.Clicked += btnRemoveCategory_Clicked;

        var CategoryEntryWrapper = new HorizontalStackLayout();
        CategoryEntryWrapper.Children.Add(CategoryPicker);
        CategoryEntryWrapper.Children.Add(btnRemoveCategory);

        CategoriesWrapper.Children.Add(CategoryEntryWrapper);

        listCategoryEntryWrapper.Add(CategoryEntryWrapper);
        listBtnRemoveCategory.Add(btnRemoveCategory);
    }
    private void create_btnAddCategory()
    {
        var btnAddCategory = new ImageButton() { Style = (Style)this.Resources["AddButtonStyle"] };
        btnAddCategory.Clicked += btnAddCategory_Clicked;
        CategoriesWrapper.Children.Add(btnAddCategory);
    }
    private void remove_CategoryEntry(int i)
    {
        listCategoryEntryWrapper.RemoveAt(i);
        listBtnRemoveCategory.RemoveAt(i);

        CategoriesWrapper.Children.RemoveAt(i);
    }

    public void OnAddCategoryPageReturn(string category_name, Color category_color, int caller_IndexInWraper)
    {
        listCategoriesName[listCategoriesName.Count - 1] = category_name;
        listCategoriesName.Add(App.Setting.IsVietnamese ? "Thêm nhãn" : "Add Tag");

        for (int i = 0; i < listCategoryEntryWrapper.Count; i++)
        {
            if (i == caller_IndexInWraper)
            {
                listCategoryEntryWrapper[i].Children[0] = new Picker()
                {
                    ItemsSource = listCategoriesName,
                    Title = "Select a category",
                    Margin = new Thickness(0, 0, 30, 0),
                    SelectedIndex = listCategoriesName.Count - 2,
                    TextColor = category_color
                };
            }
            else
            {
                var picker = listCategoryEntryWrapper[i].Children[0] as Picker;
                int picker_selectedIndex = picker.SelectedIndex;
                Color picker_txtcolor = picker.TextColor;

                listCategoryEntryWrapper[i].Children[0] = new Picker()
                {
                    ItemsSource = listCategoriesName,
                    Title = "Select a category",
                    Margin = new Thickness(0, 0, 30, 0),
                    SelectedIndex = picker_selectedIndex,
                    TextColor = picker_txtcolor
                };
            }
        }
    }

    private void btnAddNotiTime_Clicked(object sender, EventArgs e)
    {
        NotiTimeWrapper.Children.RemoveAt(NotiTimeWrapper.Children.Count - 1);

        create_NotiControls();
    }

    private void btnRemoveNotiTime_Clicked(object sender, EventArgs e)
    {
        for (int i = 0; i < listBtnRemoveNoti.Count; i++)
        {
            if (listBtnRemoveNoti[i] == sender)
            {
                remove_NotiTimeEntry(i);
                return;
            }
        }
    }

    private void btnAddCategory_Clicked(object sender, EventArgs e)
    {
        CategoriesWrapper.Children.RemoveAt(CategoriesWrapper.Children.Count - 1);

        create_CategoryControls();
    }

    private void btnRemoveCategory_Clicked(object sender, EventArgs e)
    {
        for (int i = 0; i < listBtnRemoveCategory.Count; i++)
        {
            if (listBtnRemoveCategory[i] == sender)
            {
                remove_CategoryEntry(i);
                return;
            }
        }
    }

    private async void OnSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;

        if (picker.SelectedIndex == -1) return;
        if (picker.SelectedIndex == listCategoriesName.Count - 1)
        {
            var picker_index = -1;
            for (int i = 0; i < listCategoryEntryWrapper.Count; i++)
            {
                if (listCategoryEntryWrapper[i].Children[0] == picker)
                {
                    picker_index = i;
                    break;
                }
            }

            picker.SelectedIndex = -1;
            await Navigation.PushAsync(new AddCategoryPage(this, picker_index));
            return;
        }
        var picker_txtcolor = App.Database.GetAllCategories()[picker.SelectedIndex].Color_Hex;
        picker.TextColor = Color.FromArgb(picker_txtcolor);
    }

    private void checkNoDeadline_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value == true)
        {
            foreach (VisualElement element in DeadlineSettingWrapper.Children)
            {
                element.IsEnabled = false;
            }
        }
        else
        {
            foreach (VisualElement element in DeadlineSettingWrapper.Children)
            {
                element.IsEnabled = true;
            }
        }
    }

    private async void btnAddTask_Clicked(object sender, EventArgs e)
    {
        // Check if task name is invalid
        if (!IsNameValid())
        {
            txtName.Text = string.Empty;
            txtName.Placeholder = App.Setting.IsVietnamese ? "* Ô này không thể để trống" : "* This box can't be blank"; 
            txtName.PlaceholderColor = Colors.Red;
            return;
        }

        var task = new ToDo_Task()
        {
            id = App.Database.GenerateRandomTaskId(),
            Name = txtName.Text,
            Description = txtDescription.Text,
            AddTime = DateTime.Now
        };

        if (!checkNoDeadline.IsChecked)
        {
            var date = dateDeadline.Date;
            var time = timeDeadline.Time;
            task.DeadlineTime = new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);
        }

        if (listNotiDatePickers.Count != 0)
        {
            List<DateTime> list_NotiTime = new List<DateTime>();
            for (int i = 0; i < listNotiDatePickers.Count; i++)
            {
                var date = listNotiDatePickers[i].Date;
                var time = listNotiTimePickers[i].Time;
                list_NotiTime.Add(new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds));
            }
            if (list_NotiTime.Count != 0)
            {
                task.str_NotificationTime = JsonSerializer.Serialize(list_NotiTime, typeof(List<DateTime>));
            }
        }

        if (listCategoryEntryWrapper.Count != 0)
        {
            List<int> categoryId = new List<int>();
            foreach (var wrapper in listCategoryEntryWrapper)
            {
                var picker = wrapper.Children[0] as Picker;
                if (!App.Database.IsCategoryNameTaken(picker.SelectedItem as string))
                {
                    App.Database.AddCategory(new Category()
                    {
                        Id = App.Database.GenerateRandomCategoryId(),
                        Name = picker.SelectedItem as string,
                        Color_Hex = picker.TextColor.ToHex()
                    });
                }
                var category = App.Database.FindCategory(picker.SelectedItem as string);
                if (category != null)
                {
                    categoryId.Add(category.Id);
                }
            }

            task.str_CategoryId = JsonSerializer.Serialize(categoryId, typeof(List<int>));
        }

        App.Database.AddTask(task);
        App.Database.DeleteObsoleteCategory();
        App.mainPage.RefreshTaskViewWrapper();
        await Navigation.PopAsync();
    }

    private bool IsNameValid()
    {
        if (txtName.Text == null || txtName.Text.Replace(" ", "") == "")
        {
            return false;
        }

        return true;
    }
}