using System.Linq;
using System.Windows.Forms;
using pwiz.Skyline.Controls.GroupComparison;
using pwiz.Skyline.Model;
using pwiz.Skyline.Model.DocSettings;
using pwiz.Skyline.Model.GroupComparison;
using pwiz.Skyline.Properties;
using pwiz.Skyline.Util;

namespace pwiz.Skyline.SettingsUI
{
    public partial class DocumentSettingsDlg : FormEx
    {
        private readonly SettingsListBoxDriver<AnnotationDef> _annotationsListBoxDriver;
        private readonly SettingsListBoxDriver<GroupComparisonDef> _groupComparisonsListBoxDriver;

        public DocumentSettingsDlg(IDocumentContainer documentContainer)
        {
            InitializeComponent();
            Icon = Resources.Skyline;
            DocumentContainer = documentContainer;
            _annotationsListBoxDriver = new SettingsListBoxDriver<AnnotationDef>(
                checkedListBoxAnnotations, Settings.Default.AnnotationDefList);
            _annotationsListBoxDriver.LoadList(
                DocumentContainer.Document.Settings.DataSettings.AnnotationDefs);
            _groupComparisonsListBoxDriver = new SettingsListBoxDriver<GroupComparisonDef>(
                checkedListBoxGroupComparisons, Settings.Default.GroupComparisonDefList);
            _groupComparisonsListBoxDriver.LoadList(
                DocumentContainer.Document.Settings.DataSettings.GroupComparisonDefs);
        }

        public IDocumentContainer DocumentContainer { get; private set; }

        public DataSettings GetDataSettings(DataSettings dataSettings)
        {
            return dataSettings.ChangeAnnotationDefs(_annotationsListBoxDriver.Chosen)
                .ChangeGroupComparisonDefs(_groupComparisonsListBoxDriver.Chosen);
        }

        private void btnAddAnnotation_Click(object sender, System.EventArgs e)
        {
            using (var editDlg = new DefineAnnotationDlg(Settings.Default.AnnotationDefList))
            {
                if (editDlg.ShowDialog(this) == DialogResult.OK)
                {
                    var chosen = _annotationsListBoxDriver.Chosen.ToList();
                    var annotationDef = editDlg.GetAnnotationDef();
                    chosen.Add(annotationDef);
                    Settings.Default.AnnotationDefList.Add(annotationDef);
                    _annotationsListBoxDriver.LoadList(chosen);
                }
            }
        }

        private void btnEditAnnotationList_Click(object sender, System.EventArgs e)
        {
            EditAnnotationList();
        }

        public void EditAnnotationList()
        {
            _annotationsListBoxDriver.EditList();
        }

        public CheckedListBox AnnotationsCheckedListBox { get { return checkedListBoxAnnotations; }}

        private void btnAddGroupComparison_Click(object sender, System.EventArgs e)
        {
            using (var editDlg = new EditGroupComparisonDlg(
                DocumentContainer,
                GroupComparisonDef.EMPTY.ChangeSumTransitions(true),
                Settings.Default.GroupComparisonDefList))
            {
                if (editDlg.ShowDialog(this) == DialogResult.OK)
                {
                    var chosen = _groupComparisonsListBoxDriver.Chosen.ToList();
                    Settings.Default.GroupComparisonDefList.Add(editDlg.GroupComparisonDef);
                    chosen.Add(editDlg.GroupComparisonDef);
                    _groupComparisonsListBoxDriver.LoadList(chosen);
                }
            }
        }

        public CheckedListBox GroupComparisonsCheckedListBox { get { return checkedListBoxGroupComparisons; } }

        private void btnEditGroupComparisonList_Click(object sender, System.EventArgs e)
        {
            EditGroupComparisonList();
        }

        public void EditGroupComparisonList()
        {
            _groupComparisonsListBoxDriver.EditList(DocumentContainer);
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            OkDialog();
        }

        public void OkDialog()
        {
            DialogResult = DialogResult.OK;
        }
    }
}