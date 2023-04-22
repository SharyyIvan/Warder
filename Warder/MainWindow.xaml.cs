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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Warder.Forms;
using Warder.Models;

namespace Warder
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Employee> employees= new List<Employee>();
        List<Models.Type> types= new List<Models.Type>();
        DBEnt DBEnt = new DBEnt();
        int select = 0;
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            types = DBEnt.Types.ToList();
            employees = DBEnt.Employees.ToList();
            cmbTypes.ItemsSource = types;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Employee emp = employees.Where(em => em.Login == txtLogin.Text).FirstOrDefault();
            if (emp != null)
            {
                if (emp.Password == txtPassword.Password && emp.Secret_Word == txtSecret.Text && emp.TypeID == select)
                {
                    if (emp.Approve)
                    {
                        if (select == 1)
                        {
                            Access_Сontrol form = new Access_Сontrol(DBEnt, emp, this);
                            Hide();
                            form.ShowDialog();
                        }
                        if (select == 4)
                        {
                            Verification form = new Verification(DBEnt, emp, this);
                            Hide();
                            form.ShowDialog();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Данный пользователь не активен!");
                    }
                }
                else
                {
                    MessageBox.Show("Вход не выполнен!\r\nВозможно введены не верные данные!", "Error");
                }
            }
            else
            {
                MessageBox.Show("Пользователь с таким именем пользователя не был найден!","Error");
            }
        }

        private void cmbTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectType = (Models.Type)(sender as ComboBox).SelectedItem;
            select = selectType.ID;
        }
    }
}
