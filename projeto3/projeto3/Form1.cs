/* ******************************************************************
 * Colegio Técnico Antônio Teixeira Fernandes (Univap)
 * Curso Técnico em Informática - Data de Entrega: 06 / 09 / 2022
 * Autores do Projeto: Rodrigo Lopes Marques
 *                     Gustavo Otacílio
 *
 * Turma: 2F
 * Projeto do 3° Bimestre
 * Observação: <colocar se houver>
 * 
 * ******************************************************************/


/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * COMPONENTES E SUAS FUNÇÕES:
 * 
 * textbox1 - Caixa de texto para o envio do valor da compra
 * textbox2 - Caixa de texto para o envio do número de parcelas da compra
 * 
 * label1 - Envio de mensagens de erro na parte de cadastrar parcelas de uma compra
 * label2 - Envio de mensagens de erro e informações ao pagar uma parcela
 * label3 - Envio do valor total restante da compra
 * label4 - Etiqueta para o usuario identificar o que é textbox1
 * label5 - Etiqueta para o usuario identificar o que é textbox2
 * label6 - Etiqueta para o usuario identificar o que o conteúdo da listbox1
 * label7 - Etiqueta para o usuario identificar o que o dateTimePicker2
 * 
 * button1 - Botão para efetuar o cadastro das parcelas de uma compra
 * button2 - Botão para efetuar o pagamento da parcela
 * button3 - Botão para confirmar o pagamento da parcela
 * button4 - Botão para cancelar o pagamento da parcela
 * button5 - Botão para efetuar o pagamento de todas as parcelas pendentes
 * 
 * listbox1 - Lista para guardar as parcelas de uma compra
 * 
 * dateTimePicker1 - Calendário para simular o dia da compra
 * dateTimePicker2 - Calendário para simular o dia do pagamento da parcela
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projeto3
{
    public partial class Form1 : Form
    {
        double valorTotal;
        int indexP;
        double valorParcela;
        int qtdeParcelas;
        DateTime dataCompra;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            resetar();
            label2.Text = "";
            // Validação
            double numero;
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                label1.Text = "Campo valor e/ou número parcelas vazias!";
                return;
            }
            else if (!Double.TryParse(textBox1.Text, out numero) || !Double.TryParse(textBox2.Text, out numero))
            {
                label1.Text = "Valor e/ou Número parcelas inválidas!";
                return;
            }
            else if (valorTotal != 0)
            {
                label1.Text = "Pague todas as parcelas antes de cadastrar mais.";
                return;
            }

            // Recebe os dados
            valorTotal = double.Parse(textBox1.Text);
            qtdeParcelas = int.Parse(textBox2.Text);
            dataCompra = dateTimePicker1.Value.Date;

            label3.Text = String.Format("Total a pagar = {0:C2}", valorTotal);
            valorParcela = valorTotal / qtdeParcelas;


            // Cria as parcelas
            DateTime dataParcela = dataCompra;
            for (int i = 0; i < qtdeParcelas; i++)
            {
                listBox1.Items.Add(String.Format("{0} - {1:C2} - {2}", i + 1, valorParcela, dataParcela.ToString("dd/MM/yyyy")));
                dataParcela = gerarData(dataParcela, 1);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            resetar();
            // Valida se uma parcela foi selecionada
            indexP = listBox1.SelectedIndex;
            if (indexP == -1)
            {
                label2.Text = "Selecione uma parcela para pagar.";
                return;
            }
            else if (indexP != 0)
            {
                label2.Text = "Pague a parcela anterior, antes de efetuar a compra dessa.";
                return;
            }

            // Pega a data do dia do pagamento
            DateTime dataPagamento = dateTimePicker2.Value.Date;
            DateTime dataParcela = gerarData(dataCompra, qtdeParcelas - listBox1.Items.Count);

            if (DateTime.Compare(dataParcela, dataPagamento) < 0)
            {
                double juros = valorParcela * 0.03;
                label2.ForeColor = Color.Red;
                label2.Text = String.Format("Parcela Atrasada!\nValor da parcela reajustado para {0:C2}\nDeseja Pagar?", valorParcela + juros);
            }
            else
            {
                label2.Text = String.Format("Parcela de {0:C2}\nDeseja Pagar?", valorParcela);
            }
            button3.Visible = true;
            button4.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            valorTotal -= valorParcela;
            label3.Text = String.Format("Total a Pagar: {0:C2}", valorTotal);
            listBox1.Items.RemoveAt(indexP);
            label2.Text = "Parcela Paga.";
            resetar();
            label2.ForeColor = Color.Green;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            resetar();
            label2.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (valorTotal == 0)
            {
                label2.Text = "Nenhuma parcela para pagar.";
                return;
            }
            valorTotal = 0;
            label3.Text = String.Format("Total a Pagar: {0:C2}", valorTotal);
            label2.Text = "Todas as parcelas pagas.";
            listBox1.Items.Clear();
            resetar();
            label2.ForeColor = Color.Green;
        }

        private void resetar()
        {
            label1.Text = "";
            label2.ForeColor = Color.Black;
            button3.Visible = false;
            button4.Visible = false;
            indexP = 0;
        }

        private DateTime gerarData(DateTime data, int mes)
        {
            for (int i = 0; i < mes; i++)
            {
                data = data.AddMonths(1);
                int semana = (int)data.DayOfWeek;
                if (semana == 0)
                {
                    data = data.AddDays(1);
                }
                else if (semana == 6)
                {
                    data = data.AddDays(2);
                }
            }

            return data;
        }
    }
}