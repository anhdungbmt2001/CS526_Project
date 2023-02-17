﻿using CS526_Project.Model;
using CS526_Project.UserControls;

namespace CS526_Project;

public partial class AddCategoryPage : ContentPage
{
	private Color selected_color = Colors.Black;
	private AddTaskPage parentPage_Add;
    private EditTaskPage parentPage_Edit;
	private int caller_IndexInWraper;
	public AddCategoryPage(AddTaskPage parentPage, int caller_IndexInWraper)
	{
		InitializeComponent();
		this.parentPage_Add = parentPage;
		this.caller_IndexInWraper = caller_IndexInWraper;
	}
    public AddCategoryPage(EditTaskPage parentPage, int caller_IndexInWraper)
    {
        InitializeComponent();
        this.parentPage_Edit = parentPage;
        this.caller_IndexInWraper = caller_IndexInWraper;
    }

    private void btnColor_Clicked(object sender, EventArgs e)
    {
		foreach (ImageButton btn in ColorBtnWrapper.Children)
		{
			btn.Style = (Style)this.Resources["ColorButtonStyle"];
        }

		var button = sender as ImageButton;
		button.Style = (Style)this.Resources["SelectedColorButtonStyle"];
		selected_color = button.BackgroundColor;
    }

    private async void btnAddCategory_Clicked(object sender, EventArgs e)
    {
        if (!IsNameValid())
        {
            txtName.Text = string.Empty;
            txtName.Placeholder = "* Ô này không thể để trống";
            txtName.PlaceholderColor = Colors.Red;
            return;
        }

        if (App.Database.IsCategoryNameTaken(txtName.Text))
        {
            labelError.Text = "* Tên nhãn đã tồn tại. Vui lòng đặt tên khác.";
            labelError.IsVisible = true;
            return;
        }
        if (parentPage_Add != null)
        {
            parentPage_Add.OnAddCategoryPageReturn(txtName.Text, selected_color, caller_IndexInWraper);
        }
        else if (parentPage_Edit != null)
        {
            parentPage_Edit.OnAddCategoryPageReturn(txtName.Text, selected_color, caller_IndexInWraper);
        }
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