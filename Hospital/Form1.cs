using Hospital.application.usecases;

namespace Hospital
{
    public partial class Form1 : Form
    {
        private OrderUseCase _orderUseCase;
        public Form1( OrderUseCase orderUseCase)
        {
            InitializeComponent();
            this._orderUseCase = orderUseCase;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("al hacer click en el boton mueste un mensaje" + textBox1.Text);

            _orderUseCase.CreateOrder(null);
        }
    }
}
