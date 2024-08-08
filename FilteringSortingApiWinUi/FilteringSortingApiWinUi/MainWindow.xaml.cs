using FilteringSortingApiWinUi.ControlPages;
using FilteringSortingApiWinUi.controls;
using FilteringSortingApiWinUi.Models;
using FilteringSortingApiWinUi.Services;
using FilteringSortingApiWinUi.Utils;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FilteringSortingApiWinUi
{

    public sealed partial class MainWindow : Window
    {

        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {         
            string valueName = txtNameRegister.Text;
            string valueSurname = txtSurnameRegister.Text;
            string valueCpf = txtCpfRegister.Text;

            if (string.IsNullOrEmpty(valueName) || string.IsNullOrEmpty(valueSurname) || string.IsNullOrEmpty(valueCpf))
            {
                ShowContentDialog("Erro", "Todos os campos são de preenchimento obrigatório!");
            }
            else
            {
                int totRegistered = peopleListControl.People.Count;
                Person newPerson = new Person(totRegistered + 1, valueName, valueSurname, valueCpf);
                peopleListControl.AddPerson(newPerson);

                ShowContentDialog("Sucesso", "Cadastro realizado com sucesso!");
                
                txtNameRegister.Text = "";
                txtSurnameRegister.Text = "";
                txtCpfRegister.Text = "";
            }

        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            FilterPeople();
        }

        // Filtragem de pessoas por Nome || Sobrenome || Nome, Sobrenome e CPF
        private void FilterPeople()
        {
            string nameField = txtNameFilter.Text.ToLower();
            string surnameField = txtSurnameFilter.Text.ToLower();
            string cpfField = txtCpfFilter.Text;

            if (!string.IsNullOrWhiteSpace(nameField))
            {
                //Filtrando APENAS por NOME(S);
                List<Person> filteredNames = peopleListControl.People.Where(p => p.Name.ToLower().StartsWith(nameField)).ToList();
                
                if (filteredNames.Count > 0)
                    peopleListControl.Filter(filteredNames);
                else
                    ShowContentDialog("Não localizado", "Nenhum registro localizado com o NOME informado!");
            }
            else if (!string.IsNullOrWhiteSpace(surnameField))
            {
                // Filtrando APENAS por SOBRENOME(S)         
                List<Person> filteredNames = peopleListControl.People
                    .Where(p => p.Surname.ToLower().StartsWith(surnameField)).ToList();

                if (filteredNames.Count > 0)
                   peopleListControl.Filter(filteredNames);
                else
                    ShowContentDialog("Não localizado", "Nenhum registro localizado com o SOBRENOME informado!");
                
                
            }
            else if (!string.IsNullOrWhiteSpace(cpfField))
            {
                // Filtrando APENAS por CPF
                Person personFound = peopleListControl.People.FirstOrDefault(p => p.Cpf == cpfField);

                if (personFound != null)
                {
                    List<Person> person = new List<Person> { personFound };
                    peopleListControl.Filter(person);
                }
                else
                  ShowContentDialog("Não localizado", "Nenhum registro localizado com o CPF informado!");

            }
            else if (!string.IsNullOrWhiteSpace(nameField) && !string.IsNullOrWhiteSpace(surnameField) &&
                !string.IsNullOrWhiteSpace(cpfField))
            {
                // Filtrando por NOME, SOBRENOME e CPF
                Person personFound = peopleListControl.People.FirstOrDefault(
                p => p.Name.ToLower() == nameField && p.Surname.ToLower() == surnameField && p.Cpf == cpfField);

                if (personFound != null) {
                    List<Person> person = new List<Person> { personFound };
                    peopleListControl.Filter(person);
                } else
                {
                    ShowContentDialog("Não localizado", "Nenhum registro localizado com o NOME, SOBRENOME e CPF informados!");
                }
            }
            else
            {
                peopleListControl.Filter(peopleListControl.People.ToList());
                ShowContentDialog("Info", "Preencha pelo menos um campo para aplicar filtro.");
            }

        }


        private void cbbOrdination_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cbb = cbbOrdination.SelectedValue as ComboBoxItem; // Cast
            var selectedItem = cbb.Content.ToString();
            
            switch (selectedItem)
            {
                case "Por Nome":
                    peopleListControl.SetSortedPeople(peopleListControl.People.OrderBy(p => p.Name).ToList());
                    break;
                case "Por Sobrenome":
                    peopleListControl.SetSortedPeople(peopleListControl.People.OrderBy(p => p.Surname).ToList());
                    break;
                case "Padrão":
                    peopleListControl.SetSortedPeople(peopleListControl.People.OrderBy(p => p.Id).ToList());
                    break;
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


        private void cpf_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (sender is TextBox textBox) textBox.Text = FormatData.FormatCpf(textBox.Text);
        }


    }
}
