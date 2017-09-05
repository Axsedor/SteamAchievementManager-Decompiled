// Decompiled with JetBrains decompiler
// Type: SAM.Picker.GamePicker
// Assembly: SAM.Picker, Version=6.3.0.987, Culture=neutral, PublicKeyToken=null
// MVID: EFF56F92-DCC2-4727-8A90-101E83408D77
// Assembly location: F:\Windows\Desktop\SAM\SAM.Picker.exe

using SAM.API;
using SAM.Picker.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml.XPath;

namespace SAM.Picker
{
  public class GamePicker : Form
  {
    private WebClient GameListDownloader = new WebClient();
    private WebClient LogoDownloader = new WebClient();
    private List<GameInfo> Games = new List<GameInfo>();
    private List<GameInfo> LogoQueue = new List<GameInfo>();
    private IContainer components;
    private DoubleBufferedListView gameList;
    private ImageList gameLogos;
    private System.Windows.Forms.Timer callbackTimer;
    private ToolStrip toolStrip1;
    private ToolStripButton refreshGamesButton;
    private ToolStripTextBox addGameText;
    private ToolStripButton addGameButton;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripDropDownButton filterButton;
    private ToolStripMenuItem filterGamesMenuitem;
    private ToolStripMenuItem filterJunkMenuItem;
    private ToolStripMenuItem filterDemosMenuItem;
    private ToolStripMenuItem filterModsMenuItem;
    private StatusStrip statusStrip1;
    private ToolStripStatusLabel downloadStatusLabel;
    private ToolStripStatusLabel pickerStatusLabel;
    private Client SteamClient;
    private SAM.API.Callbacks.AppDataChanged AppDataChangedCallback;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (GamePicker));
      this.gameLogos = new ImageList(this.components);
      this.callbackTimer = new System.Windows.Forms.Timer(this.components);
      this.toolStrip1 = new ToolStrip();
      this.refreshGamesButton = new ToolStripButton();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.addGameText = new ToolStripTextBox();
      this.addGameButton = new ToolStripButton();
      this.toolStripSeparator2 = new ToolStripSeparator();
      this.filterButton = new ToolStripDropDownButton();
      this.filterGamesMenuitem = new ToolStripMenuItem();
      this.filterDemosMenuItem = new ToolStripMenuItem();
      this.filterModsMenuItem = new ToolStripMenuItem();
      this.filterJunkMenuItem = new ToolStripMenuItem();
      this.gameList = new DoubleBufferedListView();
      this.statusStrip1 = new StatusStrip();
      this.pickerStatusLabel = new ToolStripStatusLabel();
      this.downloadStatusLabel = new ToolStripStatusLabel();
      this.toolStrip1.SuspendLayout();
      this.statusStrip1.SuspendLayout();
      this.SuspendLayout();
      this.gameLogos.ColorDepth = ColorDepth.Depth8Bit;
      this.gameLogos.ImageSize = new Size(184, 69);
      this.gameLogos.TransparentColor = Color.Transparent;
      this.callbackTimer.Enabled = true;
      this.callbackTimer.Tick += new EventHandler(this.OnTimer);
      this.toolStrip1.Items.AddRange(new ToolStripItem[6]
      {
        (ToolStripItem) this.refreshGamesButton,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.addGameText,
        (ToolStripItem) this.addGameButton,
        (ToolStripItem) this.toolStripSeparator2,
        (ToolStripItem) this.filterButton
      });
      this.toolStrip1.Location = new Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new Size(742, 25);
      this.toolStrip1.TabIndex = 1;
      this.toolStrip1.Text = "toolStrip1";
      this.refreshGamesButton.Image = (Image) Resources.Refresh;
      this.refreshGamesButton.ImageTransparentColor = Color.Magenta;
      this.refreshGamesButton.Name = "refreshGamesButton";
      this.refreshGamesButton.Size = new Size(105, 22);
      this.refreshGamesButton.Text = "Refresh Games";
      this.refreshGamesButton.Click += new EventHandler(this.OnRefresh);
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new Size(6, 25);
      this.addGameText.Name = "addGameText";
      this.addGameText.Size = new Size(100, 25);
      this.addGameButton.Image = (Image) Resources.Search;
      this.addGameButton.ImageTransparentColor = Color.Magenta;
      this.addGameButton.Name = "addGameButton";
      this.addGameButton.Size = new Size(83, 22);
      this.addGameButton.Text = "Add Game";
      this.addGameButton.Click += new EventHandler(this.OnAddGame);
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new Size(6, 25);
      this.filterButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.filterButton.DropDownItems.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.filterGamesMenuitem,
        (ToolStripItem) this.filterDemosMenuItem,
        (ToolStripItem) this.filterModsMenuItem,
        (ToolStripItem) this.filterJunkMenuItem
      });
      this.filterButton.Image = (Image) Resources.Filter;
      this.filterButton.ImageTransparentColor = Color.Magenta;
      this.filterButton.Name = "filterButton";
      this.filterButton.Size = new Size(29, 22);
      this.filterButton.Text = "Game filtering";
      this.filterGamesMenuitem.Checked = true;
      this.filterGamesMenuitem.CheckOnClick = true;
      this.filterGamesMenuitem.CheckState = CheckState.Checked;
      this.filterGamesMenuitem.Name = "filterGamesMenuitem";
      this.filterGamesMenuitem.Size = new Size(152, 22);
      this.filterGamesMenuitem.Text = "Show &games";
      this.filterGamesMenuitem.CheckedChanged += new EventHandler(this.OnFilterUpdate);
      this.filterDemosMenuItem.CheckOnClick = true;
      this.filterDemosMenuItem.Name = "filterDemosMenuItem";
      this.filterDemosMenuItem.Size = new Size(152, 22);
      this.filterDemosMenuItem.Text = "Show &demos";
      this.filterDemosMenuItem.CheckedChanged += new EventHandler(this.OnFilterUpdate);
      this.filterModsMenuItem.CheckOnClick = true;
      this.filterModsMenuItem.Name = "filterModsMenuItem";
      this.filterModsMenuItem.Size = new Size(152, 22);
      this.filterModsMenuItem.Text = "Show &mods";
      this.filterModsMenuItem.CheckedChanged += new EventHandler(this.OnFilterUpdate);
      this.filterJunkMenuItem.CheckOnClick = true;
      this.filterJunkMenuItem.Name = "filterJunkMenuItem";
      this.filterJunkMenuItem.Size = new Size(152, 22);
      this.filterJunkMenuItem.Text = "Show &junk";
      this.filterJunkMenuItem.CheckedChanged += new EventHandler(this.OnFilterUpdate);
      this.gameList.BackColor = Color.Black;
      this.gameList.Dock = DockStyle.Fill;
      this.gameList.ForeColor = Color.White;
      this.gameList.LargeImageList = this.gameLogos;
      this.gameList.Location = new Point(0, 25);
      this.gameList.MultiSelect = false;
      this.gameList.Name = "gameList";
      this.gameList.Size = new Size(742, 245);
      this.gameList.SmallImageList = this.gameLogos;
      this.gameList.Sorting = SortOrder.Ascending;
      this.gameList.TabIndex = 0;
      this.gameList.TileSize = new Size(184, 69);
      this.gameList.UseCompatibleStateImageBehavior = false;
      this.gameList.ItemActivate += new EventHandler(this.OnSelectGame);
      this.statusStrip1.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.pickerStatusLabel,
        (ToolStripItem) this.downloadStatusLabel
      });
      this.statusStrip1.Location = new Point(0, 270);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new Size(742, 22);
      this.statusStrip1.TabIndex = 2;
      this.statusStrip1.Text = "statusStrip";
      this.pickerStatusLabel.Name = "pickerStatusLabel";
      this.pickerStatusLabel.Size = new Size(727, 17);
      this.pickerStatusLabel.Spring = true;
      this.pickerStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
      this.downloadStatusLabel.Image = (Image) Resources.Download;
      this.downloadStatusLabel.Name = "downloadStatusLabel";
      this.downloadStatusLabel.Size = new Size(111, 17);
      this.downloadStatusLabel.Text = "Download status";
      this.downloadStatusLabel.Visible = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(742, 292);
      this.Controls.Add((Control) this.gameList);
      this.Controls.Add((Control) this.statusStrip1);
      this.Controls.Add((Control) this.toolStrip1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (GamePicker);
      this.Text = "Steam Achievement Manager 6.3 | Pick a game... Any game...";
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public List<GameInfo> _Games
    {
      get
      {
        return this.Games;
      }
    }

    public GamePicker(Client client)
    {
      this.InitializeComponent();
      this.gameLogos.Images.Add("Blank", (Image) new Bitmap(this.gameLogos.ImageSize.Width, this.gameLogos.ImageSize.Height));
      this.GameListDownloader.DownloadDataCompleted += new DownloadDataCompletedEventHandler(this.OnGameListDownload);
      this.LogoDownloader.DownloadDataCompleted += new DownloadDataCompletedEventHandler(this.OnLogoDownload);
      this.SteamClient = client;
      this.AppDataChangedCallback = client.CreateAndRegisterCallback<SAM.API.Callbacks.AppDataChanged>();
      this.AppDataChangedCallback.OnRun += new SAM.API.Callback<SAM.API.Types.AppDataChanged>.CallbackFunction(this.OnAppDataChanged);
      this.AddGames();
    }

    private void OnAppDataChanged(SAM.API.Types.AppDataChanged param)
    {
      if (!param.m_eResult)
        return;
      foreach (GameInfo game in this.Games)
      {
        if (game.Id == (long) param.m_nAppID)
        {
          game.Name = this.SteamClient.SteamApps001.GetAppData((uint) game.Id, "name");
          this.AddGameToLogoQueue(game);
          this.gameList.Sort();
          this.gameList.Update();
          break;
        }
      }
    }

    private void OnGameListDownload(object sender, DownloadDataCompletedEventArgs e)
    {
      if (e.Error == null && !e.Cancelled)
      {
        MemoryStream memoryStream = new MemoryStream();
        memoryStream.Write(e.Result, 0, e.Result.Length);
        memoryStream.Seek(0L, SeekOrigin.Begin);
        XPathNodeIterator xpathNodeIterator = new XPathDocument((Stream) memoryStream).CreateNavigator().Select("/games/game");
        while (xpathNodeIterator.MoveNext())
        {
          string type = xpathNodeIterator.Current.GetAttribute("type", "");
          if (type == string.Empty)
            type = "normal";
          this.AddGame(xpathNodeIterator.Current.ValueAsLong, type);
        }
      }
      else
        this.AddDefaultGames();
      this.RefreshGames();
      this.refreshGamesButton.Enabled = true;
      this.DownloadNextLogo();
    }

    private void RefreshGames()
    {
      this.gameList.BeginUpdate();
      this.gameList.Items.Clear();
      foreach (GameInfo game in this.Games)
      {
        if ((!(game.Type == "normal") || this.filterGamesMenuitem.Checked) && (!(game.Type == "demo") || this.filterDemosMenuItem.Checked) && ((!(game.Type == "mod") || this.filterModsMenuItem.Checked) && (!(game.Type == "junk") || this.filterJunkMenuItem.Checked)))
          this.gameList.Items.Add(game.Item);
      }
      this.gameList.EndUpdate();
      this.pickerStatusLabel.Text = string.Format("Displaying {0} games. Total {1} games.", (object) this.gameList.Items.Count, (object) this.Games.Count);
    }

    private void OnLogoDownload(object sender, DownloadDataCompletedEventArgs e)
    {
      if (e.Error == null && !e.Cancelled)
      {
        GameInfo userState = e.UserState as GameInfo;
        Image image;
        try
        {
          using (MemoryStream memoryStream = new MemoryStream())
          {
            memoryStream.Write(e.Result, 0, e.Result.Length);
            image = (Image) new Bitmap((Stream) memoryStream);
          }
        }
        catch
        {
          image = (Image) null;
        }
        if (image != null)
        {
          userState.ImageIndex = this.gameLogos.Images.Count;
          this.gameLogos.Images.Add(userState.Logo, image);
          this.gameList.Update();
        }
      }
      this.DownloadNextLogo();
    }

    private void DownloadNextLogo()
    {
      if (this.LogoQueue.Count == 0)
      {
        this.downloadStatusLabel.Visible = false;
      }
      else
      {
        if (this.LogoDownloader.IsBusy)
          return;
        this.downloadStatusLabel.Text = string.Format("Downloading {0} game icons...", (object) this.LogoQueue.Count);
        this.downloadStatusLabel.Visible = true;
        GameInfo logo = this.LogoQueue[0];
        this.LogoQueue.RemoveAt(0);
        this.LogoDownloader.DownloadDataAsync(new Uri(string.Format("http://media.steamcommunity.com/steamcommunity/public/images/apps/{0}/{1}.jpg", (object) logo.Id, (object) logo.Logo)), (object) logo);
      }
    }

    private void AddGameToLogoQueue(GameInfo info)
    {
      string appData = this.SteamClient.SteamApps001.GetAppData((uint) info.Id, "logo");
      if (appData == null)
        return;
      info.Logo = appData;
      int num = this.gameLogos.Images.IndexOfKey(appData);
      if (num >= 0)
      {
        info.ImageIndex = num;
      }
      else
      {
        this.LogoQueue.Add(info);
        this.DownloadNextLogo();
      }
    }

    private bool OwnsGame(long id)
    {
      return this.SteamClient.SteamApps003.IsSubscribedApp(id);
    }

    private void AddGame(long id, string type)
    {
      foreach (GameInfo game in this.Games)
      {
        if (game.Id == id)
          return;
      }
      if (!this.OwnsGame(id))
        return;
      GameInfo info = new GameInfo(id, type);
      info.Name = this.SteamClient.SteamApps001.GetAppData((uint) info.Id, "name");
      this.Games.Add(info);
      this.AddGameToLogoQueue(info);
    }

    private void AddGames()
    {
      this.gameList.Items.Clear();
      this.Games = new List<GameInfo>();
      this.GameListDownloader.DownloadDataAsync(new Uri(string.Format("http://gib.me/sam/games.xml")));
      this.refreshGamesButton.Enabled = false;
    }

    private void AddDefaultGames()
    {
      this.AddGame(480L, "normal");
    }

    private void OnTimer(object sender, EventArgs e)
    {
      this.callbackTimer.Enabled = false;
      this.SteamClient.RunCallbacks(false);
      this.callbackTimer.Enabled = true;
    }

    private void OnSelectGame(object sender, EventArgs e)
    {
      if (this.gameList.SelectedItems.Count == 0)
        return;
      GameInfo tag = this.gameList.SelectedItems[0].Tag as GameInfo;
      try
      {
        Process.Start("SAM.Game.exe", tag.Id.ToString());
      }
      catch (Win32Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, "Failed to start SAM.Game.exe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void OnRefresh(object sender, EventArgs e)
    {
      this.addGameText.Text = "";
      this.AddGames();
    }

    private void OnAddGame(object sender, EventArgs e)
    {
      long result;
      if (!long.TryParse(this.addGameText.Text, out result))
      {
        int num1 = (int) MessageBox.Show((IWin32Window) this, "Please enter a valid game ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (!this.OwnsGame(result))
      {
        int num2 = (int) MessageBox.Show((IWin32Window) this, "You don't own that game.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.addGameText.Text = "";
        this.gameList.Items.Clear();
        this.Games = new List<GameInfo>();
        this.AddGame(result, "normal");
        this.filterGamesMenuitem.Checked = true;
        this.RefreshGames();
        this.DownloadNextLogo();
      }
    }

    private void OnFilterUpdate(object sender, EventArgs e)
    {
      this.RefreshGames();
    }
  }
}
