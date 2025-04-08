using SysView;

using System;
using System.IO;
using System.Drawing;
using System.Net.Http;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

public class InstallerPrompt : Form
{
    ProgressBar pb = new ProgressBar();
    SystemData sd = new SystemData();
    Label fileInstallerText = null;
    List<string> FileList = new List<string>();
    bool formDied = false;
    bool prepDone = false;
    bool lightMode;

    public InstallerPrompt()
    {
        lightMode = sd.UsingLightTheme();

        this.Text = "Windows Driver Reinstaller";
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        int formWidth = 460;
        this.Size = new Size(formWidth, 300);
        this.StartPosition = FormStartPosition.CenterScreen;

        if (!lightMode)
        {
            this.BackColor = Color.FromArgb(32, 32, 32);
            this.ForeColor = Color.White;
        }

        int prgbarWidth = 340;
        int prgbarPosX = (formWidth/2) - (prgbarWidth/2) - 8;

        pb.Location = new Point(prgbarPosX, 20);
        pb.Size = new Size(prgbarWidth, 25);
        pb.Minimum = 0;
        pb.Maximum = 100;
        pb.Style = ProgressBarStyle.Continuous;
        this.Controls.Add(pb);

        fileInstallerText = new Label
        {
            Text = "Preparing Repair. . .",
            Font = new Font("Segoe UI", 7),
            Location = new Point(prgbarPosX, 50),
            Size = new Size(prgbarWidth, 50)
        };
        this.Controls.Add(fileInstallerText);

        this.FormClosing += OnCloseEvent;

        Thread t = new Thread(new ThreadStart(PrepareInstallation));
        t.Start();
    }

//==============================================================================
//==============================================================================
//==============================================================================

    void OnCloseEvent(object sender, FormClosingEventArgs e)
    {
        formDied = true;
    }

    void PrepareInstallation()
    {
        Thread animThread = new Thread(new ThreadStart(AnimatePrepText));
        animThread.Start();

        GetFileListAsync().Wait();
        prepDone = true;

        Thread t = new Thread(new ThreadStart(ShowProgress));
        t.Start();
    }

    void AnimatePrepText()
    {
        while (!prepDone)
        {
            for (int i = 0; i < 3; i++)
            {
                if (prepDone) return;
                if (i==0) fileInstallerText.Text = "Preparing Repair.";
                if (i==1) fileInstallerText.Text = "Preparing Repair. .";
                if (i==2) fileInstallerText.Text = "Preparing Repair. . .";
                if (prepDone) return;
                Thread.Sleep(200);
                if (prepDone) return;
            }
        }
    }

    // Asynchronous method to load the file list
    async Task GetFileListAsync()
    {
        await GetFileList(); // execute async function
    }

    async Task GetFileList()
    {
        string url = "https://pastes.io/raw/list-32865";
        HttpClient client = new HttpClient();

        try
        {
            string content = await client.GetStringAsync(url);

            // Split the content by newlines to get each line
            string[] lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Add each line to the list
            foreach (string line in lines)
            {
                FileList.Add(line);
            }
        } catch (Exception ex)
            {
                MessageBox.Show("GetFileList Error: " + ex.Message);
            }
    }

    void ShowProgress()
    {
        while (!formDied && pb.Value < 100)
        {
            if (pb.Value <= 40) {
                pb.Value += 1;
                Thread.Sleep(35);
            } else if (pb.Value > 40 && pb.Value <= 65) {
                pb.Value += 1;
                Thread.Sleep(100);
            } else if (pb.Value >= 65 && pb.Value < 80) {
                pb.Value += 1;
                Thread.Sleep(45);
            } else if (pb.Value >= 80 && pb.Value < 90) {
                pb.Value += 1;
                Thread.Sleep(60);
            } else {
                pb.Value += 1;
                Thread.Sleep(40);
            }

            if (pb.Value < FileList.Count) {
                string filePath = FileList[pb.Value];
                fileInstallerText.Text = filePath;
            }
        }
        fileInstallerText.Text = "Repair Completed! You may safely close this window.";
    }

    // This method provides external objects to trigger this winform
    public void ShowPrompt()
    {
        Application.EnableVisualStyles();
        Application.Run(new InstallerPrompt());
    }
}