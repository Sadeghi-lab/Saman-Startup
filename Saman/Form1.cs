using Saman.FTPFunctions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
    
namespace Saman
{
    public partial class Form1 : Form
    {
        private string selectedCategory = "";
        Directories dir;
        public Form1()
        {
            InitializeComponent();
            dir = new Directories();
        }

        void toolitemStyle(ref ToolStripButton item)
        {
            Font font = new Font("Segoe UI",15);
            item.ForeColor = Color.White;
            item.Font = font;
            item.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            item.ImageAlign = ContentAlignment.MiddleLeft;
            item.Width = 200;
            item.Image = imageList1.Images[0];
            item.Click += Item_Click;

        }

        Panel createlabelItem(string name,string version,string publish,string des)
        {
            Panel label = new Panel();
            label.Width = 668;
            label.Height= 104;
            label.BackColor=Color.White;
            label.Location = new Point(19,57);
            label.BorderStyle = BorderStyle.FixedSingle;
            label.BackColor = System.Drawing.ColorTranslator.FromHtml("#25263b");

            Label lblname = new Label();
            lblname.BackColor=Color.White;
            lblname.Font = new Font("Segoe UI", 10,FontStyle.Bold, GraphicsUnit.Point);
            lblname.Text = name;
            lblname.Location=new Point(11,18);
            lblname.BackColor= System.Drawing.ColorTranslator.FromHtml("#25263b");
            lblname.ForeColor=Color.White;
            
            label.Controls.Add(lblname);

            Label lblversion = new Label();
            lblversion.BackColor = Color.White;
            lblversion.Font = new Font("Segoe UI", 10, FontStyle.Bold, GraphicsUnit.Point);
            lblversion.Text = version;
            lblversion.Location = new Point(11, 46);
            lblversion.BackColor = System.Drawing.ColorTranslator.FromHtml("#25263b");
            lblversion.ForeColor = Color.White;

            label.Controls.Add(lblversion);


            Label lblpub = new Label();
            lblpub.BackColor = Color.White;
            lblpub.Font = new Font("Segoe UI", 10, FontStyle.Bold, GraphicsUnit.Point);
            lblpub.Text = publish;
            lblpub.Location = new Point(11, 74);
            lblpub.BackColor = System.Drawing.ColorTranslator.FromHtml("#25263b");
            lblpub.ForeColor = Color.White;

            label.Controls.Add(lblpub);

            Label lbldes = new Label();
            lbldes.BackColor = Color.White;
            lbldes.Font = new Font("Segoe UI", 10,  GraphicsUnit.Point);
            lbldes.Text = des;
            lbldes.TextAlign = ContentAlignment.TopLeft;
            lbldes.Size = new Size(450, 82);
            lbldes.BackColor = System.Drawing.ColorTranslator.FromHtml("#25263b");
            lbldes.ForeColor = Color.White;

            lbldes.Location = new Point(142,9);

            label.Controls.Add(lbldes);



            PictureBox picture = new PictureBox();
            picture.Location = new Point(596, 20);
            picture.Image=Saman.Properties.Resources.down;
            picture.SizeMode = PictureBoxSizeMode.StretchImage;
            picture.Size = new Size(new Point(67, 62));
            picture.Tag=name;
            picture.Click += Download_Click;

            label.Controls.Add(picture);



            return label;
        }
        void loaddirectories()
        {
            try
            {
                var directories = new FTPFunctions.Directories().GetDirectories();
                foreach (var item in directories)
                {
                    ToolStripButton btn = new ToolStripButton();
                    btn.Text = item + "                    ";
                    toolitemStyle(ref btn);
                    toolStrip1.Items.Add(btn);

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("ارتباط با اینترنت شناسایی نشد"+Environment.NewLine+ex.Message,"",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            loaddirectories();
            panel3.BackColor = System.Drawing.ColorTranslator.FromHtml("#1b1b31");
            groupBox1.BackColor= System.Drawing.ColorTranslator.FromHtml("#25263b");
            toolStrip1.BackColor= System.Drawing.ColorTranslator.FromHtml("#25263b");
            
            //item1.BackColor= System.Drawing.ColorTranslator.FromHtml("#25263b");
            //item2.BackColor= System.Drawing.ColorTranslator.FromHtml("#25263b");
            //item3.BackColor= System.Drawing.ColorTranslator.FromHtml("#25263b");
            //item4.BackColor= System.Drawing.ColorTranslator.FromHtml("#25263b");
            //item5.BackColor= System.Drawing.ColorTranslator.FromHtml("#25263b");
        }

        private void Item_Click(object sender, EventArgs e)
        {
            selectedCategory = (sender as ToolStripButton).Text;
            var files = dir.GetFilesName(selectedCategory);
            panel3.Controls.Clear();
            int count = 0;
            foreach (var file in files)
            {
                count++;
                var item = createlabelItem(file, "...", "....", "");
                if (count == 1) item.Top = 19;
                else
                    item.Top = item.Height + 30;
                panel3.Controls.Add(item);
            }
        }



        private void Download_Click(object sender, EventArgs e)
        {
            string filename = (sender as PictureBox).Tag.ToString();
            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.FileName = filename;
            savedialog.ShowDialog();
            if (string.IsNullOrEmpty(savedialog.FileName) || savedialog.FileName == filename )  return ;

            
            var res = dir.Download(selectedCategory,filename,savedialog.FileName);
            
            if(res.result)
            {
                MessageBox.Show("دانلود کامل شد", "دانلود کامل شد", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }else
            {
                MessageBox.Show(res.message, "دانلود با خطا مواجه شده است", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}   
