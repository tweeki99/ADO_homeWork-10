using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicAppEF
{
    public partial class FormMain : Form
    {
        MusicAppEfContext context;
        
        public FormMain()
        {
            InitializeComponent();
            context = new MusicAppEfContext();
            LoadDataGridViewGroups();
        }

        private void ButtonAddGroupClick(object sender, EventArgs e)
        {
            FormAddGroup formAddGroup = new FormAddGroup();
            DialogResult result = formAddGroup.ShowDialog(this);

            if (result == DialogResult.Cancel)
                return;

            var group = context.MusicalGroups.ToList().Where(n => n.Name == formAddGroup.textBox1.Text).FirstOrDefault();
            if (group != null)
            {
                MessageBox.Show("Группа с таким именем уже существует");
                return;
            }
            
            MusicalGroup musicalGroup = new MusicalGroup();
            musicalGroup.Name = formAddGroup.textBox1.Text;
            musicalGroup.Songs = new List<Song>();
            context.MusicalGroups.Add(musicalGroup);
            context.SaveChanges();
            LoadDataGridViewGroups();
        }

        private void ButtonAddSongClick(object sender, EventArgs e)
        {
            if (dataGridViewGroups.SelectedRows.Count > 0)
            {
                FormAddSong formAddSong = new FormAddSong();
                DialogResult result = formAddSong.ShowDialog(this);

                if (result == DialogResult.Cancel)
                    return;
                
                if (formAddSong.textBoxName.Text.Length<1)
                {
                    MessageBox.Show("Вы не указали название песни");
                    return;
                }
                if (formAddSong.richTextBoxText.Text.Length < 1)
                {
                    MessageBox.Show("Вы не указали текст песни");
                    return;
                }

                Song song = new Song();
                song.Name = formAddSong.textBoxName.Text;
                song.Rating = Convert.ToInt32(formAddSong.numericUpDownRating.Value);
                song.Text = formAddSong.richTextBoxText.Text;
                song.MusicalGroupId = context.MusicalGroups.ToList().Where(n => n.Name == dataGridViewGroups.SelectedRows[0].Cells["Name"].Value.ToString()).FirstOrDefault().Id;
                song.MusicalGroup = context.MusicalGroups.ToList().Where(n => n.Name == dataGridViewGroups.SelectedRows[0].Cells["Name"].Value.ToString()).FirstOrDefault();

                context.Songs.Add(song);
                context.SaveChanges();
                LoadDataGridViewGroups();
                LoadDataGridViewSongs();
            }
        }

        private void DataGridViewGroupsSelectionChanged(object sender, EventArgs e)
        {
            LoadDataGridViewSongs();
        }

        private void DataGridViewSongsSelectionChanged(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            
            if (dataGridViewSongs.SelectedRows.Count>0)
            {
                var song = context.Songs.ToList().Where(n => n.MusicalGroup.Name == dataGridViewGroups.SelectedRows[0].Cells["Name"].Value.ToString() &&
                                                   n.Name == dataGridViewSongs.SelectedRows[0].Cells["Name"].Value.ToString()).FirstOrDefault();

                if (song != null)
                    richTextBox1.Text = song.Text;
            }
        }

        private void LoadDataGridViewGroups()
        {
            dataGridViewGroups.Rows.Clear();
            dataGridViewGroups.Columns.Clear();

            dataGridViewGroups.Columns.Add("Name", "Название группы");
            dataGridViewGroups.Columns.Add("Rating", "Рейтинг");
            for (int i = 0; i < context.MusicalGroups.Count(); i++)
            {
                List<string> data = new List<string>();

                data.Add(context.MusicalGroups.ToList()[i].Name);

                if (context.MusicalGroups.ToList()[i].Songs.ToList().Count() > 0)
                    data.Add(context.MusicalGroups.ToList()[i].Songs.Average(r => r.Rating).ToString());
                else data.Add("???");

                dataGridViewGroups.Rows.Add(data.ToArray());
            }
        }

        private void LoadDataGridViewSongs()
        {
            dataGridViewSongs.Rows.Clear();
            dataGridViewSongs.Columns.Clear();

            dataGridViewSongs.Columns.Add("Name", "Название песни");
            dataGridViewSongs.Columns.Add("Rating", "Рейтинг");

            if (dataGridViewGroups.SelectedRows.Count > 0)
            {
                var groupSongs = context.Songs.ToList().Where(n => n.MusicalGroup.Name == dataGridViewGroups.SelectedRows[0].Cells["Name"].Value.ToString());

                foreach (var groupSong in groupSongs)
                {
                    List<string> data = new List<string>();

                    data.Add(groupSong.Name);
                    data.Add(groupSong.Rating.ToString());
                    dataGridViewSongs.Rows.Add(data.ToArray());
                }
            }
        }
    }
}
