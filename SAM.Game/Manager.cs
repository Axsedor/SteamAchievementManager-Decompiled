// Decompiled with JetBrains decompiler
// Type: SAM.Game.Manager
// Assembly: SAM.Game, Version=6.3.0.987, Culture=neutral, PublicKeyToken=null
// MVID: C778E0E6-4989-4E7A-95E7-530B61DDD3BB
// Assembly location: F:\Windows\Desktop\SAM\SAM.Game.exe

using SAM.API;
using SAM.API.Types;
using SAM.Game.Properties;
using SAM.Game.Stats;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SAM.Game
{
  public class Manager : Form
  {
    private WebClient IconDownloader = new WebClient();
    private List<AchievementInfo> IconQueue = new List<AchievementInfo>();
    private List<StatDefinition> StatDefinitions = new List<StatDefinition>();
    private List<AchievementDefinition> AchievementDefinitions = new List<AchievementDefinition>();
    private Dictionary<string, AchievementInfo> Achievements = new Dictionary<string, AchievementInfo>();
    private BindingList<StatInfo> Statistics = new BindingList<StatInfo>();
    private long GameId;
    private Client SteamClient;
    private SAM.API.Callbacks.UserStatsReceived UserStatsReceivedCallback;
    private bool IsUpdatingAchievementList;
    private IContainer components;
    private ToolStrip mainToolStrip;
    private ToolStripButton storeButton;
    private ToolStripButton reloadButton;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel countryLabel;
    private ToolStripStatusLabel gameStatusLabel;
    private ImageList achievementIcons;
    private System.Windows.Forms.Timer callbackTimer;
    private TabControl mainTabs;
    private TabPage achievementsTab;
    private TabPage statisticsTab;
    private DoubleBufferedListView achievementList;
    private ColumnHeader achievementNameColumn;
    private ColumnHeader achievementDescriptionColumn;
    private ToolStrip achievementsStrip;
    private ToolStripButton lockAllButton;
    private ToolStripButton invertAllButton;
    private ToolStripButton unlockAllButton;
    private DataGridView statisticsList;
    public CheckBox enableStatsEditingCheckBox;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripButton resetButton;
    private ToolStripStatusLabel downloadStatusLabel;

    public Manager(long gameId, Client client)
    {
      this.InitializeComponent();
      this.mainTabs.SelectedTab = this.achievementsTab;
      this.achievementIcons.Images.Add("Blank", (Image) new Bitmap(64, 64));
      this.statisticsList.AutoGenerateColumns = false;
      this.statisticsList.Columns.Add("name", "Name");
      this.statisticsList.Columns[0].ReadOnly = true;
      this.statisticsList.Columns[0].Width = 200;
      this.statisticsList.Columns[0].DataPropertyName = "DisplayName";
      this.statisticsList.Columns.Add("value", "Value");
      this.statisticsList.Columns[1].ReadOnly = !this.enableStatsEditingCheckBox.Checked;
      this.statisticsList.Columns[1].Width = 90;
      this.statisticsList.Columns[1].DataPropertyName = "Value";
      this.statisticsList.Columns.Add("extra", "Extra");
      this.statisticsList.Columns[2].ReadOnly = true;
      this.statisticsList.Columns[2].Width = 200;
      this.statisticsList.Columns[2].DataPropertyName = "Extra";
      this.statisticsList.DataSource = (object) new BindingSource()
      {
        DataSource = (object) this.Statistics
      };
      this.GameId = gameId;
      this.SteamClient = client;
      this.IconDownloader.DownloadDataCompleted += new DownloadDataCompletedEventHandler(this.OnIconDownload);
      string appData = this.SteamClient.SteamApps001.GetAppData((uint) this.GameId, "name");
      if (appData != null)
      {
        Manager manager = this;
        string str = manager.Text + " | " + appData;
        manager.Text = str;
      }
      else
      {
        Manager manager = this;
        string str = manager.Text + " | " + this.GameId.ToString();
        manager.Text = str;
      }
      this.UserStatsReceivedCallback = client.CreateAndRegisterCallback<SAM.API.Callbacks.UserStatsReceived>();
      this.UserStatsReceivedCallback.OnRun += new SAM.API.Callback<SAM.API.Types.UserStatsReceived>.CallbackFunction(this.OnUserStatsReceived);
      this.RefreshStats();
    }

    private void AddAchievementIcon(AchievementInfo info, Image icon)
    {
      if (icon == null)
      {
        info.ImageIndex = 0;
      }
      else
      {
        info.ImageIndex = this.achievementIcons.Images.Count;
        this.achievementIcons.Images.Add(info.Achieved ? info.Icon : info.IconGray, icon);
      }
    }

    private void OnIconDownload(object sender, DownloadDataCompletedEventArgs e)
    {
      if (e.Error == null && !e.Cancelled)
      {
        AchievementInfo userState = e.UserState as AchievementInfo;
        Bitmap bitmap;
        try
        {
          MemoryStream memoryStream = new MemoryStream();
          memoryStream.Write(e.Result, 0, e.Result.Length);
          bitmap = new Bitmap((Stream) memoryStream);
        }
        catch (Exception ex)
        {
          bitmap = (Bitmap) null;
        }
        this.AddAchievementIcon(userState, (Image) bitmap);
        this.achievementList.Update();
      }
      this.DownloadNextIcon();
    }

    private void DownloadNextIcon()
    {
      if (this.IconQueue.Count == 0)
      {
        this.downloadStatusLabel.Visible = false;
      }
      else
      {
        if (this.IconDownloader.IsBusy)
          return;
        this.downloadStatusLabel.Text = string.Format("Downloading {0} icons...", (object) this.IconQueue.Count);
        this.downloadStatusLabel.Visible = true;
        AchievementInfo icon = this.IconQueue[0];
        this.IconQueue.RemoveAt(0);
        this.IconDownloader.DownloadDataAsync(new Uri(string.Format("http://media.steamcommunity.com/steamcommunity/public/images/apps/{0}/{1}", (object) this.GameId, icon.Achieved ? (object) icon.Icon : (object) icon.IconGray)), (object) icon);
      }
    }

    private string TranslateError(int id)
    {
      if (id == 2)
        return "generic error -- this usually means you don't own the game";
      return id.ToString();
    }

    private string GetLocalizedString(KeyValue kv, string language, string defaultValue)
    {
      string str1 = kv[language].AsString("");
      if (!string.IsNullOrEmpty(str1))
        return str1;
      if (language != "english")
      {
        string str2 = kv["english"].AsString("");
        if (!string.IsNullOrEmpty(str2))
          return str2;
      }
      string str3 = kv.AsString("");
      if (!string.IsNullOrEmpty(str3))
        return str3;
      return defaultValue;
    }

    private bool LoadUserGameStatsSchema()
    {
      string path;
      try
      {
        path = Path.Combine(Path.Combine(Path.Combine(Steam.GetInstallPath(), "appcache"), "stats"), string.Format("UserGameStatsSchema_{0}.bin", (object) this.GameId));
        if (!System.IO.File.Exists(path))
          return false;
      }
      catch
      {
        return false;
      }
      KeyValue keyValue1 = KeyValue.LoadAsBinary(path);
      if (keyValue1 == null)
        return false;
      string currentGameLanguage = this.SteamClient.SteamApps003.GetCurrentGameLanguage();
      this.AchievementDefinitions.Clear();
      this.StatDefinitions.Clear();
      KeyValue keyValue2 = keyValue1[this.GameId.ToString()]["stats"];
      if (!keyValue2.Valid || keyValue2.Children == null)
        return false;
      foreach (KeyValue child1 in keyValue2.Children)
      {
        if (child1.Valid)
        {
          switch (child1["type"].AsInteger(0))
          {
            case 1:
              string defaultValue1 = child1["name"].AsString("");
              string localizedString1 = this.GetLocalizedString(child1["display"]["name"], currentGameLanguage, defaultValue1);
              List<StatDefinition> statDefinitions1 = this.StatDefinitions;
              IntegerStatDefinition integerStatDefinition1 = new IntegerStatDefinition();
              integerStatDefinition1.Id = child1["name"].AsString("");
              integerStatDefinition1.DisplayName = localizedString1;
              integerStatDefinition1.MinValue = child1["min"].AsInteger(int.MinValue);
              integerStatDefinition1.MaxValue = child1["max"].AsInteger(int.MaxValue);
              integerStatDefinition1.MaxChange = child1["maxchange"].AsInteger(0);
              integerStatDefinition1.IncrementOnly = child1["incrementonly"].AsBoolean(false);
              integerStatDefinition1.DefaultValue = child1["default"].AsInteger(0);
              integerStatDefinition1.Permission = child1["permission"].AsInteger(0);
              IntegerStatDefinition integerStatDefinition2 = integerStatDefinition1;
              statDefinitions1.Add((StatDefinition) integerStatDefinition2);
              continue;
            case 2:
            case 3:
              string defaultValue2 = child1["name"].AsString("");
              string localizedString2 = this.GetLocalizedString(child1["display"]["name"], currentGameLanguage, defaultValue2);
              List<StatDefinition> statDefinitions2 = this.StatDefinitions;
              FloatStatDefinition floatStatDefinition1 = new FloatStatDefinition();
              floatStatDefinition1.Id = child1["name"].AsString("");
              floatStatDefinition1.DisplayName = localizedString2;
              floatStatDefinition1.MinValue = child1["min"].AsFloat(float.MinValue);
              floatStatDefinition1.MaxValue = child1["max"].AsFloat(float.MaxValue);
              floatStatDefinition1.MaxChange = child1["maxchange"].AsFloat(0.0f);
              floatStatDefinition1.IncrementOnly = child1["incrementonly"].AsBoolean(false);
              floatStatDefinition1.DefaultValue = child1["default"].AsFloat(0.0f);
              floatStatDefinition1.Permission = child1["permission"].AsInteger(0);
              FloatStatDefinition floatStatDefinition2 = floatStatDefinition1;
              statDefinitions2.Add((StatDefinition) floatStatDefinition2);
              continue;
            case 4:
            case 5:
              if (child1.Children != null)
              {
                using (IEnumerator<KeyValue> enumerator = child1.Children.Where<KeyValue>((Func<KeyValue, bool>) (b => b.Name.ToLowerInvariant() == "bits")).GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    KeyValue current = enumerator.Current;
                    if (current.Valid && current.Children != null)
                    {
                      foreach (KeyValue child2 in current.Children)
                      {
                        string defaultValue3 = child2["name"].AsString("");
                        string localizedString3 = this.GetLocalizedString(child2["display"]["name"], currentGameLanguage, defaultValue3);
                        string localizedString4 = this.GetLocalizedString(child2["display"]["desc"], currentGameLanguage, "");
                        this.AchievementDefinitions.Add(new AchievementDefinition()
                        {
                          Id = defaultValue3,
                          Name = localizedString3,
                          Description = localizedString4,
                          Icon = child2["display"]["icon"].AsString(""),
                          IconGray = child2["display"]["icon_gray"].AsString(""),
                          Hidden = child2["display"]["hidden"].AsBoolean(false),
                          Permission = child2["permission"].AsInteger(0)
                        });
                      }
                    }
                  }
                  continue;
                }
              }
              else
                continue;
            default:
              throw new InvalidOperationException("invalid stat type");
          }
        }
      }
      return true;
    }

    private void OnUserStatsReceived(SAM.API.Types.UserStatsReceived param)
    {
      if (param.m_eResult != 1)
      {
        this.gameStatusLabel.Text = string.Format("Error while retrieving stats: {0}", (object) this.TranslateError(param.m_eResult));
        this.EnableInput();
      }
      else if (!this.LoadUserGameStatsSchema())
      {
        this.gameStatusLabel.Text = "Failed to load schema.";
        this.EnableInput();
      }
      else
      {
        this.GetAchievements();
        this.GetStatistics();
        this.gameStatusLabel.Text = string.Format("Retrieved {0} achievements and {1} statistics.", (object) this.achievementList.Items.Count, (object) this.statisticsList.Rows.Count);
        this.EnableInput();
      }
    }

    private void OnUserStatsStored(UserStatsStored param)
    {
      int eResult = param.m_eResult;
    }

    private void RefreshStats()
    {
      this.achievementList.Items.Clear();
      this.statisticsList.Rows.Clear();
      if (!this.SteamClient.SteamUserStats.RequestCurrentStats())
      {
        int num = (int) MessageBox.Show((IWin32Window) this, "Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.gameStatusLabel.Text = "Retrieving stat information...";
        this.DisableInput();
      }
    }

    private void GetAchievements()
    {
      this.IsUpdatingAchievementList = true;
      this.achievementList.Items.Clear();
      this.achievementList.BeginUpdate();
      this.Achievements.Clear();
      foreach (AchievementDefinition achievementDefinition in this.AchievementDefinitions)
      {
        if (!(achievementDefinition.Id == string.Empty))
        {
          bool achieved = false;
          if (this.SteamClient.SteamUserStats.GetAchievementState(achievementDefinition.Id, ref achieved))
          {
            AchievementInfo info = new AchievementInfo();
            info.Id = achievementDefinition.Id;
            info.Achieved = achieved;
            info.Icon = string.IsNullOrEmpty(achievementDefinition.Icon) ? (string) null : achievementDefinition.Icon;
            info.IconGray = string.IsNullOrEmpty(achievementDefinition.IconGray) ? info.Icon : achievementDefinition.IconGray;
            info.Permission = achievementDefinition.Permission;
            info.Name = achievementDefinition.Name;
            info.Description = achievementDefinition.Description;
            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Checked = achieved;
            listViewItem.Tag = (object) info;
            listViewItem.Text = info.Name;
            listViewItem.BackColor = (achievementDefinition.Permission & 2) == 0 ? Color.Black : Color.FromArgb(64, 0, 0);
            info.Item = listViewItem;
            if (listViewItem.Text.StartsWith("#"))
              listViewItem.Text = info.Id;
            else
              listViewItem.SubItems.Add(info.Description);
            info.ImageIndex = 0;
            if (!false)
              this.AddAchievementToIconQueue(info, false);
            this.achievementList.Items.Add(listViewItem);
            this.Achievements.Add(info.Id, info);
          }
        }
      }
      this.achievementList.EndUpdate();
      this.IsUpdatingAchievementList = false;
      this.DownloadNextIcon();
    }

    private void GetStatistics()
    {
      this.Statistics.Clear();
      foreach (StatDefinition statDefinition in this.StatDefinitions)
      {
        if (!string.IsNullOrEmpty(statDefinition.Id))
        {
          if (statDefinition is IntegerStatDefinition)
          {
            IntegerStatDefinition integerStatDefinition = (IntegerStatDefinition) statDefinition;
            int num = 0;
            if (this.SteamClient.SteamUserStats.GetStatValue(integerStatDefinition.Id, ref num))
            {
              BindingList<StatInfo> statistics = this.Statistics;
              IntStatInfo intStatInfo1 = new IntStatInfo();
              intStatInfo1.Id = integerStatDefinition.Id;
              intStatInfo1.DisplayName = integerStatDefinition.DisplayName;
              intStatInfo1.IntValue = num;
              intStatInfo1.OriginalValue = num;
              intStatInfo1.IncrementOnly = integerStatDefinition.IncrementOnly;
              intStatInfo1.Permission = integerStatDefinition.Permission;
              IntStatInfo intStatInfo2 = intStatInfo1;
              statistics.Add((StatInfo) intStatInfo2);
            }
          }
          else if (statDefinition is FloatStatDefinition)
          {
            FloatStatDefinition floatStatDefinition = (FloatStatDefinition) statDefinition;
            float num = 0.0f;
            if (this.SteamClient.SteamUserStats.GetStatValue(floatStatDefinition.Id, ref num))
            {
              BindingList<StatInfo> statistics = this.Statistics;
              FloatStatInfo floatStatInfo1 = new FloatStatInfo();
              floatStatInfo1.Id = floatStatDefinition.Id;
              floatStatInfo1.DisplayName = floatStatDefinition.DisplayName;
              floatStatInfo1.FloatValue = num;
              floatStatInfo1.OriginalValue = num;
              floatStatInfo1.IncrementOnly = floatStatDefinition.IncrementOnly;
              floatStatInfo1.Permission = floatStatDefinition.Permission;
              FloatStatInfo floatStatInfo2 = floatStatInfo1;
              statistics.Add((StatInfo) floatStatInfo2);
            }
          }
        }
      }
    }

    private void AddAchievementToIconQueue(AchievementInfo info, bool startDownload)
    {
      int num = this.achievementIcons.Images.IndexOfKey(info.Achieved ? info.Icon : info.IconGray);
      if (num >= 0)
      {
        info.ImageIndex = num;
      }
      else
      {
        this.IconQueue.Add(info);
        if (!startDownload)
          return;
        this.DownloadNextIcon();
      }
    }

    private int StoreAchievements()
    {
      if (this.achievementList.Items.Count == 0)
        return 0;
      List<AchievementInfo> achievementInfoList = new List<AchievementInfo>();
      foreach (ListViewItem listViewItem in this.achievementList.Items)
      {
        AchievementInfo tag = listViewItem.Tag as AchievementInfo;
        if (tag.Achieved != listViewItem.Checked)
        {
          tag.Achieved = listViewItem.Checked;
          achievementInfoList.Add(listViewItem.Tag as AchievementInfo);
        }
      }
      if (achievementInfoList.Count == 0)
        return 0;
      foreach (AchievementInfo achievementInfo in achievementInfoList)
      {
        if (!this.SteamClient.SteamUserStats.SetAchievement(achievementInfo.Id, achievementInfo.Achieved))
        {
          int num = (int) MessageBox.Show((IWin32Window) this, string.Format("An error occured while setting the state for {0}, aborting store.", (object) achievementInfo.Id), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return -1;
        }
      }
      return achievementInfoList.Count;
    }

    private int StoreStatistics()
    {
      if (this.Statistics.Count == 0)
        return 0;
      List<StatInfo> statInfoList = new List<StatInfo>();
      foreach (StatInfo statistic in (Collection<StatInfo>) this.Statistics)
      {
        if (statistic.Modified)
          statInfoList.Add(statistic);
      }
      if (statInfoList.Count == 0)
        return 0;
      foreach (StatInfo statInfo in statInfoList)
      {
        if (statInfo is IntStatInfo)
        {
          IntStatInfo intStatInfo = (IntStatInfo) statInfo;
          if (!this.SteamClient.SteamUserStats.SetStatValue(intStatInfo.Id, intStatInfo.IntValue))
          {
            int num = (int) MessageBox.Show((IWin32Window) this, string.Format("An error occured while setting the value for {0}, aborting store.", (object) statInfo.Id), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return -1;
          }
        }
        else
        {
          if (!(statInfo is FloatStatInfo))
            throw new Exception();
          FloatStatInfo floatStatInfo = (FloatStatInfo) statInfo;
          if (!this.SteamClient.SteamUserStats.SetStatValue(floatStatInfo.Id, floatStatInfo.FloatValue))
          {
            int num = (int) MessageBox.Show((IWin32Window) this, string.Format("An error occured while setting the value for {0}, aborting store.", (object) statInfo.Id), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return -1;
          }
        }
      }
      return statInfoList.Count;
    }

    private Bitmap MakeBitmapFromData(int width, int height, byte[] buffer)
    {
      Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
      uint num1 = 0;
      while ((long) num1 < (long) (width * height * 4))
      {
        byte num2 = buffer[(IntPtr) num1];
        buffer[(IntPtr) num1] = buffer[(IntPtr) (num1 + 2U)];
        buffer[(IntPtr) (num1 + 2U)] = num2;
        num1 += 4U;
      }
      Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
      BitmapData bitmapdata = bitmap.LockBits(rect, ImageLockMode.WriteOnly, bitmap.PixelFormat);
      Marshal.Copy(buffer, 0, bitmapdata.Scan0, width * height * 4);
      bitmap.UnlockBits(bitmapdata);
      return bitmap;
    }

    private void DisableInput()
    {
      this.reloadButton.Enabled = false;
      this.storeButton.Enabled = false;
    }

    private void EnableInput()
    {
      this.reloadButton.Enabled = true;
      this.storeButton.Enabled = true;
    }

    private void OnTimer(object sender, EventArgs e)
    {
      this.callbackTimer.Enabled = false;
      this.SteamClient.RunCallbacks(false);
      this.callbackTimer.Enabled = true;
    }

    private void OnRefresh(object sender, EventArgs e)
    {
      this.RefreshStats();
    }

    private void OnLockAll(object sender, EventArgs e)
    {
      foreach (ListViewItem listViewItem in this.achievementList.Items)
        listViewItem.Checked = false;
    }

    private void OnInvertAll(object sender, EventArgs e)
    {
      foreach (ListViewItem listViewItem in this.achievementList.Items)
        listViewItem.Checked = !listViewItem.Checked;
    }

    private void OnUnlockAll(object sender, EventArgs e)
    {
      foreach (ListViewItem listViewItem in this.achievementList.Items)
        listViewItem.Checked = true;
    }

    private bool Store()
    {
      if (this.SteamClient.SteamUserStats.StoreStats())
        return true;
      int num = (int) MessageBox.Show((IWin32Window) this, "An error occured while storing, aborting.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    private void OnStore(object sender, EventArgs e)
    {
      int num1 = this.StoreAchievements();
      if (num1 < 0)
      {
        this.RefreshStats();
      }
      else
      {
        int num2 = this.StoreStatistics();
        if (num2 < 0)
          this.RefreshStats();
        else if (!this.Store())
        {
          this.RefreshStats();
        }
        else
        {
          int num3 = (int) MessageBox.Show((IWin32Window) this, string.Format("Stored {0} achievements and {1} statistics.", (object) num1, (object) num2), "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.RefreshStats();
        }
      }
    }

    private void OnStatDataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      if (e.Context != DataGridViewDataErrorContexts.Commit)
        return;
      if (e.Exception is StatIsProtectedException)
      {
        e.ThrowException = false;
        e.Cancel = true;
        ((DataGridView) sender).Rows[e.RowIndex].ErrorText = "Stat is protected! -- you can't modify it";
      }
      else
      {
        e.ThrowException = false;
        e.Cancel = true;
        ((DataGridView) sender).Rows[e.RowIndex].ErrorText = "Invalid value";
      }
    }

    private void OnStatAgreementChecked(object sender, EventArgs e)
    {
      this.statisticsList.Columns[1].ReadOnly = !this.enableStatsEditingCheckBox.Checked;
    }

    private void OnStatCellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      ((DataGridView) sender).Rows[e.RowIndex].ErrorText = "";
    }

    private void OnResetAllStats(object sender, EventArgs e)
    {
      if (MessageBox.Show("Are you absolutely sure you want to reset stats?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        return;
      bool achievementsToo = DialogResult.Yes == MessageBox.Show("Do you want to reset achievements too?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
      if (MessageBox.Show("Really really sure?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.No)
        return;
      if (!this.SteamClient.SteamUserStats.ResetAllStats(achievementsToo))
      {
        int num = (int) MessageBox.Show((IWin32Window) this, "Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        this.RefreshStats();
    }

    private void OnCheckAchievement(object sender, ItemCheckEventArgs e)
    {
      if (sender != this.achievementList || this.IsUpdatingAchievementList)
        return;
      AchievementInfo tag = this.achievementList.Items[e.Index].Tag as AchievementInfo;
      if (tag == null || (tag.Permission & 2) == 0)
        return;
      int num = (int) MessageBox.Show("Sorry, but this is a protected achievement and cannot be managed with Steam Achievement Manager.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      e.NewValue = e.CurrentValue;
    }

    private void OnStatValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
      if (e.ColumnIndex != 1)
        return;
      int columnIndex = e.ColumnIndex;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Manager));
      this.mainToolStrip = new ToolStrip();
      this.storeButton = new ToolStripButton();
      this.reloadButton = new ToolStripButton();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.resetButton = new ToolStripButton();
      this.achievementIcons = new ImageList(this.components);
      this.statusStrip = new StatusStrip();
      this.countryLabel = new ToolStripStatusLabel();
      this.gameStatusLabel = new ToolStripStatusLabel();
      this.downloadStatusLabel = new ToolStripStatusLabel();
      this.callbackTimer = new System.Windows.Forms.Timer(this.components);
      this.mainTabs = new TabControl();
      this.achievementsTab = new TabPage();
      this.achievementList = new DoubleBufferedListView();
      this.achievementNameColumn = new ColumnHeader();
      this.achievementDescriptionColumn = new ColumnHeader();
      this.achievementsStrip = new ToolStrip();
      this.lockAllButton = new ToolStripButton();
      this.invertAllButton = new ToolStripButton();
      this.unlockAllButton = new ToolStripButton();
      this.statisticsTab = new TabPage();
      this.enableStatsEditingCheckBox = new CheckBox();
      this.statisticsList = new DataGridView();
      this.mainToolStrip.SuspendLayout();
      this.statusStrip.SuspendLayout();
      this.mainTabs.SuspendLayout();
      this.achievementsTab.SuspendLayout();
      this.achievementsStrip.SuspendLayout();
      this.statisticsTab.SuspendLayout();
      ((ISupportInitialize) this.statisticsList).BeginInit();
      this.SuspendLayout();
      this.mainToolStrip.Items.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.storeButton,
        (ToolStripItem) this.reloadButton,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.resetButton
      });
      this.mainToolStrip.Location = new Point(0, 0);
      this.mainToolStrip.Name = "mainToolStrip";
      this.mainToolStrip.Size = new Size(632, 25);
      this.mainToolStrip.TabIndex = 1;
      this.storeButton.Alignment = ToolStripItemAlignment.Right;
      this.storeButton.Enabled = false;
      this.storeButton.Image = (Image) Resources.Save;
      this.storeButton.ImageTransparentColor = Color.Magenta;
      this.storeButton.Name = "storeButton";
      this.storeButton.Size = new Size(54, 22);
      this.storeButton.Text = "Store";
      this.storeButton.ToolTipText = "Store achievements and statistics for active game.";
      this.storeButton.Click += new EventHandler(this.OnStore);
      this.reloadButton.Enabled = false;
      this.reloadButton.Image = (Image) Resources.Refresh;
      this.reloadButton.ImageTransparentColor = Color.Magenta;
      this.reloadButton.Name = "reloadButton";
      this.reloadButton.Size = new Size(66, 22);
      this.reloadButton.Text = "Refresh";
      this.reloadButton.ToolTipText = "Refresh achievements and statistics for active game.";
      this.reloadButton.Click += new EventHandler(this.OnRefresh);
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new Size(6, 25);
      this.resetButton.Image = (Image) Resources.Reset;
      this.resetButton.ImageTransparentColor = Color.Magenta;
      this.resetButton.Name = "resetButton";
      this.resetButton.Size = new Size(55, 22);
      this.resetButton.Text = "Reset";
      this.resetButton.ToolTipText = "Reset achievements and/or statistics for active game.";
      this.resetButton.Click += new EventHandler(this.OnResetAllStats);
      this.achievementIcons.ColorDepth = ColorDepth.Depth8Bit;
      this.achievementIcons.ImageSize = new Size(64, 64);
      this.achievementIcons.TransparentColor = Color.Transparent;
      this.statusStrip.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.countryLabel,
        (ToolStripItem) this.gameStatusLabel,
        (ToolStripItem) this.downloadStatusLabel
      });
      this.statusStrip.Location = new Point(0, 370);
      this.statusStrip.Name = "statusStrip";
      this.statusStrip.Size = new Size(632, 22);
      this.statusStrip.TabIndex = 4;
      this.statusStrip.Text = "statusStrip1";
      this.countryLabel.Name = "countryLabel";
      this.countryLabel.Size = new Size(0, 17);
      this.gameStatusLabel.Name = "gameStatusLabel";
      this.gameStatusLabel.Size = new Size(617, 17);
      this.gameStatusLabel.Spring = true;
      this.gameStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
      this.downloadStatusLabel.Image = (Image) Resources.Download;
      this.downloadStatusLabel.Name = "downloadStatusLabel";
      this.downloadStatusLabel.Size = new Size(111, 17);
      this.downloadStatusLabel.Text = "Download status";
      this.downloadStatusLabel.Visible = false;
      this.callbackTimer.Enabled = true;
      this.callbackTimer.Tick += new EventHandler(this.OnTimer);
      this.mainTabs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.mainTabs.Controls.Add((Control) this.achievementsTab);
      this.mainTabs.Controls.Add((Control) this.statisticsTab);
      this.mainTabs.Location = new Point(8, 33);
      this.mainTabs.Name = "mainTabs";
      this.mainTabs.SelectedIndex = 0;
      this.mainTabs.Size = new Size(616, 334);
      this.mainTabs.TabIndex = 5;
      this.achievementsTab.Controls.Add((Control) this.achievementList);
      this.achievementsTab.Controls.Add((Control) this.achievementsStrip);
      this.achievementsTab.Location = new Point(4, 22);
      this.achievementsTab.Name = "achievementsTab";
      this.achievementsTab.Padding = new Padding(3);
      this.achievementsTab.Size = new Size(608, 308);
      this.achievementsTab.TabIndex = 0;
      this.achievementsTab.Text = "Achievements";
      this.achievementsTab.UseVisualStyleBackColor = true;
      this.achievementList.Activation = ItemActivation.OneClick;
      this.achievementList.BackColor = Color.Black;
      this.achievementList.BackgroundImageTiled = true;
      this.achievementList.CheckBoxes = true;
      this.achievementList.Columns.AddRange(new ColumnHeader[2]
      {
        this.achievementNameColumn,
        this.achievementDescriptionColumn
      });
      this.achievementList.Dock = DockStyle.Fill;
      this.achievementList.ForeColor = Color.White;
      this.achievementList.FullRowSelect = true;
      this.achievementList.GridLines = true;
      this.achievementList.HideSelection = false;
      this.achievementList.LargeImageList = this.achievementIcons;
      this.achievementList.Location = new Point(3, 28);
      this.achievementList.Name = "achievementList";
      this.achievementList.Size = new Size(602, 277);
      this.achievementList.SmallImageList = this.achievementIcons;
      this.achievementList.Sorting = SortOrder.Ascending;
      this.achievementList.TabIndex = 4;
      this.achievementList.UseCompatibleStateImageBehavior = false;
      this.achievementList.View = View.Details;
      this.achievementList.ItemCheck += new ItemCheckEventHandler(this.OnCheckAchievement);
      this.achievementNameColumn.Text = "Name";
      this.achievementNameColumn.Width = 200;
      this.achievementDescriptionColumn.Text = "Description";
      this.achievementDescriptionColumn.Width = 380;
      this.achievementsStrip.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.lockAllButton,
        (ToolStripItem) this.invertAllButton,
        (ToolStripItem) this.unlockAllButton
      });
      this.achievementsStrip.Location = new Point(3, 3);
      this.achievementsStrip.Name = "achievementsStrip";
      this.achievementsStrip.Size = new Size(602, 25);
      this.achievementsStrip.TabIndex = 5;
      this.lockAllButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.lockAllButton.Image = (Image) Resources.Lock;
      this.lockAllButton.ImageTransparentColor = Color.Magenta;
      this.lockAllButton.Name = "lockAllButton";
      this.lockAllButton.Size = new Size(23, 22);
      this.lockAllButton.Text = "Lock All";
      this.lockAllButton.ToolTipText = "Lock all achievements.";
      this.lockAllButton.Click += new EventHandler(this.OnLockAll);
      this.invertAllButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.invertAllButton.Image = (Image) Resources.Invert;
      this.invertAllButton.ImageTransparentColor = Color.Magenta;
      this.invertAllButton.Name = "invertAllButton";
      this.invertAllButton.Size = new Size(23, 22);
      this.invertAllButton.Text = "Invert All";
      this.invertAllButton.ToolTipText = "Invert all achievements.";
      this.invertAllButton.Click += new EventHandler(this.OnInvertAll);
      this.unlockAllButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.unlockAllButton.Image = (Image) Resources.Unlock;
      this.unlockAllButton.ImageTransparentColor = Color.Magenta;
      this.unlockAllButton.Name = "unlockAllButton";
      this.unlockAllButton.Size = new Size(23, 22);
      this.unlockAllButton.Text = "Unlock All";
      this.unlockAllButton.ToolTipText = "Unlock all achievements.";
      this.unlockAllButton.Click += new EventHandler(this.OnUnlockAll);
      this.statisticsTab.Controls.Add((Control) this.enableStatsEditingCheckBox);
      this.statisticsTab.Controls.Add((Control) this.statisticsList);
      this.statisticsTab.Location = new Point(4, 22);
      this.statisticsTab.Name = "statisticsTab";
      this.statisticsTab.Padding = new Padding(3);
      this.statisticsTab.Size = new Size(608, 308);
      this.statisticsTab.TabIndex = 1;
      this.statisticsTab.Text = "Statistics";
      this.statisticsTab.UseVisualStyleBackColor = true;
      this.enableStatsEditingCheckBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.enableStatsEditingCheckBox.AutoSize = true;
      this.enableStatsEditingCheckBox.Location = new Point(6, 285);
      this.enableStatsEditingCheckBox.Name = "enableStatsEditingCheckBox";
      this.enableStatsEditingCheckBox.Size = new Size(512, 17);
      this.enableStatsEditingCheckBox.TabIndex = 1;
      this.enableStatsEditingCheckBox.Text = "I understand by modifying the values of stats, I may screw things up and can't blame anyone but myself.";
      this.enableStatsEditingCheckBox.UseVisualStyleBackColor = true;
      this.enableStatsEditingCheckBox.CheckedChanged += new EventHandler(this.OnStatAgreementChecked);
      this.statisticsList.AllowUserToAddRows = false;
      this.statisticsList.AllowUserToDeleteRows = false;
      this.statisticsList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.statisticsList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.statisticsList.Location = new Point(6, 6);
      this.statisticsList.Name = "statisticsList";
      this.statisticsList.Size = new Size(596, 273);
      this.statisticsList.TabIndex = 0;
      this.statisticsList.CellEndEdit += new DataGridViewCellEventHandler(this.OnStatCellEndEdit);
      this.statisticsList.CellValidating += new DataGridViewCellValidatingEventHandler(this.OnStatValidating);
      this.statisticsList.DataError += new DataGridViewDataErrorEventHandler(this.OnStatDataError);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(632, 392);
      this.Controls.Add((Control) this.mainToolStrip);
      this.Controls.Add((Control) this.mainTabs);
      this.Controls.Add((Control) this.statusStrip);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MinimumSize = new Size(640, 50);
      this.Name = nameof (Manager);
      this.Text = "Steam Achievement Manager 6.3";
      this.mainToolStrip.ResumeLayout(false);
      this.mainToolStrip.PerformLayout();
      this.statusStrip.ResumeLayout(false);
      this.statusStrip.PerformLayout();
      this.mainTabs.ResumeLayout(false);
      this.achievementsTab.ResumeLayout(false);
      this.achievementsTab.PerformLayout();
      this.achievementsStrip.ResumeLayout(false);
      this.achievementsStrip.PerformLayout();
      this.statisticsTab.ResumeLayout(false);
      this.statisticsTab.PerformLayout();
      ((ISupportInitialize) this.statisticsList).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
