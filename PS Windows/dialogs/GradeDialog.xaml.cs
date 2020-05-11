using System;
using System.Collections.Generic;
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
using PS_Windows.model;
using PS_Windows.services;

// Pour plus d'informations sur le modèle d'élément Boîte de dialogue de contenu, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace PS_Windows.dialogs
{
    public sealed partial class GradeDialog : ContentDialog
    {
        public Grade result { get; private set; }

        private String moduleId;

        public enum GradeDialogMode
        {
            CREATE,
            EDIT
        }

        private GradeDialogMode mode;

        public GradeDialog(Grade grade, String moduleId, GradeDialogMode mode) {
            this.InitializeComponent();
            this.mode = mode;
            this.result = grade;
            this.moduleId = moduleId;
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender,
            ContentDialogButtonClickEventArgs args) {
            if (this.mode == GradeDialogMode.CREATE) { // Create
                this.result = await PsApi.getInstance().createGrade(this.result, this.moduleId);
            }
            else if (this.mode == GradeDialogMode.EDIT) { // Update
                this.result = await PsApi.getInstance().updateGrade(this.result, this.moduleId);
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
        }
    }
}