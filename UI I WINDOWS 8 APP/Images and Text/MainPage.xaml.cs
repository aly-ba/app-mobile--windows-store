using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace UserInterface
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : UserInterface.Common.LayoutAwarePage
    {
        private Frame HiddenFrame = null;

        public MainPage()
        {
            this.InitializeComponent();

            InitializeDemos();

            HiddenFrame = new Windows.UI.Xaml.Controls.Frame();
            HiddenFrame.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            ContentRoot.Children.Add(HiddenFrame);
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        public void LoadDemo(Type demoClass)
        {
            // Load the DemoX.xaml file into the Frame.
            HiddenFrame.Navigate(demoClass, this);

            // Get the top element, the Page, so we can look up the elements
            // that represent the input and output sections of the DemoX file.
            Page hiddenPage = HiddenFrame.Content as Page;

            UIElement output = hiddenPage.FindName("Output") as UIElement;

            // Find the LayoutRoot which parents the output section in the main page.
            Panel panel = hiddenPage.FindName("LayoutRoot") as Panel;

            if (panel != null)
            {
                // Get rid of the content that is currently in the output section.
                panel.Children.Remove(output);

                // Populate the output section with the newly loaded content.
                OutputSection.Content = output;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LoadDemo(typeof(ResourceHierarchy2));
        }

        List<Demo> demos = new List<Demo>
        {
            new Demo() { Title = "Intro Resource", ClassType = typeof(IntroResource) },
            new Demo() { Title = "Resource Hierarchy", ClassType = typeof(ResourceHierarchy) },
            new Demo() { Title = "Resource Hierarchy 1", ClassType = typeof(ResourceHierarchy1) },
            new Demo() { Title = "Resource Hierarchy 2", ClassType = typeof(ResourceHierarchy2) },
            new Demo() { Title = "Application-Level Resources", ClassType = typeof(ApplicationLevelResources) },
            new Demo() { Title = "Resource Dictionary", ClassType = typeof(ResourceDictionary) },
            new Demo() { Title = "Simple Style", ClassType = typeof(SimpleStyle) },
            new Demo() { Title = "Style Inheritance", ClassType = typeof(StyleInheritance) },

        };

        private void InitializeDemos()
        {
            var DemoList = new ObservableCollection<object>();
            int i = 0;

            // Populate the ListBox with the list of scenarios as defined in Constants.cs.
            foreach (var demo in demos)
            {
                ListBoxItem item = new ListBoxItem();
                demo.Title = (++i).ToString() + ".) " + demo.Title;
                item.Content = demo;
                item.Name = demo.ClassType.FullName;
                DemoList.Add(item);
            }

            // Bind the ListBox to the scenario list.
            DemoListBox.ItemsSource = DemoList;
        }

        private void DemoListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DemoListBox.SelectedItem != null)
            {
                ListBoxItem selectedListBoxItem = DemoListBox.SelectedItem as ListBoxItem;
                Demo demo = selectedListBoxItem.Content as Demo;
                LoadDemo(demo.ClassType);
            }

        }
    }
}
