using FilteringSortingApiWPF.Models;
using FilteringSortingApiWPF.Services;
using FilteringSortingApiWPF.Utils;
using System.Collections.ObjectModel;
using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace FilteringSortingApiWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly ApiClient _apiClient;
        private const string URL = "https://gerador-nomes.wolan.net/";
        private readonly int totalNames = 20;

        //List<Person> people;
        public ObservableCollection<Person> people { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            _apiClient = new ApiClient(URL);
            //people = new List<Person>();
            people = new ObservableCollection<Person>();
            txtRegisterCpf.KeyUp += cpf_KeyUp;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> names = await _apiClient.GetNames(totalNames);
            List<string> surnames = await _apiClient.GetSurname(totalNames);

            for (int i = 0; i < totalNames; i++)
            {
                Person p = new Person(i + 1, names[i], surnames[i], GenerateCpf.Create());
                people.Add(p);
            }

            dataGrid.ItemsSource = people;
        }


        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string valueName = txtRegisterName.Text;
            string valueSurname = txtRegisterSurname.Text;
            string valueCpf = txtRegisterCpf.Text;

            if(string.IsNullOrEmpty(valueName) || string.IsNullOrEmpty(valueSurname) 
                || string.IsNullOrEmpty(valueCpf))
            {
                MessageBox.Show("Todos os campos são de preenchimento obrigatório!", "Erro", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            } else
            {
                int totRegistered = people.Count;
                people.Add(new Person(totRegistered + 1, valueName, valueSurname, valueCpf));

                MessageBox.Show("Cadastro realizado com sucesso!", "Sucesso", 
                MessageBoxButton.OK, MessageBoxImage.Information);
                
                dataGrid.Items.Refresh();
                txtRegisterName.Text = "";
                txtRegisterSurname.Text = "";
                txtRegisterCpf.Text = "";
            }
        }

        private void cpf_KeyUp(object sender, KeyEventArgs e)
        {
            if(sender is TextBox textBox) textBox.Text = FormatData.FormatCpf(textBox.Text);
            //txtCpf.Text = FormatData.FormatCpf(txtCpf.Text);
            /*
            if (!ValidateData.IsValidCpf(txtCpf.Text))
                txtCpf.BorderBrush = Brushes.Red;
            else
                txtCpf.BorderBrush = Brushes.Green;
            */
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            filterPeople();
        }

        // Filtro por NOME || SOBRENOME || CPF || NOME, SOBRENOME e CPF
        private void filterPeople()
        {
            string NameField = txtName.Text.ToLower();
            string SurnameField = txtSurname.Text.ToLower();
            string CpfField = txtCpf.Text;

            if (!string.IsNullOrWhiteSpace(NameField))
            {
                //MessageBox.Show("Filtrando APENAS por NOME(S)");
                List<Person> filteredNames = people.Where(n => n.Name.ToLower().StartsWith(NameField)).ToList();

                if (filteredNames.Count > 0)
                    dataGrid.ItemsSource = filteredNames;
                else
                    MessageBox.Show("Nenhum registro localizado com o NOME informado!",
                        "Não localizado", MessageBoxButton.OK, MessageBoxImage.Information);

            } else if (!string.IsNullOrWhiteSpace(SurnameField))
            {
                //MessageBox.Show("Filtrando APENAS por SOBRENOME(S)");
                List<Person> filteredNames = people.Where(n => n.Surname.ToLower().StartsWith(SurnameField)).ToList();

                if (filteredNames.Count > 0)
                    dataGrid.ItemsSource = filteredNames;
                else
                    MessageBox.Show("Nenhum registro localizado com o SOBRENOME informado!",
                        "Não localizado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (!string.IsNullOrWhiteSpace(CpfField))
            {
                //MessageBox.Show("Filtrando APENAS por CPF");
                /*
                if (!ValidateData.IsValidCpf(CpfField))
                {
                    MessageBox.Show("CPF inválido!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                */

                Person personFound = people.FirstOrDefault(p => p.Cpf == CpfField);

                if (personFound != null)
                {
                    List<Person> personList = new List<Person> { personFound };
                    dataGrid.ItemsSource = personList;
                }
                else
                {
                    MessageBox.Show("Nenhum registro localizado com o CPF informado!",
                        "Não localizado", MessageBoxButton.OK, MessageBoxImage.Information);
                }


            }
            else if (!string.IsNullOrWhiteSpace(NameField) && !string.IsNullOrWhiteSpace(SurnameField) && !string.IsNullOrWhiteSpace(CpfField))
            {
                //MessageBox.Show("Filtrando por NOME, SOBRENOME e CPF");
                /*
                if (!ValidateData.IsValidCpf(CpfField))
                {
                    MessageBox.Show("CPF inválido!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                */

                Person personFound = people.FirstOrDefault(
                    p => p.Name.ToLower() == NameField && p.Surname.ToLower() == SurnameField && p.Cpf == CpfField);

                if (personFound != null)
                {
                    List<Person> personList = new List<Person> { personFound };
                    dataGrid.ItemsSource = personList;
                }
                else
                {
                    MessageBox.Show("Nenhum registro localizado com o NOME, SOBRENOME e CPF informados!",
                        "Não localizado", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            else
            {
                dataGrid.ItemsSource = people;
                //txtCpf.BorderBrush = Brushes.Blue;
                //MessageBox.Show("Nenhum filtro aplicado");
            }
                
        }

        private void cbbOrdination_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _=cbbOrdination.SelectedValue; // "_=" => Discard Operator
            var cbb = cbbOrdination.SelectedValue as ComboBoxItem; // Cast

            var selectedItem = cbb.Content.ToString();

            if (selectedItem == "Ordenar por Nome") {
                dataGrid.ItemsSource = people.OrderBy(p => p.Name).ToList();
            } else if(selectedItem == "Ordenar por Sobrenome")
            {
                dataGrid.ItemsSource = people.OrderBy(p => p.Surname).ToList();
            } else if(people != null)
            {
                dataGrid.ItemsSource = people;

            }
        }


        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Obtém o item associado ao botão
            var button = sender as Button;
            var person = button?.Tag as Person;

            if (person != null)
            {
                var editWindow = new EditWindow(person.Clone());
                //editWindow.DataContext = person;
               
                var result = editWindow.ShowDialog();

                if (result == true)
                {
                    // Atualizar as propriedades do objeto original com as propriedades do objeto editável
                    person.Name = editWindow.EditablePerson.Name;
                    person.Surname = editWindow.EditablePerson.Surname;
                    person.Cpf = editWindow.EditablePerson.Cpf;
                    dataGrid.Items.Refresh();
                }
            }

        }


    }
}