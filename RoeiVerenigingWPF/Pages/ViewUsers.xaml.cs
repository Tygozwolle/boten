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
using DataAccessLibary;
using RoeiVerenigingLibary;

namespace RoeiVerenigingWPF.Pages
{
    /// <summary>
    /// Interaction logic for ViewUsers.xaml
    /// </summary>
    public partial class ViewUsers : Page
    {
        private List<Member> _memberList;
        public ViewUsers()
        {
            MemberService service = new MemberService(new MemberRepository());
            InitializeComponent();
            this.DataContext = this;
            _memberList = service.GetMembers();
            ___UserList_.ItemsSource = _memberList;
        }
    }
}
