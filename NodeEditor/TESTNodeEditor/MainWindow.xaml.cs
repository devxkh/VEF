using DLL.NodeEditor.Nodes;
using DLL.NodeEditor.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
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
using VEF.Model.PFExplorer;
using VEF.Utils;
using VEF.VEF_Helpers;
using VEX.Core.Model.Project;
using VEX.Core.Shared.Model.Scene.Objects.ChildObject.Animation;
using VEX.Core.Shared.ViewModel.Editor;
using VEX.Core.Shared.ViewModel.Editor.AnimationNodes;
using VEX.Model.Scene;
using VEX.Model.Scene.Model;

namespace TESTNodeEditor
{
    //[DataContract]
    //public class TestConnectionViewModel : ConnectionViewModel
    //{
    // //   public override OutputConnectorViewModel From { get; set; }
    //  //  public override InputConnectorViewModel To { get; set; }

    //    //   public NodeConnection NodeConnection { get; set; }
    //}

    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    //[KnownType(typeof(TestConnectionViewModel))]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               // ObservableCollection<ConnectionViewModel> test = new ObservableCollection<ConnectionViewModel>();

                AnimationNodeViewModel evm = new AnimationNodeViewModel();
                OutputNodeViewModel onvm = new OutputNodeViewModel();

                OutputConnectorViewModel opcvm = new OutputConnectorViewModel() {  Element = evm };
                InputConnectorViewModel ipcvm = new InputConnectorViewModel() { Element = onvm };

                var cvm = new ConnectionViewModel() { From = opcvm, To = ipcvm };
                //test.Add();

                List<Type> knownTypes = new List<Type>() { typeof(AnimationNodeViewModel), typeof(OutputNodeViewModel) } ;

                AnimationComponent ac = new AnimationComponent();
                ac.FB_AnimationComponent.AnimationBlendTree.AnimNodes.Add(evm);
                ac.FB_AnimationComponent.AnimationBlendTree.AnimNodes.Add(onvm);
                ac.FB_AnimationComponent.AnimationBlendTree.NodeConnections.Add(cvm);
                
               ObjectSerialize.Serialize(ac, "./test", knownTypes);

            //   var testRes = ObjectSerialize.Deserialize<AnimationComponent>("./test");
               var testRes = ObjectSerialize.Deserialize<VEXProjectModel>(@"F:\Projekte\coop\XGame\data\Editor\New VEX Project xyy.oideProj");
           }
            catch (Exception ex)
            {

            }

        }

        //private GraphViewModel ViewModel
        //{
        //    get { return (GraphViewModel)DataContext; }
        //}
    }
}
