using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Channels;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QLCVN1.CS
{
    public class Program
    {

        public static ProjectManager projectManager = new ProjectManager();
        public static AccountManager AccManager = new AccountManager();

        public static Employee[] employees = new Employee[10];
        public static Account currentAccount;
        public static Report report;

        // Khởi tạo các nhân viên
        public static void WaitForEscKey()
        {
            Console.WriteLine("Nhấn Esc để thoát.");
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    // Lấy phím bấm
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    // Kiểm tra nếu phím bấm là Esc
                    if (key.Key == ConsoleKey.Escape)
                    {
                        // Thoát khỏi vòng lặp và case
                        Console.Clear();
                        break;
                    }
                }

                // Ngủ trong một thời gian ngắn để tránh lặp quá nhanh
                System.Threading.Thread.Sleep(100);
            }
        }
        static void Main(string[] args)
        {
            int dangnhap = 0;
            Console.OutputEncoding = Encoding.UTF8;

            employees[0] = new Employee("NV001", "Nguyễn Văn A", "nva@example.com", true, 30);
            employees[1] = new Employee("NV002", "Trần Thị B", "ttb@example.com", false, 25);
            employees[2] = new Employee("NV003", "Lê Văn C", "lvc@example.com", true, 35);
            employees[3] = new Employee("NV004", "Hoàng Thị D", "htd@example.com", false, 28);
            employees[4] = new Employee("NV005", "Phạm Văn E", "pve@example.com", true, 32);
            employees[5] = new Employee("NV006", "Nguyễn Thị F", "ntf@example.com", false, 27);
            employees[6] = new Employee("NV007", "Trần Văn G", "tvg@example.com", true, 29);
            employees[7] = new Employee("NV008", "Lê Thị H", "lth@example.com", false, 31);
            employees[8] = new Employee("NV009", "Hoàng Văn I", "hvi@example.com", true, 33);
            employees[9] = new Employee("NV010", "Phạm Thị K", "ptk@example.com", false, 26);


            Project project1 = new Project("P1", "Project Alpha", 10000, new DateTime(2024, 5, 1), new DateTime(2024, 12, 1), "Description of Project Alpha", "Đang triễn khai");

            Project project2 = new Project("P2", "Project Beta", 20000, new DateTime(2024, 6, 1), new DateTime(2025, 1, 1), "Description of Project Beta", "Đang thực hiện");

            Project project3 = new Project("P3", "Project Gamma", 30000, new DateTime(2024, 7, 1), new DateTime(2024, 12, 31), "Description of Project Gamma", "Pending");
            AccManager.accounts.Add(new Account("Quản lý", "admin", "123", AccountType.Admin, "adminID", null, true));
            AccManager.accounts.Add(new Account("Nguyễn Văn A", "NV001", "123", AccountType.Member, "NV001", project1, true));
            AccManager.accounts.Add(new Account("Trần Thị B", "NV002", "123", AccountType.Member, "NV002", project2, true));
            AccManager.accounts.Add(new Account("Lê Văn C", "NV003", "123", AccountType.Member, "NV003", project3, true));

            project1.Members.Add(new Member("NV001", "Nguyễn Văn A", "nva@example.com", true, 30, project1));
            project2.Members.Add(new Member("NV002", "Trần Thị B", "ttb@example.com", false, 25, project2));
            project3.Members.Add(new Member("NV003", "Lê Văn C", "lvc@example.com", true, 35, project3));

            projectManager.projects.Add(project1);
            projectManager.projects.Add(project2);
            projectManager.projects.Add(project3);

            project1.AddTask(new Task("Task 1A", new DateTime(2024, 4, 10), new DateTime(2024, 4, 30), 0, null));
            project1.AddTask(new Task("Task 1B", new DateTime(2024, 4, 15), new DateTime(2024, 4, 20), 0, null));
            project2.AddTask(new Task("Task 2A", new DateTime(2024, 4, 30), new DateTime(2024, 5, 30), 0, null));
            project2.AddTask(new Task("Task 2B", new DateTime(2024, 4, 20), new DateTime(2024, 5, 1), 0, null));
            project3.AddTask(new Task("Task 3A", new DateTime(2024, 5, 2), new DateTime(2024, 5, 15), 0, null));
            project3.AddTask(new Task("Task 3B", new DateTime(2024, 4, 20), new DateTime(2024, 5, 1), 0, null));

            project1.Meetings.Add(new Meeting(project1.Name, new DateTime(2024, 5, 22), "Topic1", "Content1"));
            project2.Meetings.Add(new Meeting(project2.Name, new DateTime(2024, 5, 21), "Topic2", "Content2"));
            project3.Meetings.Add(new Meeting(project3.Name, new DateTime(2024, 5, 23), "Topic3", "Content3"));

            project1.Tasks[0].Incharge = project1.Members[0];

            report = new Report();
            while (true)
            {
                // Đặt currentAccount thành null trước khi bắt đầu kiểm tra đăng nhập
                currentAccount = null;

                // Vòng lặp kiểm tra đăng nhập
                int maxAttempts = 3;
                for (int attempt = 1; attempt <= maxAttempts; attempt++)
                {
                    Console.Clear();

                    // Khởi tạo màu và font chữ cho tiêu đề
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
                    Console.WriteLine("║                      QUẢN LÝ CÔNG VIỆC NHÓM              ║");
                    Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
                    Console.ResetColor();

                    // Lấy độ dài của dòng tiêu đề
                    int titleLength = "╔══════════════════════════════════════════════════════════╗".Length;

                    // Tính toán độ dài của dòng nhập username và password
                    string inputPrompt = "Nhập username: ";
                    int inputLength = inputPrompt.Length;
                    int remainingSpaces = (titleLength - inputLength) / 2;

                    // In ra màn hình sao cho nội dung được căn giữa
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(new string(' ', remainingSpaces));
                    Console.WriteLine(inputPrompt);
                    Console.ResetColor();
                    string inputUsername = Console.ReadLine();

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    inputPrompt = "Nhập password: ";
                    inputLength = inputPrompt.Length;
                    remainingSpaces = (titleLength - inputLength) / 2;
                    Console.Write(new string(' ', remainingSpaces));
                    Console.WriteLine(inputPrompt);
                    Console.ResetColor();
                    string inputPassword = Console.ReadLine();

                    // Kiểm tra đăng nhập
                    currentAccount = AccManager.CheckLogin(inputUsername, inputPassword);

                    if (currentAccount != null)
                    {
                        // Đăng nhập thành công
                        Console.Clear();

                        Console.WriteLine("Đăng nhập thành công!");
                        break;
                    }
                    else
                    {
                        // Đăng nhập thất bại
                        Console.WriteLine("Đăng nhập thất bại! Username hoặc password không chính xác hoặc tài khoản đã bị khoá");

                        // Nếu đã thử 3 lần mà vẫn không đăng nhập được, thông báo cho người dùng và thoát
                        if (attempt == maxAttempts)
                        {
                            Console.WriteLine("Bạn đã thử đăng nhập sai 3 lần. Hãy thử lại sau.");
                            Environment.Exit(0);  // Kết thúc toàn bộ chương trình
                        }
                    }
                }

                // Sau khi đăng nhập thành công, thực hiện các hoạt động theo kiểu tài khoản (Admin hoặc Member)
                if (currentAccount != null)
                {
                    if (currentAccount.Type == AccountType.Admin)
                    {
                        // Menu Admin
                        while (true)
                        {
                            dangnhap++;
                            if (dangnhap > 1) Console.Clear();

                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("╔═════════════════════════╗");
                            Console.WriteLine("║      Menu Quản Trị      ║");
                            Console.WriteLine("╚═════════════════════════╝");
                            Console.ResetColor();

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("1. Xem dự án hiện có");
                            Console.WriteLine("2. Set up dự án");
                            Console.WriteLine("3. Tạo dự án mới");
                            Console.WriteLine("4. Sửa dự án");
                            Console.WriteLine("5. Xoá dự án");
                            Console.WriteLine("6. Các meeting sắp đến");
                            Console.WriteLine("7. Báo cáo");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("-------------------------");
                            Console.WriteLine("8. Thoát");
                            Console.ResetColor();

                            Console.WriteLine();
                            Console.Write("Chọn một tùy chọn: ");
                            int choice2;
                            int choice1;
                            if (!int.TryParse(Console.ReadLine(), out choice2))
                            {
                                Console.WriteLine("Vui lòng chọn một tùy chọn hợp lệ.");
                                continue;
                            }

                            switch (choice2)
                            {
                                case 2:

                                    projectManager.SetUpProject();
                                    break;
                                case 1:
                                    Console.Clear();
                                    projectManager.DisplayProjects(currentAccount);
                                    Console.WriteLine("Nhấn Esc để thoát.");

                                    // Lắng nghe phím bấm từ người dùng
                                    while (true)
                                    {
                                        if (Console.KeyAvailable)
                                        {
                                            // Lấy phím bấm
                                            ConsoleKeyInfo key = Console.ReadKey(true);

                                            // Kiểm tra nếu phím bấm là Esc
                                            if (key.Key == ConsoleKey.Escape)
                                            {
                                                // Thoát khỏi vòng lặp và case
                                                break;
                                            }
                                        }

                                        // Ngủ trong một thời gian ngắn để tránh lặp quá nhanh
                                        System.Threading.Thread.Sleep(100);
                                    }
                                    break;
                                case 3:
                                    projectManager.CreateNewProject();
                                    break;
                                case 4:
                                    projectManager.EditProject();
                                    break;
                                case 5:
                                    int projectIndex = projectManager.ChooseProjectToDelete();
                                    projectManager.DeleteProjectByIndex(projectIndex);
                                    break;
                                case 6:
                                    projectManager.DisplayMeetingsAfterToday();
                                    break;
                                case 7:
                                    Console.WriteLine("Danh sách các báo cáo :");
                                    Console.WriteLine($"1. Báo cáo tiến độ project");
                                    Console.WriteLine($"2. Báo cáo tiến độ task");
                                    Console.WriteLine($"Nhâp sự lựa chọn:");

                                    while (!int.TryParse(Console.ReadLine(), out choice1) || choice1 < 1 || choice1 > 2)
                                    {
                                        Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng nhập lại.");
                                        Console.Write("\nNhập số thứ tự của báo cáo bạn muốn chọn: ");
                                    }
                                    switch (choice1)
                                    {
                                        case 1:
                                            report.GenerateAdminProjectReport(projectManager.projects);
                                            break;
                                        case 2:
                                            List<Project> displayedProjects = projectManager.DisplayProjects(currentAccount);

                                            if (displayedProjects.Count == 0)
                                            {
                                                return; // Nếu không có dự án nào để hiển thị, dừng lại
                                            }

                                            // Yêu cầu người dùng chọn số thứ tự của dự án
                                            Console.Write("Vui lòng chọn số thứ tự của dự án: ");
                                            int userInput;
                                            while (!int.TryParse(Console.ReadLine(), out userInput) || userInput < 1 || userInput > displayedProjects.Count)
                                            {
                                                Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng nhập lại.");
                                                Console.Write("\nNhập số thứ tự của dự án bạn muốn chọn: ");
                                            }

                                            // Lưu lại dự án mà người dùng đã chọn
                                            Project selectedProject = displayedProjects[userInput - 1];
                                            report.GenerateTaskReportForAdmin(selectedProject);
                                            break;

                                    }
                                    Program.WaitForEscKey();
                                    break;
                                case 8:
                                    // Thoát menu và quay lại bước đăng nhập
                                    Console.Clear();
                                    break;
                                default:
                                    Console.WriteLine("Tùy chọn không hợp lệ. Vui lòng chọn lại.");
                                    break;
                            }

                            if (choice2 == 8)
                            {

                                // Thoát menu và quay lại bước đăng nhập
                                break;
                            }
                        }
                    }
                    else
                    {
                        // Menu Member
                        while (true)
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("╭──────────────────────────────────╮");
                            Console.WriteLine("│           Menu Member            │");
                            Console.WriteLine("╰──────────────────────────────────╯");
                            Console.ResetColor();

                            Console.WriteLine("1. Xem dự án tham gia");
                            Console.WriteLine("2. Cập nhật tiến độ dự án");
                            Console.WriteLine("3. Groupchat");
                            Console.WriteLine("4. Các meeting sắp đến");
                            Console.WriteLine("5. Báo cáo tiến độ task");
                            Console.WriteLine("6. Quay lại");
                            Console.WriteLine();

                            Console.WriteLine("Chọn một tùy chọn:");

                            int choice;
                            if (!int.TryParse(Console.ReadLine(), out choice))
                            {
                                Console.WriteLine("Vui lòng chọn một tùy chọn hợp lệ.");
                                continue;
                            }

                            switch (choice)
                            {
                                case 1:
                                    projectManager.DisplayProjects(currentAccount);
                                    Program.WaitForEscKey();
                                    break;
                                case 2:
                                    List<Project> selectedproject = projectManager.DisplayProjects(currentAccount);
                                    if (selectedproject.Count == 0)
                                    {
                                        Console.WriteLine("Bạn chưa tham gia project nào");

                                        break;
                                    }
                                    
                                    int luaChon;
                                    do
                                    {
                                        Console.Write("Vui lòng chọn một dự án bằng cách nhập số thứ tự: ");
                                        bool validInput = int.TryParse(Console.ReadLine(), out luaChon);

                                        // Kiểm tra tính hợp lệ của đầu vào
                                        if (validInput && luaChon >= 1 && luaChon <= selectedproject.Count)
                                        {
                                            break; // Thoát vòng lặp nếu đầu vào hợp lệ
                                        }
                                        else
                                        {
                                            Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng thử lại.");
                                        }
                                    } while (true); // Lặp lại cho đến khi người dùng nhập đúng
                                    Project duAnDuocChon = selectedproject[luaChon - 1];
                                    List<Task> taskselected = duAnDuocChon.ShowTasksForAccount(currentAccount);
                                    if (taskselected.Count == 0)
                                    {
                                        Console.WriteLine("Bạn chưa được phân công task trong project này.");
                                        break;
                                    }
                                    Console.WriteLine("Danh sách các task bạn đang tham gia trong project:");
                                    for (int i = 0; i < taskselected.Count; i++)
                                    {
                                        Console.WriteLine($"{i + 1}. Tên task : {taskselected[i].Name}");
                                        Console.WriteLine($"   Ngày bắt đầu: {taskselected[i].StartDate:dd/MM/yyyy}"); // Định dạng ngày
                                        Console.WriteLine($"   Ngày kết thúc: {taskselected[i].EndDate:dd/MM/yyyy}"); // Định dạng ngày
                                        Console.WriteLine($"   Process: {taskselected[i].Process}");
                                        Console.WriteLine();
                                    }

                                    do
                                    {
                                        Console.Write("Vui lòng chọn một task bằng cách nhập số thứ tự: ");
                                        bool validInput = int.TryParse(Console.ReadLine(), out luaChon);

                                        // Kiểm tra tính hợp lệ của đầu vào
                                        if (validInput && luaChon >= 1 && luaChon <= taskselected.Count)
                                        {
                                            break; // Thoát vòng lặp nếu đầu vào hợp lệ
                                        }
                                        else
                                        {
                                            Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng thử lại.");
                                        }
                                    } while (true); // Lặp lại cho đến khi người dùng nhập đúng

                                    Task newtaks = taskselected[luaChon - 1];
                                    newtaks.UpdateTaskProgress();
                                    // Thực hiện cập nhật tiến độ (bạn có thể bổ sung mã ở đây)
                                    break;
                                case 3:
                                    List<Project> selectedproject1 = projectManager.DisplayProjects(currentAccount);
                                    if (selectedproject1.Count == 0)
                                    {
                                        Console.WriteLine("Bạn chưa tham gia project nào");
                                        break;
                                    }
                                    

                                    do
                                    {
                                        Console.Write("Vui lòng chọn một dự án bằng cách nhập số thứ tự: ");
                                        bool validInput = int.TryParse(Console.ReadLine(), out luaChon);

                                        // Kiểm tra tính hợp lệ của đầu vào
                                        if (validInput && luaChon >= 1 && luaChon <= selectedproject1.Count)
                                        {
                                            break; // Thoát vòng lặp nếu đầu vào hợp lệ
                                        }
                                        else
                                        {
                                            Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng thử lại.");
                                        }
                                    } while (true); // Lặp lại cho đến khi người dùng nhập đúng
                                    Project duAnDuocChon1 = selectedproject1[luaChon - 1];
                                    duAnDuocChon1.CreateMessage(currentAccount);
                                    break;
                                case 4:
                                    projectManager.DisplayMeetingsAfterToday();
                                    break;
                                case 5:
                                    Console.Clear();
                                    List<Project> projectsForCurrentAccount = currentAccount.Projects;

                                    // Duyệt qua từng dự án mà nhân viên tham gia
                                    foreach (var project in projectsForCurrentAccount)
                                    {
                                        report.GenerateTaskReportForUser(project, currentAccount);
                                    }
                                    Program.WaitForEscKey();
                                    break;
                                case 6:
                                    Console.Clear();
                                    break;
                                default:
                                    Console.WriteLine("Tùy chọn không hợp lệ. Vui lòng chọn lại.");
                                    break;
                            }

                            if (choice == 6)
                            {
                                // Thoát menu và quay lại bước đăng nhập
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}

