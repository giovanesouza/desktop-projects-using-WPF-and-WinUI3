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
using RegisterWPF.Models;
using RegisterWPF.Exceptions;
using System.Xml.Linq;

namespace RegisterWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int id = 1;
        private List<Client> clients;

        public MainWindow()
        {
            InitializeComponent();
            clients = new List<Client>();
            InitializeListBox(); 
        }

        private void InitializeListBox()
        {
            foreach (Client client in clients)
            {
                listClients.Items.Add($"Nome: {client.Name}");
            }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClientValidations.ValidateName(txtName.Text);
                ClientValidations.ValidateCpf(txtCpf.Text);

                Client newClient = new Client(id, txtName.Text, txtCpf.Text);

                clients.Add(newClient);
                MessageBox.Show($"Cliente cadastrado com sucesso!\n" +
                    $"Id: {newClient.Id}\nNome:{newClient.Name}\nCpf: {newClient.Cpf}");

                id++;
                UpdateListBox();

                clearFields();
            }
            catch (ValidationException ex) // Exceções personalizadas
            {
                MessageBox.Show(ex.Message, "Erro de Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
            } catch (Exception ex) // Exceções genéricas
            {
                MessageBox.Show($"Erro inesperado: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void clearFields()
        {
            txtName.Clear();
            txtCpf.Clear();
        }

        // Nomenclatura dos métodos OK?
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            clearFields();
        }

        private void listClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
            int index = listClients.SelectedIndex;
            var c = clients[index];

            if (c != null)
                MessageBox.Show(clients.ElementAt(index).Cpf.ToString(),
                    $"CPF do usuário selecionado - {c.Name}", MessageBoxButton.OK, MessageBoxImage.Information);
            */

            if (listClients.SelectedItem is string selectedItem)
            {
                var client = clients[listClients.SelectedIndex];
                MessageBox.Show(client.Cpf, $"CPF do usuário selecionado - {client.Name}",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Melhorar - add apenas o último registro inserido
        public void UpdateListBox()
        {
            if (clients.Count > 0) {
                Client lastClient = clients.Last();
                listClients.Items.Add($"Nome: {lastClient.Name}");
            }
        }

        private void txtCpf_TextChanged(object sender, TextChangedEventArgs e)
        {

            txtCpf.Text = ClientValidations.FormatCpf(txtCpf.Text);
            /*
            if (ClientValidations.IsValidCpf(txtCpf.Text)) {
                txtCpf.Background = Brushes.Green;
            } else if (string.IsNullOrWhiteSpace(txtCpf.Text))
                txtCpf.Background = Brushes.DarkGray;
            else txtCpf.Background = Brushes.Red;
            */
        }
    

    }
}
