using PS_Windows.model;
using PS_Windows.services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace PS_Windows.pages
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class Home : Page
    {
        private ObservableCollection<LearningUnit> learningUnits;
        private LearningUnit selectedUnit;

        public Home() {
            this.InitializeComponent();

            this.learningUnits = PsApi.getInstance().learningUnits;
            this.fetchLearningUnits();
        }

        public void fetchLearningUnits() {
            PsApi.getInstance().getLearningUnitsAsync();
            Debug.WriteLine(this.learningUnits.Count);
        }

        private void handleChangeSelectedUnit(Microsoft.UI.Xaml.Controls.NavigationView sender,
            Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args) {
            this.selectedUnit = args.SelectedItem as LearningUnit;
            this.contentFrame.Navigate(typeof(LearningUnitView), this.selectedUnit);
        }

        private void handleClickAddUnit(object sender, TappedRoutedEventArgs e) {
            contentFrame.Navigate(typeof(AddUnit));
        }
    }
}