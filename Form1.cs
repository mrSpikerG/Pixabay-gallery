using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pixabayGui
{
    public partial class gallery : Form
    {
        public static int page = 1;
        private static string defaultmsg = "smile";
        private WebClient client = new WebClient();
        public gallery()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.textBox1.TextChanged += findimg;
            this.updateGalery();

        }

        private void findimg(object sender, EventArgs e)
        {
            defaultmsg = this.textBox1.Text == String.Empty ? "smile" : this.textBox1.Text;
            updateGalery();
        }

        public void updateGalery()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    string json = DownloadString($"https://pixabay.com/api/?key=28501180-ed62740d943f5323ad160fea6&q={defaultmsg}&image_type=all&pretty=true&page={page}&per_page=6");
                    Models.ListHits hits = System.Text.Json.JsonSerializer.Deserialize<Models.ListHits>(json);


                    this.pictureBox1.Image = new Bitmap(client.OpenRead(hits.hits[0].previewURL));
                    this.pictureBox2.Image = new Bitmap(client.OpenRead(hits.hits[1].previewURL));
                    this.pictureBox3.Image = new Bitmap(client.OpenRead(hits.hits[2].previewURL));
                    this.pictureBox4.Image = new Bitmap(client.OpenRead(hits.hits[3].previewURL));
                    this.pictureBox5.Image = new Bitmap(client.OpenRead(hits.hits[4].previewURL));
                    this.pictureBox6.Image = new Bitmap(client.OpenRead(hits.hits[5].previewURL));
                }
                catch
                {

                }
            });
        }

        public static string DownloadString(string address)
        {
            WebClient client = new WebClient();
            string reply = client.DownloadString(address);

            return reply;
        }
        public void SaveImage(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile("http://yoururl.com/image.png", "image.png");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                page++;
                this.button2.Enabled = page == 1 ? false : true;

                string json = DownloadString($"https://pixabay.com/api/?key=28501180-ed62740d943f5323ad160fea6&q={defaultmsg}&image_type=all&pretty=true&page={page}&per_page=6");
                this.updateGalery();
            }
            catch (WebException exc)
            {
                this.button1.Enabled = false;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            page--;
            this.button2.Enabled = page == 1 ? false : true;
            this.button1.Enabled = true;
            this.updateGalery();
        }
    }
}
