using PS_Windows.model;
using PS_Windows.services;
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

// Pour plus d'informations sur le modèle d'élément Boîte de dialogue de contenu, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace PS_Windows.dialogs
{
    public sealed partial class ModuleDialog : ContentDialog
    {
        public Module result { get; private set; }

        private String unitId;

        public enum ModuleDialogMode
        {
            CREATE,
            EDIT
        }

        private ModuleDialogMode mode;

        public ModuleDialog(Module module, String unitId, ModuleDialogMode mode) {
            this.InitializeComponent();
            this.mode = mode;
            this.result = module;
            this.unitId = unitId;
        }

        private async void handleClickSave(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            if (this.mode == ModuleDialogMode.CREATE) { // Create
                this.result = await PsApi.getInstance().createModule(this.result, this.unitId);
            }
            else if (this.mode == ModuleDialogMode.EDIT) { // Update
                this.result = await PsApi.getInstance().updateModule(this.result, this.unitId);
            }
        }

        private void handleClickCancel(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
        }
    }
}