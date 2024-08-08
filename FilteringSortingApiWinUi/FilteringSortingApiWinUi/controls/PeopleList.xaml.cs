using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
// Imports adicionados manualmente
using FilteringSortingApiWinUi.Models;
using FilteringSortingApiWinUi.Services;
using FilteringSortingApiWinUi.Utils;
using System.Collections.ObjectModel;
using FilteringSortingApiWinUi.ControlPages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FilteringSortingApiWinUi.controls
{
    public sealed partial class PeopleList : UserControl
    {

        private readonly ApiClient _apiClient;
        private const string URL = "https://gerador-nomes.wolan.net/";
        private readonly int totalNames = 20;

        public ObservableCollection<Person> People { get; set; }

        public PeopleList()
        {
            this.InitializeComponent();
            _apiClient = new ApiClient(URL);
            this.People = new ObservableCollection<Person>();
            DataContext = this;
            RandomRegistrationPeople();
        }

        private async void RandomRegistrationPeople()
        {
            List<string> names = await _apiClient.GetNames(totalNames);
            List<string> surnames = await _apiClient.GetSurname(totalNames);

            for (int i = 0; i < totalNames; i++)
            {
                Person p = new Person(i + 1, names[i], surnames[i], GenerateCpf.Create());
                People.Add(p);
            }

        }

        public void AddPerson(Person person)
        {
            People.Add(person);
            dataGrid.ItemsSource = People;
        }

 
        public void SetSortedPeople(List<Person> sortedPeople)
        {
            People = new ObservableCollection<Person>(sortedPeople);
            dataGrid.ItemsSource = People;
        }


        public void Filter(List<Person> filtered)
        {
            dataGrid.ItemsSource = filtered;
        }

        private async void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var person = button?.Tag as Person;

                if (person != null)
                {
                    // Cria uma cópia da pessoa para edição
                    var editDialog = new EditDialog(person.Clone())
                    {
                        XamlRoot = this.XamlRoot
                    };
                   
                    // Abre janela e aguarda o resultado
                    var result = await editDialog.ShowAsync();

                    if (result == ContentDialogResult.Primary)
                    {
                        person.Name  = editDialog.EditablePerson.Name;
                        person.Surname = editDialog.EditablePerson.Surname;
                        person.Cpf = editDialog.EditablePerson.Cpf;

                        dataGrid.ItemsSource = People; // Se editado, exibe a lista atualizada
                    } else
                    {
                        ShowContentDialog("Atualização cancelada!", "");
                    }


                }
            }
            catch (Exception ex)
            {
                // Exibe qualquer erro que ocorrer
                ShowContentDialog("Error", ex.Message);
            }

        }



        private async void ShowContentDialog(string title, string message)
        {
            var dialogContent = new ContentDialogContent();
            dialogContent.SetMessage(message);

            var dialog = new ContentDialog
            {
                XamlRoot = this.Content.XamlRoot,
                Title = title,
                Content = dialogContent,
                PrimaryButtonText = "OK",
                //CloseButtonText = "Cancel"
            };

            await dialog.ShowAsync();
        }


    }
}
