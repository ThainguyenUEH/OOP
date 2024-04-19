using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QLCVN1.CS
{
    // Inheritance: kế thừa các thuộc tính và phương thức từ lớp cơ sở
    public class Member : Employee
    {
        public Member(string id, string name, string email, bool gender, int age, Project project)
            : base(id, name, email, gender, age)
        {
          //  Projects = new List<Project>();
           // Projects.Add(project);
        }
     //   public List<Project> Projects { get; set; }

        // Polymorphism: cùng một phương thức hoạt động khác nhau ở các lớp con
        public override string ToString()
        {
            return $"ID: {Id}\n" +
                   $"Tên: {Name}\n" +
                   $"Email: {Email}\n" +
                   $"Giới tính: {(Gender ? "Nam" : "Nữ")}\n" +
                   $"Tuổi: {Age}\n";
        }
    }
}