using PS_Windows.dialogs;
using PS_Windows.model;
using PS_Windows.services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using PS_Windows.Annotations;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace PS_Windows.pages {
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    ///
    public sealed partial class LearningUnitView : Page
    {
        private String learningUnitName;
        private LearningUnit learningUnit;
        private ObservableCollection<Module> modules = new ObservableCollection<Module>();

        public LearningUnitView() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            this.learningUnit = e.Parameter as LearningUnit;
            this.modules.Clear();
            this.learningUnit.modules.ForEach(modules.Add);
            this.learningUnitName = learningUnit.name;
        }

        private async void handleClickCreateModule(object sender, RoutedEventArgs e) {
            var moduleDialog =
                new ModuleDialog(new Module(), this.learningUnit.id, ModuleDialog.ModuleDialogMode.CREATE);
            var buttonClicked = await moduleDialog.ShowAsync();

            if (buttonClicked == ContentDialogResult.Primary) {
                this.modules.Add(moduleDialog.result);
            }
        }

        private async void handleClickAddGrade(object sender, RoutedEventArgs e) {
            Module module = (Module)((Button)sender).DataContext;
            var gradeDialog = new GradeDialog(new Grade(), module.id, GradeDialog.GradeDialogMode.CREATE);
            var buttonClicked = await gradeDialog.ShowAsync();

            module.grades.Add(gradeDialog.result);

            if (buttonClicked == ContentDialogResult.Primary) {
                this.learningUnit = await PsApi.getInstance().getLearningUnitAsync(this.learningUnit.id);
                this.modules.Clear();
                this.learningUnit.modules.ForEach(modules.Add);
            }
        }

        private async void handleClickUpdateModule(object sender, RoutedEventArgs e) {
            Module module = (Module)((Button)sender).DataContext;
            var moduleDialog = new ModuleDialog(module, module.id, ModuleDialog.ModuleDialogMode.EDIT);
            var buttonClicked = await moduleDialog.ShowAsync();

            if (buttonClicked == ContentDialogResult.Primary) {

                var index = this.modules.IndexOf(module);
                modules.RemoveAt(index);
                modules.Insert(index, module);
            }
        }

        private void handleClickDeleteModule(object sender, RoutedEventArgs e) {
            Module module = (Module)((Button)sender).DataContext;
            PsApi.getInstance().deleteModule(module.id, this.learningUnit.id);
            this.modules.Remove(module);
        }

        private void handleClickDeleteUnit(object sender, RoutedEventArgs e) {
            PsApi.getInstance().deleteUnit(this.learningUnit.id);
            this.Frame.Navigate(typeof(Empty));
        }

        private async void handleClickDeleteGrade(object sender, RoutedEventArgs e) {
            Grade grade = (Grade)((Button)sender).DataContext;
            Module module = modules.FirstOrDefault(m => m.grades.Select(g => grade.id).Contains(grade.id));
            PsApi.getInstance().deleteGrade(grade.id, module.id);
            module.grades.Remove(grade);
            int index = this.modules.IndexOf(module);
            modules.RemoveAt(index);
            modules.Insert(index, module);

        }

        private async void handleClickUpdateGrade(object sender, RoutedEventArgs e) {
            Grade grade = (Grade)((Button)sender).DataContext;
            var gradeDialog = new GradeDialog(grade, grade.id, GradeDialog.GradeDialogMode.EDIT);
            var buttonClicked = await gradeDialog.ShowAsync();

            if (buttonClicked == ContentDialogResult.Primary) {
                Module module = modules.FirstOrDefault(m => m.grades.Select(g => grade.id).Contains(grade.id));
                grade.name = gradeDialog.result.name;
                grade.coefficient = gradeDialog.result.coefficient;
                grade.score = gradeDialog.result.score;
                module.grades.Remove(grade);
                module.grades.Add(grade);

                var index = this.modules.IndexOf(module);
                modules.RemoveAt(index);
                modules.Insert(index, module);
            }
        }

        private async void handleClickUpdateUnit(object sender, RoutedEventArgs e) {
            var unitDialog = new UnitDialog(this.learningUnit);
            var buttonClicked = await unitDialog.ShowAsync();

            if (buttonClicked == ContentDialogResult.Primary) {
                Debug.WriteLine(unitDialog.result.name);
                this.learningUnitName = unitDialog.result.name;
            }
        }
    }
}