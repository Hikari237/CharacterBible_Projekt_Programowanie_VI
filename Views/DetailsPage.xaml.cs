using System.Text.Json;
using CharacterBible.Models;
using CharacterBible.Views;

namespace CharacterBible.Views;

public partial class DetailsPage : ContentPage
{
    private CharacterDraft _character;

    public DetailsPage(CharacterDraft character)
    {
        InitializeComponent();
        _character = character;

        LblName.Text = _character.Name;
        LblDetails.Text = $"P³eæ: {_character.Gender}  |  Energia: {_character.Vibe}  |  Rola: {_character.Role}";
        EditorNotes.Text = _character.Notes;
    }

    private void OnSaveClicked(object sender, EventArgs e)
    {
        _character.Notes = EditorNotes.Text;

        string savedJson = Preferences.Get("saved_characters", "[]");
        var list = JsonSerializer.Deserialize<List<CharacterDraft>>(savedJson) ?? new List<CharacterDraft>();

        var existing = list.FirstOrDefault(c => c.Id == _character.Id);
        if (existing != null)
        {
            existing.Notes = _character.Notes;
        }
        else
        {
            list.Add(_character);
        }

        string updatedJson = JsonSerializer.Serialize(list);
        Preferences.Set("saved_characters", updatedJson);

        DisplayAlert("Sukces", "Opis postaci zosta³ zapisany w bazie danych!", "OK");
    }

    private void OnBackClicked(object sender, EventArgs e)
    {
        var listPage = new ListPage();
        listPage.MainMenuContainer.IsVisible = false;
        listPage.ListLibraryContainer.IsVisible = true;
        listPage.ListTitleLabel.Text = $"POSTACIE: {_character.Genre.ToUpper()}";
        listPage.DisplayFilteredCharacters(_character.Genre);
        Application.Current.Windows[0].Page = listPage;
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Potwierdzenie", $"Czy na pewno chcesz bezpowrotnie usun¹æ postaæ: {_character.Name}?", "Tak, usuñ", "Anuluj");

        if (answer == false) return;

        string savedJson = Preferences.Get("saved_characters", "[]");
        var list = JsonSerializer.Deserialize<List<CharacterDraft>>(savedJson) ?? new List<CharacterDraft>();

        var toRemove = list.FirstOrDefault(c => c.Id == _character.Id);
        if (toRemove != null)
        {
            list.Remove(toRemove);
        }

        string updatedJson = JsonSerializer.Serialize(list);
        Preferences.Set("saved_characters", updatedJson);

        var listPage = new ListPage();
        listPage.MainMenuContainer.IsVisible = false;
        listPage.ListLibraryContainer.IsVisible = true;
        listPage.ListTitleLabel.Text = $"POSTACIE: {_character.Genre.ToUpper()}";
        listPage.DisplayFilteredCharacters(_character.Genre);
        Application.Current.Windows[0].Page = listPage;
    }
}