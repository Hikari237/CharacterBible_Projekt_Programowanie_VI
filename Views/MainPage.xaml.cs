namespace CharacterBible.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

 

    private void OnCreateCyberpunkClicked(object sender, EventArgs e)
    {
        Application.Current.Windows[0].Page = new StepZeroPage("cyber_data.json", "Cyberpunk");
    }

    private void OnCreateFantasyClicked(object sender, EventArgs e)
    {
        Application.Current.Windows[0].Page = new StepZeroPage("fantasy_data.json", "Fantasy");
    }
}