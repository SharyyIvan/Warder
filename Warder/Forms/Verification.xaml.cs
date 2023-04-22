using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Warder.Models;

namespace Warder.Forms
{
    /// <summary>
    /// Логика взаимодействия для Verification.xaml
    /// </summary>
    public partial class Verification : Window
    {
        DBEnt DBEnt;
        Employee emp;
        MainWindow window;
        List<Employee> employees = new List<Employee>();
        List<Models.Type> types = new List<Models.Type>();
        List<Employee_Levels> levels = new List<Employee_Levels>();
        SolidColorBrush brushBlue = new SolidColorBrush(Color.FromRgb(0, 191, 255));
        SolidColorBrush brushGray = new SolidColorBrush(Color.FromRgb(211, 211, 211));
        public Verification(DBEnt ent, Employee employee, MainWindow w)
        {
            InitializeComponent();
            Init(ent, employee, w);
            CheckLevel();
        }

        private void Init(DBEnt ent, Employee employee, MainWindow w)
        {
            DBEnt = ent;
            emp = employee;
            employees = DBEnt.Employees.ToList();
            types = DBEnt.Types.ToList();
            txtUser.Text = emp.FIO;
            levels = DBEnt.Employee_Levels.ToList();
            employees.ForEach(e => e.Types = types);
            //employees.ForEach(e => e.Approve = e.Approve==null? false:e.Approve);
            window = w;
            dgEmp.ItemsSource = employees;
            dgMin.ItemsSource = employees;
            bVer.Background = brushBlue;
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            window.Show();
            window.txtLogin.Clear();
            window.txtPassword.Clear();
            window.txtSecret.Clear();
        }
        private void CheckLevel() 
        {

            if (emp.Employee_Levels.Count(l=> l.Access_Level_ID == 1)>0)
            {
                btnApply.IsEnabled = true;
                btnSave.IsEnabled = true;
            }
            else
            {
                btnApply.IsEnabled = false;
                btnSave.IsEnabled = false;
            }
            if (emp.Employee_Levels.Count(l => l.Access_Level_ID == 2) > 0)
            {
                gVer.Visibility = Visibility.Visible;
                spPages.Visibility = Visibility.Visible;
            }
            else
            {
                gVer.Visibility = Visibility.Hidden;
                spPages.Visibility = Visibility.Hidden;
            }
        }
        private void btnVerification_Click(object sender, RoutedEventArgs e)
        {
            gMin.Visibility = Visibility.Hidden;
            gVer.Visibility = Visibility.Visible;
            bVer.Background = brushBlue;
            bMin.Background = brushGray;
        }

        private void btnMindats_Click(object sender, RoutedEventArgs e)
        {
            gVer.Visibility = Visibility.Hidden;
            gMin.Visibility = Visibility.Visible;
            bMin.Background = brushBlue;
            bVer.Background = brushGray;
        }

        private void cmbTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int empID = (int)(sender as ComboBox).Tag;
            employees.First(em => em.ID == empID).TypeID = (int)(sender as ComboBox).SelectedValue;
        }

        private  void btnSave_Click(object sender, RoutedEventArgs e)
        {
           DBEnt.SaveChanges();
           MessageBox.Show("Данные успешно обновлены!");
        }

        private void chbApprove_Unchecked(object sender, RoutedEventArgs e)
        {
            int empID = (int)(sender as CheckBox).Tag;
            employees.First(em => em.ID == empID).Approve = (bool)(sender as CheckBox).IsChecked;
        }

        private void chbApprove_Checked(object sender, RoutedEventArgs e)
        {
            int empID = (int)(sender as CheckBox).Tag;
            employees.First(em => em.ID == empID).Approve = (bool)(sender as CheckBox).IsChecked;
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            DBEnt.SaveChanges();
        }


        private void chbFormat_Checked(object sender, RoutedEventArgs e)
        {

            int id = (int)(sender as CheckBox).Tag;
            if (levels.Count(l => l.Access_Level_ID == 3 && l.EmployeeID == id) == 0)
            {
                Employee_Levels employee_Levels = new Employee_Levels();
                employee_Levels.Access_Level_ID = 3;
                employee_Levels.EmployeeID = id;
                DBEnt.Employee_Levels.Add(employee_Levels);
            }
        }

        private void chbFormat_Unchecked(object sender, RoutedEventArgs e)
        {
            int id = (int)(sender as CheckBox).Tag;
            var level = levels.Where(l => l.Access_Level_ID == 3 && l.EmployeeID == id).FirstOrDefault();
            var a = employees.First(em => em.ID == id);
            if (level != null)
            {
                a.Employee_Levels.Remove(level);
                levels.Remove(level);
                DBEnt.Employee_Levels.Remove(level);
            }
        }

        private void chbView_Checked(object sender, RoutedEventArgs e)
        {
            int id = (int)(sender as CheckBox).Tag;
            if (levels.Count(l => l.Access_Level_ID == 3 && l.EmployeeID == id) == 0)
            {
                Employee_Levels employee_Levels = new Employee_Levels();
                employee_Levels.Access_Level_ID = 2;
                employee_Levels.EmployeeID = id;
                DBEnt.Employee_Levels.Add(employee_Levels);
            }
        }

        private void chbView_Unchecked(object sender, RoutedEventArgs e)
        {
            int id = (int)(sender as CheckBox).Tag;
            var level = levels.Where(l => l.Access_Level_ID == 2 && l.EmployeeID == id).FirstOrDefault();
            var a = employees.First(em => em.ID == id);
            if (level !=null)
            {
                a.Employee_Levels.Remove(level);
                levels.Remove(level);
                DBEnt.Employee_Levels.Remove(level);
            }
        }

        private void chbAdd_Checked(object sender, RoutedEventArgs e)
        {
            int id = (int)(sender as CheckBox).Tag;
            if (levels.Count(l => l.Access_Level_ID == 1 && l.EmployeeID == id) == 0)
            {
                Employee_Levels employee_Levels = new Employee_Levels();
                employee_Levels.Access_Level_ID = 1;
                employee_Levels.EmployeeID = id;
                DBEnt.Employee_Levels.Add(employee_Levels);
            }          
        }

        private void chbAdd_Unchecked(object sender, RoutedEventArgs e)
        {
            int id = (int)(sender as CheckBox).Tag;
            var level = levels.Where(l=> l.Access_Level_ID==1 && l.EmployeeID == id).FirstOrDefault();
            var a = employees.First(em => em.ID == id);
            if (level != null)
            {
                a.Employee_Levels.Remove(level);
                levels.Remove(level);
                DBEnt.Employee_Levels.Remove(level);
            }
        }
    }
}
