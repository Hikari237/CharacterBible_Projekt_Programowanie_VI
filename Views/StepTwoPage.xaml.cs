using System.Text.Json;
using CharacterBible.Models;

namespace CharacterBible.Views;

public partial class StepTwoPage : ContentPage
{
    private CharacterDraft _draft;
    private string _jsonFileName; 

    public StepTwoPage(CharacterDraft draft, string jsonFileName) 
    {
        InitializeComponent();
        _draft = draft;
        _jsonFileName = jsonFileName; 
        LoadRolesFromJson();
    }

    private async void LoadRolesFromJson()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync(_jsonFileName);
        using var reader = new StreamReader(stream);
        var jsonContents = await reader.ReadToEndAsync();
        var db = JsonSerializer.Deserialize<CyberDataBases>(jsonContents);

        if (db?.Roles == null) return;

        foreach (var roleText in db.Roles)
        {
            var btn = new Button
            {
                Text = roleText,
                BackgroundColor = Color.Parse("#d9c58b"),
                FontFamily = "Gabriola",
                HorizontalOptions = LayoutOptions.Fill
            };
            btn.Clicked += OnRoleButtonClicked;
            RolesContainer.Children.Add(btn);
        }
    }

    private void OnRoleButtonClicked(object sender, EventArgs e)
    {
        var clickedButton = (Button)sender;
        _draft.Role = clickedButton.Text;
        SaveAndExit();
    }

    private void OnCustomRoleSubmitted(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(CustomRoleEntry.Text))
        {
            DisplayAlert("B³¹d", "Wpisz coœ, zanim zatwierdzisz!", "OK");
            return;
        }
        _draft.Role = CustomRoleEntry.Text;
        SaveAndExit();
    }

    private void SaveAndExit()
    {
        string savedJson = Preferences.Get("saved_characters", "[]");
        var list = JsonSerializer.Deserialize<List<CharacterDraft>>(savedJson) ?? new List<CharacterDraft>();
        list.Add(_draft);

        string updatedJson = JsonSerializer.Serialize(list);
        Preferences.Set("saved_characters", updatedJson);

        var listPage = new ListPage();

        listPage.MainMenuContainer.IsVisible = false;
        listPage.ListLibraryContainer.IsVisible = true;

        if (_draft.Genre == "Cyberpunk")
        {
            listPage.ListTitleLabel.Text = "POSTACIE: CYBERPUNK";
            listPage.DisplayFilteredCharacters("Cyberpunk");
        }
        else
        {
            listPage.ListTitleLabel.Text = "POSTACIE: FANTASY";
            listPage.DisplayFilteredCharacters("Fantasy");
        }

        Application.Current.Windows[0].Page = listPage;
    }
}