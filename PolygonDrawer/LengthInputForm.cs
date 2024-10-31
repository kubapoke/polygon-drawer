namespace PolygonDrawer
{
    public partial class LengthInputForm : Form
    {
        public double? Result = null;
        private Func<double, bool> lengthValidator;
        public LengthInputForm(double initialValue, Func<double, bool> validator)
        {
            InitializeComponent();

            lengthTextBox.Text = $"{Math.Round(initialValue, 1)}";
            lengthValidator = validator;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (double.TryParse(lengthTextBox.Text, out double value) && value >= 1 && value <= 1000 && lengthValidator(value))
                {
                    Result = value;
                    DialogResult = DialogResult.OK;
                }
                else if (value < 1 || value > 1000)
                {
                    MessageBox.Show("Please enter a valid number between 1 and 1000.");
                }
                else
                {
                    MessageBox.Show("Selecting this length would create an impossible shape.");
                }
            }
            else if (keyData == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (double.TryParse(lengthTextBox.Text, out double value))
            {
                Result = value;
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Please enter a valid number.");
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
