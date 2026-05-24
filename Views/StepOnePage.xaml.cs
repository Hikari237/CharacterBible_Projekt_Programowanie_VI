using System.Text.Json;
using CharacterBible.Models;

namespace CharacterBible.Views;

public partial class StepOnePage : ContentPage
{
    private CharacterDraft _draft;
    private string _jsonFileName; // Dodajemy zmienn¹

    public StepOnePage(CharacterDraft draft, string jsonFileName) // Zmieniamy konstruktor
    {
        InitializeComponent();
        _draft = draft;
        _jsonFileName = jsonFileName; // Zapisujemy
        LoadVibesFromJson();
    }

    private async void LoadVibesFromJson()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync(_jsonFileName);
        using var reader = new StreamReader(stream);
        var jsonContents = await reader.ReadToEndAsync();
        var db = JsonSerializer.Deserialize<CyberDataBases>(jsonContents);

        if (db?.Vibes == null) return;

        foreach (var vibeText in db.Vibes)
        {
            var btn = new Button
            {
                Text = vibeText,
                BackgroundColor = Color.Parse("#2C3E50"),
                HorizontalOptions = LayoutOptions.Fill 
            };
            btn.Clicked += OnVibeButtonClicked;
            VibesContainer.Children.Add(btn);
        }
    }


    private void OnVibeButtonClicked(object sender, EventArgs e)
    {
        var clickedButton = (Button)sender;
        _draft.Vibe = clickedButton.Text;
        Application.Current.Windows[0].Page = new StepTwoPage(_draft, _jsonFileName);
    }


    private void OnCustomVibeSubmitted(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(CustomVibeEntry.Text)) return;
        _draft.Vibe = CustomVibeEntry.Text;
        Application.Current.Windows[0].Page = new StepTwoPage(_draft, _jsonFileName);
    }
}