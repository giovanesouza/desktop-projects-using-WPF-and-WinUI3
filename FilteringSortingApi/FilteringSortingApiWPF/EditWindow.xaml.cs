using FilteringSortingApiWPF.Models;
using FilteringSortingApiWPF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace FilteringSortingApiWPF
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public Person EditablePerson { get; set; }

        public EditWindow(Person person)
        {
            InitializeComponent();

            EditablePerson = person.Clone();
            DataContext = EditablePerson;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrEmpty(EditablePerson.Name) &&
                !string.IsNullOrEmpty(EditablePerson.Surname) && 
                !string.IsNullOrEmpty(EditablePerson.Cpf))
            {
                var res = MessageBox.Show("Você tem certeza que deseja modificar este registro?",
                $"Confirmar atualização", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (res == MessageBoxResult.Yes)
                {
                    MessageBox.Show("Atualizado com sucesso!", "Sucesso!", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true; // Fechar a janela e retornar indicando que a edição foi realizada
                }

            } else
            {         
                MessageBox.Show("Todos os campos são obrigatórios.",
                    "Erro", MessageBoxButton.OK, MessageBoxImage.Error);          
            }
   
        }

        private void txtCpf_KeyUp(object sender, KeyEventArgs e)
        {
            txtCpf.Text = FormatData.FormatCpf(txtCpf.Text);
        }



    }
}
