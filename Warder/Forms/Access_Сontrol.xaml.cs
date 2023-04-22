using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
    /// Логика взаимодействия для Access_Сontrol.xaml
    /// </summary>
    public partial class Access_Сontrol : Window
    {
        DBEnt DBEnt;
        Employee emp;
        MainWindow window;
        Employee empl = new Employee();
        List<Post> posts = new List<Post>();
        int block = 0;
        public Access_Сontrol(DBEnt ent, Employee employee, MainWindow w)
        {
            InitializeComponent();
            DBEnt = ent;
            emp= employee;
            posts = DBEnt.Posts.ToList();
            txtUser.Text = emp.FIO;
            this.DataContext = empl;
            window = w;
            Blocking();
            CheckLevel();
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

            if (emp.Employee_Levels.Count(l => l.Access_Level_ID == 1) > 0)
            {
                btnSave.IsEnabled = true;
            }
            else
            {
                btnSave.IsEnabled = false;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены в отмене создания пользователя?","Warning",MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                empl = new Employee();
                DataContext = empl;
                imgNewUser.Source = null;
                cmbGender.SelectedItem = null;
                txtPost.Text = "";
                MessageBox.Show("Данные успешно очищены!");
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены в создании нового пользователя?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (empl.LastName != "" && empl.FirstName != "" && empl.Patronymic != "" && empl.Gender != null && txtPost.Text != "")
                {
                    block = 0;
                    int id = 0;
                    foreach (var item in posts)
                    {
                        if (item.Title.Contains(txtPost.Text))
                        {
                            id = item.ID;
                            empl.PostID = id;
                            break;
                        }
                    }
                    if (id == 0)
                    {
                        Post post = new Post();
                        post.ID = id;
                        post.Title = txtPost.Text;
                        empl.Post = post;

                    }
                    DBEnt.Employees.Add(empl);
                    await DBEnt.SaveChangesAsync();
                    MessageBox.Show("Данные успешно сохранены!");
                    empl = new Employee();
                    DataContext = empl;
                    txtPost.Text = "";
                    imgNewUser.Source = null;
                    cmbGender.SelectedItem = null;
                }
                else
                {
                    block++;
                    MessageBox.Show("Заполнены не все поля!");
                    Blocking();
                }
            }
        }

        private async void Blocking()
        {
            if (File.Exists(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "blocking")) == true)
            {
                block = 2;
            }
            if (block == 2)
            {
                File.WriteAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"blocking"),"block");
                form.IsEnabled= false;
                await Task.Delay(300000);
                form.IsEnabled= true;
                File.Delete(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "blocking"));
            }
        }

        private void btnAddPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog= new OpenFileDialog();
            fileDialog.Filter = "photos|*.jpeg";
            fileDialog.ShowDialog();
            if (fileDialog.FileName != "")
            {
                FileInfo info = new FileInfo(fileDialog.FileName);

                if (info.Exists) { 
                    var bytes = File.ReadAllBytes(info.FullName);
                    var image = new BitmapImage(new Uri(fileDialog.FileName));

                    if(image.Height== 100 && image.Width == 100) 
                    {
                        empl.Photo = bytes;
                        imgNewUser.Source = image;
                    }
                    else 
                    {
                        MessageBox.Show("Размер изображения не соответствует необходимому!(100x100)","Error");
                    }
                }
            }            
        }

        private void cmbGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = (sender as ComboBox).SelectedIndex;
            if (index == 0)
            {
                empl.Gender = "М";
            }
            else
            {
                empl.Gender = "Ж";
            }

        }
    }
}
