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
         //   ___UserList_.Items.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
        }

        public void SortMember(object sender, RoutedEventArgs routedEventArgs)
        {
      ContextMenu   sendercast = (System.Windows.Controls.ContextMenu)sender;
      if (sendercast.Name == "Id")
      {
          MenuItem routedEventArgsCast = (MenuItem)routedEventArgs.Source;
          if (routedEventArgsCast.Header.ToString() == "Ascending")
          {
              ___UserList_.Items.SortDescriptions.Clear();
                    ___UserList_.Items.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
          }
          else
          {
              ___UserList_.Items.SortDescriptions.Clear();
                    ___UserList_.Items.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));
          }
      }
        }
    }
}
