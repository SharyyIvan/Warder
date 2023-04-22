using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Warder.Models
{
    public class Base
    {
    }
    public partial class Employee
    {
        [NotMapped]
        public string FIO 
        {
            get
            {
                return $"{LastName} {FirstName.Substring(0,1)}.{Patronymic.Substring(0,1)}.";
            }
        }
        [NotMapped]
        public string PostTitle 
        { 
            get 
            
            {
                return $"{Post.Title}";
            } 
        }
        public bool Format 
        { 
            get 
            {
                bool check = false;
                if (Employee_Levels.Count > 0)
                {
                    foreach (var item in Employee_Levels)
                    {
                        if (item.Access_Level_ID == 3)
                        {
                            check = true;
                        }
                    }
                    return check;
                }
                else
                {
                    return check;
                }
            } 
        }
        public bool View
        {
            get
            {
                bool check = false;
                if (Employee_Levels.Count > 0)
                {
                    foreach (var item in Employee_Levels)
                    {
                        if (item.Access_Level_ID == 2)
                        {
                            check = true;
                        }
                    }
                    return check;
                }
                else
                {
                    return check;
                }
            }
        }
        public bool Add
        {
            get
            {
                bool check = false;
                if (Employee_Levels.Count > 0)
                {
                    foreach (var item in Employee_Levels)
                    {
                        if (item.Access_Level_ID == 1)
                        {
                            check = true;
                        }
                    }
                    return check;
                }
                else
                {
                    return check;
                }
            }
        }

        [NotMapped]
        public virtual List<Type> Types { get; set; }

    }
}
