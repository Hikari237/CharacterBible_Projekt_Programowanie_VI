using System.Text.Json;
using CharacterBible.Models;

namespace CharacterBible.Views;

public partial class StepZeroPage : ContentPage
{
    private CharacterDraft _draft = new CharacterDraft();
    private CyberDataBases _db;
    private string _jsonFileName; 

    public StepZeroPage(string jsonFileName, string genreName)
    {
        InitializeComponent();

        _jsonFileName = jsonFileName;
        _draft.Genre = genreName; 
        LoadJsonData();
    }

    private async void LoadJsonData()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync(_jsonFileName);
        using var reader = new StreamReader(stream);
        var jsonContents = await reader.ReadToEndAsync();
        _db = JsonSerializer.Deserialize<CyberDataBases>(jsonContents);
    }


    private void OnGenderSelected(object sender, EventArgs e)
    {
        var clicked = (Button)sender;
        _draft.Gender = clicked.Text;
        BtnMale.BackgroundColor = _draft.Gender == "Mê¿czyzna" ? Color.Parse("#C2B280 ") : Color.Parse("#F5F5E4");
        BtnFemale.BackgroundColor = _draft.Gender == "Kobieta" ? Color.Parse("#C2B280 ") : Color.Parse("#F5F5E4 ");
    }

    private void OnRandomNameClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_draft.Gender))
        {
            DisplayAlert("Najpierw p³eæ", "Wybierz p³eæ, aby dopasowaæ imiê!", "OK");
            return;
        }

        var rand = new Random();

        if (_draft.Gender == "Mê¿czyzna" && _db?.MaleNames?.Length > 0)
            FirstNameEntry.Text = _db.MaleNames[rand.Next(_db.MaleNames.Length)];
        else if (_draft.Gender == "Kobieta" && _db?.FemaleNames?.Length > 0)
            FirstNameEntry.Text = _db.FemaleNames[rand.Next(_db.FemaleNames.Length)];

        if (_db?.Surnames?.Length > 0)
            LastNameEntry.Text = _db.Surnames[rand.Next(_db.Surnames.Length)];
    }

    private void OnRandomPseudoClicked(object sender, EventArgs e)
    {
        if (_db?.Pseudonyms?.Length > 0)
        {
            var rand = new Random();
            PseudoEntry.Text = _db.Pseudonyms[rand.Next(_db.Pseudonyms.Length)];
        }
    }

    private void OnNextClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_draft.Gender))
        {
            DisplayAlert("B³¹d", "Wybierz p³eæ postaci!", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(FirstNameEntry.Text) && string.IsNullOrWhiteSpace(PseudoEntry.Text))
        {
            DisplayAlert("B³¹d", "Postaæ musi mieæ przynajmniej imiê lub pseudonim!", "OK");
            return;
        }

        string finalName = FirstNameEntry.Text + " " + LastNameEntry.Text;
        finalName = finalName.Trim();

        if (!string.IsNullOrWhiteSpace(PseudoEntry.Text))
        {
            if (string.IsNullOrWhiteSpace(finalName))
                finalName = $"\"{PseudoEntry.Text}\"";
            else
                finalName = $"{finalName} \"{PseudoEntry.Text}\"";
        }

        _draft.Name = finalName;
        Application.Current.Windows[0].Page = new StepOnePage(_draft, _jsonFileName);
    }
}

public class CyberDataBases
{
    public string[] MaleNames { get; set; } = Array.Empty<string>();
    public string[] FemaleNames { get; set; } = Array.Empty<string>();
    public string[] Surnames { get; set; } = Array.Empty<string>();
    public string[] Pseudonyms { get; set; } = Array.Empty<string>();
    public string[] Vibes { get; set; } = Array.Empty<string>();
    public string[] Roles { get; set; } = Array.Empty<string>();
}