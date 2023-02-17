﻿using CS526_Project.Model;
using System.Text.Json;

namespace CS526_Project;

public partial class App : Application
{
    public static DatabaseHelper Database { get; } = new DatabaseHelper();

    public static MainPage mainPage;

    public static Color PrimaryColor { get; private set; }
    public static Color SecondaryColor { get; private set; }
    public static Color TertiaryColor { get; private set; }

	public static List<ToDo_Task> ToDo_List = new List<ToDo_Task>();
	public App()
	{
		InitializeComponent();

        var rd_colors = App.Current.Resources.MergedDictionaries.First();
        PrimaryColor = rd_colors["Primary"] as Color;
        SecondaryColor = rd_colors["Secondary"] as Color;
        TertiaryColor = rd_colors["Tertiary"] as Color;

        {
            ToDo_Task task1 = new();
            {
                task1.id = 1;
                task1.Name = "FakeTask1";
                task1.Description = "Test1";
                task1.AddTime = new DateTime(2023, 1, 17, 14, 0, 0);
                task1.DeadlineTime = new DateTime(2023, 2, 15, 9, 0, 0);
                task1.IsDone = true;

                var CategoryId = new List<int> { 1, 2 };
                var NotificationTime = new List<DateTime> { new DateTime(2023, 1, 31, 14, 0, 0) };
                task1.str_CategoryId = JsonSerializer.Serialize(CategoryId, typeof(List<int>));
                task1.str_NotificationTime = JsonSerializer.Serialize(NotificationTime, typeof(List<DateTime>));
            }
            ToDo_Task task2 = new();
            {
                task2.id = 2;
                task2.Name = "FakeTask2";
                task2.Description = "Test2";
                var CategoryId = new List<int>();
                task2.AddTime = new DateTime(2023, 1, 1, 0, 0, 0);
                task2.DeadlineTime = new DateTime(2023, 2, 17, 0, 0, 0);
                var NotificationTime = new List<DateTime> { new DateTime(2023, 1, 21, 0, 0, 0),
                                                        new DateTime(2023, 1, 21, 20, 0, 0) };
                task2.str_CategoryId = JsonSerializer.Serialize(CategoryId, typeof(List<int>));
                task2.str_NotificationTime = JsonSerializer.Serialize(NotificationTime, typeof(List<DateTime>));
            }
            ToDo_Task task3 = new();
            {
                task3.id = 3;
                task3.Name = "FakeTask3";
                var CategoryId = new List<int> { 1, 2, 3 };
                task3.AddTime = new DateTime(2023, 1, 1, 0, 0, 0);
                task3.DeadlineTime = new DateTime(2023, 2, 13, 23, 59, 59);
                var NotificationTime = new List<DateTime> { new DateTime(2023, 1, 17, 0, 0, 0) };
                task3.str_CategoryId = JsonSerializer.Serialize(CategoryId, typeof(List<int>));
                task3.str_NotificationTime = JsonSerializer.Serialize(NotificationTime, typeof(List<DateTime>));
            }
            ToDo_List.Add(task1);
            ToDo_List.Add(task2);
            ToDo_List.Add(task3);
            App.Database.DeleteAllTasks();
            App.Database.AddTask(App.ToDo_List[0]); App.Database.AddTask(App.ToDo_List[1]); App.Database.AddTask(App.ToDo_List[2]);
        }

        {
            Category cat1 = new Category() { Id = 1, Name = "Category1", Color_Hex = Colors.Red.ToHex() };
            Category cat2 = new Category() { Id = 2, Name = "Category2", Color_Hex = Colors.SkyBlue.ToHex() };
            Category cat3 = new Category() { Id = 3, Name = "Category3" };
            App.Database.DeleteAllCategories();
            App.Database.AddCategory(cat1); App.Database.AddCategory(cat2); App.Database.AddCategory(cat3);
        }

        mainPage = new();
        MainPage = new NavigationPage(mainPage);
        
	}
}
