using PS_Windows.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace PS_Windows.services
{
    class PsApi
    {
        // Singleton
        private static PsApi INSTANCE = new PsApi();

        public static PsApi getInstance() {
            return INSTANCE;
        }

        private readonly static String url = "https://passage-simulator.herokuapp.com";
        private HttpClient http;

        private bool authenticated = false;
        private Token token;

        private ObservableCollection<LearningUnit> _learningUnits = new ObservableCollection<LearningUnit>();

        public ObservableCollection<LearningUnit> learningUnits
        {
            get { return this._learningUnits; }
        }

        private PsApi() {
            this.http = new HttpClient();
        }


        public async Task<Boolean> authenticateAsync(String login, String password) {
            HttpResponseMessage response =
                await this.http.PostAsJsonAsync(url + "/authenticate", new LoginInfos(login, password));
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode) {
                this.token = await response.Content.ReadAsAsync<Token>();
                this.http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", this.token.token);
                return true;
            }

            return false;
        }


        public async void getLearningUnitsAsync() {
            HttpResponseMessage response = await this.http.GetAsync(url + "/api/learningunit");
            if (response.IsSuccessStatusCode) {
                List<LearningUnit> units = await response.Content.ReadAsAsync<List<LearningUnit>>();
                this._learningUnits.Clear();
                units.ForEach(this._learningUnits.Add);
            }
        }

        public async void deleteUnit(String id) {
            String endpoint = url + "/api/learningunit/" + id;
            HttpResponseMessage response = await this.http.DeleteAsync(endpoint);

            if (response.IsSuccessStatusCode) {
                var learningUnit = this._learningUnits.FirstOrDefault(unit => unit.id.Equals(id));
                this._learningUnits.Remove(learningUnit);
            }
            else {
                ContentDialog dialog = new ContentDialog()
                {
                    Title = response.StatusCode,
                    Content = endpoint,
                    PrimaryButtonText = "OK"
                };
                await dialog.ShowAsync();
            }
        }

        public async Task addLearningUnitAsync(LearningUnit learningUnit) {
            HttpResponseMessage response = await this.http.PostAsJsonAsync(url + "/api/learningunit", learningUnit);

            if (response.IsSuccessStatusCode) {
                LearningUnit unit = await response.Content.ReadAsAsync<LearningUnit>();
                this._learningUnits.Add(unit);
            }
            else {
                ContentDialog dialog = new ContentDialog()
                {
                    Title = response.StatusCode
                };
                await dialog.ShowAsync();
            }
        }

        public async Task<Module> createModule(Module result, String unitId) {
            HttpResponseMessage response =
                await this.http.PostAsJsonAsync(url + $"/api/learningunit/{unitId}/module", result);

            if (response.IsSuccessStatusCode) {
                Module module = await response.Content.ReadAsAsync<Module>();

                var unit = this._learningUnits.FirstOrDefault(learningUnit => learningUnit.id.Equals(unitId));
                unit.modules.Add(module);
                return module;
            }

            return null;
        }

        public async Task<Module> updateModule(Module updatedModule, String unitId) {
            HttpResponseMessage response =
                await this.http.PostAsJsonAsync(url + $"/api/module/{updatedModule.id}", updatedModule);

            if (!response.IsSuccessStatusCode) {
                return null;
            }

            var module = await response.Content.ReadAsAsync<Module>();

            var unit = this._learningUnits.FirstOrDefault(learningUnit => learningUnit.id.Equals(unitId));
            var moduleToDelete = unit.modules.FirstOrDefault(m => m.id.Equals(module.id));

            unit.modules.Remove(moduleToDelete);
            unit.modules.Add(module);

            return module;

        }

        public async void deleteModule(String moduleId, String unitId) {
            HttpResponseMessage response =
                await this.http.DeleteAsync(url + $"/api/learningunit/{unitId}/module/{moduleId}");
            Debug.WriteLine(response.StatusCode);
            this.getLearningUnitsAsync();
        }

        public async Task<Grade> createGrade(Grade result, string moduleId) {
            HttpResponseMessage response =
                await this.http.PostAsJsonAsync(url + $"/api/module/{moduleId}/grade", result);

            if (response.IsSuccessStatusCode) {
                Grade grade = await response.Content.ReadAsAsync<Grade>();
                this.getLearningUnitsAsync();
                return grade;
            }

            return null;
        }

        public async Task<Grade> updateGrade(Grade grade, string gradeId) {
            HttpResponseMessage response =
                await this.http.PutAsJsonAsync(url + $"/api/grade/{gradeId}", grade);
            if (response.IsSuccessStatusCode) {
                Grade updatedGrade = await response.Content.ReadAsAsync<Grade>();
                this.getLearningUnitsAsync();
                return grade;
            }

            return null;
        }

        public async void deleteGrade(string gradeId, string moduleId) {
            HttpResponseMessage response =
                await this.http.DeleteAsync(url + $"/api/module/{moduleId}/grade/{gradeId}");
            this.getLearningUnitsAsync();
        }

        public async Task<LearningUnit> getLearningUnitAsync(string learningUnitId) {
            HttpResponseMessage response =
                await this.http.GetAsync(url + $"/api/learningunit/{learningUnitId}");

            if (response.IsSuccessStatusCode) {
                LearningUnit learningUnit = await response.Content.ReadAsAsync<LearningUnit>();
                return learningUnit;
            }

            return null;
        }


        public async Task<LearningUnit> updateUnit(LearningUnit unit) {
            HttpResponseMessage response =
                await this.http.PutAsJsonAsync(url + $"/api/learningunit/{unit.id}", unit);

            if (response.IsSuccessStatusCode) {
                LearningUnit learningUnit = await response.Content.ReadAsAsync<LearningUnit>();
                this.getLearningUnitsAsync();
                return learningUnit;
            }

            return null;
        }
    }
}