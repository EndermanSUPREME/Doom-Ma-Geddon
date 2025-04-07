using StartPrompt; // import my custom namespace

using System;
using System.Drawing;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

public class DiversionPrompt : Form
{
    bool lightMode;

    public DiversionPrompt()
    {
        lightMode = UsingLightTheme();

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
            Text = "Driver: ene.sys\nene.sys\n\nWindows cannot load "+
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

        CurvedButton cancel = new CurvedButton(5, 2);
        cancel.Text = "Reinstall";
        cancel.Location = new Point(230, 210);
        cancel.Size = new Size(185, 30);
        cancel.FlatStyle = FlatStyle.Flat;
        
        if (!lightMode)
        {
            cancel.BackColor = Color.FromArgb(64, 64, 64);
            cancel.ForeColor = Color.White;
        }

        // Add event handlers for mouse and keyboard interactions
        cancel.MouseEnter += ButtonOnFocus;
        cancel.MouseLeave += ButtonOffFocus;
        cancel.GotFocus += ButtonOnFocus;
        cancel.LostFocus += ButtonOffFocus;

        cancel.Click += (s, e) => this.Close();
        this.Controls.Add(cancel);
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