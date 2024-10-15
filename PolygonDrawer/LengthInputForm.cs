namespace PolygonDrawer
{
    public partial class LengthInputForm : Form
    {
        public double? Result = null;
        public LengthInputForm(double initialValue)
        {
            InitializeComponent();

            lengthTextBox.Text = $"{Math.Round(initialValue, 1)}";
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
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
