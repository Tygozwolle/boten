using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingWPF.Frames;
using Color = System.Windows.Media.Color;
using Image = System.Windows.Controls.Image;

namespace RoeiVerenigingWPF.Pages
{
    /// <summary>
    /// Interaction logic for ViewUsers.xaml
    /// </summary>
    public partial class ViewUsers : Page
    {
        private List<Member> _memberList;
        public Member SelectedMember { get; set; }
        private MainWindow _mainWindow;

        private SolidColorBrush _textColor = new SolidColorBrush(Color.FromArgb(255, 4, 48, 73));
        private SolidColorBrush _borderColor = new SolidColorBrush(Color.FromArgb(255, 19, 114, 160));

        private SolidColorBrush
            _evenRowColor = new SolidColorBrush(Color.FromArgb(255, 232, 246, 252)); // Background color for even rows

        private SolidColorBrush
            _oddRowColor = new SolidColorBrush(Color.FromArgb(255, 182, 227, 251)); // Background color for odd rows

        public ViewUsers(MainWindow mainWindow)
        {
            MemberService service = new MemberService(new MemberRepository());
            InitializeComponent();
            _mainWindow = mainWindow;
            Search_Icon.Source = new BitmapImage(new Uri("/Images/Icons/search.png", UriKind.Relative));
            DataContext = this;
            _memberList = service.GetMembers();
            PopulateUserList(_memberList);
            
        }

        public void PopulateUserList(List<Member> memberList)
        {
            UserStackPanel.Children.Clear();
            for (int i = 0; i < memberList.Count; i++)
            {
                var member = memberList[i];
                var grid = new Grid();
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10) });

                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(250) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(250) });

                grid.Children.Add(new TextBlock
                {
                    Text = member.Id.ToString(), VerticalAlignment = VerticalAlignment.Center, FontSize = 16,
                    Foreground = _textColor
                });
                Grid.SetColumn(grid.Children[0], 0);

                grid.Children.Add(new TextBlock
                {
                    Text = member.FirstName, VerticalAlignment = VerticalAlignment.Center, FontSize = 16,
                    Foreground = _textColor
                });
                Grid.SetColumn(grid.Children[1], 1);

                grid.Children.Add(new TextBlock
                {
                    Text = member.Infix, VerticalAlignment = VerticalAlignment.Center, FontSize = 16,
                    Foreground = _textColor
                });
                Grid.SetColumn(grid.Children[2], 2);

                grid.Children.Add(
                    new TextBlock
                    {
                        Text = member.LastName, VerticalAlignment = VerticalAlignment.Center, FontSize = 16,
                        Foreground = _textColor
                    });
                Grid.SetColumn(grid.Children[3], 3);

                grid.Children.Add(new TextBlock
                {
                    Text = member.Email, VerticalAlignment = VerticalAlignment.Center, FontSize = 16,
                    Foreground = _textColor
                });
                Grid.SetColumn(grid.Children[4], 4);

                grid.Children.Add(new TextBlock
                {
                    Text = member.RolesString, VerticalAlignment = VerticalAlignment.Center, FontSize = 16,
                    Foreground = _textColor
                });
                Grid.SetColumn(grid.Children[5], 5);

                var border = new Border
                {
                    BorderThickness = new System.Windows.Thickness(1),
                    BorderBrush = _borderColor,
                    CornerRadius = new CornerRadius(5),
                    Padding = new Thickness(10),
                    Child = grid,
                    Background = i % 2 == 0 ? _evenRowColor : _oddRowColor // Alternate row background color
                };

                border.MouseLeftButtonUp += (sender, e) =>
                {
                    // Select the user here
                };

                // Track last click time and clicked border
                DateTime lastClickTime = DateTime.MinValue;
                Border lastClickedBorder = null;

                // Handle PreviewMouseLeftButtonDown event for double-click
                border.PreviewMouseLeftButtonDown += (sender, e) =>
                {
                    // Check if it's a double-click
                    if (lastClickedBorder == sender && (DateTime.Now - lastClickTime).TotalMilliseconds < 500)
                    {
                        var clickedUser = _memberList[UserStackPanel.Children.IndexOf((UIElement)sender)];

                        // Open a new page with the selected user information
                        AdminEditUser userDetailsPage = new AdminEditUser(_mainWindow, clickedUser.Id);
                        NavigationService.Navigate(userDetailsPage);
                    }

                    lastClickedBorder = (Border)sender;
                    lastClickTime = DateTime.Now;
                };

                UserStackPanel.Children.Add(border);
            }
        }

        private void EditMember_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedMember == null)
            {
                MessageBox.Show("Selecteer een lid");
                return;
            }

            _mainWindow.MainContent.Navigate(new AdminEditUser(_mainWindow, SelectedMember.Id));
        }


        private void FilterUsers(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            List<Member> selectedMemberList = new List<Member>();

            if (clickedButton != null)
            {
                // Use the Name property to identify which button was clicked
                switch (clickedButton.Name)
                {
                    case "Id":
                        selectedMemberList = _memberList.OrderBy(member => member.Id).ToList();
                        break;
                    case "Voornaam":
                        selectedMemberList = _memberList.OrderBy(member => member.FirstName).ToList();
                        break;
                    case "Tussenv":
                        selectedMemberList = _memberList.OrderBy(member => member.Infix).ToList();
                        break;
                    case "Achternaam":
                        selectedMemberList = _memberList.OrderBy(member => member.LastName).ToList();
                        break;
                    case "Email":
                        selectedMemberList = _memberList.OrderBy(member => member.Email).ToList();
                        break;
                    case "Rollen":
                        selectedMemberList = _memberList.OrderBy(member => member.RolesString).ToList();
                        break;
                }
                PopulateUserList(selectedMemberList);
            }
        }
    }
}

/* Extra xaml shit:

 // public void SortMember(object sender, RoutedEventArgs routedEventArgs)
        // {
        //     try
        //     {
        //         ContextMenu sendercast = (System.Windows.Controls.ContextMenu)sender;
        //         string[] validStrings = { "Id", "FirstName", "LastName", "Email" };
        //         if (validStrings.Contains(sendercast.Name))
        //         {
        //             MenuItem routedEventArgsCast = (MenuItem)routedEventArgs.Source;
        //             if (routedEventArgsCast.Header.ToString() == "Ascending")
        //             {
        //                 UserList.Items.SortDescriptions.Clear();
        //                 UserList.Items.SortDescriptions.Add(new SortDescription(sendercast.Name,
        //                     ListSortDirection.Ascending));
        //             }
        //             else
        //             {
        //                 UserList.Items.SortDescriptions.Clear();
        //                 UserList.Items.SortDescriptions.Add(new SortDescription(sendercast.Name,
        //                     ListSortDirection.Descending));
        //             }
        //         }
        //     }
        //     catch (Exception e)
        //     {
        //         Debug.WriteLine(e);
        //     }
        // }
        //
        // public void UpdateFilter(object sender, RoutedEventArgs routedEventArgs)
        // {
        //     CollectionViewSource.GetDefaultView(UserList.ItemsSource).Refresh();
        // }

        // private bool Filter(object item)
        // {
        //     List<bool> result = new List<bool>();
        //     // filter ID
        //     if (String.IsNullOrEmpty(IdFilter.Text))
        //     {
        //         result.Add(true);
        //     }
        //     else
        //     {
        //         result.Add(((item as Member).Id.ToString()
        //             .IndexOf($"{IdFilter.Text}", StringComparison.OrdinalIgnoreCase) >= 0));
        //     }
        //
        //     //Filter FirstName
        //     if (String.IsNullOrEmpty(FirstNameFilter.Text))
        //     {
        //         result.Add(true);
        //     }
        //     else
        //     {
        //         result.Add(((item as Member).FirstName.IndexOf($"{FirstNameFilter.Text}",
        //             StringComparison.OrdinalIgnoreCase) >= 0));
        //     }
        //
        //     //Filter LastName
        //     if (String.IsNullOrEmpty(LastNameFilter.Text))
        //     {
        //         result.Add(true);
        //     }
        //     else
        //     {
        //         result.Add(((item as Member).LastName.IndexOf($"{LastNameFilter.Text}",
        //             StringComparison.OrdinalIgnoreCase) >= 0));
        //     }
        //
        //     //Filter Email
        //     if (String.IsNullOrEmpty(EmailFilter.Text))
        //     {
        //         result.Add(true);
        //     }
        //     else
        //     {
        //         result.Add(((item as Member).Email.IndexOf($"{EmailFilter.Text}", StringComparison.OrdinalIgnoreCase) >=
        //                     0));
        //     }
        //
        //     //Filter Roles
        //     if (String.IsNullOrEmpty(RolesFilter.Text))
        //     {
        //         result.Add(true);
        //     }
        //     else
        //     {
        //         result.Add(((item as Member).RolesString.IndexOf($"{RolesFilter.Text}",
        //             StringComparison.OrdinalIgnoreCase) >= 0));
        //     }
        //
        //     return !result.Contains(false);
        // }


 <!-- <StackPanel Background="White"> -->
                <!--     <ListView x:Name="UserList" ItemsSource="{Binding _memberList}" BorderBrush="Transparent" -->
                <!--               SelectedItem="{Binding SelectedMember}" -->
                <!--               Margin="40,20,40,20"> -->
                <!-- -->
                <!--         <ListView.Resources> -->
                <!--             ~1~ Modern style for GridViewColumnHeader @1@ -->
                <!--             <Style TargetType="GridViewColumnHeader"> -->
                <!--                 <Setter Property="HorizontalContentAlignment" Value="Left" /> -->
                <!--                 <Setter Property="Padding" Value="10" /> -->
                <!--                 <Setter Property="Background" Value="#1892cd" /> -->
                <!--                 <Setter Property="Foreground" Value="#e8f6fc" /> -->
                <!--                 <Setter Property="FontWeight" Value="Bold" /> -->
                <!--                 <Setter Property="FontSize" Value="14" /> -->
                <!--             </Style> -->
                <!-- -->
                <!--             ~1~ Modern style for ListViewItem @1@ -->
                <!--             <Style TargetType="ListViewItem"> -->
                <!--                 <Setter Property="HorizontalContentAlignment" Value="Stretch" /> -->
                <!--                 <Setter Property="Padding" Value="10" /> -->
                <!--                 <Setter Property="Margin" Value="2" /> -->
                <!--                 <Setter Property="Background" Value="#e8f6fc" /> -->
                <!--                 <Setter Property="BorderBrush" Value="#031017" /> -->
                <!--                 <Setter Property="BorderThickness" Value="0,0,0,1" /> -->
                <!--                 <Setter Property="FontSize" Value="14" /> -->
                <!--                 <Style.Triggers> -->
                <!--                     <Trigger Property="IsMouseOver" Value="True"> -->
                <!--                         <Setter Property="Background" Value="#e8f6fc" /> -->
                <!--                     </Trigger> -->
                <!--                     <Trigger Property="IsSelected" Value="True"> -->
                <!--                         <Setter Property="Background" Value="#007ACC" /> -->
                <!--                         <Setter Property="Foreground" Value="White" /> -->
                <!--                     </Trigger> -->
                <!--                 </Style.Triggers> -->
                <!--             </Style> -->
                <!-- -->
                <!--             ~1~ Add placeholder text functionality @1@ -->
                <!--             <Style TargetType="TextBox"> -->
                <!--                 <Setter Property="Template"> -->
                <!--                     <Setter.Value> -->
                <!--                         <ControlTemplate TargetType="TextBox"> -->
                <!--                             <Grid> -->
                <!--                                 <TextBox x:Name="textBox" Text="{TemplateBinding Text}" -->
                <!--                                          Background="{TemplateBinding Background}" -->
                <!--                                          BorderBrush="{TemplateBinding BorderBrush}" -->
                <!--                                          BorderThickness="{TemplateBinding BorderThickness}" -->
                <!--                                          Foreground="{TemplateBinding Foreground}" /> -->
                <!--                                 <TextBlock x:Name="placeholderTextBlock" -->
                <!--                                            Text="{Binding RelativeSource={RelativeSource TemplatedParent}, Path=Tag}" -->
                <!--                                            Foreground="Gray" Margin="5,0,0,0" IsHitTestVisible="False" -->
                <!--                                            VerticalAlignment="Center" /> -->
                <!--                             </Grid> -->
                <!--                             <ControlTemplate.Triggers> -->
                <!--                                 <Trigger Property="Text" Value=""> -->
                <!--                                     <Setter TargetName="placeholderTextBlock" Property="Visibility" -->
                <!--                                             Value="Visible" /> -->
                <!--                                 </Trigger> -->
                <!--                                 <Trigger Property="Text" Value="{x:Null}"> -->
                <!--                                     <Setter TargetName="placeholderTextBlock" Property="Visibility" -->
                <!--                                             Value="Visible" /> -->
                <!--                                 </Trigger> -->
                <!--                                 <Trigger Property="Text" Value="{x:Static sys:String.Empty}"> -->
                <!--                                     <Setter TargetName="placeholderTextBlock" Property="Visibility" -->
                <!--                                             Value="Visible" /> -->
                <!--                                 </Trigger> -->
                <!--                                 <Trigger Property="Text" Value="*"> -->
                <!--                                     <Setter TargetName="placeholderTextBlock" Property="Visibility" -->
                <!--                                             Value="Collapsed" /> -->
                <!--                                 </Trigger> -->
                <!--                             </ControlTemplate.Triggers> -->
                <!--                         </ControlTemplate> -->
                <!--                     </Setter.Value> -->
                <!--                 </Setter> -->
                <!--             </Style> -->
                <!--         </ListView.Resources> -->
                <!-- -->
                <!--         <ListView.View> -->
                <!--             <GridView> -->
                <!--                 <GridViewColumn Header="ID" DisplayMemberBinding="{Binding Id}" Width="40" /> -->
                <!--                 <GridViewColumn Header="First Name" DisplayMemberBinding="{Binding FirstName}" -->
                <!--                                 Width="100" /> -->
                <!--                 <GridViewColumn Header="Infix" DisplayMemberBinding="{Binding Infix}" -->
                <!--                                 Width="70" /> -->
                <!--                 <GridViewColumn Header="Last Name" DisplayMemberBinding="{Binding LastName}" -->
                <!--                                 Width="150" /> -->
                <!--                 <GridViewColumn Header="Email" DisplayMemberBinding="{Binding Email}" Width="200" /> -->
                <!--                 <GridViewColumn Header="Roles" DisplayMemberBinding="{Binding RolesString}" Width="150" /> -->
                <!--             </GridView> -->
                <!--         </ListView.View> -->
                <!--     </ListView> -->
                <!-- </StackPanel> -->

                <!-- <ListView Visibility="Hidden" x:Name="___UserList_" Margin="0,14,0,0" VerticalAlignment="Top" -->
                <!--           d:ItemsSource="{d:SampleData ItemCount=5}" HorizontalAlignment="Center" Grid.Row="1" -->
                <!--           ScrollViewer.CanContentScroll="True" ScrollViewer.VerticalScrollBarVisibility="Visible" -->
                <!--           MaxHeight="700" SelectedItem="{Binding SelectedMember}"> -->
                <!--     <ListView.View> -->
                <!--         <GridView> -->
                <!--             <GridViewColumn DisplayMemberBinding="{Binding Id}"> -->
                <!--                 <GridViewColumnHeader> -->
                <!--                     <StackPanel> -->
                <!--                         <TextBlock Text="Id" MinWidth="60" /> -->
                <!--                         <TextBox Name="IdFilter" TextChanged="UpdateFilter"></TextBox> -->
                <!--                     </StackPanel> -->
                <!--                     <GridViewColumnHeader.ContextMenu> -->
                <!--                         <ContextMenu MenuItem.Click="SortMember" Name="Id"> -->
                <!--                             <MenuItem Header="Ascending" /> -->
                <!--                             <MenuItem Header="Descending" /> -->
                <!--                         </ContextMenu> -->
                <!--                     </GridViewColumnHeader.ContextMenu> -->
                <!--                 </GridViewColumnHeader> -->
                <!--             </GridViewColumn> -->
                <!-- -->
                <!--             <GridViewColumn DisplayMemberBinding="{Binding FirstName}"> -->
                <!--                 <GridViewColumnHeader> -->
                <!--                     <StackPanel> -->
                <!--                         <TextBlock Text="FirstName" MinWidth="60" /> -->
                <!--                         <TextBox Name="FirstNameFilter" TextChanged="UpdateFilter"></TextBox> -->
                <!--                     </StackPanel> -->
                <!--                     <GridViewColumnHeader.ContextMenu> -->
                <!--                         <ContextMenu MenuItem.Click="SortMember" Name="FirstName"> -->
                <!--                             <MenuItem Header="Ascending" /> -->
                <!--                             <MenuItem Header="Descending" /> -->
                <!--                         </ContextMenu> -->
                <!--                     </GridViewColumnHeader.ContextMenu> -->
                <!--                 </GridViewColumnHeader> -->
                <!--             </GridViewColumn> -->
                <!-- -->
                <!--             <GridViewColumn Header="Infix" DisplayMemberBinding="{Binding  Infix}" /> -->
                <!-- -->
                <!--             <GridViewColumn DisplayMemberBinding="{Binding LastName}"> -->
                <!--                 <GridViewColumnHeader> -->
                <!--                     <StackPanel> -->
                <!--                         <TextBlock Text="LastName" MinWidth="60" /> -->
                <!--                         <TextBox Name="LastNameFilter" TextChanged="UpdateFilter"></TextBox> -->
                <!--                     </StackPanel> -->
                <!--                     <GridViewColumnHeader.ContextMenu> -->
                <!--                         <ContextMenu MenuItem.Click="SortMember" Name="LastName"> -->
                <!--                             <MenuItem Header="Ascending" /> -->
                <!--                             <MenuItem Header="Descending" /> -->
                <!--                         </ContextMenu> -->
                <!--                     </GridViewColumnHeader.ContextMenu> -->
                <!--                 </GridViewColumnHeader> -->
                <!--             </GridViewColumn> -->
                <!-- -->
                <!--             <GridViewColumn DisplayMemberBinding="{Binding Email}"> -->
                <!--                 <GridViewColumnHeader> -->
                <!--                     <StackPanel> -->
                <!--                         <TextBlock Text="Email" MinWidth="60" /> -->
                <!--                         <TextBox Name="EmailFilter" TextChanged="UpdateFilter"></TextBox> -->
                <!--                     </StackPanel> -->
                <!--                     <GridViewColumnHeader.ContextMenu> -->
                <!--                         <ContextMenu MenuItem.Click="SortMember" Name="Email"> -->
                <!--                             <MenuItem Header="Ascending" /> -->
                <!--                             <MenuItem Header="Descending" /> -->
                <!--                         </ContextMenu> -->
                <!--                     </GridViewColumnHeader.ContextMenu> -->
                <!--                 </GridViewColumnHeader> -->
                <!--             </GridViewColumn> -->
                <!-- -->
                <!--             <GridViewColumn DisplayMemberBinding="{Binding RolesString}"> -->
                <!--                 <GridViewColumnHeader> -->
                <!--                     <StackPanel> -->
                <!--                         <TextBlock Text="Roles" MinWidth="60" /> -->
                <!--                         <TextBox Name="RolesFilter" TextChanged="UpdateFilter"></TextBox> -->
                <!--                     </StackPanel> -->
                <!--                     <GridViewColumnHeader.ContextMenu> -->
                <!--                         <ContextMenu MenuItem.Click="SortMember" Name="Roles"> -->
                <!--                             <MenuItem Header="Ascending" /> -->
                <!--                             <MenuItem Header="Descending" /> -->
                <!--                         </ContextMenu> -->
                <!--                     </GridViewColumnHeader.ContextMenu> -->
                <!--                 </GridViewColumnHeader> -->
                <!--             </GridViewColumn> -->
                <!-- -->
                <!--         </GridView> -->
                <!--     </ListView.View> -->
                <!-- </ListView> -->
 */