using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp18
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";

            fontDialog1.ShowColor = true;
            colorDialog1.FullOpen = true;
        }

        private Stack<TextState> textStateStack = new Stack<TextState>();

        private class TextState
        {
            public string Text { get; set; }
            public Color SelectionColor { get; set; }
            public Font SelectionFont { get; set; }
            public FontStyle SelectionFontStyle { get; set; }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var currentState = new TextState
            {
                Text = richTextBox1.Text,
                SelectionColor = richTextBox1.SelectionColor,
                SelectionFont = richTextBox1.SelectionFont,
                SelectionFontStyle = richTextBox1.SelectionFont.Style
            };
            textStateStack.Push(currentState);
        }

        private void SaveTextState()
        {
            var currentState = new TextState
            {
                Text = richTextBox1.Text,
                SelectionColor = Color.FromArgb(richTextBox1.SelectionColor.ToArgb()), // Создаем новый объект Color
                SelectionFont = new Font(richTextBox1.SelectionFont.FontFamily, richTextBox1.SelectionFont.Size, richTextBox1.SelectionFont.Style), // Создаем новый объект Font
                SelectionFontStyle = richTextBox1.SelectionFont.Style
            };
            textStateStack.Push(currentState);
        }

//Open
private void button2_Click(object sender, EventArgs e)
		{
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            string filename = openFileDialog1.FileName;

            string fileText = System.IO.File.ReadAllText(filename);

            richTextBox1.Text = fileText;

            SaveTextState(); // Сохраняем состояние после загрузки файла
            MessageBox.Show("File open");
        }
        //Save
		private void button3_Click(object sender, EventArgs e)
		{
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            string filename = saveFileDialog1.FileName;

            System.IO.File.WriteAllText(filename, richTextBox1.Text);

            SaveTextState(); // Сохраняем состояние после сохранения файла
            MessageBox.Show("File saved");
        }

        //Cancel
		private void button5_Click(object sender, EventArgs e)
		{
            if (textStateStack.Count > 0)
            {
                var previousState = textStateStack.Pop();
                richTextBox1.Text = previousState.Text; // Восстанавливаем текст
                richTextBox1.SelectionStart = 0;
                richTextBox1.SelectionLength = richTextBox1.Text.Length;
                richTextBox1.SelectionColor = previousState.SelectionColor; // Восстанавливаем цвет
                richTextBox1.SelectionFont = new Font(previousState.SelectionFont.FontFamily, previousState.SelectionFont.Size, previousState.SelectionFontStyle); // Восстанавливаем шрифт и начертание
            }
        }
        private void ApplyTextAttributesToSelectedText(Color color, Font font, FontStyle style)
        {
            SaveTextState(); // Сохраняем текущее состояние текста в стеке
            if (richTextBox1.SelectionLength > 0)
            {
                int selectionStart = richTextBox1.SelectionStart;
                int selectionLength = richTextBox1.SelectionLength;

                richTextBox1.SelectionColor = color;
                richTextBox1.SelectionFont = new Font(font, style); // Устанавливаем цвет и начертание одновременно
                richTextBox1.Select(selectionStart + selectionLength, 0); // Снимаем выделение
            }
        }

        //Font
        private void button6_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();

            DialogResult result = fontDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                Font selectedFont = fontDialog.Font;
                ApplyFontToSelectedText(selectedFont);
            }
        }
        //TextStyle
        private void button7_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectionLength > 0)
            {
                FontStyle newFontStyle = FontStyle.Bold;

                if (richTextBox1.SelectionFont != null && richTextBox1.SelectionFont.Bold)
                {
                    newFontStyle = FontStyle.Regular;
                }

                Font newFont = new Font(
                    richTextBox1.SelectionFont?.FontFamily,
                    richTextBox1.SelectionFont?.Size ?? richTextBox1.Font.Size,
                    newFontStyle);

                ApplyFontStyleToSelectedText(newFontStyle);
            }
        }
        //FontSize
        private void button8_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();

            DialogResult result = fontDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                Font selectedFont = fontDialog.Font;
                ApplyFontToSelectedText(selectedFont);
            }
        }
        //Color
        private void button9_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            DialogResult result = colorDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                Color selectedColor = colorDialog.Color;
                ApplyColorToSelectedText(selectedColor);
            }
        }

        private void ApplyFontToSelectedText(Font font)
        {
            if (richTextBox1.SelectionLength > 0)
            {
                int selectionStart = richTextBox1.SelectionStart;
                int selectionLength = richTextBox1.SelectionLength;

                richTextBox1.SelectionFont = font;
                richTextBox1.Select(selectionStart + selectionLength, 0); // Снять выделение
            }
        }

        private void ApplyFontStyleToSelectedText(FontStyle fontStyle)
        {
            if (richTextBox1.SelectionLength > 0)
            {
                int selectionStart = richTextBox1.SelectionStart;
                int selectionLength = richTextBox1.SelectionLength;

                Font currentFont = richTextBox1.SelectionFont;
                Font newFont = new Font(currentFont, fontStyle);

                richTextBox1.SelectionFont = newFont;
                richTextBox1.Select(selectionStart + selectionLength, 0); // Снять выделение
            }
        }

        private void ApplyColorToSelectedText(Color color)
        {
            if (richTextBox1.SelectionLength > 0)
            {
                int selectionStart = richTextBox1.SelectionStart;
                int selectionLength = richTextBox1.SelectionLength;

                richTextBox1.SelectionColor = color;
                richTextBox1.Select(selectionStart + selectionLength, 0); // Снять выделение
            }
        }

    }
}
