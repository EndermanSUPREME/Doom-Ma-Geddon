using SysView;

using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

public class InstallerPrompt : Form
{
    ProgressBar pb = new ProgressBar();
    SystemData sd = new SystemData();
    bool lightMode;
    bool formDied = false;

    public InstallerPrompt()
    {
        lightMode = sd.UsingLightTheme();

        this.Text = "Windows Driver Reinstaller";
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.Size = new Size(460, 300);
        this.StartPosition = FormStartPosition.CenterScreen;

        if (!lightMode)
        {
            this.BackColor = Color.FromArgb(32, 32, 32);
            this.ForeColor = Color.White;
        }

        pb.Location = new Point(20, 20);
        pb.Size = new Size(340, 25);
        pb.Minimum = 0;
        pb.Maximum = 100;
        pb.Style = ProgressBarStyle.Continuous;
        this.Controls.Add(pb);

        this.FormClosing += OnCloseEvent;

        Thread t = new Thread(new ThreadStart(ShowProgress));
        t.Start();
    }

//==============================================================================
//==============================================================================
//==============================================================================

    private void OnCloseEvent(object sender, FormClosingEventArgs e)
    {
        formDied = true;
    }

    void ShowProgress()
    {
        while (!formDied && pb.Value < 100)
        {
            if (pb.Value <= 40) {
                pb.Value += 1;
                Thread.Sleep(500);
            } else if (pb.Value > 40 && pb.Value < 80) {
                pb.Value += 1;
                Thread.Sleep(1000);
            } else {
                pb.Value += 1;
                Thread.Sleep(700);
            }
        }
    }

    // This method provides external objects to trigger this winform
    public void ShowPrompt()
    {
        Application.EnableVisualStyles();
        Application.Run(new InstallerPrompt());
    }
}