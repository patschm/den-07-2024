namespace Calculator;

public partial class CalculatorApp : Form
{
    private SynchronizationContext? _main;
    public CalculatorApp()
    {
        _main = SynchronizationContext.Current;
        InitializeComponent();
    }

    private async void button1_Click(object sender, EventArgs e)
    {
        if (int.TryParse(txtA.Text, out int a) && int.TryParse(txtB.Text, out int b))
        {
            //int result = LongAdd(a, b);
            //UpdateAnswer(result);
            //Task.Run(() => LongAdd(a, b)).ContinueWith(pt =>
            //{
            //    _main?.Post(UpdateAnswer!, pt.Result);
            //    //UpdateAnswer(pt.Result);
            //});
            //Task.Delay(100000).Wait();
            int result = await LongAddAsync(a, b);
            UpdateAnswer(result);

            //Task<int> tx = DoeIets(a,b).ConfigureAwait(true);
            //int result = tx.Result;
            // Dead-lock!
            //int result = DoeIets(a,b).Result;
            //UpdateAnswer(result);
        }
    }

    private async Task<int> DoeIets(int a, int b)
    {
        return await LongAddAsync(a, b);
    }

    private void UpdateAnswer(object result)
    {
        lblAnswer.Text = result.ToString();
    }

    private int LongAdd(int a, int b)
    {
        Task.Delay(10000).Wait();
        return a + b;
    }
    private Task<int> LongAddAsync(int a, int b)
    {
        return Task.Run(() => LongAdd(a, b));
    }
}