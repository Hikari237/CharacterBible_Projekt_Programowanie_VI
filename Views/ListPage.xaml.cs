using System.Text.Json;
using CharacterBible.Models;

namespace CharacterBible.Views;

public partial class ListPage : ContentPage
{
    public ListPage()
    {
        InitializeComponent();

        MainMenuContainer.IsVisible = true;
        ListLibraryContainer.IsVisible = false;
    }

    private void OnCreateCyberpunkClicked(object sender, EventArgs e)
    {
        if (Application.Current?.Windows.Count > 0)
            Application.Current.Windows[0].Page = new StepZeroPage("cyber_data.json", "Cyberpunk");
    }

    private void OnCreateFantasyClicked(object sender, EventArgs e)
    {
        if (Application.Current?.Windows[0] != null)
            Application.Current.Windows[0].Page = new StepZeroPage("fantasy_data.json", "Fantasy");
    }

    private void OnOpenListSelectionClicked(object sender, EventArgs e)
    {
        MainMenuContainer.IsVisible = false;
        ListLibraryContainer.IsVisible = true;

        CharactersContainer.Children.Clear();
        ListTitleLabel.Text = "WYBIERZ GATUNEK DO PODGL¥DU";
    }

    private void OnFilterCyberpunkClicked(object sender, EventArgs e)
    {
        ListTitleLabel.Text = "POSTACIE: CYBERPUNK";
        DisplayFilteredCharacters("Cyberpunk");
    }

    private void OnFilterFantasyClicked(object sender, EventArgs e)
    {
        ListTitleLabel.Text = "POSTACIE: FANTASY";
        DisplayFilteredCharacters("Fantasy");
    }

    public void DisplayFilteredCharacters(string genre)
    {
        CharactersContainer.Children.Clear();

        string savedJson = Preferences.Get("saved_characters", "[]");
        var allCharacters = JsonSerializer.Deserialize<List<CharacterDraft>>(savedJson) ?? new List<CharacterDraft>();

        var filtered = allCharacters.Where(c => c.Genre == genre).ToList();

        if (filtered.Count == 0)
        {
            var emptyLabel = new Label { Text = "Brak zapisanych postaci w tym gatunku.", TextColor = Color.Parse("Gray"), HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0, 20, 0, 0) };
            CharactersContainer.Children.Add(emptyLabel);
            return;
        }

        foreach (var character in filtered)
        {
            var btn = new Button
            {
                Text = $"{character.Name} ({character.Role})",
                BackgroundColor = Color.Parse("#9e9e9e"),
                TextColor = Color.Parse("#000000"),
                FontFamily = "Gabriola",
                FontSize = 26,
                HeightRequest = 55,
                HorizontalOptions = LayoutOptions.Fill,
                Margin = new Thickness(0, 0, 0, 6)
            };

            btn.Clicked += (s, e) => {
                if (Application.Current?.Windows.Count > 0)
                    Application.Current.Windows[0].Page = new DetailsPage(character);
            };

            CharactersContainer.Children.Add(btn);
        }
    }

    private void OnBackToMenuClicked(object sender, EventArgs e)
    {
        // Powrót do menu: ukrywamy bibliotekê, pokazujemy menu g³ówne
        ListLibraryContainer.IsVisible = false;
        MainMenuContainer.IsVisible = true;
    }
}